using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MatchPerYear
{
    ONE = 1,
    TWO = 2,
    THREE = 3,
    FOUR = 4
};

//rename UI manager after proto
public class ScoreManager : Singleton<ScoreManager> {

    [Header("Turn-Based Set Up")]
    public MatchPerYear matchPerYear;
    public int yearPerMatch = 1;
    public int currentMatchesCompleted;
    public RectXformMover[] m_sceneLoops;

    [Header("Happiness UI")]
    public const int MAX_HAPPINESS = 6;
    public Text happinessText;
    public int happinessValue = MAX_HAPPINESS;

    [Header("Negative Feedback UI")]
    public GameObject warningBorder;
    public Text negativeText;
    public bool incomePieceOnly = false;
    public float incomeTileValue;

    [Header("Basic Set Up")]
    public Text[] incomeScoreLabels;
    public Text[] savingsScoreLabels;
    public Text studentLoanScoreLabel;
    public GameObject withLoanUI;
    public GameObject noLoanUI;
    //public Text incomeScoreText;
    //public Text savingsScoreText;
    public Text timeLeftScoreText;
    public Text ageText;
    public Text retirementAgeText;
    public Text ageSavingsLeft;
    public Slider ageSlider;
    public Slider ageLeftSlider;

    //public Text scoreNameText;
    //public Text scoreOperatorText;
    [Header("Final Feedback UI")]
    public Text scoreTotalText;
    public Text topScoreText;
    public Text bottomScoreText;
    //public Text scoreBonusText;

    [Header("Score UI")]
    public GameObject noLoanScoreUI;
    public GameObject withLoanScoreUI;
    public Text[] totalSavedTexts;
    public Text[] finalRetiredAgeTexts;
    public Text[] loanAmtTexts;
    public GameObject gameStatScreen;
    public Transform scoreContentScroll;
    public GameScoreContent[] scorePrefabs;
    public GameObject annuityScoreScreen;
    public Text annuitySavingText;
    public Text perYearText;

    public Color incomeAddScoreColor;
    public Color incomeSubScoreColor;
    public Color savingsAddScoreColor;
    public Color incomeColor;
    public Color expenseColor;
    public Color savingsColor;
    public Color loanColor;
    Color m_incomeColor;
    Color m_savingsColor;
    Color m_studentLoanColor;
    public float scoreFlashDelay = 0.5f;

    float m_cashCounterValue = 0;
    float m_savingsCounterValue = 0;
    float m_loanCounterValue = 0;
    public float counterInc = 100.0f;
    public float countTime = 1f;

    public float totalCashOnHand = 0;
    public float totalSecondsToAge = 0;
    public float totalRetirementSavings = 0;
    public float totalStudentLoan = 0;
    public float totalSavingsMultiplier = 0;

    public float totalExpenses = 0f;
    public float totalIncome = 0f;
    public float totalAnnuity = 0f;
    public float totalMutualFund = 0f;
    public float totalSavings = 0f;
    public float totalTIAA = 0f;
    public float totalStudentLoanPayed = 0f;

    public int totalExpenseCount = 0;
    public int totalIncomeCount = 0;
    public int totalAnnuityCount = 0;
    public int totalMutualFundCount = 0;
    public int totalSavingsCount = 0;
    public int totalTIAACount = 0;
    public int totalStudentLoanCount = 0;

    public float totalAnnuityDeposited = 0f;
    public float totalTIAADeposited = 0f;
    public float totalMutualFundDeposited = 0f;
    public float totalSavingsDeposited = 0f;

    public List<float> allAnnuityMultipliers = new List<float>();
    public List<float> allTIAAMultipliers = new List<float>();
    public List<float> allMutualFundMultipliers = new List<float>();
    public List<float> allSavingsMultipliers = new List<float>();

    public GameObject scorePanel;
    public Vector3 incomeScoreOffset;
    public Vector3 savingsScoreOffset;
    Vector3 m_baseScorePos;

    //float m_points = 0f;
    Color m_pointsColor;

    public bool isScorePopupActive = false;

    Board m_board;

