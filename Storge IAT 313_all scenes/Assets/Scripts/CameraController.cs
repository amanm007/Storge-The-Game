using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   [SerializeField] private Transform june;

  
    private void Update()
    {
        transform.position = new Vector3(june.position.x, june.position.y, transform.position.z);

    }
}
