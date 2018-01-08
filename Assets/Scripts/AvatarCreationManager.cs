using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvatarCreationManager : Singleton<AvatarCreationManager> {

    public AvatarData[] presetAvatars;
    public int totalHairs;
    public int totalFaces;
    public int totalOutfits;
    public Image hairSprite;
    public Image faceSprite;
    public Image outfiteSprite;
    public Image bodySprite;
    public InputField nameInput;
    public Button startGameButton;
    public Color[] hairColors;
    public Color[] bodyColors;
    public Image hairColorButton;
    public Image bodyColorButton;
    public int curHairColorIndex;
    public int curBodyColorIndex;
    public int curHairIndex;
    public int curFaceIndex;
    public int curOutfitIndex;
    public int currentPresetAvatar;

	// Use this for initialization
	void Start () {
        //randomize the avatar sprites, and se the current hair and body tint index
        UpdateAvatarSpritesByPreset(0);
	}

    public void ChangeHairColor()
    {     
        if (curHairColorIndex < hairColors.Length - 1) curHairColorIndex++;
        else curHairColorIndex = 0;

        hairSprite.color = hairColors[curHairColorIndex];
        hairColorButton.color = hairColors[curHairColorIndex];
    }

    public void ChangeBodyColor()
    {
        if (curBodyColorIndex < bodyColors.Length - 1) curBodyColorIndex++;
        else curBodyColorIndex = 0;

        bodySprite.color = bodyColors[curBodyColorIndex];
        bodyColorButton.color = bodyColors[curBodyColorIndex];
    }

    public void ChangePresetAvatar(bool next)
    {
        if (next && currentPresetAvatar < presetAvatars.Length - 1) currentPresetAvatar++;
        else if (!next && currentPresetAvatar > 0) currentPresetAvatar--;

        UpdateAvatarSpritesByPreset(currentPresetAvatar);
    }

    public void ChangeHairSprite(bool next)
    {
        if (next && curHairIndex < totalHairs) curHairIndex++;
        else if (!next && curHairIndex > 01) curHairIndex--;

        hairSprite.sprite = Resources.Load("FemaleAvatar/hair_female_" + curHairIndex.ToString(), typeof(Sprite)) as Sprite;
    }

    public void ChangeFaceSprite(bool next)
    {
        if (next && curFaceIndex < totalFaces) curFaceIndex++;
        else if (!next && curFaceIndex > 1) curFaceIndex--;

        faceSprite.sprite = Resources.Load("FemaleAvatar/face_female_" + curFaceIndex.ToString(), typeof(Sprite)) as Sprite;
    }

    public void ChangeOutfitSprite(bool next)
    {
        if (next && curOutfitIndex < totalOutfits) curOutfitIndex++;
        else if (!next && curOutfitIndex > 1) curOutfitIndex--;

        outfiteSprite.sprite = Resources.Load("FemaleAvatar/outfit_female_med_" + curOutfitIndex.ToString(), typeof(Sprite)) as Sprite;
    }

    void UpdateAvatarSpritesByPreset(int presetIndex)
    {
        hairSprite.sprite = Resources.Load("FemaleAvatar/hair_female_" + presetAvatars[presetIndex].hairSprite.ToString(), typeof(Sprite)) as Sprite;
        hairSprite.color = presetAvatars[presetIndex].hairColor;
        curHairColorIndex = GetHairColorIndex(presetAvatars[presetIndex].hairColor);
        curHairIndex = presetAvatars[presetIndex].hairSprite;
        faceSprite.sprite = Resources.Load("FemaleAvatar/face_female_" + presetAvatars[presetIndex].faceSprite.ToString(), typeof(Sprite)) as Sprite;
        curFaceIndex = presetAvatars[presetIndex].faceSprite;
        outfiteSprite.sprite = Resources.Load("FemaleAvatar/outfit_female_med_" + presetAvatars[presetIndex].outfitSprite.ToString(), typeof(Sprite)) as Sprite;
        curOutfitIndex = presetAvatars[presetIndex].outfitSprite;
        outfiteSprite.color = presetAvatars[presetIndex].outfitColor;
        bodySprite.sprite = Resources.Load("FemaleAvatar/body_female_med_" + presetAvatars[presetIndex].bodySprite.ToString(), typeof(Sprite)) as Sprite;
        bodySprite.color = presetAvatars[presetIndex].bodyColor;
        curBodyColorIndex = GetBodyColorIndex(presetAvatars[presetIndex].bodyColor);
    }

    public void RandomizeCharacter()
    {
        curHairColorIndex = Random.Range(0, hairColors.Length);
        curBodyColorIndex = Random.Range(0, bodyColors.Length);
        curHairIndex = Random.Range(1, totalHairs + 1);
        curFaceIndex = Random.Range(1, totalFaces + 1);     
        curOutfitIndex = Random.Range(1, totalOutfits + 1);

        hairSprite.sprite = Resources.Load("FemaleAvatar/hair_female_" + curHairIndex.ToString(), typeof(Sprite)) as Sprite;
        faceSprite.sprite = Resources.Load("FemaleAvatar/face_female_" + curFaceIndex.ToString(), typeof(Sprite)) as Sprite;
        outfiteSprite.sprite = Resources.Load("FemaleAvatar/outfit_female_med_" + curOutfitIndex.ToString(), typeof(Sprite)) as Sprite;

        hairSprite.color = hairColors[curHairColorIndex];
        hairColorButton.color = hairColors[curHairColorIndex];
        bodySprite.color = bodyColors[curBodyColorIndex];
        bodyColorButton.color = bodyColors[curBodyColorIndex];
    }

    public void SubmitPlayerName()
    {
        PlayerData.Instance.playerName = nameInput.text;
        startGameButton.interactable = true;
    }

    public void ToMapScene(bool usingPreset)
    {
        ChooseCharacter(usingPreset);
        //StartCoroutine(LoadScene());
        SceneManager.LoadScene("Map");
    }

    public void ChooseCharacter(bool usePreset)
    {
        if(usePreset) PlayerData.Instance.playerAvatar = presetAvatars[currentPresetAvatar];
        else
        {
            PlayerData.Instance.playerAvatar.hairSprite = curHairIndex;
            PlayerData.Instance.playerAvatar.faceSprite = curFaceIndex;
            PlayerData.Instance.playerAvatar.outfitSprite = curOutfitIndex;
            PlayerData.Instance.playerAvatar.bodySprite = 1;

            PlayerData.Instance.playerAvatar.hairColor = hairColors[curHairColorIndex];
            PlayerData.Instance.playerAvatar.bodyColor = bodyColors[curBodyColorIndex];
            PlayerData.Instance.playerAvatar.outfitColor = presetAvatars[0].outfitColor;
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Map");
    }

    public void ToCustomization()
    {
        for (int i = 0; i < hairColors.Length; i++)
        {
            if (hairSprite.color == hairColors[i])
            {
                curHairColorIndex = i;
                hairColorButton.color = presetAvatars[currentPresetAvatar].hairColor;
                break;
            }
        }

        for (int i = 0; i < bodyColors.Length; i++)
        {
            if (bodySprite.color == bodyColors[i])
            {
                curBodyColorIndex = i;
                bodyColorButton.color = presetAvatars[currentPresetAvatar].bodyColor;
                break;
            }
        }
    }

    int GetHairColorIndex(Color color)
    {
        int result = 0;
        for (int i = 0; i < hairColors.Length; i++)
        {
            if (color == hairColors[i])
            {
                result = i;
            }
        }

        return result;
    }

    int GetBodyColorIndex(Color color)
    {
        int result = 0;
        for (int i = 0; i < bodyColors.Length; i++)
        {
            if (color == bodyColors[i])
            {
                result = i;
            }
        }

        return result;
    }
}
