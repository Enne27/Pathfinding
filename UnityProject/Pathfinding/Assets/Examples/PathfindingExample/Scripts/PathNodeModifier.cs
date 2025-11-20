using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeModifier : MonoBehaviour {
    public float modifier;
    public GameObject visualsObject;

    public void HideVisuals() {
        visualsObject.SetActive(false);
    }

    public void ShowVisuals() {
        visualsObject.SetActive(true);
    }

}
