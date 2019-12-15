using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTracker : MonoBehaviour
{
    public GameObject respawnPoint;
    private bool respawnFlag = false;
    public float invulnerableTime = 3.0f;
    private float originalInvTime;
    public void Respawn(bool heal)
    {
        if (respawnPoint != null)
        {
            if (heal)
            {
                GetComponent<Player>().mCurrentHealth = GetComponent<Player>().mMaxHealth; // heal the player before respawning them
                respawnFlag = true;
                invulnerableTime = originalInvTime;
            }
            transform.position = respawnPoint.transform.position;
            if(GetComponent<ElementalAttributes>() != null)
            {
                ElementalAttributes mEle;
                mEle = GetComponent<ElementalAttributes>();
                for (int i = 0; i < mEle.mElement.Length; i++) //reset debuff counters
                {
                    mEle.mElement[i].mActiveDuration = 0;
                    mEle.mElement[i].mCount = 0;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        originalInvTime = invulnerableTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Player>().mCurrentHealth <= 0)
        {
            Respawn(true);
        }
        if(respawnFlag)
        {
            if (GetComponent<ElementalAttributes>() != null)
            {
                GetComponent<Player>().mCurrentHealth = GetComponent<Player>().mMaxHealth;
                ElementalAttributes mEle;
                mEle = GetComponent<ElementalAttributes>();
                for (int i = 0; i < mEle.mElement.Length; i++) //reset debuff counters
                {
                    mEle.mElement[i].mActiveDuration = 0;
                    mEle.mElement[i].mCount = 0;
                }
            }
            if(invulnerableTime <= 0.0f)
            {
                respawnFlag = false;
            }
            else
            {
                invulnerableTime -= Time.deltaTime;
            }
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject != null)
        {
            respawnPoint = collision.gameObject;
        }
    }
    */
}
