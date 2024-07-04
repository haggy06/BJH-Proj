using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Check : MonoBehaviour
{
    [SerializeField]
    private Vector2 originalPos;

    private bool alreadySelect = false;

    private SpriteRenderer sprite;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        GameManager.gameManager.GameStatusChangeEvent += StatusChanged;
    }
    private void StatusChanged(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.Play:
                sprite.color = Color.white;
                transform.position = originalPos;
                alreadySelect = false;
                break;

            case GameStatus.GameOver:
                sprite.color = Color.red;
                LeanTween.cancel(id);
                alreadySelect = true;
                break;

            case GameStatus.GameClear:

                break;
        }
    }

    int id;
    public void Select(Choice choice)
    {
        if (!alreadySelect)
        {
            alreadySelect = true;
            id = LeanTween.move(gameObject, choice.transform, 0.75f).setEase(LeanTweenType.easeOutQuart).setOnComplete(() => GameManager.gameManager.Choiced(choice.Answer)).id;
        }
    }
}
