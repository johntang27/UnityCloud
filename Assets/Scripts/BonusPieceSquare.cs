using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPieceSquare : BonusPiece {

    [Header("Square Only Variables")]
    public TextMesh x2Bonus;
    public TextMesh x3Bonus;
    public SpriteRenderer icon;
    public SpriteRenderer blankBackground;
    Color fadeColor = Color.white;
    Color bgColor;
    Color bgStartingColor;

    public float bonusFadeDuration;
    public float bonusFadeCounter;

    protected override void Start()
    {
        bgColor = bgStartingColor = blankBackground.color;
        base.Start();
    }

    protected override void OnMouseDown()
    {
        if (isAttached) return;

        BonusGameManager.Instance.SquarePieceTapped(pieceColor, BonusGameManager.Instance.isBonusFading);
        UpdateTextMesh("+" + BonusGameManager.Instance.squareScore.ToString() + "!", Color.white);

        base.OnMouseDown();
    }

    IEnumerator HandleBonus(bool isX2)
    {
        while (bonusFadeCounter < 1)
        {
            BonusGameManager.Instance.isBonusFading = true;
            bonusFadeCounter += Time.deltaTime / bonusFadeDuration;
            fadeColor.a = Mathf.Lerp(1, 0, bonusFadeCounter);
            bgColor.a = Mathf.Lerp(1, 0, bonusFadeCounter);
            if (isX2) x2Bonus.color = fadeColor;
            else x3Bonus.color = fadeColor;
            blankBackground.color = bgColor;
            yield return null;
        }

        BonusGameManager.Instance.isBonusFading = false; //make sure the game manager value is also reset, so new piece won't show the bonus
        bonusFadeCounter = 0;
        x2Bonus.color = Color.white;
        x3Bonus.color = Color.white;
        blankBackground.color = bgStartingColor;
        blankBackground.gameObject.SetActive(false);
        x2Bonus.gameObject.SetActive(false);
        x3Bonus.gameObject.SetActive(false);
        BonusGameManager.Instance.currentTapStreak.currentColor = PieceColor.None;
        BonusGameManager.Instance.currentTapStreak.streak = 1;
    }


    public void ShowBonus(bool isX2, bool alreadyShowingBonus)
    {
        if (alreadyShowingBonus) bonusFadeCounter = 0;

        x2Bonus.gameObject.SetActive(isX2);
        x3Bonus.gameObject.SetActive(!isX2);
        blankBackground.gameObject.SetActive(true);

        if (isX2) x2Bonus.color = Color.white;
        else x3Bonus.color = Color.white;

        fadeColor = Color.white;
        bgColor = bgStartingColor;

        StartCoroutine(HandleBonus(isX2));
    }

    public void ResetColor()
    {
        icon.color = Color.white;

        bgStartingColor.a = 1;
        blankBackground.color = bgColor = bgStartingColor;
    }
}
