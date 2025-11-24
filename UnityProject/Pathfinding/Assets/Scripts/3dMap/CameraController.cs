using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float maxZ = 1f;
    private void LateUpdate()
    {
        #region VIEJO
        //if (transform.position.z > 1)
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        //}
        #endregion
        Vector3 pos = transform.position;
        if (pos.z > maxZ)
            pos.z = maxZ;

        transform.position = pos;
    }
}
