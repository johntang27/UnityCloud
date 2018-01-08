using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoalRetirementReached : LevelGoal
{
	public int retirementAgeGoal;

	float m_gameTimer;
	float m_timeToAgeUp;
	float m_secondsToAgeUp;
	float m_bonusSecondsAdded;

	void Start()
	{
		//m_timeToAgeUp = GameManager.Instance.timeToAgeUp;

		if (retirementAgeGoal == 0)
		{
            retirementAgeGoal = PlayerData.Instance.currentLevelData.retirementAge;//GameManager.Instance.retirementAge;
		}
	}

	public override void ProcessRequirements()
	{
		Debug.Log("ProcessRequirements()");

		//TODO: based on level data
		//if (!GameManager.Instance.isTurnBased)
		//{
		//	m_gameTimer = GameManager.Instance.gameTimer;
		//	m_secondsToAgeUp = GameManager.Instance.secondsToAgeUp;
		//	m_bonusSecondsAdded = GameManager.Instance.bonusSecondsAdded;

		//	m_timeToAgeUp = m_secondsToAgeUp + m_bonusSecondsAdded;

		//	if (m_gameTimer >= (m_secondsToAgeUp + m_bonusSecondsAdded))
		//	{
		//		ScoreManager.Instance.AddAge(1);
		//		m_gameTimer = 0f;
		//		m_bonusSecondsAdded = 0f;
		//	}
		//}
	}

	public override bool IsCompleted()
	{
		//Debug.Log(GameManager.Instance.currentAge >= retirementAgeGoal);
		return (GameManager.Instance.currentAge >= retirementAgeGoal);
	}
}
