using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public enum TransitionType
    {
        CrossFade,
        ShapeWipe,
        LogoWipe
    }


    [Tooltip("Transition time is time needed for one part to run.")]
    [SerializeField] float transitionTime = 1.0f;
    [SerializeField] Animator CrossFade = null;
    [SerializeField] Animator ShapeWipe = null;
    [SerializeField] Animator LogoWipe = null;

    private void Awake()
    {
        GameEvents.current.DoTransition += HandleTransition;
    }


    public void HandleTransition(TransitionType type, int buildIndex)
    {
        switch (type)
        {
            case TransitionType.CrossFade:
                DoCrossFade(buildIndex);
                break;
            case TransitionType.ShapeWipe:
                DoShapeWipe(buildIndex);
                break;
            case TransitionType.LogoWipe:
                DoLogoWipe(buildIndex);
                break;
            default:
                break;
        }
    }

    private void DoCrossFade(int buildIndex)
    {
        StartCoroutine(SwitchScene(CrossFade, buildIndex));
    }
    private void DoShapeWipe(int buildIndex)
    {
        StartCoroutine(SwitchScene(ShapeWipe, buildIndex));
    }
    private void DoLogoWipe(int buildIndex)
    {
        StartCoroutine(SwitchScene(LogoWipe, buildIndex));
    }

    IEnumerator SwitchScene(Animator anim, int buildIndex)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(buildIndex);
    }

}
