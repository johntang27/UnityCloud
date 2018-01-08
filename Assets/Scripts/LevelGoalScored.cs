using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoalScored : LevelGoal
{
	public int[] incomeGoals = new int[3] { 40000, 200000, 300000 };
	public int[] savingsGoals = new int[3] { 300000, 2000000, 3000000 };

	void Start()
	{
		Init();
	}

	void Init()
	{
		for (int i = 1; i < incomeGoals.Length; i++)
		{
			if (incomeGoals[i] < incomeGoals[i - 1])
			{
				Debug.LogWarning("LEVELGOAL: Setup income goals in increasing order!");
			}
		}
		for (int i = 1; i < savingsGoals.Length; i++)
		{
			if (savingsGoals[i] < savingsGoals[i - 1])
			{
				Debug.LogWarning("LEVELGOAL: Setup saving goals in increasing order!");
			}
		}
	}

	public override bool IsCompleted()
	{
		if(ScoreManager.Instance != null)
		{
			if (incomeGoals.Length > 0 && savingsGoals.Length > 0)
			{
				return (ScoreManager.Instance.totalCashOnHand >= incomeGoals[0] || ScoreManager.Instance.totalRetirementSavings >= savingsGoals[0]);
			}
			else if (incomeGoals.Length > 0)
			{
				return (ScoreManager.Instance.totalCashOnHand >= incomeGoals[0]);
			}
			else if (savingsGoals.Length > 0)
			{
				return (ScoreManager.Instance.totalRetirementSavings >= savingsGoals[0]);
			}
		}
		return false;
	}
}
