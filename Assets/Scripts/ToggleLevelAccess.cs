using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleLevelAccess : MonoBehaviour {

    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public Button levelButton;
    public LevelData levelData;

    public void UpdateLevelData(LevelData data)
    {
        levelData = data;
    }

    public void SetButtonStatus(bool unlocked)
    {
        if (unlocked) levelButton.GetComponent<Image>().color = Color.green;
        else levelButton.GetComponent<Image>().color = Color.red;

        //To do, when we have art asset
        //if (unlocked) levelButton.GetComponent<Image>().sprite = unlockedSprite;
        //else levelButton.GetComponent<Image>().sprite = lockedSprite;

        levelButton.interactable = unlocked;
    }
}
