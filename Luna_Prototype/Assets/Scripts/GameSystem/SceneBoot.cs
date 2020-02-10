using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
        GameObject systemsGO = new GameObject("[Services]");
        systemsGO.tag = "Services";
        DontDestroyOnLoad(systemsGO);


        foreach (var c in _comps)
        {
            switch (c)
            {
                case ComponentType.GameManager:
                    CreateGameManager();
                    break;
                case ComponentType.UIManager:
                    CreateUIManager();
                    break;
                case ComponentType.AudioManager:
                    break;
                default:
                    break;
            }
        }
    }
    

    private void CreateGameManager()
    {
        GameObject gameManagerGO = new GameObject("GameManager");
        gameManagerGO.transform.SetParent(transform);
        GameManager gameMngrComp = gameManagerGO.AddComponent<GameManager>();
        ServiceLocator.Register<GameManager>(gameMngrComp);

    }
    private void CreateUIManager()
    {
        UIManager UIManageComp = FindObjectOfType<UIManager>();
        Assert.IsNotNull(UIManageComp, "[GameLoader] UIManager not found in scene [GameLoader]");
        UIManageComp.transform.SetParent(transform);
        ServiceLocator.Register<UIManager>(UIManageComp);

    }
    private void CreateAudioManager()
    {

    }
}
