using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    public void PlayAgain()
    {
        ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Level1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
