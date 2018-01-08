using UnityEngine;
using System.Collections;

public enum MatchValue
{
    None,
    Expense,
    Income,
    Annuity,
    MutualFund,
    AnnuityTIAA,
    StudentLoan,
    SavingsAccount,
    //Red,
};

public enum GamePieceType
{
	None, CashOnHand, SecondsToAge, RetirementSavings, StudentLoan
};

public class GamePiece : MonoBehaviour {

    public MatchValue matchValue;
    public int xIndex, yIndex;
    public string pieceName = "";

    public float cashOnHand = 0;
    public float secondsToAge = 0.0f;
    public float retirementSavings = 0;
    public float interestRate = 0;
    public float happinessIncrease = 0;
    public float studentLoan = 0;

    public SpriteRenderer sprite;

    Board m_board;
	bool m_isMoving = false;

	public void Init(Board board)
	{
		m_board = board;
        sprite = this.GetComponent<SpriteRenderer>() as SpriteRenderer;
        if(matchValue == MatchValue.Income) ScoreManager.Instance.incomeTileValue = cashOnHand;
    }

	public void SetPos(int x, int y)
	{
		xIndex = x;
		yIndex = y;
		this.name = "GamePiece (" + x + ", " + y + ")";
	}

	public void MoveTo(int destX, int destY, float moveTime)
	{
		if (!m_isMoving)
		{
			StartCoroutine(MoveToRoutine(new Vector3(destX, destY, 0), moveTime));
		}
	}

	IEnumerator MoveToRoutine(Vector3 dest, float timeToMove)
	{
		Vector3 startPos = transform.position;

		bool reachedPos = false;
		float elapsedTime = 0f;
		m_isMoving = true;

		while (!reachedPos)
		{ 
			if(Vector3.Distance(transform.position, dest) < 0.01f) //threshold for move to pos
			{
				reachedPos = true;

				if (m_board != null)
				{
					m_board.PlaceGamePiece(this, (int)dest.x, (int) dest.y);
				}
				break;
			}

			elapsedTime += Time.deltaTime;
			float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
			t = t * t * t * (t * (t * 6 - 15) + 10); //smoother-step interp

			transform.position = Vector3.Lerp(startPos, dest, t);

			yield return null;
		}

		m_isMoving = false;
	}

    public void ToggleSpriteVisibility(bool showDisabledState)
    {
        Color tempColor = sprite.color;

        if (showDisabledState) 
            tempColor.a = 0.5f;
        else
            tempColor.a = 1f;
        
        sprite.color = tempColor;
    }
}
