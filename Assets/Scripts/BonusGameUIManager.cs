using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusGameUIManager : Singleton<BonusGameUIManager> {

    public ScreenFader screenFader;
    public MessageWindow messageWindow;
    public GameObject gameOverPanel;
    public Text savingLabel;
    public Text scoreLabel;
    public Text currentAgeLabel;
    public Text endRetirementAgeLabel;
    public Slider ageSlider;
    public Text finalScoreLabel;
    
    public int oldScore;
    float scoreTimer;

    void Start()
    {
        currentAgeLabel.text = PlayerData.Instance.retirementAge.ToString();
        endRetirementAgeLabel.text = PlayerData.Instance.endRetirementAge.ToString();
        ageSlider.minValue = PlayerData.Instance.retirementAge;
        ageSlider.maxValue = PlayerData.Instance.endRetirementAge;
        savingLabel.text = Utility.FormatCurrency(PlayerData.Instance.savedAmount, true);
        scoreLabel.text = BonusGameManager.Instance.currentScore.ToString();
        oldScore = BonusGameManager.Instance.currentScore;
    }

    public void UpdateSavingLabel(float change)
    {
        BonusGameManager.Instance.savingsLeft -= change * (Time.deltaTime / BonusGameManager.Instance.ageProgressionDelay);
        savingLabel.text = Utility.FormatCurrency(BonusGameManager.Instance.savingsLeft, true);
    }

    public void UpdateAgeLabel()
    {
        ageSlider.value += Time.deltaTime / BonusGameManager.Instance.ageProgressionDelay;
        currentAgeLabel.text = Mathf.RoundToInt(ageSlider.value).ToString();
    }

    public void ToggleMessageWindow(bool isOn)
    {
        if (BonusGameUIManager.Instance.messageWindow != null)
        {
            BonusGameUIManager.Instance.messageWindow.gameObject.SetActive(isOn);
        }
    }

    public void UpdateFinalScore()
    {
        finalScoreLabel.text = BonusGameManager.Instance.currentScore.ToString();
        PlayerData.Instance.UpdateScore(BonusGameManager.Instance.currentScore);
        if(PlayerData.Instance.currentLevelData.unlockingMastery != BadgeMastery.None)
        {
            //do stuff;
            PlayerData.Instance.AddUnlockedMastery();
        }
    }

    public void ScreenFaderChange(bool isFadeIn)
    {
        if (screenFader != null)
        {
            if (isFadeIn) screenFader.FadeIn();
            else screenFader.FadeOut();
        }
    }

    public bool isGameOver()
    {
        if (ageSlider.value >= PlayerData.Instance.endRetirementAge)
            return true;
        else
            return false;
    }

    public bool isAfterOneYear()
    {
        if ((int)ageSlider.value >= PlayerData.Instance.retirementAge + 1)
            return true;
        else
            return false;
    }

    public void ScoreUpdate(int scoreChange)
    {
        oldScore = BonusGameManager.Instance.currentScore;
        BonusGameManager.Instance.currentScore += scoreChange;
        scoreTimer = 0;
        StartCoroutine(UpdateScore(1));  
    }

    IEnumerator UpdateScore(float duration)
    {
        int displayScore = oldScore;
        while (scoreTimer < 1)
        {
            scoreTimer += Time.deltaTime / duration;
            displayScore = (int)Mathf.Lerp(oldScore, BonusGameManager.Instance.currentScore, scoreTimer);
            scoreLabel.text = displayScore.ToString();
            yield return null;
        }
    }
}
