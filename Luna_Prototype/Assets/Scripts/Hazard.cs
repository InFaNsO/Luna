using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    //[SerializeField]
    //protected int hazardDamage;
    //[SerializeField] ElementalType mElement;
    [SerializeField]
    protected float timePerTick = 1.0f;
    public bool delayedTick = false;
    private float originalTick;
    private float currentTick;
    [SerializeField]
    protected bool applyDebuff = false;
    private void OnCollisionExit2D(Collision2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if(cha != null && delayedTick)
        {
            timePerTick = originalTick;
        }
        else if(cha != null && !delayedTick)
        {
            timePerTick = 0.0f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null)
        {
            //cha.ReceiveDebuff(hazardDamage, hazardDuration);
            if (currentTick <= 0.0f)
            {
                if (GetComponent<ElementalAttributes>() != null)
                {
                    GetComponent<ElementalAttributes>().ApplyDamage(cha, applyDebuff);
                }
                currentTick = originalTick;
            }
            currentTick -= Time.deltaTime;
        }
        Debug.Log("HazardDebuff");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null)
        {
            //cha.ReceiveDebuff(hazardDamage, hazardDuration);
            if (currentTick <= 0.0f)
            {
                if (GetComponent<ElementalAttributes>() != null)
                {
                    GetComponent<ElementalAttributes>().ApplyDamage(cha, applyDebuff);
                }
                currentTick = originalTick;
            }
            currentTick -= Time.deltaTime;
        }
        Debug.Log("HazardDebuff");
    }
    // Start is called before the first frame update
    void Start()
    {
        originalTick = timePerTick;
        if(delayedTick)
        {
            currentTick = originalTick;
        }
        else
        {
            currentTick = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
