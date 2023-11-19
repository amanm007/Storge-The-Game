using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sound : MonoBehaviour
{
    public float timeToSelfDestruct = 200f; // How long the object will stay around before it destroys itself
    public float triggerLifetime = 0.3f; // How long the trigger will be active (the trigger that alerts the monster)
    private float timer;

    private Collider2D trigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //timers
        timer += Time.deltaTime;
        if(timer >= triggerLifetime && trigger.enabled != false) //disable trigger when timer's reached
        {
            trigger.enabled = false;
        }
        if(timer >= timeToSelfDestruct) //destroy self when timer's up
        {
            Destroy(this.gameObject);
        }
    }
}
