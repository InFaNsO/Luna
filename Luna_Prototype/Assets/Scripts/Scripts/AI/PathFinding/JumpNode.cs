using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpNode : MonoBehaviour
{
    [SerializeField] public List<JumpNode> children = new List<JumpNode>();
    void Start()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
