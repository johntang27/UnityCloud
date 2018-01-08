using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeMasteryUI : MonoBehaviour {

    public GameObject crown;
    public Image masterySprite;
    public Text masteryText;

	public void UnlockMastery()
    {
        crown.SetActive(true);
        masterySprite.color = Color.white;

        Color tempColor = masteryText.color;
        tempColor.a = 1;
        masteryText.color = tempColor;
    }
}
