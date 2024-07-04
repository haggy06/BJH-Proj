using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Choice : MonoBehaviour
{
    [SerializeField]
    private Check check;

    [SerializeField]
    private bool answer;
    public bool Answer => answer;

    private void Start()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.enabled = false;
        GameManager.gameManager.GameStartEvent += () =>
        {
            col.enabled = true;
        };
    }
    private void OnMouseDown()
    {
        check.Select(this);
    }
}
