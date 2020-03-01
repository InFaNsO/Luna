using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Health : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float Health = 30.0f;

    public E_Element myElement;

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    public void TakeHealth(float hp)
    {
        Health += hp;
    }

    public bool IsAlive()
    {
        return Health > 0.0f;
    }
}