    void Start() {
        m_board = FindObjectOfType<Board>();

        //m_incomeColor = incomeScoreText.color;
        //m_savingsColor = savingsScoreText.color;
        if (PlayerData.Instance.currentLevelData.studentLoan > 0) withLoanUI.SetActive(true);
        else noLoanUI.SetActive(true);

        totalStudentLoan = PlayerData.Instance.currentLevelData.studentLoan;

        m_incomeColor = incomeScoreLabels[0].color;
        m_savingsColor = savingsScoreLabels[0].color;
        m_studentLoanColor = studentLoanScoreLabel.color;
        m_baseScorePos = scorePanel.GetComponent<RectTransform>().anchoredPosition;

        //Turn-Based
        //m_sceneLoops = GameObject.FindObjectsOfType<RectXformMover>();

        ageSlider.minValue = GameManager.Instance.currentAge - 2;
        ageSlider.maxValue = GameManager.Instance.retirementAge;

        ageLeftSlider.minValue = GameManager.Instance.retirementAge;
        ageLeftSlider.maxValue = 100;
        ageLeftSlider.value = GameManager.Instance.retirementAge;
        ageLeftSlider.GetComponent<CanvasGroup>().alpha = 0f;

        scorePanel.SetActive(false);
        isScorePopupActive = false;

        totalExpenses = 0f;
        totalIncome = 0f;
        totalAnnuity = 0f;
        totalMutualFund = 0f;
        totalSavings = 0f;
        totalTIAA = 0f;
        totalStudentLoanPayed = 0;

        totalExpenseCount = 0;
        totalIncomeCount = 0;
        totalAnnuityCount = 0;
        totalMutualFundCount = 0;
        totalSavingsCount = 0;
        totalTIAACount = 0;
        totalStudentLoanCount = 0;

        UpdateScoreDisplay();
        UpdateTimeLeftDisplay();
        UpdateAgeDisplay();
        retirementAgeText.text = GameManager.Instance.retirementAge.ToString();
    }

    public void ResetSceneLoops()
    {
        Debug.Log("Resetting scene loop array");
        RectXformMover[] temp = new RectXformMover[3];
        temp[0] = m_sceneLoops[1];
        temp[1] = m_sceneLoops[2];
        temp[2] = m_sceneLoops[0];

        m_sceneLoops = temp;
    }

    public void UpdateUIAfterLevelData()
    {
        ageSlider.minValue = GameManager.Instance.currentAge - 2;
        ageSlider.maxValue = GameManager.Instance.retirementAge;

        ageLeftSlider.minValue = GameManager.Instance.retirementAge;
        ageLeftSlider.maxValue = GameManager.Instance.retirementAge + 30;
        ageLeftSlider.value = GameManager.Instance.retirementAge;

        retirementAgeText.text = GameManager.Instance.retirementAge.ToString();
    }

	public void AddScore(GamePieceType scoreType, float amt, MatchValue matchValue, float baseValue = 0f)//, float multiplier = 0f)
    {
        int inc = (amt >= 0) ? 1 : -1;

        switch (scoreType)
        {
            case GamePieceType.CashOnHand:
                totalCashOnHand += amt;
                StartCoroutine(CountIncomeScoreRoutine(inc));
                break;
            case GamePieceType.RetirementSavings:
                totalRetirementSavings += amt;
                StartCoroutine(CountSavingsScoreRoutine(inc));
                break;
            case GamePieceType.StudentLoan:
                totalStudentLoan += amt;
                StartCoroutine(CountStudentLoanRoutine(inc));
                break;
            case GamePieceType.SecondsToAge:
                totalSecondsToAge += amt;
                GameManager.Instance.bonusSecondsAdded += amt;
                break;
            default:
                Debug.LogWarning("Missing scoreType.");
                break;
        }

        switch (matchValue)
        {
            case MatchValue.Expense:
                totalExpenses += amt;
                totalExpenseCount++;
                break;
            case MatchValue.Income:
                totalIncome += amt;
                totalIncomeCount++;
                break;
            case MatchValue.Annuity:
                totalAnnuity += amt;
                totalAnnuityCount++;
                totalAnnuityDeposited += (int)baseValue;
                break;
            case MatchValue.MutualFund:
                totalMutualFund += amt;
				totalMutualFundCount++;
                totalMutualFundDeposited += (int)baseValue;
                break;
            case MatchValue.SavingsAccount:
                totalSavings += amt;
                totalSavingsCount++;
                totalSavingsDeposited += (int)baseValue;
                break;
            case MatchValue.AnnuityTIAA:
                totalTIAA += amt;
                totalTIAACount++;
                totalTIAADeposited += (int)baseValue;
                break;
            case MatchValue.StudentLoan:
                totalStudentLoanPayed += amt;
                totalStudentLoanCount++;
                break;
            default:
                Debug.LogWarning("Missing matchType.");
                break;
        }
    }

