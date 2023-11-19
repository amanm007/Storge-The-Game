using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 lastPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector2 currentPosition = transform.position;

        
        if (currentPosition.x != lastPosition.x)
        {
            spriteRenderer.flipX = currentPosition.x < lastPosition.x;
        }

        lastPosition = currentPosition;
    }
}

