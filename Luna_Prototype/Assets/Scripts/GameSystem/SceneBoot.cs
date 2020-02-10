using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
/// <summary>
/// RickH 2020 02 09
/// Attach to a gameobject in level scenes 
/// to help register critical component to [ServiceLocator] when testing
/// </summary>


public class SceneBoot : MonoBehaviour
{
    public enum ComponentType
    {
        GameManager = 0,
        UIManager,
        AudioManager
    }

    [SerializeField]
    private List<ComponentType> _comps;

    [SerializeField]
    private GameObject _uiMngrPrefeb;

    [SerializeField]
    private GameObject _audioMngrPrefeb;

    private void Awake()
    {
  
        // Setup System GameObject
        GameObject systemsGO = new GameObject("[Services] from [" + SceneManager.GetActiveScene().name + "]");
        systemsGO.tag = "Services";
        DontDestroyOnLoad(systemsGO);
        Transform systemsParent = systemsGO.transform;


        foreach (var c in _comps)
        {
            switch (c)
            {
                case ComponentType.GameManager:
                    CreateGameManager(systemsParent);
                    break;
                case ComponentType.UIManager:
                    CreateUIManager(systemsParent);
                    break;
                case ComponentType.AudioManager:
                    break;
                default:
                    break;
            }
        }
    }
    

    private void CreateGameManager(Transform systemsParent)
    {
        GameObject gameManagerGO = new GameObject("GameManager");
        gameManagerGO.transform.SetParent(systemsParent);
        GameManager gameMngrComp = gameManagerGO.AddComponent<GameManager>();
        ServiceLocator.Register<GameManager>(gameMngrComp);

    }
    private void CreateUIManager(Transform systemsParent)
    {
        GameObject UIMngrGO = Instantiate(_uiMngrPrefeb);
        UIManager UIManageComp = UIMngrGO.GetComponent<UIManager>();
        Assert.IsNotNull(UIManageComp, "[SceneBoot] UIManager not found in scene [GameLoader]");
        UIMngrGO.transform.SetParent(systemsParent);
        ServiceLocator.Register<UIManager>(UIManageComp);

    }
    private void CreateAudioManager(Transform systemsParent)
    {
        //AudioManager 
        GameObject AudioMngrGO = Instantiate(_audioMngrPrefeb);
        AudioManager AudioManagerComp = AudioMngrGO.GetComponent<AudioManager>();
        Assert.IsNotNull(AudioManagerComp, "[GameLoader] AudioManager not found in scene [GameLoader]");
        AudioMngrGO.transform.SetParent(systemsParent);
        ServiceLocator.Register<AudioManager>(AudioManagerComp);
    }
}
