using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simplePathFollower : MonoBehaviour
{
    public Vector2 destination;
    public float speed = 0.5f;
    private Vector2 startPos;
    private bool going = true;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        destination = startPos + destination;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(this.transform.position, startPos) < 0.5f)
        {
            going = true;
        }
        if (Vector2.Distance(this.transform.position, destination) < 0.5f)
        {
            going = false;
        }

        if (going)
        {
            transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, startPos, Time.deltaTime * speed);
        }
    }
}