    IEnumerator CountStudentLoanRoutine(int inc = 1)
    {
        while (isScorePopupActive) { yield return null; }

        int counter = 0;

        if(inc == -1)
        {
            studentLoanScoreLabel.color = incomeSubScoreColor;
            while (m_loanCounterValue > totalStudentLoan && counter < 100000)
            {
                m_loanCounterValue += counterInc * inc;
                UpdateStudentLoanDisplay(m_loanCounterValue);
                counter++;

                yield return null;
            }
        }
        else
        {
            studentLoanScoreLabel.color = incomeAddScoreColor;
            while (m_loanCounterValue < totalStudentLoan && counter < 100000)
            {
                m_loanCounterValue += counterInc * inc;
                UpdateStudentLoanDisplay(m_loanCounterValue);
                counter++;

                yield return null;
            }
        }

        m_loanCounterValue = totalStudentLoan;
        UpdateStudentLoanDisplay(totalStudentLoan);

        yield return new WaitForSeconds(scoreFlashDelay);

        studentLoanScoreLabel.color = m_studentLoanColor;
    }

    IEnumerator CountIncomeScoreRoutine(int inc = 1)
    {
        while (isScorePopupActive) { yield return null; }

        int counter = 0;

        if (inc == -1)
        {
            //incomeScoreText.color = incomeSubScoreColor;
            incomeScoreLabels[0].color = incomeSubScoreColor;
            incomeScoreLabels[1].color = incomeSubScoreColor;
            while (m_cashCounterValue > totalCashOnHand && counter < 100000)
            {
                m_cashCounterValue += counterInc * inc;
                UpdateIncomeDisplay(m_cashCounterValue);
                counter++;

                yield return null;
            }
        }
        else
        {
            //incomeScoreText.color = incomeAddScoreColor;
            incomeScoreLabels[0].color = incomeAddScoreColor;
            incomeScoreLabels[1].color = incomeAddScoreColor;
            while (m_cashCounterValue < totalCashOnHand && counter < 100000)
            {
                m_cashCounterValue += counterInc * inc;
                UpdateIncomeDisplay(m_cashCounterValue);
                counter++;

                yield return null;
            }
        }

        m_cashCounterValue = totalCashOnHand;
        UpdateIncomeDisplay(totalCashOnHand);

        yield return new WaitForSeconds(scoreFlashDelay);

        //incomeScoreText.color = m_incomeColor;
        incomeScoreLabels[0].color = m_incomeColor;
        incomeScoreLabels[1].color = m_incomeColor;
    }

    IEnumerator CountSavingsScoreRoutine(int inc = 1)
    {
        while (isScorePopupActive) { yield return null; }

        int counter = 0;
        //savingsScoreText.color = savingsAddScoreColor;
        savingsScoreLabels[0].color = savingsAddScoreColor;
        savingsScoreLabels[1].color = savingsAddScoreColor;

        while (m_savingsCounterValue < totalRetirementSavings && counter < 100000)
        {
            m_savingsCounterValue += counterInc;
            UpdateSavingsDisplay(m_savingsCounterValue);
            counter++;

            yield return null;
        }

        m_savingsCounterValue = totalRetirementSavings;
        UpdateSavingsDisplay(totalRetirementSavings);
        UpdateAgeSavingsDisplay();

        yield return new WaitForSeconds(scoreFlashDelay);

        //savingsScoreText.color = m_savingsColor;
        savingsScoreLabels[0].color = m_savingsColor;
        savingsScoreLabels[1].color = m_savingsColor;
    }


    public void AddAge(int amt = 1)
    {
        GameManager.Instance.currentAge += amt;
        UpdateAgeDisplay();
        UpdateAgeLeftDisplay();
    }

