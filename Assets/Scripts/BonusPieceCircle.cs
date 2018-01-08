using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPieceCircle : BonusPiece {

    [Header("Circle Only Variables")]
    public float annuityAmt;

    protected override void OnMouseDown()
    {
        BonusGameManager.Instance.savingsLeft += annuityAmt;
        UpdateTextMesh("+$" + annuityAmt.ToString(), Color.green);

        base.OnMouseDown();
    }
}
