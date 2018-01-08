using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLevelButton : MonoBehaviour {

    [Header("UI")]
    public Image buttonImage;
    public GameObject lockedImage;
    public Sprite[] starSprites;
    public GameObject[] stars;
    [Header("Non UI")]
    public int levelID;
    public LevelData levelData;

    Button m_button;

    public void SetLevelDataAndUI(LevelData[] data)
    {
        for(int i=0; i<data.Length; i++)
        {
            if(data[i].levelID == levelID)
            {
                levelData = data[i];
                break;
            }
        }

        m_button = this.GetComponent<Button>();

        if (PlayerData.Instance.levelScores.ContainsKey(levelData.previousLevelID.ToString()) || levelData.previousLevelID == 0) //check scores dictionary to see previous (unlocking) level ID has score or if level 0
        {
            int previousLevelScore = PlayerData.Instance.GetScoreByLevelID(levelData.previousLevelID.ToString());

            if (levelData.previousLevelID == 0 || previousLevelScore > levelData.minimumScore)
            {
                UpdateButtonUI(true);

                int currentLevelScore = PlayerData.Instance.GetScoreByLevelID(levelData.levelID.ToString());

                if ((float)currentLevelScore > (float)(levelData.minimumScore * PlayerData.Instance.scoreMultipliers[0])) stars[0].GetComponent<Image>().sprite = starSprites[1];
                if ((float)currentLevelScore > (float)(levelData.minimumScore * PlayerData.Instance.scoreMultipliers[1])) stars[1].GetComponent<Image>().sprite = starSprites[1];
                if ((float)currentLevelScore > (float)(levelData.minimumScore * PlayerData.Instance.scoreMultipliers[2])) stars[2].GetComponent<Image>().sprite = starSprites[1];
            }
        }
        else
            UpdateButtonUI(false);
    }

    public void LevelButtonClicked()
    {
        MapManager.Instance.UpdateLevelInfoUI(levelData);
    }

    void UpdateButtonUI(bool unlocked)
    {
        lockedImage.SetActive(!unlocked);
        buttonImage.enabled = unlocked;
        for(int i=0; i<stars.Length; i++)
        {
            stars[i].SetActive(unlocked);
        }
        m_button.interactable = unlocked;
    }
}