    void UpdateIncomeDisplay(float amt)
    {
        if (incomeScoreLabels[0] != null && incomeScoreLabels[1] != null)
        {
            //incomeScoreText.text = Utility.FormatCurrency(amt);
            incomeScoreLabels[0].text = Utility.FormatCurrency(amt);
            incomeScoreLabels[1].text = Utility.FormatCurrency(amt);
        }
    }

    void UpdateStudentLoanDisplay(float amt)
    {
        if(studentLoanScoreLabel != null)
        {
            studentLoanScoreLabel.text = Utility.FormatCurrency(amt);
        }
    }

    public void UpdateNegativeMoney()
    {
        UpdateNegativeUI(MatchValue.Income, "Low cash!", true);
    }

    void UpdateSavingsDisplay(float amt)
    {
        if (savingsScoreLabels[0] != null && savingsScoreLabels[1] != null)
        {
            //savingsScoreText.text = Utility.FormatCurrency(amt);
            savingsScoreLabels[0].text = Utility.FormatCurrency(amt);
            savingsScoreLabels[1].text = Utility.FormatCurrency(amt);
        }
    }

    void UpdateAgeSavingsDisplay()
    {
        if (ageSavingsLeft != null)
        {
            int yearsOfSavings = (int)(totalRetirementSavings / GameManager.Instance.monthlyIncome);
            int savingAge = GameManager.Instance.retirementAge + yearsOfSavings;
            int ageToCheck = GameManager.Instance.retirementAge + 1;
            GameManager.Instance.savingsLeftAge = savingAge;
            ageSavingsLeft.text = savingAge.ToString();

            if(savingAge >= ageToCheck && ageLeftSlider.GetComponent<CanvasGroup>().alpha == 0f)
            {
                ageLeftSlider.GetComponent<CanvasGroup>().alpha = 1f;
            }
        }
    }

    void UpdateScoreDisplay()
	{
        UpdateIncomeDisplay(totalCashOnHand);
        UpdateSavingsDisplay(totalRetirementSavings);
        UpdateStudentLoanDisplay(totalStudentLoan);
        UpdateAgeSavingsDisplay();
    }

	public void UpdateTimeLeftDisplay()
	{
		if (timeLeftScoreText != null)
		{
            string min = ((int)GameManager.Instance.gameTimer / 60).ToString();
            string sec = (GameManager.Instance.gameTimer % 60).ToString("f2");
            timeLeftScoreText.text = min + ":" + sec;

            //timeLeftScoreText.text = (GameManager.Instance.gameTimer).ToString("f2");
            //timeLeftScoreText.text = ((int)GameManager.Instance.gameTimer).ToString();
        }
    }

    public void UpdateAgeDisplay()
    {
        ageText.text = GameManager.Instance.currentAge.ToString();
        ageSlider.value = GameManager.Instance.currentAge;
    }

    public void UpdateAgeLeftDisplay()
    {
        ageText.text = GameManager.Instance.currentAge.ToString();
        ageLeftSlider.value = Mathf.Min(GameManager.Instance.savingsLeftAge, 100);
    }

    public void UpdatePointsPopup(GamePiece piece, int count)
    {
        StartCoroutine(UpdatePointsPopupRoutine(piece, count));
    }

