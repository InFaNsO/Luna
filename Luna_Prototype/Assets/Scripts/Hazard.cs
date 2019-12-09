using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    //[SerializeField]
    //protected int hazardDamage;
    //[SerializeField] ElementalType mElement;
    [SerializeField]
    protected bool delayedTick = false;
    [SerializeField]
    protected float timePerTick = 1.0f;
    [SerializeField]
    protected bool applyDebuff = false;

    private float mTick;
    private void OnCollisionExit2D(Collision2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if(cha != null)
        {
            if (delayedTick)
            {
                mTick = 0.0f;
            }
            else
            {
                mTick = timePerTick;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null)
        {
            if (delayedTick)
            {
                mTick = 0.0f;
            }
            else
            {
                mTick = timePerTick;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null)
        {
            //cha.ReceiveDebuff(hazardDamage, hazardDuration);
            if (mTick >= timePerTick)
            {
                GetComponent<ElementalAttributes>().ApplyDamage(ref cha, applyDebuff);
                mTick = 0.0f;
            }
            mTick += Time.deltaTime;
        }
        Debug.Log("HazardDebuff");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null)
        {
            //cha.ReceiveDebuff(hazardDamage, hazardDuration);
            if (mTick >= timePerTick)
            {
                GetComponent<ElementalAttributes>().ApplyDamage(ref cha, applyDebuff);
                mTick = 0.0f;
            }
            mTick += Time.deltaTime;
        }
        Debug.Log("HazardDebuff");
    }
    // Start is called before the first frame update
    void Start()
    {
        if(delayedTick)
        {
            mTick = 0.0f;
        }
        else
        {
            mTick = timePerTick;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
