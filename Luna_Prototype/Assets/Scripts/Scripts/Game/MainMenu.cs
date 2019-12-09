using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, UI_Interface
{
    [Header("Cover of game")]
    private Button startScreen;

    private int levelToLoad = 1;

    private void Awake()
    {
        startScreen = transform.Find("cover").gameObject.GetComponent<Button>();
    }
    public void ResetUI()
    {
         {
            startScreen.gameObject.SetActive(true);
        }
    }


    public void OnLevelButtonClick(int level)
    {
        levelToLoad = level;
        //ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Level1);
        StartCoroutine(LoadLevelRoutine());
    }



    public void Button_StartGame()
    {
        ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Level1);
        //StartCoroutine(LoadLevelRoutine());
    }

    public void Touch_StartScreen()
    {
        startScreen.gameObject.SetActive(false);
    }



    private IEnumerator LoadLevelRoutine()
    {
        yield return SceneManager.LoadSceneAsync(levelToLoad);
    }
}