    IEnumerator UpdatePointsPopupRoutine(GamePiece piece, int count)
    {
        //Turn-Based
        currentMatchesCompleted++;
        bool isFood = false;

        isScorePopupActive = true;
        scorePanel.SetActive(true);

        //scoreNameText.text = "";
        //scoreOperatorText.text = "";
        scoreTotalText.text = "";
        topScoreText.text = "";
        bottomScoreText.text = "";
        //scoreBonusText.text = "";
        scorePanel.GetComponent<RectTransform>().anchoredPosition = m_baseScorePos;

        float scoreCashOnHand = 0;
        //float scoreSecondsToAge = 0;
        float happinessIncrease = 0;
        float scoreRetirementSaving = 0;
        float scoreRetirementMultiplier = 0;
        float loanPayed = 0;

        float[] points = GameManager.Instance.GetPoints(piece);
        for (int i = 0; i < points.Length; i++)
        {
            switch (i)
            {
                case 0:
                    scoreCashOnHand += (points[i] * count);
                    break;
                case 1:
                    //scoreSecondsToAge += points[i] * count;
                    happinessIncrease = points[i] * count;
                    break;
                case 2:
                    scoreRetirementSaving += points[i] * count;
                    break;
                case 3:
                    scoreRetirementMultiplier = points[i];
                    break;
                case 4:
                    loanPayed += (points[i] * count);
                    break;
                    
            }
        }

        scorePanel.GetComponent<MaskableGraphic>().color = piece.GetComponent<SpriteRenderer>().color;
        //scoreNameText.text = piece.pieceName;

        switch (piece.matchValue)
        {
            case MatchValue.Income:
                //scoreOperatorText.text = "+";
                scoreTotalText.text = "+" + Utility.FormatCurrency(scoreCashOnHand);
                scoreTotalText.color = incomeColor;
                scorePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(m_baseScorePos.x + incomeScoreOffset.x, m_baseScorePos.y + incomeScoreOffset.y, m_baseScorePos.z);
                break;
            case MatchValue.Expense:
                //scoreOperatorText.text = Utility.FormatCurrency(scoreCashOnHand);
                happinessIncrease = Mathf.Min(Mathf.Floor(happinessIncrease), MAX_HAPPINESS);
                topScoreText.text = Utility.FormatCurrency(scoreCashOnHand);
                topScoreText.color = incomeSubScoreColor;
                bottomScoreText.text += "+" + happinessIncrease + " happiness bonus";
                bottomScoreText.color = expenseColor;
                happinessValue += (int)happinessIncrease;
                happinessValue = Mathf.Min(happinessValue, MAX_HAPPINESS);
                happinessText.text = happinessValue.ToString();
                scorePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(m_baseScorePos.x + incomeScoreOffset.x, m_baseScorePos.y + incomeScoreOffset.y, m_baseScorePos.z);
                break;
            case MatchValue.Annuity:
				//GameManager.Instance.allGreenMultipliers.Add(scoreRetirementMultiplier); //not the best way to do this
                allAnnuityMultipliers.Add(scoreRetirementMultiplier);
				//scoreOperatorText.text = Utility.FormatCurrency(scoreRetirementSaving);
				//scoreTotalText.text = "x" + scoreRetirementMultiplier.ToString("f0");
                topScoreText.text = Utility.FormatCurrency(scoreCashOnHand);
                topScoreText.color = incomeSubScoreColor;
                bottomScoreText.text = "+" + Utility.FormatCurrency(scoreRetirementSaving * scoreRetirementMultiplier);
                bottomScoreText.color = savingsColor;
				// totalsText += "+" + Utility.FormatCurrency(scoreRetirementSaving) + " saving x" + scoreRetirementMultiplier;
				//scoreBonusText.text = "age bonus";
				scorePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(m_baseScorePos.x + savingsScoreOffset.x, m_baseScorePos.y + savingsScoreOffset.y, m_baseScorePos.z);
				break;
			case MatchValue.MutualFund:
				//GameManager.Instance.allPurpleMultipliers.Add(scoreRetirementMultiplier); //not the best way to do this
                allMutualFundMultipliers.Add(scoreRetirementMultiplier);
				//scoreOperatorText.text = Utility.FormatCurrency(scoreRetirementSaving);
                //scoreTotalText.text = "x" + scoreRetirementMultiplier.ToString("f0");
                topScoreText.text = Utility.FormatCurrency(scoreCashOnHand);
                topScoreText.color = incomeSubScoreColor;
                bottomScoreText.text = "+" + Utility.FormatCurrency(scoreRetirementSaving * scoreRetirementMultiplier);
                bottomScoreText.color = savingsColor;
                // totalsText += "+" + Utility.FormatCurrency(scoreRetirementSaving) + " saving x" + scoreRetirementMultiplier;
                //scoreBonusText.text = "age bonus";
                scorePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(m_baseScorePos.x + savingsScoreOffset.x, m_baseScorePos.y + savingsScoreOffset.y, m_baseScorePos.z);
                break;
            case MatchValue.AnnuityTIAA:
                allTIAAMultipliers.Add(scoreRetirementMultiplier);
                topScoreText.text = Utility.FormatCurrency(scoreCashOnHand);
                topScoreText.color = incomeSubScoreColor;
                bottomScoreText.text = "+" + Utility.FormatCurrency(scoreRetirementSaving * scoreRetirementMultiplier);
                bottomScoreText.color = savingsColor;
                scorePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(m_baseScorePos.x + savingsScoreOffset.x, m_baseScorePos.y + savingsScoreOffset.y, m_baseScorePos.z);
                break;
            case MatchValue.SavingsAccount:
                allSavingsMultipliers.Add(scoreRetirementMultiplier);
                topScoreText.text = Utility.FormatCurrency(scoreCashOnHand);
                topScoreText.color = incomeSubScoreColor;
                bottomScoreText.text = "+" + Utility.FormatCurrency(scoreRetirementSaving * scoreRetirementMultiplier);
                bottomScoreText.color = savingsColor;
                scorePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(m_baseScorePos.x + savingsScoreOffset.x, m_baseScorePos.y + savingsScoreOffset.y, m_baseScorePos.z);
                break;
            case MatchValue.StudentLoan:
                topScoreText.text = Utility.FormatCurrency(scoreCashOnHand);
                topScoreText.color = incomeSubScoreColor;
                bottomScoreText.text = Utility.FormatCurrency(loanPayed);
                bottomScoreText.color = loanColor;
                scorePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(m_baseScorePos.x + incomeScoreOffset.x, m_baseScorePos.y + incomeScoreOffset.y, m_baseScorePos.z);
                break;
            default:
                Debug.LogWarning("Missing matchType.");
                break;
        }

        //Turn-Based
        if (GameManager.Instance.isTurnBased)
        {
            GameObject.FindObjectOfType<AgePickupMover>().GetComponent<AgePickupMover>().MoveByMatches(isFood);
            foreach (RectXformMover scene in m_sceneLoops)
            {
                scene.MoveByMatches();
            }

            if (currentMatchesCompleted == (int)matchPerYear)
            {
                AddAge(yearPerMatch);
                currentMatchesCompleted = 0;
                happinessValue--;
                happinessValue = Mathf.Max(0, happinessValue);
                happinessText.text = happinessValue.ToString();
            }

            StartCoroutine(AvatarAnimationComplete());
        }

        //animate, yield till finished then update score

        if (totalCashOnHand <= 0) m_board.onlyIncomePiece = true;
        if (happinessValue <= 2 && !m_board.onlyIncomePiece) m_board.onlyExpensePiece = true;

        yield return new WaitForSeconds(1.0f);
 
        scorePanel.SetActive(false);
        isScorePopupActive = false;

        //UpdateScoreDisplay();

        bool isCashNegative = false;

        if(totalCashOnHand < incomeTileValue * 2 && totalCashOnHand > 0)
        {
            Debug.Log("cash is less than 2 income");
            UpdateNegativeUI(MatchValue.Income, "Low cash!", true, false);
            isCashNegative = true;
        }
        else if (totalCashOnHand <= 0)
        {
            Debug.Log("cash is negative");
            UpdateNegativeUI(MatchValue.Income, "Low cash!", true);
            isCashNegative = true;
        }
        else if (happinessValue == 2 && !isCashNegative)
        {
            Debug.Log("happiness is 2");
            UpdateNegativeUI(MatchValue.Expense, "So hungry!", true, false);
        }
        else if (happinessValue <= 1 && !isCashNegative)
        {
            Debug.Log("happiness is bad");
            UpdateNegativeUI(MatchValue.Expense, "So hungry!", true);
        }
        else
        {
            Debug.Log("all positive");
            UpdateNegativeUI(MatchValue.None, "", false);
        }
    }

