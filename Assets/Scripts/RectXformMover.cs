using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RectXformMover : MonoBehaviour {

    Vector3 startPos;
    Vector3 onScreenPos;
    Vector3 endPos;
    Vector3 spawnPos;
    public float maxSceneWidth;
    public RectTransform attachedTo;

    public float timeToMove = 1f;

    RectTransform m_rectXform;

    bool m_isMoving = false;

	void Start () {
        m_rectXform = GetComponent<RectTransform>();

        startPos = m_rectXform.anchoredPosition;
        endPos = new Vector3(-1 * (maxSceneWidth + maxSceneWidth/3.6f), startPos.y, startPos.z);

        this.GetComponent<Image>().sprite = Resources.Load("Scenes/scene_" + PlayerData.Instance.currentLevelData.retirementDream.ToString(), typeof(Sprite)) as Sprite;
    }

    public void MoveByMatches()
    {
        Vector3 tempPos = m_rectXform.anchoredPosition;
        tempPos.x -= maxSceneWidth / 3.6f;
        m_rectXform.anchoredPosition = tempPos;
        //spawnPos = new Vector3(attachedTo.anchoredPosition.x + (maxSceneWidth - maxSceneWidth / 3.6f), startPos.y, startPos.z);
        spawnPos = new Vector3(attachedTo.anchoredPosition.x + maxSceneWidth, startPos.y, startPos.z);
        Debug.Log(this.gameObject.name);
        if (tempPos.x <= endPos.x)
        {
            spawnPos = new Vector3(attachedTo.anchoredPosition.x + maxSceneWidth, startPos.y, startPos.z);
            m_rectXform.anchoredPosition = spawnPos;
        }

        if (m_rectXform.anchoredPosition.x < -2000f)
        {
            if (ScoreManager.Instance != null) ScoreManager.Instance.ResetSceneLoops();
        }
    }

    public void StartMove()
    {
        Move(startPos, endPos);
    }

    void Move(Vector3 start, Vector3 end)
    {
        if(!m_isMoving)
        {
            StartCoroutine(MoveRoutine(start, end));
        }
    }

    IEnumerator MoveRoutine(Vector3 start, Vector3 end)
    {
        bool reachedDest = false;
        m_isMoving = true;

        //Debug.Log(start.ToString());
        //Debug.Log(end.ToString());

        while (!reachedDest)
        {
            if (m_rectXform.anchoredPosition.x < endPos.x)
            {
                reachedDest = true;
                m_rectXform.anchoredPosition = spawnPos;
                break;
            }

            if (m_rectXform != null)
            {
                m_rectXform.anchoredPosition += Vector2.left * Time.deltaTime * timeToMove;
                //spawnPos = new Vector3(attachedTo.anchoredPosition.x + (maxSceneWidth - maxSceneWidth/3.6f), startPos.y, startPos.z);
                spawnPos = new Vector3(attachedTo.anchoredPosition.x + maxSceneWidth, startPos.y, startPos.z);
                //transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
            yield return null;
        }

        m_isMoving = false;
        Scroll();
    }

    public void Scroll()
    {
        Move(spawnPos, endPos);
    }
}
