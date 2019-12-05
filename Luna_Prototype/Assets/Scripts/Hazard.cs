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
    [SerializeField]
    protected bool applyDebuff = false;
    private void OnCollisionExit2D(Collision2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if(cha != null)
        {
            timePerTick = 1.0f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null)
        {
            //cha.ReceiveDebuff(hazardDamage, hazardDuration);
            if (timePerTick >= 1.0f)
            {
                GetComponent<ElementalAttributes>().ApplyDamage(ref cha, applyDebuff);
                timePerTick = 0.0f;
            }
            timePerTick += Time.deltaTime;
        }
        Debug.Log("HazardDebuff");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null)
        {
            //cha.ReceiveDebuff(hazardDamage, hazardDuration);
            if (timePerTick >= 1.0f)
            {
                GetComponent<ElementalAttributes>().ApplyDamage(ref cha, applyDebuff);
                timePerTick = 0.0f;
            }
            timePerTick += Time.deltaTime;
        }
        Debug.Log("HazardDebuff");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
