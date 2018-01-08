using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroLoginManager : Singleton<IntroLoginManager> {

    public Text messageLabel;
    public GameObject incorrectLogins;
    public InputField userNameField;
    public InputField passwordField;
    public Button goButton;

    bool userNameCorrect = false;
    bool passwordCorrect = false;

    public void LoginOnSubmit()
    {
        if(PlayerPrefs.GetInt("hasAccount") == 1)
        {
            if (PlayerPrefs.GetString("un") == userNameField.text)
                userNameCorrect = true;
            if (PlayerPrefs.GetString("pw") == passwordField.text)
                passwordCorrect = true;
            else
                incorrectLogins.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetString("un", userNameField.text); userNameCorrect = true;
            PlayerPrefs.SetString("pw", passwordField.text); passwordCorrect = true;
        }

        if (userNameCorrect && passwordCorrect && userNameField.text != "" && passwordField.text != "")
        {
            goButton.interactable = true;
            incorrectLogins.SetActive(false);
        }
        else
            goButton.interactable = false;
    }

    public void ShowLogin()
    {
        if (PlayerPrefs.GetInt("hasAccount") == 1)
            messageLabel.text = "Has Account, please login";
        else
            messageLabel.text = "No Account, please create";
    }

    public void GoButtonClicked()
    {
        if (!PlayerPrefs.HasKey("hasAccount"))
            PlayerPrefs.SetInt("hasAccount", 1);

        SceneManager.LoadScene("AvatarCreation");
    }

    public void ClearAccount()
    {
        PlayerPrefs.DeleteAll();
        messageLabel.text = "No Account, please create";
        goButton.interactable = false;
        userNameCorrect = false;
        passwordCorrect = false;
    }
}
