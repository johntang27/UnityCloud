using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour {

    public Text nameText;
    //public Text countText;
    public Text totalText;
    //public Text operatorText;
    public Text bonusValueText;
    public Text bonusText;

    string m_tileName = "";
    int m_tileCount = 0;
    //string m_totalsText = "";

    float m_cashOnHandTotal = 0f;
    //float m_secondsToAgeTotal = 0f;
    float m_happinessIncrease = 0f;
    float m_retirementSavingTotal = 0f;
    float m_retirementMultiplier = 0f;
    float m_studentLoan = 0f;

    public Color incomeColor;
    public Color expenseColor;
    public Color savingsColor;
    public Color mutualFundColor;
    public Color annuityColor;
    public Color tiaaColor;
    public Color loanColor;
    public Color negativeColor;

    GamePiece m_currentPiece;
    //string m_prefix = "$";

    MaskableGraphic m_graphic;

    void Start()
    {
        m_graphic = GetComponent<MaskableGraphic>();

        Reset();
    }

    public void SetFollowPanel(GamePiece piece)
    {
        //m_tileName = piece.pieceName;
        m_tileCount = 1;
        SetPointTotals(GameManager.Instance.GetPoints(piece), 1);
        m_currentPiece = piece;

        Color pieceColor = piece.GetComponent<SpriteRenderer>().color;
        m_graphic.color = new Color(pieceColor.r, pieceColor.g, pieceColor.b, m_graphic.color.a);

        DisplayTotal();
    }

    void SetPointTotals(float[] points, int inc=1)
    {
        for(int i=0; i < points.Length; i++ )
        {
            switch(i)
            {
                case 0:
                    m_cashOnHandTotal += points[i] * inc;
                    break;
                case 1:
                    //m_secondsToAgeTotal += points[i] * inc;
                    m_happinessIncrease += points[i] * inc;
                    //m_happinessIncrease = Mathf.Min(m_happinessIncrease, 6);
                    break;
                case 2:
                    m_retirementSavingTotal += points[i] * inc;
                    break;
                case 3:
                    m_retirementMultiplier = points[i];
                    break;
                case 4:
                    m_studentLoan += points[i] * inc;
                    break;

            }
            //Debug.Log("update total count: " + inc + " points[" + i + "]: " + points[i]);
        }

        //Debug.Log("m_cashOnHandTotal = " + m_cashOnHandTotal);
        //Debug.Log("m_secondsToAgeTotal = " + m_secondsToAgeTotal);
        //Debug.Log("m_retirementSavingTotal = " + m_retirementSavingTotal);
        //Debug.Log("m_retirementMultiplier = " + m_retirementMultiplier);
    }

    public void UpdateTotal(float[] piecePoints, int inc)
    {
        //Debug.Log("update total count: " + inc + " total: " + total);
        m_tileCount += inc;
        SetPointTotals(piecePoints, inc);

        DisplayTotal();
    }

    public void Reset()
    {
        m_tileName = "";
        m_tileCount = 0;
        //m_totalsText = "";
        m_cashOnHandTotal = 0f;
        //m_secondsToAgeTotal = 0f;
        m_happinessIncrease = 0f;
        m_retirementSavingTotal = 0f;
        m_retirementMultiplier = 0f;

        nameText.text = "";
        //operatorText.text = "";
        totalText.text = "";
        bonusValueText.text = "";
        bonusText.text = "";
    }

    void DisplayTotal()
    {
        //nameText.text = m_tileName;

        switch (m_currentPiece.matchValue)
        {
            case MatchValue.Income:
                //operatorText.text = "+";
                nameText.text = "INCOME";
                nameText.color = incomeColor;
                totalText.text = "+" + Utility.FormatCurrency(m_cashOnHandTotal);
                totalText.color = incomeColor;
                break;
            case MatchValue.Expense:
                //operatorText.text = Utility.FormatCurrency(m_cashOnHandTotal);
                nameText.text = "EXPENSE";
                nameText.color = expenseColor;
                totalText.text = Utility.FormatCurrency(m_cashOnHandTotal);
                totalText.color = negativeColor;
                bonusValueText.text = "+" + Mathf.Min(Mathf.Floor(m_happinessIncrease), 6);
                bonusValueText.color = expenseColor;
                bonusText.text = "HAPPINESS BONUS";
                bonusText.color = expenseColor;
                break;
            case MatchValue.SavingsAccount:
                nameText.text = "SAVINGS";
                nameText.color = savingsColor;
                totalText.text = "+" + Utility.FormatCurrency(m_retirementSavingTotal);
                totalText.color = savingsColor;
                bonusValueText.text = "x " + m_retirementMultiplier.ToString("f0");
                bonusValueText.color = savingsColor;
                bonusText.text = "Multiplier";
                bonusText.color = savingsColor;
                break;
            case MatchValue.Annuity:
                nameText.text = "ANNUITY";
                nameText.color = annuityColor;
                totalText.text = "+" + Utility.FormatCurrency(m_retirementSavingTotal);
                totalText.color = savingsColor;
                bonusValueText.text = "x " + m_retirementMultiplier.ToString("f0");
                bonusValueText.color = annuityColor;
                bonusText.text = "Multiplier";
                bonusText.color = annuityColor;
                break;
            case MatchValue.AnnuityTIAA:
                nameText.text = "ANNUITY TIAA";
                nameText.color = tiaaColor;
                totalText.text = "+" + Utility.FormatCurrency(m_retirementSavingTotal);
                totalText.color = savingsColor;
                bonusValueText.text = "x " + m_retirementMultiplier.ToString("f0");
                bonusValueText.color = tiaaColor;
                bonusText.text = "Multiplier";
                bonusText.color = tiaaColor;
                break;
            case MatchValue.MutualFund:
                //operatorText.text = Utility.FormatCurrency(m_retirementSavingTotal);
                //totalText.text = "x" + m_retirementMultiplier.ToString("f0");
                //bonusText.text = "age bonus";
                //break;
                nameText.text = "MUTUAL FUND";
                nameText.color = mutualFundColor;
                totalText.text = "+" + Utility.FormatCurrency(m_retirementSavingTotal);
                totalText.color = savingsColor;
                bonusValueText.text = "x " + m_retirementMultiplier.ToString("f0");
                bonusValueText.color = mutualFundColor;
                bonusText.text = "Multiplier";
                bonusText.color = mutualFundColor;
                break;
            case MatchValue.StudentLoan:
                nameText.text = "LOAN PAYMENT";
                nameText.color = loanColor;
                totalText.text = Utility.FormatCurrency(m_studentLoan);
                totalText.color = loanColor;
                break;
            default:
                Debug.LogWarning("Missing matchType.");
                break;
        }
    }

}
