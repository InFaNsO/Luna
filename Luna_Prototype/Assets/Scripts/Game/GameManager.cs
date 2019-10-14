using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//========== [ISceneInfo] ===================
// 2019-10-12 [Rick H]
 //=============================

public class GameManager : MonoBehaviour
{
    public enum ESceneIndex //start at 1
    {
        MainMenu = 1 // Title Scene index 1
    }

    //private Player Player;

    private void Awake()
    {
        //SceneManager.sceneLoaded += SetUpManagers;
     }


    /// <summary>
    /// call by other object, e.g. when player character died,
    /// compelete the quest, defeat the boss, enter another room
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void SwitchScene(ESceneIndex sceneIndex)
    {
        StartCoroutine(LoadLevelRoutine(sceneIndex));
    }

    private IEnumerator LoadLevelRoutine(ESceneIndex sceneIndex)
    {
        yield return SceneManager.LoadSceneAsync((int)sceneIndex);
    }


    //public void SetUpManagers(Scene scene, LoadSceneMode mode)
    //{

    // }

    //public IEnumerator ResetObjects()
    //{
    //    //reset managers or gameobjects

    //    yield return null;
    //}

}
