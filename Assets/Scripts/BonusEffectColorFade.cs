using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEffectColorFade : MonoBehaviour {

    public float fadeDuration;
    float fadeCounter;
    Color fadeColor;

	// Use this for initialization
	void Start () {
        fadeColor = this.GetComponent<TextMesh>().color;
        StartCoroutine(Fade());
	}

    IEnumerator Fade()
    {
        while (fadeCounter < 1)
        {
            fadeCounter += Time.deltaTime / fadeDuration;
            fadeColor.a = Mathf.Lerp(1, 0, fadeCounter);
            this.GetComponent<TextMesh>().color = fadeColor;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