    IEnumerator AvatarAnimationComplete()
    {
        GameManager.Instance.avatar.PlayAnimation();
        yield return new WaitForSeconds(1);
        GameManager.Instance.avatar.StopAnimation();
    }

    void UpdateNegativeUI(MatchValue matchType, string msg, bool showMessageText, bool disableOther = true)
    {
        //if (showMessageText)
            
        //else
        //    gameBackground.color = gameColorBackground;

        warningBorder.SetActive(showMessageText);

        negativeText.text = msg;
        negativeText.gameObject.SetActive(showMessageText);
        m_board.EnableAllPieces();
        if(disableOther)
            m_board.DisableSelectedPieces(matchType);
    }

    public void UpdateGameSummaryUI()
    {
        if(totalStudentLoan < PlayerData.Instance.currentLevelData.studentLoan)
        {
            noLoanScoreUI.SetActive(false);
            withLoanScoreUI.SetActive(true);

            float loanLeft = PlayerData.Instance.currentLevelData.studentLoan - totalStudentLoan;
            UpdateTextArray(loanAmtTexts, Utility.FormatCurrency(loanLeft).ToString());
        }

        UpdateTextArray(totalSavedTexts, Utility.FormatCurrency(totalRetirementSavings));
        PlayerData.Instance.savedAmount = totalRetirementSavings;

        int yearsOfSavings = (int)(totalRetirementSavings / PlayerData.Instance.currentLevelData.baseSalary);
        int finalRetiredAge = PlayerData.Instance.currentLevelData.retirementAge + yearsOfSavings;
        PlayerData.Instance.retirementAge = PlayerData.Instance.currentLevelData.retirementAge;
        PlayerData.Instance.endRetirementAge = finalRetiredAge;

        UpdateTextArray(finalRetiredAgeTexts, finalRetiredAge.ToString());
    }

