//used to create grid as slots for gamepieces

using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	//pubilc
	public int xIndex, yIndex;

	//private
	Board m_board;

	public void Init(int x, int y, Board board)
	{
		xIndex = x;
		yIndex = y;
		m_board = board;
	}

	private void OnMouseDown()
	{
		if(m_board != null)
		{
			m_board.SelectStartingTile(this);
		}
	}

	private void OnMouseEnter()
	{
		if (m_board != null)
		{
			m_board.DragSelectionToTile(this);
		}
	}

	private void OnMouseUp()
	{
		if (m_board != null)
		{
			m_board.ReleaseSelection();
		}
	}
}
