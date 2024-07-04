using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteChanger : MonoBehaviour
{
    [SerializeField]
    private Sprite aliveImg;
    [SerializeField]
    private Sprite deadImg;
    [SerializeField]
    private Sprite clearImg;

    private SpriteRenderer sprite;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        GameManager.gameManager.GameStatusChangeEvent += ChangeImage;
    }
    private void ChangeImage(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.Play:
                sprite.sprite = aliveImg;
                break;

            case GameStatus.GameOver:

                sprite.sprite = deadImg;
                break;

            case GameStatus.GameClear:
                sprite.sprite = clearImg;
                break;
        }
    }
}
