using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundSpawner : MonoBehaviour
{
    public Camera cam;
    public GameObject soundPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Instantiate(soundPrefab, mouseWorldPos, Quaternion.identity);
        }
    }
}
