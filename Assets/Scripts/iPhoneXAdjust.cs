using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iPhoneXAdjust : MonoBehaviour {

    public Vector2 offset;

	// Use this for initialization
	void Start () {
        if(Screen.width == 1125f && Screen.height == 2436f)
        {
            Debug.Log("iphone x");
            this.GetComponent<RectTransform>().anchoredPosition += offset;
        }
		
	}
}
