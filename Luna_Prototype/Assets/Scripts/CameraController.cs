using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// camera look at
    /// </summary>
    public Transform target;

    

    //actual location camera looks at depends on [offset]
    [SerializeField]
    private Transform _pivot;


    public float offsetX;
    public float cameraFollowSpeedX;
    public float cameraFollowSpeedY;


    public float camera_z = -10.0f;

    private void Awake()
    {
        _pivot = transform.Find("Pivot");
        SetCameraFocus(target);
        
    }

    public void SetCameraFocus(Transform newTarget)
    {
        if (newTarget == null)
        {
            Debug.Log("[CameraController] : newTarget is null");
            return;
        }

        target = newTarget;
        _pivot.transform.position = new Vector3(target.transform.position.x + offsetX, target.transform.position.y, camera_z);
        _pivot.transform.SetParent(newTarget);
        
    }

    private void FixedUpdate()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (Mathf.Abs (transform.position.x - _pivot.position.x) > 0.5f  /*|| Mathf.Abs(transform.position.y - _pivot.position.y) > 0.5f*/)
        {
            transform.Translate(  (cameraFollowSpeedX * Time.deltaTime) * new Vector3(  (_pivot.position.x - transform.position.x ), 0f, 0f));
        }

        if (/*Mathf.Abs(transform.position.x - _pivot.position.x) > 0.5f ||*/ Mathf.Abs(transform.position.y - _pivot.position.y) > 0.5f)
        {
            transform.Translate((cameraFollowSpeedY * Time.deltaTime) * new Vector3(0f, (_pivot.position.y - transform.position.y), 0f));
        }
    }
}
