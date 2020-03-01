using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Character : MonoBehaviour
{
    public E_Health myHealth;
    public Rigidbody2D mRigidBody;


    //Weapon Minghuo
    public bool isGrounded;
    public Transform mAttackMomentumPos;

    //Elemental Parts
    //public ElementalAttributes elementalAttributes;
    public E_Element element;


    private void Awake()
    {
        if (transform.Find("AttackMomentumPos") != null)
        {
            mAttackMomentumPos = transform.Find("AttackMomentumPos").transform;                              //|--- [Mingzhuo Zhang] Edit: finding the mAttackMomentumPos
        }
        else
        {
            mAttackMomentumPos = new GameObject().transform;
            mAttackMomentumPos.SetParent(gameObject.transform);
            mAttackMomentumPos.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }

        element = GetComponent<E_Element>();
        if (element == null)
            element = new E_Element();

        if(element.Type == E_ElementTypes.none)
            element.Randomize();
    }

    void Start()
    {
        myHealth = GetComponent<E_Health>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!myHealth.IsAlive())
            Die();
    }


    public void GetHit(float dmg)
    {
        myHealth.TakeDamage(dmg);
    }
    public void GetHit(E_Element dmg)
    {
        //myHealth.TakeDamage(dmg);
    }

    public virtual void Die()
    {

    }
}
