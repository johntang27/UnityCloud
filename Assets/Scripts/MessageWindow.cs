using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour {

    public Image messageIcon;
    public Text messageText;
    public Text buttonText;
    public Toggle turnBasedToggle;
    public InputField inputField;
    public Dropdown dropDown;
    public Toggle bgScrollToggle;

    public void ShowMessage(Sprite sprite = null, string message = "", string buttonMsg = "")
    {
        if(messageIcon != null)
        {
            messageIcon.sprite = sprite;
        }
        if (messageText != null)
        {
            messageText.text = message;
        }
        if (buttonText != null)
        {
            buttonText.text = buttonMsg;
        }
    }

    public void ToggleTurnBased()
    {
        GameManager.Instance.isTurnBased = turnBasedToggle.isOn;
    }

    public void InputSubmit()
    {
        int yearValue = 0;
        int.TryParse(inputField.text, out yearValue);
        ScoreManager.Instance.yearPerMatch = yearValue;
    }

    public void SelectMatches()
    {
        ScoreManager.Instance.matchPerYear = (MatchPerYear)(dropDown.value + 1);
    }

    //public void ToggleBackground()
    //{
    //    GameManager.Instance.isScrollingBG = bgScrollToggle.isOn;
    //}
}
