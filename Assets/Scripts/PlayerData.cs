using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AvatarData
{
    public int hairSprite;
    public int faceSprite;
    public int outfitSprite;
    public int bodySprite;
    public Color hairColor;
    public Color outfitColor;
    public Color bodyColor;
}

public class PlayerData : Singleton<PlayerData> {

    public string playerName;
    public AvatarData playerAvatar;
    public int playerProgression;
    public float savedAmount;
    public int retirementAge;
    public int endRetirementAge;
    public int totalScore;
    public LevelData currentLevelData;
    public Dictionary<string, int> levelScores = new Dictionary<string, int>();
    public float[] scoreMultipliers = new float[3];
    public List<BadgeMastery> unlockedMasteries = new List<BadgeMastery>();

    // Use this for initialization
	void Start () {
		
	}
	
	public void UpdateScore(int score)
    {
        string levelID = currentLevelData.levelID.ToString();
        if (!levelScores.ContainsKey(levelID)) levelScores.Add(levelID, score);
        else
        {
            int currentScore = 0;
            levelScores.TryGetValue(levelID, out currentScore);
            if (currentScore < score) levelScores[levelID] = score;
        }
    }

    public void CalculateTotalScore()
    {
        totalScore = 0;
        if(levelScores.Count > 0)
        {
            foreach (KeyValuePair<string, int> pair in levelScores)
                totalScore += pair.Value;
        }
    }

    public int GetScoreByLevelID(string levelID)
    {
        int result = 0;

        levelScores.TryGetValue(levelID, out result);

        return result;
    }

    public void AddUnlockedMastery()
    {
        unlockedMasteries.Add(currentLevelData.unlockingMastery);
    }
}
