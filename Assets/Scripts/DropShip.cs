using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShip : MonoBehaviour
{
    private LocalLevelManager mLocalLevelManager;
    private Animator mAnimator;

    public float ShipTeleportDelay = 1.0f;
    public float ToWinSceneDelay = 3.0f;

    public ParticleSystem mGetInShipParticle;

    void Awake()
    {
        mLocalLevelManager = GameObject.Find("LocalLevelManager").GetComponent<LocalLevelManager>();
        mAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (mLocalLevelManager && mLocalLevelManager.mIsCountingDown)
            {
                mLocalLevelManager.mIsCountingDown = false;

                other.gameObject.SetActive(false);
                if(mGetInShipParticle)
                {
                    var spwanPos = new Vector3(other.transform.position.x, other.transform.position.y, -2.0f);
                    ParticleSystem newParticle = Instantiate(mGetInShipParticle, spwanPos, Quaternion.identity);
                    newParticle.Play();
                }
                StartCoroutine("TeleportShip");
                StartCoroutine("ToWinScene");
            }


        }
    }

    private IEnumerator TeleportShip()
    {
        var localCounter = 0.0f;
        while (localCounter < ShipTeleportDelay)
        {
            localCounter += Time.deltaTime;
            yield return null;
        }

        if(mAnimator)
            mAnimator.SetTrigger("Teleport");
        yield return null;
    }

    private IEnumerator ToWinScene()
    {
        var localCounter = 0.0f;
        while (localCounter < ToWinSceneDelay)
        {
            localCounter += Time.deltaTime;
            yield return null;
        }

        //Bhavil's addition Friday May 15-16
        GameEvents.current.OnDoTransitionAction(TransitionManager.TransitionType.LogoWipe, GameManager.ESceneIndex.Mainmenu);

        //ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.WinScene);
        yield return null;
    }

    public void ShakeScreen()
    {
        var mMainCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        if (mMainCamera)
        {
            mMainCamera.Shake();
        }
    }
}
