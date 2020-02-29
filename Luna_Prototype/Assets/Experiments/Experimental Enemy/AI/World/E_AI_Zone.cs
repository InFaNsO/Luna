using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AI_Zone : MonoBehaviour
{
    public List<E_ZoneConnector> mConnections = new List<E_ZoneConnector>();
    public List<Platform> mPlatforms = new List<Platform>();
    public List<E_Enemy> myEnemies = new List<E_Enemy>();
    public E_AIWorld mWorld;
    public Transform mPlayerTransform;

    public E_PathFinding mPathFinding;

    BoxCollider2D myArea;

    public float mMaxDistanceTravelled = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(mPlatforms.Count > 0)
            mPathFinding.Initialize(mMaxDistanceTravelled);
    }

    private void Awake()
    {
        for (int i = 0; i < mConnections.Count; ++i)
        {
            mConnections[i].From = this;
        }
        myArea = GetComponent<BoxCollider2D>();
        mPathFinding = GetComponent<E_PathFinding>();

        var plats = GetComponentsInChildren<Platform>();
        for (int i = 0; i < plats.Length; ++i)
            mPlatforms.Add(plats[i]);

        mPlayerTransform = mWorld.mPlayer.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register(E_Enemy enem)
    {
        if (enem.mZone == this)
            return;

        myEnemies.Add(enem);
        mWorld.mEnemies.Add(enem);
        enem.mZone = this;
        enem.mPathFinding = mPathFinding;
        enem.IsRunning = false;
        //enem.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            AwakeEnemies();
            mPlayerTransform = collision.GetComponent<Transform>();
            return;
        }
        var enem = collision.GetComponentInParent<E_Enemy>();
        if(enem != null)
        {
            Register(enem);
            return;
        }
        var plat = collision.GetComponent<Platform>();
        if(plat != null)
        {
            mPlatforms.Add(plat);
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            SleepEnemies();
        }
    }

    void AwakeEnemies()
    {
        for (int i = 0; i < myEnemies.Count; ++i)
        {
            myEnemies[i].IsRunning = true;
        }
    }
    void SleepEnemies()
    {
        for (int i = 0; i < myEnemies.Count; ++i)
        {
            myEnemies[i].IsRunning = false;
        }
    }
}
