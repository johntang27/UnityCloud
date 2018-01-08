using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LevelGoal))]
public class GameManager : Singleton<GameManager>
{
    [Header("Turn-Based Set Up")]
    public RectXformMover firstScene;
    public bool isTurnBased = true;
    //public bool isScrollingBG = false;

    public AvatarWalkAnimation avatar;

    [Header("Basic Set Up")]
    public int currentAge = 22;
    public int retirementAge = 65;
    public int savingsLeftAge = 65;
    public float secondsToAgeUp = 2.0f;
    public float bonusSecondsAdded = 0f;
    public float gameTimer = 0.0f;
    public float monthlyIncome = 50000.0f;

    public string gameGoalDesc;
    public Text gameGoalText;

    public float delayToStart = 1.0f;
	public float delayToSetupBoard = 2.0f;
    public float delayToEndGame = 1.0f;

    public GameObject gameOverPanel;

 //   public Text totalSavingText;
 //   public Text totalSavingLabel;
 //   public Text blueTotalText;
 //   public Text goldTotalText;
 //   public Text greenTotalText;
 //   public Text purpleTotalText;
 //   public Text blueCountText;
 //   public Text goldCountText;
 //   public Text greenCountText;
 //   public Text purpleCountText;
	//public Text greenAvgText;
	//public Text purpleAvgText;
	public Text messageText;

	//public List<float> allGreenMultipliers = new List<float>();
	//public List<float> allPurpleMultipliers = new List<float>();

	public ScreenFader screenFader;
    public MessageWindow messageWindow;

    public Sprite startIcon;
    public Sprite endIcon;

	LevelGoal m_levelGoal;

    AgePickupMover m_agePickup;
    RectXformMover[] m_sceneLoops;

	Board m_board;
	bool m_isReadyToBegin = false;
	bool m_isGameOver = false;
    public bool m_isStartingGame = false;
    bool m_isReadyToRestart = false;
	public bool isPaused = false;

    public float timeToAgeUp = 0f;

	public override void Awake()
	{
		base.Awake();

		m_board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
		m_agePickup = GameObject.FindObjectOfType<AgePickupMover>().GetComponent<AgePickupMover>();
		m_sceneLoops = GameObject.FindObjectsOfType<RectXformMover>();
		m_levelGoal = GetComponent<LevelGoal>();
	}

	void Start()
	{
        //Set up data from PlayerData CurrentLevelData
        currentAge = PlayerData.Instance.currentLevelData.startingAge;
        retirementAge = PlayerData.Instance.currentLevelData.retirementAge;
        monthlyIncome = (float)PlayerData.Instance.currentLevelData.baseSalary;
        if (PlayerData.Instance.currentLevelData.timeMode == TimeMode.TurnBased)
            isTurnBased = true;
        else
            isTurnBased = false;

        gameGoalDesc = PlayerData.Instance.currentLevelData.levelName;

        ScoreManager.Instance.UpdateUIAfterLevelData();

		//Debug.Log(Utility.FormatCurrency(500999.00f, true));
		//Debug.Log(Utility.FormatCurrency(1540999.00f, true));

		StartCoroutine(ExecuteGameLoop());
	}

	IEnumerator ExecuteGameLoop()
	{
        messageWindow.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);

