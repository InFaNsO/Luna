using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class Scene : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        //Code to quit game here
    }

    public void Credits()
    {
        //SceneManager.LoadScene("CreditsScene");
        //There is no credits scene yet
    }
}
