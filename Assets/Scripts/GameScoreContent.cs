using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreContent : MonoBehaviour {

    public Text totalCountText;
    public Text totalScoreText;
    public Text avgBonusText;
    public Text totalWithBonusText;

    public void Init(int count, float score, int avgBonus = 0, float bonusScore = 0f)
    {
        if (totalCountText != null) totalCountText.text = count.ToString();
        if (totalScoreText != null) totalScoreText.text = Utility.FormatCurrency(score);
        if (avgBonusText != null) avgBonusText.text = "X" + avgBonus.ToString();
        if (totalWithBonusText != null) totalWithBonusText.text = Utility.FormatCurrency(bonusScore);
    }
}
