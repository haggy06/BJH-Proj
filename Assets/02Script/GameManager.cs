using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField]
    private MedicineQ medicineQ;

    [SerializeField]
    public float time = 5f;
    private float cTime;

    [Header("Hospital Components")]
    [SerializeField]
    private Transform clockNeedle;
    [SerializeField]
    private Transform bedSheet;

    [Header("UI")]
    [SerializeField]
    private CanvasGroup qPopup;
    [SerializeField]
    private TextMeshProUGUI qText;

    [Space(5)]
    [SerializeField]
    private CanvasGroup wrPopup;
    [SerializeField]
    private TextMeshProUGUI wrText;

    [Space(5)]
    [SerializeField]
    private CanvasGroup readyPopup;
    [SerializeField]
    private TextMeshProUGUI readyText;

    [Space(5)]
    [SerializeField]
    private CanvasGroup speedUpPopup;

    [Space(5)]
    [SerializeField]
    private CanvasGroup timeOverPopup;

    [Space(5)]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    int wrID, qID, rID, sID, tID;
    public Sheet1 cQ { get; private set; }
    private void Awake()
    {
        gameManager = this;
        GameStart();
    }
    public void GameStart()
    {
        GameStatusChangeEvent.Invoke(GameStatus.Play);
        StartCoroutine("CountBeforeStart");
    }

    WaitForSeconds wfs = new WaitForSeconds(2f);
    private IEnumerator CountBeforeStart()
    {
        bedSheet.localScale = new Vector2(0.5f, 1f);
        clockNeedle.eulerAngles = Vector3.zero;

        LeanTween.cancel(sID);
        speedUpPopup.alpha = 0f;

        LeanTween.cancel(tID);
        timeOverPopup.alpha = 0f;

        scoreText.text = "0";
        LeanTween.cancel(qID);
        qPopup.alpha = 0f;
        LeanTween.cancel(rID);
        readyPopup.alpha = 0f;
        LeanTween.cancel(wrID);
        wrPopup.alpha = 0f;

        rID = LeanTween.alphaCanvas(readyPopup, 1f, 0.75f).id;
        readyText.text = "준비...";

        yield return wfs;

        readyText.text = "시작!!";
        rID = LeanTween.alphaCanvas(readyPopup, 0f, 0.75f).id;

        GameStartEvent.Invoke();
        qID = LeanTween.alphaCanvas(qPopup, 1f, 0.5f).id;
        CQ();
    }
    public event Action GameStartEvent = () => { };

    public event Action<GameStatus> GameStatusChangeEvent = (value) => Debug.Log("게임 상태 변경 : " + value);
    private void CQ()
    {
        GameStatusChangeEvent(GameStatus.Play);
        cQ = medicineQ.Sheet1[UnityEngine.Random.Range(0, medicineQ.Sheet1.Count)];

        qText.text = cQ.question;

        StartCoroutine("TimerAnimation");
    }
    private IEnumerator TimerAnimation()
    {
        bedSheet.localScale = Vector2.one;
        clockNeedle.eulerAngles = Vector3.zero;

        cTime = 0f;
        while (cTime < time)
        {
            cTime += Time.deltaTime;

            bedSheet.localScale = new Vector2(0.5f + (cTime / time / 2f), 1f);
            clockNeedle.eulerAngles = new Vector3(0f, 180f, (cTime / time * 360f));

            yield return null;
        }

        TimeOver();
    }

    [SerializeField]
    private int score = 0;
    public int Score
    {
        get => score;
        set
        {
            score = value;

            if (score % 100 == 0)
            {
                time = Mathf.Clamp(time - 0.5f, 1f, 10f);

                speedUpPopup.alpha = 1f;
                sID = LeanTween.alphaCanvas(speedUpPopup, 0f, 1f).id;
            }
            scoreText.text = score.ToString();
        }
    }

    private void TimeOver()
    {
        tID = LeanTween.alphaCanvas(timeOverPopup, 1f, 0.75f).id;

        Choiced(!cQ.answer);
    }
    public void Choiced(bool answer)
    {
        StopCoroutine("TimerAnimation");

        if (cQ.answer == answer) // 맞았을 경우
        {
            Score += 10;

            Invoke("CQ", 1f);
            GameStatusChangeEvent(GameStatus.GameClear);
        }
        else // 틀렸을 경우
        {
            bedSheet.localScale = Vector2.one;

            wrPopup.blocksRaycasts = true;
            wrID = LeanTween.alphaCanvas(wrPopup, 1f, 0.5f).id;
            wrText.text = cQ.wrongComment;
            GameStatusChangeEvent(GameStatus.GameOver);
        }
    }
}

public enum GameStatus
{
    Play,
    GameOver,
    GameClear,

}