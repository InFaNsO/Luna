using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTracker : MonoBehaviour
{
    public GameObject respawnPoint;
    public bool healToFullOnRespawn = false;
    private bool respawnFlag = false;
    public float invulnerableTime = 3.0f;
    private float originalInvTime;
    public float recordedHP;
    public List<SimpleCheckPoint> cpList;
    public void Respawn(bool healToFull)
    {
        if (respawnPoint != null)
        {
            HealPlayer(healToFull);
            transform.position = respawnPoint.transform.position;
            ResetBuffs();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //recordedHP = gameObject.GetComponent<Player>().myHealth.GetMaxHealth(); //initialize recorded hp
        originalInvTime = invulnerableTime;
        GameObject[] _objList;
        _objList = GameObject.FindGameObjectsWithTag("Checkpoint");
        for (int i = 0; i < _objList.Length; ++i)
        {
            cpList.Add(_objList[i].GetComponent<SimpleCheckPoint>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Player>().myHealth.IsAlive())
        {
            Respawn(healToFullOnRespawn);
        }
        if(respawnFlag)
        {
            ResetBuffs();
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

    private void HealPlayer(bool toFull)
    {
        if(toFull)
        {
            GetComponent<Player>().myHealth.Respawn(); // heal the player before respawning them
            respawnFlag = true;
            invulnerableTime = originalInvTime;
        }
        else
        {
            float currentHP = GetComponent<Player>().myHealth.GetHealth();
            GetComponent<Player>().myHealth.TakeHealth(recordedHP - (currentHP));
        }
    }
    private void ResetBuffs()
    {
        if (GetComponent<ElementalAttributes>() != null)
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

    public void ResetCheckpoints()
    {
        for (int i = 0; i < cpList.Count; ++i)
        {
            if(cpList[i].gameObject != respawnPoint)
            {
                cpList[i].activated = false;
                cpList[i].currentPoint = false;
            }
        }
    }

    public void SetRecordedHealth(float val)
    {
        recordedHP = val;
    }
}
