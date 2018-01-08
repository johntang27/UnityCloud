using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPieceTriangle : BonusPiece {

    [Header("Triangle Only Variables")]
    public float perClickCost;
    public List<GameObject> attachedPieces;

    protected override void Start()
    {
        int numOfClicks = attachedPieces.Count;
        if (numOfClicks == 0) numOfClicks = 1;
        perClickCost = BonusGameManager.Instance.triangleCost / numOfClicks;
        base.Start();
    }

    protected override void OnMouseDown()
    {
        if (attachedPieces.Count > 0)
        {
            BonusPieceSquare p = attachedPieces[attachedPieces.Count - 1].GetComponent<BonusPieceSquare>();
            BonusGameManager.Instance.squarePieces.Add(p.gameObject);
            if (BonusGameManager.Instance.currentTapStreak.currentColor == p.pieceColor && BonusGameManager.Instance.currentTapStreak.streak == 1) p.ShowBonus(true, BonusGameManager.Instance.isBonusFading);
            if (BonusGameManager.Instance.currentTapStreak.currentColor == p.pieceColor && BonusGameManager.Instance.currentTapStreak.streak >= 2) p.ShowBonus(false, BonusGameManager.Instance.isBonusFading);
            p.isAttached = false;
            p.transform.parent = null;
            p.MoveAndRotatePiece();
            p.ResetColor();
            attachedPieces.Remove(p.gameObject);
        }
        BonusGameManager.Instance.savingsLeft -= perClickCost;
        UpdateTextMesh("-$" + perClickCost.ToString(), Color.red);

        if(attachedPieces.Count == 0)
            base.OnMouseDown();
    }
}
