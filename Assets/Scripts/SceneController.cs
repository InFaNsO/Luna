using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    public void PlayAgain()
    {
        //Bhavil's addition Friday May 15-16
        GameEvents.current.OnDoTransitionAction(TransitionManager.TransitionType.LogoWipe, GameManager.ESceneIndex.Level1);

        //ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Level1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
