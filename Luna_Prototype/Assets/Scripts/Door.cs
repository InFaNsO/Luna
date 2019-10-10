using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool isDoorLocked;

    public void UnlockDoor()
    {
        isDoorLocked = true;
    }

    public bool GetDoorStatus()
    {
        return isDoorLocked;
    }

    // Start is called before the first frame update
    void Start()
    {
        isDoorLocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
