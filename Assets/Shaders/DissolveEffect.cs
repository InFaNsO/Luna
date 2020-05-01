using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] Material DissolveMaterial;
    public float DissolveAmount = 1.0f;

    private void Start()
    {
        DissolveMaterial.SetFloat("_DissolveAmount_Dissolve", DissolveAmount);
    }

    private void Update()
    {
        DissolveMaterial.SetFloat("_DissolveAmount_Dissolve", DissolveAmount);
    }

    public void IncreaseDissolve(float val = 0.0f)
    {
        if (val == 0f)
            val = Time.deltaTime;

        DissolveAmount -= val;
        DissolveAmount = Mathf.Clamp01(DissolveAmount);
        DissolveMaterial.SetFloat("_DissolveAmount_Dissolve", DissolveAmount);
    }
}
