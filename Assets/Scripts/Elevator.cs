using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private SFXGroup _SFXGroup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GetComponent<Animator>().SetBool("Trigger1", true);
            _SFXGroup.PlaySFX("Elevator_Up");

        }
    }
}