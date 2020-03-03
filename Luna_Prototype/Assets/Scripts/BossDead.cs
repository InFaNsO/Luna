using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDead : MonoBehaviour
{
    public float bossHp;
    private Character _boss;
    // Start is called before the first frame update
    private void Awake()
    {
        _boss =  GetComponent<Character>();
        //bossHp = GetComponent<Character>().mCurrentHealth;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bossHp = _boss.myHealth.GetHealth();
        if (bossHp <= 40.0f)
        {
            ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Mainmenu);
        }
    }
}
