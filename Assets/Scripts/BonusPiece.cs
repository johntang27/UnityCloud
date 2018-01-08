using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusGamePieceType
{
    None,
    Square,
    Triangle,
    Circle
}

public enum PieceColor
{
    None,
    Blue,
    Green,
    Orange,
    Maroon
}

public class BonusPiece : MonoBehaviour {

    public BonusGamePieceType pieceType;
    public PieceColor pieceColor;
    public int rotationSpeed;
    public int moveSpeed;

    public bool isAttached;

    public TextMesh effectTextMesh;

    protected virtual void Start()
    {
        StartCoroutine(MoveAndRotate());
    }

    public void MoveAndRotatePiece()
    {
        StartCoroutine(MoveAndRotate());
    }

    IEnumerator MoveAndRotate()
    {
        if (isAttached) yield break;

        while (transform.position.y > BonusGameManager.Instance.destroyLimit)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * rotationSpeed);
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed, Space.World);
            yield return null;
        }

        RemoveFromGame();
    }

    protected virtual void OnMouseDown()
    {
        RemoveFromGame();
    }

    protected void UpdateTextMesh(string labelText, Color textColor)
    {
        TextMesh tMesh = Instantiate(effectTextMesh, this.transform.position, Quaternion.identity);
        tMesh.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        tMesh.text = labelText;
        tMesh.color = textColor;
    }
	
    void RemoveFromGame()
    {
        if (pieceType == BonusGamePieceType.Square) BonusGameManager.Instance.squarePieces.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
