using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardWorldViewBehavoir : MonoBehaviour
{
 

    void Update()
    {

        transform.rotation = Quaternion.Inverse(Camera.main.gameObject.transform.rotation); 
        
    }
}