        yield return StartCoroutine("StartGameRoutine");
		yield return StartCoroutine("PlayGameRoutine");
		yield return StartCoroutine("EndGameRoutine");
	}

    public void BeginGame()
    {
        m_isStartingGame = true;
        
        //Turn-Based
        if (isTurnBased)
        {
            //firstScene.SetIncrement();
            m_agePickup.SetIncrement();
            avatar.StopAnimation();
        }
    }

	IEnumerator StartGameRoutine()
	{
		while (!m_isReadyToBegin)
		{
			yield return null;
            yield return new WaitForSeconds(delayToStart);
            m_isReadyToBegin = true;
        }

		if(screenFader != null)
		{
			screenFader.FadeOut();
		}

        yield return new WaitForSeconds(delayToSetupBoard);

        if (m_board != null)
        {
            m_board.SetupBoard(PlayerData.Instance.currentLevelData.tiles);
            ScoreManager.Instance.UpdateNegativeMoney();
        }

        if (messageWindow != null)
        {
            messageWindow.gameObject.SetActive(true);
            messageWindow.ShowMessage(startIcon, gameGoalDesc, "Start");
        }

        gameGoalText.text = gameGoalDesc;

        while (!m_isStartingGame)
        {
            yield return null;
        }
    }

    IEnumerator PlayGameRoutine()
	{
		m_board.playerInputEnabled = true;
		gameTimer = 0f;

		if (messageWindow != null)
        {
            messageWindow.gameObject.SetActive(false);
        }

        //Turn-Based
        if (!isTurnBased)
        {
            m_agePickup.StartMove(timeToAgeUp);

            foreach (RectXformMover scene in m_sceneLoops)
            {
                scene.StartMove();
            }
        }

        //if(isTurnBased && isScrollingBG)
        //{
        //    foreach (RectXformMover scene in m_sceneLoops)
        //    {
        //        scene.StartMove();
        //    }
        //}

        while (!m_isGameOver)
		{
			while(isPaused) { yield return null; }

            gameTimer += Time.deltaTime;
            timeToAgeUp = secondsToAgeUp + bonusSecondsAdded;

			if (!isTurnBased)
			{
				if (gameTimer >= (secondsToAgeUp + bonusSecondsAdded))
				{
					ScoreManager.Instance.AddAge(1);
					gameTimer = 0f;
					bonusSecondsAdded = 0f;
				}
			}
			//m_levelGoal.ProcessRequirements(); //TODO: look at this for level types

			m_isGameOver = m_levelGoal.IsGameOver();

			ScoreManager.Instance.UpdateTimeLeftDisplay();
            yield return null;
		}

		m_board.playerInputEnabled = false;
	}

	IEnumerator EndGameRoutine()
	{
        m_isReadyToRestart = false;

        yield return new WaitForSeconds(delayToEndGame);

		if (screenFader != null)
		{
			screenFader.FadeIn();
		}
        yield return new WaitForSeconds(delayToEndGame);

        gameOverPanel.gameObject.SetActive(true);

        //totalSavingText.text = Utility.FormatCurrency(ScoreManager.Instance.totalRetirementSavings, true);
        //PlayerData.Instance.savedAmount = ScoreManager.Instance.totalRetirementSavings;

        //int yearsOfSavings = (int)(ScoreManager.Instance.totalRetirementSavings / monthlyIncome);
        //totalSavingLabel.text = "This will last you till you're " + (retirementAge + yearsOfSavings).ToString() + " years old.";
        //PlayerData.Instance.retirementAge = retirementAge;
        //PlayerData.Instance.endRetirementAge = retirementAge + yearsOfSavings;
        ScoreManager.Instance.UpdateGameSummaryUI();

        ScoreManager.Instance.AddGameScoreToScrollList();

  //      blueCountText.text = ScoreManager.Instance.totalExpenseCount.ToString();
  //      blueTotalText.text = Utility.FormatCurrency(ScoreManager.Instance.totalExpenses, true);
		//blueTotalText.color = ScoreManager.Instance.totalExpenses < 0 ? ScoreManager.Instance.incomeSubScoreColor : ScoreManager.Instance.incomeAddScoreColor;

		//goldCountText.text = ScoreManager.Instance.totalIncomeCount.ToString();
  //      goldTotalText.text = Utility.FormatCurrency(ScoreManager.Instance.totalIncome, true);
		//goldTotalText.color = ScoreManager.Instance.totalIncome < 0 ? ScoreManager.Instance.incomeSubScoreColor : ScoreManager.Instance.incomeAddScoreColor;

		//greenCountText.text = ScoreManager.Instance.totalAnnuityCount.ToString();
  //      greenTotalText.text = Utility.FormatCurrency(ScoreManager.Instance.totalAnnuity, true);
		//greenTotalText.color = ScoreManager.Instance.totalAnnuity < 0 ? ScoreManager.Instance.incomeSubScoreColor : ScoreManager.Instance.incomeAddScoreColor;

		//purpleCountText.text = ScoreManager.Instance.totalMutualFundCount.ToString();
  //      purpleTotalText.text = Utility.FormatCurrency(ScoreManager.Instance.totalMutualFund, true);
		//purpleTotalText.color = ScoreManager.Instance.totalMutualFund < 0 ? ScoreManager.Instance.incomeSubScoreColor : ScoreManager.Instance.incomeAddScoreColor;

		//greenAvgText.text = "AVG x" + (GetMulitplierAvg(allGreenMultipliers)).ToString();
		//purpleAvgText.text = "AVG x" + (GetMulitplierAvg(allPurpleMultipliers)).ToString();

		while (!m_isReadyToRestart)
        {
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	int GetMulitplierAvg(List<float> multipliers)
	{
		float total = 0f;
		foreach(float m in multipliers)
		{
			total += m;
		}

		return (int)(total / multipliers.Count);
	}

	public void ScorePoints(GamePiece piece)
	{
		ScoreManager.Instance.isScorePopupActive = true;

		if (ScoreManager.Instance != null)
		{
			if (piece.cashOnHand != 0)
			{
				ScoreManager.Instance.AddScore(GamePieceType.CashOnHand, piece.cashOnHand, piece.matchValue);
			}

			if (piece.secondsToAge != 0)
			{
				ScoreManager.Instance.AddScore(GamePieceType.SecondsToAge, piece.secondsToAge, piece.matchValue);
			}

			if (piece.retirementSavings != 0)
			{
				//(Cost * Multiplier). Multiplier = (2^(Years Remaining)/(72/Interest Rate)
				//2500 * 2 ^ ( (65 - AGE) / (72 / 8) )  

				float multiplier = (Mathf.Pow(2, ((GameManager.Instance.retirementAge - GameManager.Instance.currentAge) / (72 / piece.interestRate))));
				float savings = piece.retirementSavings * multiplier;
                ScoreManager.Instance.AddScore(GamePieceType.RetirementSavings, savings, piece.matchValue, piece.retirementSavings);// multiplier);
			}

            if(piece.studentLoan != 0)
            {
                ScoreManager.Instance.AddScore(GamePieceType.StudentLoan, piece.studentLoan, piece.matchValue);
            }
		}
	}

	public float[] GetPoints(GamePiece piece)
	{
		float[] piecePoints = new float[5]; //find a better way to do this next time

		if (piece.cashOnHand != 0)
		{
			piecePoints[0] = piece.cashOnHand;
		}

		if (piece.secondsToAge != 0)
		{
			//piecePoints[1] = piece.secondsToAge;
            piecePoints[1] = piece.happinessIncrease;
		}

		if (piece.retirementSavings != 0)
		{
			//(Cost * Multiplier). Multiplier = (2^(Years Remaining)/(72/Interest Rate)
			//2500 * 2 ^ ( (65 - AGE) / (72 / 8) )  

			float mulitplier = (Mathf.Pow(2, ((GameManager.Instance.retirementAge - GameManager.Instance.currentAge) / (72 / piece.interestRate))));
			//float savings = retirementSavings * mulitplier;

			piecePoints[2] = piece.retirementSavings;
			piecePoints[3] = mulitplier;
		}

        if(piece.studentLoan != 0)
        {
            piecePoints[4] = piece.studentLoan;
        }

		return piecePoints;
	}
	public void ShowPoints(GamePiece piece)
	{
		if (ScoreManager.Instance != null)
		{
			Debug.Log("CashOnHand: " + piece.cashOnHand);
			Debug.Log("SecondsToAge: " + piece.secondsToAge);

			float savings = piece.retirementSavings * (Mathf.Pow(2, ((GameManager.Instance.retirementAge - GameManager.Instance.currentAge) / (72 / piece.interestRate))));
			Debug.Log("retirementSavings: " + savings);
		}
	}

	public void RestartScene()
    {
        m_isReadyToRestart = true;
    }

    public void ToBonusGame()
    {
        SceneManager.LoadScene("BonusGame");
    }

    public void ToAnnuityOrBonusGame(bool toBonus)
    {
        if(toBonus)
        {
            ToBonusGame();
            return;
        }

        if (ScoreManager.Instance.totalAnnuity > 0 || ScoreManager.Instance.totalTIAA > 0)
            ScoreManager.Instance.ShowAunnityScore();
        else
            ToBonusGame();
    }
}
