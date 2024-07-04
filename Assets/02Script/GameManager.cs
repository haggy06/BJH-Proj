using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

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


    public Sheet1 cQ { get; private set; }
    private void Awake()
    {
        gameManager = this;

        StartCoroutine("CountBeforeStart");
    }

    WaitForSeconds wfs = new WaitForSeconds(1f);
    private IEnumerator CountBeforeStart()
    {
        print("3");
        yield return wfs;

        print("2");
        yield return wfs;

        print("1");
        yield return wfs;

        GameStartEvent.Invoke();
        CQ();
    }
    public event Action GameStartEvent = () => { };

    public event Action<GameStatus> GameStatusChangeEvent = (value) => Debug.Log("게임 상태 변경 : " + value);
    private void CQ()
    {
        GameStatusChangeEvent(GameStatus.Play);
        cQ = medicineQ.Sheet1[UnityEngine.Random.Range(0, medicineQ.Sheet1.Count)];

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

    private void TimeOver()
    {
        GameStatusChangeEvent(GameStatus.GameOver);
    }
    public void Choiced(bool answer)
    {
        StopCoroutine("TimerAnimation");

        if (cQ.answer == answer) // 맞았을 경우
        {


            GameStatusChangeEvent(GameStatus.GameClear);
        }
        else // 틀렸을 경우
        {
            bedSheet.localScale = Vector2.one;

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