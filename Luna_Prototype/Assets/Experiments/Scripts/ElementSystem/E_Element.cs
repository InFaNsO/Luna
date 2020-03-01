using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Element : MonoBehaviour
{
    [SerializeField] float Damage = 1.0f;
    [SerializeField] float Resistance = 1.0f;
    [SerializeField] float Chance = 1.0f;
    [SerializeField] float Intensity = 1.0f;
    [SerializeField] float Duration = 1.0f;

    [SerializeField] public E_ElementTypes Type = E_ElementTypes.none;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Randomize()
    {

    }
}
