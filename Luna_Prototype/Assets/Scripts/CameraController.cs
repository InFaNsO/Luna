using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("CameraFollowing")]
    /// <summary>
    /// camera look at
    /// </summary>
    [Tooltip("Camera focus")]
    public Transform target;
    //actual location camera looks at depends on [offset]
    [SerializeField]
    private Transform _pivot;

    public float offsetX;
    public float cameraFollowSpeedX;
    public float cameraFollowSpeedY;

    public float camera_z = -10.0f;


    /// <summary>
    /// take two transforms to form a boundary for camera to stop following
    /// </summary>
    [Tooltip("Boundary allows camera to stop following")]
    [SerializeField]
    private Transform TopLeft;
    [Tooltip("Boundary allows camera to stop following")]
    [SerializeField]
    private Transform BottomRight;



    [Header("CameraShaking")] 
    [SerializeField]
    private float _shakeAmplitude = 0.1f;
    [SerializeField]
    private float _shakeFrequency = 0.1f;
 


    private void Awake()
    {
        _pivot = transform.Find("Pivot");

        if (_pivot == null) Debug.Log("[CameraController] : no pivot  found ");
        if (TopLeft == null) Debug.Log("[CameraController] : no TopLeft   found ");
        if (BottomRight == null) Debug.Log("[CameraController] : no BottomRight found ");

        SetCameraFocus(target);
        
    }
    private void FixedUpdate()
    {
        cameraFollowing();
    }

    public void Shake()
    {
        StartCoroutine("cameraShake");
    }

    public void SetCameraFocus(Transform newTarget)
    {
        if (newTarget == null)
        {
            Debug.Log("[CameraController] : new target is null");
            return;
        }

        target = newTarget;
        _pivot.transform.position = new Vector3(target.transform.position.x + offsetX, target.transform.position.y, camera_z);
        _pivot.transform.SetParent(newTarget);
        
    }


    #region private functions

    private void cameraFollowing()
    {
 
        if ((_pivot.position.x > TopLeft.position.x && _pivot.position.x < BottomRight.position.x) && (Mathf.Abs(transform.position.x - _pivot.position.x) >= 0.0f))
        {
            transform.Translate((cameraFollowSpeedX * Time.deltaTime) * new Vector3((_pivot.position.x - transform.position.x), 0f, 0f));
        }
        else if (_pivot.position.x < TopLeft.position.x)
        {
            transform.position = new Vector3(TopLeft.position.x, transform.position.y, camera_z);
        }
        else if (_pivot.position.x > BottomRight.position.x)
        {
            transform.position = new Vector3(BottomRight.position.x, transform.position.y, camera_z);
        }


        if ((_pivot.position.y < TopLeft.position.y && _pivot.position.y > BottomRight.position.y) && Mathf.Abs(transform.position.y - _pivot.position.y) >= 0.0f)
        {
            transform.Translate((cameraFollowSpeedY * Time.deltaTime) * new Vector3(0f, (_pivot.position.y - transform.position.y), 0f));
        }
        else if (_pivot.position.y > TopLeft.position.y)
        {
            transform.position = new Vector3(transform.position.x, TopLeft.position.y, camera_z);
        }
        else if (_pivot.position.y < BottomRight.position.y)
        {
            transform.position = new Vector3(transform.position.x, BottomRight.position.y, camera_z);
        }



    }

    private IEnumerator cameraShake()
    {
        //float a = _shakeFrequency * Time.deltaTime;
        this.transform.Translate(new Vector3(0.0f, _shakeFrequency, 0.0f));
        yield return new WaitForSeconds(_shakeFrequency);

        this.transform.Translate(new Vector3(0.0f, -2.0f * _shakeFrequency, 0.0f));
        yield return new WaitForSeconds(_shakeFrequency);


        this.transform.Translate(new Vector3(0.0f, _shakeFrequency, 0.0f));

    }


    #endregion


}
