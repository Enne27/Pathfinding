using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTextureSea : MonoBehaviour
{
    [SerializeField] float scrollSpeedX;
    [SerializeField] float scrollSpeedY;
    private MeshRenderer meshRenderer;

    private float x = 1;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        x += 0.01f;
        Debug.Log(x);
        meshRenderer.material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup * scrollSpeedX * Mathf.Sin(x), Time.realtimeSinceStartup * scrollSpeedY);
    }
}