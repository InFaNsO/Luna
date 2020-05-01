using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    public void SetColor()
    {
        Debug.Log("4564654");
        // _particleSystem.startColor = Color.red;
        //_particleSystem.main.startColor = Color.red;
        var main = _particleSystem.main;
        main.startColor = Color.red;
    }

    public void SetColor(Color color)
    {
        _particleSystem.startColor = color;
        //var main = _particleSystem.main;
        //main.startColor = color;
     }
    public void Activate()
    {
 
        _particleSystem.Play();
       // _particleSystem.gameObject.active = true;
        //gameObject.active = true;
        //_particleSystem.enableEmission = true;
        //var em = _particleSystem.emission;
        //em.enabled = true;
        //_particleSystem.emission.enabled = true;
    }
}
