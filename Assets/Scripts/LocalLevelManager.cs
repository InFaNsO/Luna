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
    public float mStartCountDownDelay = 0.5f;
    public bool mIsCountingDown = false;
    private bool mIsStartDelay = false;

    void Start()
    {
        //Assert.IsNotNull(_InGameUI, "[LocalLevelManager] _InGameUI is null"); [Rick H Removed]

        mUIManager = ServiceLocator.Get<UIManager>();
        mUIManager.ClearTimeCountDown();
    }

    void Update()
    {
        if (mFinalBoss!=null)
        {
            if (!mFinalBoss.activeInHierarchy && !mIsStartDelay)
            {
                StartCoroutine("StartCountDelay");
                mIsStartDelay = true;
            }
        }

        if (mIsCountingDown)
        {
            mCountDown -= Time.deltaTime;
            mUIManager.UpdateTimeCountDown(mCountDown);
            if (mCountDown < 0.0f)
            {
                //Bhavil's addition Friday May 15-16
                GameEvents.current.OnDoTransitionAction(TransitionManager.TransitionType.LogoWipe, GameManager.ESceneIndex.GameOverScene);

                //ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.GameOverScene);
            }
        }
    }

    private IEnumerator StartCountDelay()
    {
        var localCounter = 0.0f;
        while (localCounter < mStartCountDownDelay)
        {
            localCounter += Time.deltaTime;
            yield return null;
        }

        mIsCountingDown = true;
        yield return null;
    }
}
