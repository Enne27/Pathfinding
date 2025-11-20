using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private void Update()
    {
        if(transform.position.z > 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        }

    }
  
    

}
