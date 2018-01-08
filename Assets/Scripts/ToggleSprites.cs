using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSprites : MonoBehaviour {

    public Sprite[] toggledSprites = new Sprite[2];

    public void Toggle(bool on)
    {
        Image sp = this.GetComponent<Image>();

        if (on) sp.sprite = toggledSprites[0];
        else sp.sprite = toggledSprites[1];
    }
}