    public void AddGameScoreToScrollList()
    {
        if(totalIncome > 0)
        {
            GameScoreContent score = Instantiate(scorePrefabs[0]) as GameScoreContent;
            score.transform.parent = scoreContentScroll.transform;
            score.transform.localScale = Vector3.one;
            score.Init(totalIncomeCount, totalIncome);
        }

        if(totalExpenses != 0)
        {
            GameScoreContent score = Instantiate(scorePrefabs[1]) as GameScoreContent;
            score.transform.parent = scoreContentScroll.transform;
            score.transform.localScale = Vector3.one;
            score.Init(totalExpenseCount, totalExpenses);
        }

        if(totalSavings > 0)
        {
            GameScoreContent score = Instantiate(scorePrefabs[2]) as GameScoreContent;
            score.transform.parent = scoreContentScroll.transform;
            score.transform.localScale = Vector3.one;
            score.Init(totalSavingsCount, totalSavingsDeposited, GetMulitplierAvg(allSavingsMultipliers), totalSavings);
        }

        if(totalMutualFund > 0)
        {
            GameScoreContent score = Instantiate(scorePrefabs[3]) as GameScoreContent;
            score.transform.parent = scoreContentScroll.transform;
            score.transform.localScale = Vector3.one;
            score.Init(totalMutualFundCount, totalMutualFundDeposited, GetMulitplierAvg(allMutualFundMultipliers), totalMutualFund);
        }

        if(totalAnnuity > 0)
        {
            GameScoreContent score = Instantiate(scorePrefabs[4]) as GameScoreContent;
            score.transform.parent = scoreContentScroll.transform;
            score.transform.localScale = Vector3.one;
            score.Init(totalAnnuityCount, totalAnnuityDeposited, GetMulitplierAvg(allAnnuityMultipliers), totalAnnuity);
        }

        if(totalTIAA > 0)
        {
            GameScoreContent score = Instantiate(scorePrefabs[5]) as GameScoreContent;
            score.transform.parent = scoreContentScroll.transform;
            score.transform.localScale = Vector3.one;
            score.Init(totalTIAACount, totalTIAADeposited, GetMulitplierAvg(allTIAAMultipliers), totalTIAA);
        }

        if(totalStudentLoan > 0)
        {
            GameScoreContent score = Instantiate(scorePrefabs[6]) as GameScoreContent;
            score.transform.parent = scoreContentScroll.transform;
            score.transform.localScale = Vector3.one;
            score.Init(totalStudentLoanCount, totalStudentLoan);
        }
    }

    public void ShowAunnityScore()
    {
        annuityScoreScreen.SetActive(true);

        if (totalRetirementSavings > 0) annuitySavingText.text = Utility.FormatCurrency(totalRetirementSavings);
        perYearText.text = (totalRetirementSavings / (PlayerData.Instance.endRetirementAge - PlayerData.Instance.retirementAge)).ToString();
    }

    int GetMulitplierAvg(List<float> multipliers)
    {
        float total = 0f;
        foreach (float m in multipliers)
        {
            total += m;
        }

        return (int)(total / multipliers.Count);
    }

    void UpdateTextArray(Text[] textArray, string textData)
    {
        for(int i = 0; i<textArray.Length; i++)
        {
            textArray[i].text = textData;
        }
    }
}
