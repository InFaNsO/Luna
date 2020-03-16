using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Assertions;

public class LocalLevelManager : MonoBehaviour
{
    private UIManager mUIManager;
    //public UI_InGame _InGameUI; // [Rick H  replaced ]
    public GameObject mFinalBoss;
    public float mCountDown = 10.0f;
    public bool mIsCountingDown = false; 

    void Start()
    {
        //Assert.IsNotNull(_InGameUI, "[LocalLevelManager] _InGameUI is null"); [Rick H Removed]

        mUIManager = ServiceLocator.Get<UIManager>();
    }

    void Update()
    {
        if (mFinalBoss!=null)
        {
            if (!mFinalBoss.activeInHierarchy)
            {
                mIsCountingDown = true;
            }
        }

        if (mIsCountingDown)
        {
            mCountDown -= Time.deltaTime;
            mUIManager.UpdateTimeCountDown(mCountDown);
            if (mCountDown < 0.0f)
            {
                ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.GameOverScene);
            }
        }
    }
}
