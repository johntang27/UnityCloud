using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelGoal : Singleton<LevelGoal>
{
	public int levelStars = 0; //not used
	public int movesLeft = 0; //not used
	public int timeLeft = 0; //not used

	public virtual void ProcessRequirements() { }

	public abstract bool IsCompleted();

	public virtual bool IsGameOver()
	{
		return IsCompleted();
	}
}
