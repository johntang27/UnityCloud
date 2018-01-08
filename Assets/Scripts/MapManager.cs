using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BadgeMastery
{
    None,
    SimpleSavings,
    MutualFunds,
    Annuities,
    StudentLoans,
    SavingsMatch,
    Balance,
    Time,
    MutualFunds2,
    Annuities2
}

public class MapManager : Singleton<MapManager> {

    public Image screenFader;

    [Header("Header UI")]
    public Text playerNameText;
    public Text totalScoreText;

    [Header("PlayerInfo UI")]
    public Text percentageText;
    public Image[] avatarSprites;
    public Text playerScoreText;
    public BadgeMasteryUI[] badges;

    [Header("LevelInfo UI")]
    public GameObject levelInfoUI;
    public Text levelNameText;
    public ToggleSprites[] scoreStars = new ToggleSprites[3];
    public Image dreamBubbleSprite;
    public Image[] gameTiles;
    public Text requiredAmtText;
    public Text levelDescription;

    [Header("MISC")]
    public MapLevelButton[] levelButtons;
    public LevelData[] levelData;

    bool doNotLoadGame = false;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(LoadIntoMap());

        UpdateHeaderUI();

        LevelDataCollection dataCollection = LevelDataCollection.LoadFromResources("LevelData");
        levelData = dataCollection.data;

        for(int i=0; i<levelButtons.Length; i++)
        {
            levelButtons[i].SetLevelDataAndUI(levelData);
        }
    }

    IEnumerator LoadIntoMap()
    {
        screenFader.GetComponent<ScreenFader>().FadeOut();

        while (!screenFader.GetComponent<ScreenFader>().isFadingDone)
        {
            //Debug.Log(screenFader.GetComponent<ScreenFader>().isFadingDone);
            yield return null;
        }

        //Debug.Log("done");
        screenFader.gameObject.SetActive(false);
    }

    void UpdateHeaderUI()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.CalculateTotalScore();
            playerNameText.text = PlayerData.Instance.playerName;
            totalScoreText.text = PlayerData.Instance.totalScore.ToString();
        }
        else
            Debug.LogError("No Player Data");
    }

    public void UpdatePlayerInfoUI()
    {
        avatarSprites[0].sprite = Resources.Load("FemaleAvatar/body_female_med_" + PlayerData.Instance.playerAvatar.bodySprite.ToString(), typeof(Sprite)) as Sprite;
        avatarSprites[1].sprite = Resources.Load("FemaleAvatar/face_female_" + PlayerData.Instance.playerAvatar.faceSprite.ToString(), typeof(Sprite)) as Sprite;
        avatarSprites[2].sprite = Resources.Load("FemaleAvatar/hair_female_" + PlayerData.Instance.playerAvatar.hairSprite.ToString(), typeof(Sprite)) as Sprite;
        avatarSprites[3].sprite = Resources.Load("FemaleAvatar/outfit_female_med_" + PlayerData.Instance.playerAvatar.outfitSprite.ToString(), typeof(Sprite)) as Sprite;

        avatarSprites[0].color = PlayerData.Instance.playerAvatar.bodyColor;
        avatarSprites[2].color = PlayerData.Instance.playerAvatar.hairColor;
        avatarSprites[3].color = PlayerData.Instance.playerAvatar.outfitColor;

        playerScoreText.text = PlayerData.Instance.totalScore.ToString();

        if (PlayerData.Instance.unlockedMasteries.Count == 0) return;
        for(int i = 0; i < PlayerData.Instance.unlockedMasteries.Count; i++)
        {
            badges[(int)PlayerData.Instance.unlockedMasteries[i] + 1].UnlockMastery();
        }
    }

    public void UpdateLevelInfoUI(LevelData data)
    {
        if (data.levelID == 0)
        {
            doNotLoadGame = true;
            Debug.LogError("No level data");
            return;
        }

        doNotLoadGame = false;

        levelInfoUI.SetActive(true);

        levelNameText.text = data.levelName;
        for(int i=0; i<data.tiles.Length; i++)
        {
            gameTiles[i].gameObject.SetActive(true);
            gameTiles[i].sprite = Resources.Load("GameTiles/tile_" + data.tiles[i].ToString(), typeof(Sprite)) as Sprite;
        }

        PlayerData.Instance.currentLevelData = data;

        dreamBubbleSprite.sprite = Resources.Load("DreamBubbles/dreamBubble_" + data.retirementDream.ToString(), typeof(Sprite)) as Sprite;
        Debug.Log(Utility.FormatCurrency((int)data.retirementDream));
        requiredAmtText.text = Utility.FormatCurrency((int)data.retirementDream);
        levelDescription.text = data.levelDescription;

        if(PlayerData.Instance.levelScores.Count > 0 && PlayerData.Instance.levelScores.ContainsKey(data.levelID.ToString()))
        {
            int score = PlayerData.Instance.GetScoreByLevelID(data.levelID.ToString());

            if ((float)score > (float)(data.minimumScore * PlayerData.Instance.scoreMultipliers[0])) scoreStars[0].Toggle(true);
            if ((float)score > (float)(data.minimumScore * PlayerData.Instance.scoreMultipliers[1])) scoreStars[1].Toggle(true);
            if ((float)score > (float)(data.minimumScore * PlayerData.Instance.scoreMultipliers[2])) scoreStars[2].Toggle(true);
        }
    }

    public void BackToMap()
    {
        for (int i = 0; i < gameTiles.Length; i++)
            gameTiles[i].gameObject.SetActive(false);
    }

    public void LoadScene()
    {
        if(doNotLoadGame)
        {
            Debug.LogWarning("No data for this level, please check Datamaker xml");
            return;
        }
        SceneManager.LoadScene("game");
    }
}
