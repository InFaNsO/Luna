using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall_UV_Scroll : MonoBehaviour
{

    [SerializeField] float Speed = 0.75f;

    [System.Serializable]
    public enum Direction
    {
        Vertical = 0,
        Horizontal = 1
    }

    [SerializeField] Direction direction = Direction.Vertical;


    public Renderer Wf_Renderer;

    private void Start()
    {
        Wf_Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float texOffSet = Time.time * Speed;
        Wf_Renderer.material.SetTextureOffset("_MainTex", new Vector2((int)direction * texOffSet, (1 - (int)direction) * texOffSet));
    }
}
