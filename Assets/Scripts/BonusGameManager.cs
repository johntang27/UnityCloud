using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]
public class TapStreak
{
    public PieceColor currentColor = PieceColor.None;
    public int streak = 0;
}

public class BonusGameManager : Singleton<BonusGameManager> {

    public AvatarWalkAnimation avatar;

    public GameObject[] bonusPiecePrefabs;

    public float[] xRangeSpawn = new float[2];
    public float destroyLimit;

    public float delayToStart = 1.0f;

    public RectXformMover[] m_sceneLoops;
    
    public bool m_isReadyToBegin = false;
    public bool didGameStart = false;
    public bool isGameOver = false;

    public float savingsLeft;
    public float yearlyCostOfRetirement;
    public float triangleCost;

    public TapStreak currentTapStreak;

    public List<GameObject> squarePieces;

    public float spawnDelay;
    float spawnTimerCounter;

    public float ageProgressionDelay;
    float ageTimerCounter;

    public bool isBonusFading = false;

    public int currentScore;
    public int squareScore;

    // Use this for initialization
    void Start ()
    {
        m_sceneLoops = GameObject.FindObjectsOfType<RectXformMover>();
        savingsLeft = PlayerData.Instance.savedAmount;

        yearlyCostOfRetirement = PlayerData.Instance.savedAmount / (PlayerData.Instance.endRetirementAge - PlayerData.Instance.retirementAge);
        triangleCost = yearlyCostOfRetirement / 5;

        StartCoroutine(ExecuteGameLoop());
    }

    public void BeginGame()
    {
        didGameStart = true;
        avatar.PlayAnimation();
    }

    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");
        yield return StartCoroutine("EndGameRoutine");
    }

    IEnumerator StartGameRoutine()
    {
        while (!m_isReadyToBegin)
        {
            yield return null;
            yield return new WaitForSeconds(delayToStart);
            m_isReadyToBegin = true;
        }

        BonusGameUIManager.Instance.ScreenFaderChange(false);

        BonusGameUIManager.Instance.ToggleMessageWindow(true);

        while (!didGameStart)
        {
            yield return null;
        }
    }

    IEnumerator PlayGameRoutine()
    {
        BonusGameUIManager.Instance.ToggleMessageWindow(false);

        foreach (RectXformMover scene in m_sceneLoops)
        {
            scene.StartMove();
        }

        while (!isGameOver)
        {
            spawnTimerCounter += Time.deltaTime;
            if (spawnTimerCounter >= spawnDelay && BonusGameUIManager.Instance.isAfterOneYear())
            {
                SpawnGamePiece();
                spawnTimerCounter = 0;
            }

            ageTimerCounter += Time.deltaTime;
            if (ageTimerCounter >= ageProgressionDelay)
            {
                ageTimerCounter = 0;
            }
            BonusGameUIManager.Instance.ScoreUpdate(100);

            BonusGameUIManager.Instance.UpdateSavingLabel(yearlyCostOfRetirement);
            BonusGameUIManager.Instance.UpdateAgeLabel();

            isGameOver = BonusGameUIManager.Instance.isGameOver();

            yield return null;
        }
    }

    IEnumerator EndGameRoutine()
    {
        BonusGameUIManager.Instance.ScreenFaderChange(true);

        BonusGameUIManager.Instance.gameOverPanel.SetActive(true);
        BonusGameUIManager.Instance.UpdateFinalScore();

        yield return null;
    }

    public void SquarePieceTapped(PieceColor pColor, bool showingBonus)
    {
        if (currentTapStreak.currentColor == pColor)
        {
            if(currentTapStreak.streak < 3) currentTapStreak.streak++;
            if (currentTapStreak.streak >= 2 && showingBonus) //currently still showing the x2 bonus
            {
                foreach (GameObject bp in squarePieces)
                    if (bp.GetComponent<BonusPiece>().pieceColor == pColor) bp.GetComponent<BonusPieceSquare>().ShowBonus(false, showingBonus); //show all the x3 bonus
            }
            else //show the x2 bonus instead, player was too slow
            {
                currentTapStreak.currentColor = pColor;
                currentTapStreak.streak = 1;

                foreach (GameObject bp in squarePieces)
                    if (bp.GetComponent<BonusPiece>().pieceColor == pColor) bp.GetComponent<BonusPieceSquare>().ShowBonus(true, showingBonus);
            }
        }
        else
        {
            currentTapStreak.currentColor = pColor;
            currentTapStreak.streak = 1;

            foreach (GameObject bp in squarePieces)
                if (bp.GetComponent<BonusPiece>().pieceColor == pColor) bp.GetComponent<BonusPieceSquare>().ShowBonus(true, showingBonus);
        }

        switch(currentTapStreak.streak)
        {
            case 1: squareScore = 10;
                break;
            case 2: squareScore = 20;
                break;
            case 3: squareScore = 30;
                break;
        }

        currentScore += squareScore;
    }

    public void ToMapScene()
    {
        if (PlayerData.Instance.playerProgression < 14) PlayerData.Instance.playerProgression++;
        SceneManager.LoadScene("Map");
    }

    void SpawnGamePiece()
    {
        float x = UnityEngine.Random.Range(xRangeSpawn[0], xRangeSpawn[1]);
        GameObject piece = Instantiate(bonusPiecePrefabs[UnityEngine.Random.Range(0, bonusPiecePrefabs.Length)], new Vector3(x, 1, 0), Quaternion.identity);
        float scale = UnityEngine.Random.Range(0.5f, 1f);
        piece.transform.localScale = new Vector3(scale, scale, 1);

        if (piece.GetComponent<BonusPiece>().pieceType == BonusGamePieceType.Square)
        {
            squarePieces.Add(piece);
            if (piece.GetComponent<BonusPiece>().pieceColor == currentTapStreak.currentColor && currentTapStreak.streak == 1) piece.GetComponent<BonusPieceSquare>().ShowBonus(true, isBonusFading);
            if (piece.GetComponent<BonusPiece>().pieceColor == currentTapStreak.currentColor && currentTapStreak.streak >= 1) piece.GetComponent<BonusPieceSquare>().ShowBonus(false, isBonusFading);
        }
    }

    //void AgeProgression()
    //{
    //    if (BonusGameUIManager.Instance.ageSlider.value < BonusGameUIManager.Instance.ageSlider.maxValue)
    //    {
    //        BonusGameUIManager.Instance.UpdateSavingLabel(yearlyCostOfRetirement);
    //        BonusGameUIManager.Instance.ScoreUpdate(100);
    //    }
    //}
}
