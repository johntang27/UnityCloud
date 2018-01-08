using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgePickupMover : MonoBehaviour {

    public GameObject avatar;
    public Text ageText;

    Vector3 startPos;
    Vector3 endPos;
    bool m_isMoving = false;

    float moveIncremant;

    RectTransform m_rectXform;

    void Start()
    {
        m_rectXform = GetComponent<RectTransform>();

        startPos = m_rectXform.anchoredPosition;
        Vector3 avPos = avatar.GetComponent<RectTransform>().anchoredPosition;
        endPos = new Vector3(avPos.x, startPos.y, startPos.z);
    }

    //Turn-Based
    public void SetIncrement()
    {
        if (GameManager.Instance.isTurnBased)
        {
            float totalDist = Vector3.Distance(m_rectXform.anchoredPosition, endPos);
            moveIncremant = totalDist / (int)ScoreManager.Instance.matchPerYear;
            ageText.text = (GameManager.Instance.currentAge + (int)ScoreManager.Instance.yearPerMatch).ToString();
        }
    }

    public void MoveByMatches(bool isFood)
    {
        if (isFood) return;

        Vector3 tempPos = m_rectXform.anchoredPosition;
        tempPos.x -= moveIncremant;
        m_rectXform.anchoredPosition = tempPos;

        if (tempPos.x <= endPos.x) StartCoroutine(DelayResetPos());
    }

    IEnumerator DelayResetPos()
    {
        yield return new WaitForSeconds(0.5f);
        m_rectXform.anchoredPosition = startPos;
        ageText.text = (GameManager.Instance.currentAge + (int)ScoreManager.Instance.yearPerMatch).ToString();
    }

    void Move(Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        ageText.text = (GameManager.Instance.currentAge + 1).ToString();

        if (!m_isMoving)
        {
            StartCoroutine(MoveRoutine(startPos, endPos, timeToMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, float timeToMove)
    {

        if (m_rectXform != null)
        {
            m_rectXform.anchoredPosition = startPos;
        }

        bool reachedDest = false;

        float elapsedTime = 0f;
        m_isMoving = true;

        while (!reachedDest)
        {
            timeToMove = GameManager.Instance.timeToAgeUp;

            if (Vector3.Distance(m_rectXform.anchoredPosition, endPos) < 0.01f)
            {
                //do end stuff here
                reachedDest = true;
                break;

            }

            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
            t = t * t * t * (t * (t * 6 - 15) + 10);

            if (m_rectXform != null)
            {
                m_rectXform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);

            }

            yield return null;

        }

        m_isMoving = false;

        yield return new WaitForSeconds(.05f);
        Move(startPos, endPos, timeToMove);
    }

    public void StartMove(float timeToMove)
    {
        Move(startPos, endPos, timeToMove);
    }
}
