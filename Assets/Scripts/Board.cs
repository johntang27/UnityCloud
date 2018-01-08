using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {

	//public
	public int width, height;
	public float boardPadding;
    public int requiredMatches = 2;
	public int startingYOffset = 10;
	public float moveTime = 0.5f;
    public Vector3 boardOffset;
	public float shuffleSpeed = 0.5f;
	public int shuffleCount = 5;

    public GameObject tilePrefab;
	public GameObject[] gamePiecePrefab;
    public List<GameObject> instantiatePieces;

    //private
    Tile[,] m_allTiles;
	GamePiece[,] m_allGamePieces;
    List<GamePiece> m_selectedMatches = new List<GamePiece>();
    MatchValue m_selectedMatchValue;
    Tile m_selectedTile;
	public bool playerInputEnabled = false;
    bool m_isSelecting = false;

    public GameObject panelFollowMouse;
    public Vector3 panelFollowMouseOffset;
    FollowMouse m_panelFollowMouseScript;

    bool disableNonIncome = false;
    bool disableNonFood = false;

    int incomePrefabIndex;
    int expensePrefabIndex;

    public bool onlyIncomePiece = false;
    public bool onlyExpensePiece = false;
    int onlyTypePieceCount;

    void Start()
	{
		m_allTiles = new Tile[width, height];
		m_allGamePieces = new GamePiece[width, height];

        panelFollowMouse.SetActive(false);
        m_panelFollowMouseScript = panelFollowMouse.GetComponent<FollowMouse>();
    }

    private void Update()
    {
        if (m_isSelecting)
        {
            panelFollowMouse.transform.position = Input.mousePosition + panelFollowMouseOffset;
        }
    }

    public void SetupBoard(MatchValue[] tileTypes)
	{
        for (int i = 0; i < tileTypes.Length; i++)
        {
            instantiatePieces.Add(gamePiecePrefab[(int)tileTypes[i] - 1]);
            if (tileTypes[i] == MatchValue.Income) incomePrefabIndex = i;
            if (tileTypes[i] == MatchValue.Expense) expensePrefabIndex = i;
        }
		SetupTiles();
		SetupCamera();
		FillBoard(startingYOffset, moveTime);
	}

	void SetupTiles()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
				tile.name = "Tile (" + i + ", " + j + ")";
				Tile tileScript = tile.GetComponent<Tile>();
				m_allTiles[i, j] = tileScript;
				tile.transform.parent = transform;
				tileScript.Init(i, j, this);
			}
		}
	}

	void SetupCamera()
	{
		Camera.main.transform.position = new Vector3((float) (width - 1 + boardOffset.x) / 2.0f, (float) (height - 1 + boardOffset.y) / 2.0f, -10.0f);

		float aspectRatio = (float) Screen.width / (float) Screen.height;
		float vertSize = (float) height / 2.0f + (float) boardPadding;
		float horizSize = ((float) width / 2.0f + (float) boardPadding) / aspectRatio;

		Camera.main.orthographicSize = (vertSize > horizSize) ? vertSize : horizSize;
	}

	GameObject GetRandomGamePiece()
	{
        int ranIndex = Random.Range(0, instantiatePieces.Count);//gamePiecePrefab.Length);

		if (instantiatePieces[ranIndex] == null) { Debug.LogWarning("BOARD: does not contain valid GamePiece prefab at " + ranIndex); }

		return instantiatePieces[ranIndex];
	}

	public void PlaceGamePiece(GamePiece gamePiece, int x, int y)
	{
		if (gamePiece == null) { Debug.LogWarning("BOARD: invalid GamePiece"); return; }

		gamePiece.transform.position = new Vector3(x, y, 0);
		gamePiece.transform.rotation = Quaternion.identity;
		if (InBounds(x, y))
		{
			m_allGamePieces[x, y] = gamePiece;
		}
		gamePiece.SetPos(x, y);
	}

	bool InBounds(int x, int y)
	{
		return (x >= 0 && x < width && y >= 0 && y < height);
	}

	void FillBoard(int startYOffset = 0, float moveTime = 0.1f)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (m_allGamePieces[i, j] == null)
				{
					if (startYOffset == 0)
					{
						FillRandomAt(i, j);
					}
					else
					{
						FillRandomAt(i, j, startYOffset, moveTime);
					}
				}
			}
		}

        onlyIncomePiece = false;
        onlyExpensePiece = false;
        onlyTypePieceCount = 0;
	}

	void FillBoardFromList(List<GamePiece> pieces)
	{
		Queue<GamePiece> unusedPieces = new Queue<GamePiece>(pieces);

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (m_allGamePieces[i, j] == null && m_allTiles[i, j] != null)
				{
					m_allGamePieces[i, j] = unusedPieces.Dequeue();
				}
			}
		}
	}

	void FillRandomAt(int x, int y, int startYOffset = 0, float moveTime = 0.1f)
	{
        //Debug.Log("filling random");
        GameObject piece;

        if (onlyIncomePiece & onlyTypePieceCount < 2)
        {
            piece = Instantiate(instantiatePieces[incomePrefabIndex], Vector3.zero, Quaternion.identity) as GameObject;
            onlyTypePieceCount++;
        }
        else if (onlyExpensePiece & onlyTypePieceCount < 2)
        {
            piece = Instantiate(instantiatePieces[expensePrefabIndex], Vector3.zero, Quaternion.identity) as GameObject;
            onlyTypePieceCount++;
        }
        else
            piece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;

		if (piece != null)
		{
			piece.GetComponent<GamePiece>().Init(this);
			PlaceGamePiece(piece.GetComponent<GamePiece>(), x, y);

			if(startYOffset != 0)
			{
				piece.transform.position = new Vector3(x, y + startYOffset, 0);
				piece.GetComponent<GamePiece>().MoveTo(x, y, moveTime);
			}

			piece.transform.parent = transform;
		}
	}

    public void DisableSelectedPieces(MatchValue matchType)//, bool disableOthers)
    {
        foreach (GamePiece piece in m_allGamePieces)
        {
            if (matchType == MatchValue.Income && piece.matchValue != MatchValue.Income)
            {
                disableNonIncome = true;
                disableNonFood = false;
                piece.ToggleSpriteVisibility(true);
            }
            else if (matchType == MatchValue.Expense && piece.matchValue != MatchValue.Expense)
            {
                disableNonFood = true;
                disableNonIncome = false;
                piece.ToggleSpriteVisibility(true);                
            }
            else if(matchType == MatchValue.None && piece.matchValue != MatchValue.None)
            {
                disableNonFood = false;
                disableNonIncome = false;
                piece.ToggleSpriteVisibility(false);                
            }
        }
    }

    public void EnableAllPieces()
    {
        disableNonFood = false;
        disableNonIncome = false;
        foreach (GamePiece piece in m_allGamePieces)
            piece.ToggleSpriteVisibility(false);
    }

	public void SelectStartingTile(Tile tile)
	{
		if (playerInputEnabled)
		{
			if (m_selectedTile == null)
			{
				GamePiece piece = m_allGamePieces[tile.xIndex, tile.yIndex];

				if (piece != null)
				{
                    if (disableNonIncome && piece.matchValue != MatchValue.Income) return;
                    if (disableNonFood && piece.matchValue != MatchValue.Expense) return;

                    m_selectedTile = tile;
					m_selectedMatchValue = piece.matchValue;
					HighlightTileOn(tile.xIndex, tile.yIndex, piece.GetComponent<SpriteRenderer>().color);

					m_selectedMatches.Add(piece);

                    panelFollowMouse.SetActive(true);
                    m_isSelecting = true;
                    m_panelFollowMouseScript.SetFollowPanel(piece);

                    //piece.ShowPoints();
                }
            }
			//Debug.Log("SELECTED/CLICKED tile: " + tile.name);
		}
    }

	public void DragSelectionToTile(Tile tile)
	{
        if (m_allGamePieces[tile.xIndex, tile.yIndex] != null)
        {
            //deselect
            if (m_selectedTile != null && m_selectedTile != tile && m_selectedMatches.Contains(m_allGamePieces[tile.xIndex, tile.yIndex]) && IsNextTo(tile, m_selectedTile))
            {
                GamePiece piece = m_allGamePieces[m_selectedTile.xIndex, m_selectedTile.yIndex];

                HighlightTileOff(m_selectedTile.xIndex, m_selectedTile.yIndex);
                m_panelFollowMouseScript.UpdateTotal(GameManager.Instance.GetPoints(piece), -1);
                m_selectedMatches.Remove(m_allGamePieces[m_selectedTile.xIndex, m_selectedTile.yIndex]);

                m_selectedTile = tile;
                //Debug.Log("remove DRAGGED tile: " + tile.name);
            }
            //select matching pair
            else if (m_selectedTile != null && IsNextTo(tile, m_selectedTile) && m_allGamePieces[tile.xIndex, tile.yIndex].matchValue == m_selectedMatchValue) //add is a match
            {
                GamePiece piece = m_allGamePieces[tile.xIndex, tile.yIndex];

                if (piece != null)
                {
                    m_selectedTile = tile;
                    HighlightTileOn(tile.xIndex, tile.yIndex, piece.GetComponent<SpriteRenderer>().color);
                    m_panelFollowMouseScript.UpdateTotal(GameManager.Instance.GetPoints(piece), 1);

                    m_selectedMatches.Add(piece);
                    //Debug.Log("added DRAGGED tile: " + tile.name);
                }
            }
            //Debug.Log("DRAGGED over tile: " + tile.name);
        }
    }

	public void ReleaseSelection()
	{
        MatchTiles(m_selectedMatches);

        m_selectedTile = null;
        m_selectedMatches.Clear();
        m_selectedMatchValue = MatchValue.None;
        //Debug.Log("RELEASED mouse.");

        m_panelFollowMouseScript.Reset();
        panelFollowMouse.SetActive(false);
        m_isSelecting = false;
    }

    void MatchTiles(List<GamePiece> selectedMatches)
    {
		foreach (GamePiece piece in selectedMatches)
		{
			HighlightTileOff(piece.xIndex, piece.yIndex);
		}

		if (selectedMatches.Count >= requiredMatches)
		{
			//match, score and clear
			//Debug.Log("MATCHES made. Collecting points!");

			ClearAndRefillBoard(selectedMatches);
		}
	}

	bool IsNextTo(Tile start, Tile end)
	{
		if (Mathf.Abs(start.xIndex - end.xIndex) == 1 && start.yIndex == end.yIndex) { return true; }
		if (Mathf.Abs(start.yIndex - end.yIndex) == 1 && start.xIndex == end.xIndex) { return true; }

		return false;
	}

    void HighlightTileOn(int x, int y, Color col)
    {
        SpriteRenderer spriteRenderer = m_allTiles[x, y].GetComponent<SpriteRenderer>();
        spriteRenderer.color = col;
    }

    void HighlightTileOff(int x, int y)
    {
        SpriteRenderer spriteRenderer = m_allTiles[x, y].GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
    }

    void ClearPiecesAt(int x, int y)
    {
        GamePiece piece = m_allGamePieces[x, y];
        if(piece != null)
        {
            m_allGamePieces[x, y] = null;
            Destroy(piece.gameObject);
        }
    }

    void ClearPiecesAt(List<GamePiece> gamePieces)
    {
        int counter = 0;
        GamePiece matchedPiece = null;

        foreach (GamePiece piece in gamePieces)
        {
			if (piece != null)
			{
				ClearPiecesAt(piece.xIndex, piece.yIndex);
				GameManager.Instance.ScorePoints(piece);
                matchedPiece = piece;
                counter++;
            }
		}

        ScoreManager.Instance.UpdatePointsPopup(matchedPiece, counter);
    }

    List<GamePiece> CollapseColumn(int col, float collapseTime = 0.1f)
    {
        List<GamePiece> movingPieces = new List<GamePiece>();

        for(int i = 0; i < height -1; i++)
        {
            if (m_allGamePieces[col, i] == null)
            {
                for (int j = i+1; j < height; j++)
                {
                    if(m_allGamePieces[col, j] != null)
                    {
                        m_allGamePieces[col, j].MoveTo(col, i, collapseTime * (j-i));
                        m_allGamePieces[col, i] = m_allGamePieces[col, j];
                        m_allGamePieces[col, i].SetPos(col, i);

                        if (!movingPieces.Contains(m_allGamePieces[col,i]))
                        {
                            movingPieces.Add(m_allGamePieces[col, i]);
                        }

                        m_allGamePieces[col, j] = null;
                        break;
                    }
                }
            }
        }

        return movingPieces;
    }

    List<GamePiece> CollapseColumn(List<GamePiece> gamePieces)
    {
        List<GamePiece> movingPieces = new List<GamePiece>();
        List<int> columnsToCollapse = GetColumns(gamePieces);

        foreach(int col in columnsToCollapse)
        {
            movingPieces = movingPieces.Union(CollapseColumn(col)).ToList();
        }

        return movingPieces;
    }

	List<GamePiece> CollapseColumn(List<int> columnsToCollapse)
	{
		List<GamePiece> movingPieces = new List<GamePiece>();
		foreach (int col in columnsToCollapse)
		{
			movingPieces = movingPieces.Union(CollapseColumn(col)).ToList();
		}

		return movingPieces;
	}

	// checks if the GamePieces have reached their destination positions on collapse
	bool IsCollapsed(List<GamePiece> gamePieces)
	{
		foreach (GamePiece piece in gamePieces)
		{
			if (piece != null)
			{
				if (piece.transform.position.y - (float)piece.yIndex > 0.001f)
				{
					return false;
				}

				if (piece.transform.position.x - (float)piece.xIndex > 0.001f)
				{
					return false;
				}
			}
		}
		return true;
	}

	List<int> GetColumns(List<GamePiece> gamePieces)
    {
        List<int> columns = new List<int>();

        foreach(GamePiece piece in gamePieces)
        {
			if (piece != null)
			{
				if (!columns.Contains(piece.xIndex))
				{
					columns.Add(piece.xIndex);
				}
			}
        }
        return columns;
    }

	void ClearAndRefillBoard(List<GamePiece> gamePieces)
	{
		StartCoroutine(ClearAndRefillBoardRoutine(gamePieces));
	}


	IEnumerator ClearAndRefillBoardRoutine(List<GamePiece> gamePieces)
    {
		playerInputEnabled = false;

		List<GamePiece> movingPieces = new List<GamePiece>();
		List<int> columnsToCollaspe = GetColumns(gamePieces);

		ClearPiecesAt(gamePieces);
		yield return new WaitForSeconds(0.2f);

		movingPieces = CollapseColumn(columnsToCollaspe);
		while(!IsCollapsed(movingPieces))
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.2f);

		FillBoard(startingYOffset, moveTime);
		//yield return new WaitForSeconds(moveTime);

		if (IsBoardDeadlocked())
		{
			Debug.Log("=== Board DEADLOCKED ===");
			ShuffleBoard();
		}
		else
		{
			//Debug.Log("Board match found. Keep playing.");
			playerInputEnabled = true;
		}
	}

	bool IsBoardDeadlocked()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (m_allGamePieces[i, j] == null) { continue; }

				if (i + 1 < width && m_allGamePieces[i + 1, j] != null)
				{
					if (m_allGamePieces[i, j].matchValue == m_allGamePieces[i + 1, j].matchValue)
					{
						return false; 
					}
				}
				if (j + 1 < height && m_allGamePieces[i, j + 1] != null)
				{
					if (m_allGamePieces[i, j].matchValue == m_allGamePieces[i, j + 1].matchValue)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public void TestBoardButton()
	{
		if (IsBoardDeadlocked())
		{
			Debug.Log("=== Board DEADLOCKED ===");
		}
		else
		{
			Debug.Log("Board match found. Keep playing.");
		}
	}

	public void ShuffleBoard()
	{
		GameManager.Instance.isPaused = true;
		StartCoroutine(ExecuteShuffle());
	}

	IEnumerator ExecuteShuffle()
	{
		playerInputEnabled = false;

		GameManager.Instance.messageText.text = "No Matches!";
		yield return new WaitForSeconds(1.0f);
		GameManager.Instance.messageText.text = "Shuffling!";

		for (int i = 1; i <= shuffleCount; i++)
		{
			yield return StartCoroutine(ShuffleBoardRoutine());
		}

		yield return new WaitForSeconds(shuffleSpeed);

		GameManager.Instance.messageText.text = "";
		playerInputEnabled = true;
		GameManager.Instance.isPaused = false;
	}

	IEnumerator ShuffleBoardRoutine()
	{
		List<GamePiece> allPieces = new List<GamePiece>();
		foreach(GamePiece piece in m_allGamePieces)
		{
			allPieces.Add(piece);
		}

		while(!IsCollapsed(allPieces))
		{
			yield return null;
		}

		List<GamePiece> shufflePieces = RemoveNormalPieces();
		ShuffleGamePieces(shufflePieces);
		FillBoardFromList(shufflePieces);
		MoveShufflePieces();

		yield return null;
	}

	List<GamePiece> RemoveNormalPieces()
	{
		List<GamePiece> normalPieces = new List<GamePiece>();

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if(m_allGamePieces[i, j] != null)
				{
					normalPieces.Add(m_allGamePieces[i, j]);
					m_allGamePieces[i, j] = null;
				}
			}
		}

		return normalPieces;
	}

	void ShuffleGamePieces(List<GamePiece> pieces)
	{
		int maxCount = pieces.Count();
		int ran = 0;

		for (int i=0; i < maxCount - 1; i++)
		{
			ran = Random.Range(i, maxCount);
			if(ran == i) { continue; }

			GamePiece temp = pieces[ran];
			pieces[ran] = pieces[i];
			pieces[i] = temp;
		}
	}

	void MoveShufflePieces()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (m_allGamePieces[i, j] != null)
				{
					m_allGamePieces[i, j].MoveTo(i, j, shuffleSpeed);
				}
			}
		}
	}

} //end
