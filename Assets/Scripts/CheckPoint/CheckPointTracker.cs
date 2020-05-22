using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTracker : MonoBehaviour
{
    public GameObject respawnPoint;
    public bool healToFullOnRespawn = false;
    private bool respawnFlag = false;
    private bool enemyRespawn = false;
    private bool flaggedForRespawn = false;
    public float invulnerableTime = 3.0f;
    private float originalInvTime;
    public float recordedHP;
    public List<SimpleCheckPoint> cpList;
    private ScreenTransition mScreenTransition;
    private bool hToFull;

    private List<Character> enemyList;
    private List<GameObject> enemyObj;
    private List<Vector3> enemyOriginalPos;
    private List<bool> enemyAlive;
    private List<float> enemyHealth;
    private GameObject[] objList;
    public void Respawn(bool healToFull)
    {
        if (respawnPoint != null)
        {
            hToFull = healToFull;
            mScreenTransition.FadeIn();
            flaggedForRespawn = true;
            if (mScreenTransition.Fade.canvasRenderer.GetAlpha() >= 0.99f && flaggedForRespawn)
            {
                HealPlayer(healToFull);
                transform.position = respawnPoint.transform.position;
                ResetBuffs();
                flaggedForRespawn = false;
                mScreenTransition.FadeOut();
            }
            else
            {
                return;
            }
            if (enemyRespawn)
            {
                RespawnEnemies();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<Character>();
        enemyObj = new List<GameObject>();
        enemyOriginalPos = new List<Vector3>();
        enemyAlive = new List<bool>();
        enemyHealth = new List<float>();

        originalInvTime = invulnerableTime;
        GameObject[] _objList;
        _objList = GameObject.FindGameObjectsWithTag("Checkpoint");
        for (int i = 0; i < _objList.Length; ++i)
        {
            cpList.Add(_objList[i].GetComponent<SimpleCheckPoint>());
        }

        mScreenTransition = GameObject.FindGameObjectWithTag("ScreenEffectCanvas").GetComponent<ScreenTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if(flaggedForRespawn)
        {
            Respawn(hToFull);
        }
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

    public void ResetSprites()
    {
        for (int i = 0; i < cpList.Count; ++i)
        {
            if (cpList[i].gameObject != respawnPoint)
            {
                cpList[i].GetComponent<SpriteRenderer>().sprite = cpList[i].offSprite;
            }
        }
    }

    public void SetRecordedHealth(float val)
    {
        recordedHP = val;
    }

    public void InitializeEnemyStates()
    {
        if (enemyList.Count >= 1)
        {
            enemyList.Clear();
            enemyObj.Clear();
            enemyOriginalPos.Clear();
            enemyAlive.Clear();
            enemyHealth.Clear();
        }
        objList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < objList.Length; i++)
        {
            enemyList.Add(objList[i].GetComponent<Character>());
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null)
            {
                enemyOriginalPos.Add(enemyList[i].transform.position);
                enemyAlive.Add(enemyList[i].myHealth.IsAlive());
                enemyHealth.Add(enemyList[i].myHealth.GetHealth());
            }
        }
    }

    public void RespawnEnemies()
    {
        if(enemyRespawn)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] != null)
                {
                    if (enemyAlive[i]) //Only respawn enemies that are recorded as alive.
                    {
                        enemyList[i].transform.position = enemyOriginalPos[i];
                        enemyList[i].myHealth.TakeHealth(enemyHealth[i] - enemyList[i].myHealth.GetMaxHealth());
                    }
                }
            }
        }
    }
}
