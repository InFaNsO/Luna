using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("CameraFollowing")]
    public bool following = true;
    [Tooltip("Camera focus")]
    public Transform target;
    //actual location camera looks at depends on [offset]
    //[Tooltip("Actual location camera looks at, depends on [offset]")]
    //[SerializeField]
    private Transform _pivot;

    
    public float offsetX;
    public float cameraFollowSpeedX;
    public float cameraFollowSpeedY;

    public float camera_z = -10.0f;

     /// <summary>
    /// take two transforms to form a boundary for camera to stop following
    /// default 9999 if no custom transorm used
    /// </summary>
    [Tooltip("Boundary allows camera to stop following")]
    [SerializeField]
    private Transform TopLeft;
    [Tooltip("Boundary allows camera to stop following")]
    [SerializeField]
    private Transform BottomRight;

 
    [Header("CameraShaking")] 
    [SerializeField]
    private float _shakeAmplitude = 2.0f;
    [SerializeField]
    private float _shakeFrequency = 0.1f;
 


    private void Awake()
    {

        _pivot = transform.Find("Pivot");
        if (_pivot == null)
        {
            _pivot = Instantiate(new GameObject(), transform).transform;
            _pivot.name = "Pivot";
         }
        if (TopLeft == null)
        {
            Debug.Log("[CameraController] : no TopLeft Position  found ");
            TopLeft = Instantiate(new GameObject(), transform).transform;
            TopLeft.name = "TopLeft";
            TopLeft.position = new Vector3(-9999f, 9999f, 0f);
        }
        if (BottomRight == null)
        {
            Debug.Log("[CameraController] : no BottomRight Position found ");
            BottomRight = Instantiate(new GameObject(), transform).transform;
            BottomRight.name = "BottomRight";
            BottomRight.position = new Vector3(9999f, -9999f, 0f);
        }


        SetCameraFocus(target);
        
    }
    private void FixedUpdate()
    {
        if (following)
        {
            cameraFollowing();
        }
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
    public void CameraZoom(float destSize, float time)
    {
        StartCoroutine(cameraZoom(destSize, time));
    }

    #region private functions

    private void cameraFollowing()
    {


        if ((_pivot.position.x > TopLeft.position.x && _pivot.position.x < BottomRight.position.x) && (Mathf.Abs(transform.position.x - _pivot.position.x) >= 0.0f))
        {
            transform.Translate((cameraFollowSpeedX * Time.deltaTime) * new Vector3((_pivot.position.x - transform.position.x), 0f, 0f));
 
        }
         else if ((_pivot.position.x < TopLeft.position.x) && ((TopLeft.position.x - transform.position.x) < 0.0f) )
        {
            transform.Translate((cameraFollowSpeedX * Time.deltaTime) * new Vector3((TopLeft.position.x - transform.position.x), 0f, 0f));

         }
        else if ((_pivot.position.x > BottomRight.position.x) && ((BottomRight.position.x - transform.position.x) > 0.0f))
        {
            transform.Translate((cameraFollowSpeedX * Time.deltaTime) * new Vector3((BottomRight.position.x - transform.position.x), 0f, 0f));

         }


        if ((_pivot.position.y < TopLeft.position.y && _pivot.position.y > BottomRight.position.y) && Mathf.Abs(transform.position.y - _pivot.position.y) >= 0.0f)
        {
            transform.Translate((cameraFollowSpeedY * Time.deltaTime) * new Vector3(0f, (_pivot.position.y - transform.position.y), 0f));
         }
 
        else if ((_pivot.position.y > TopLeft.position.y) && ((TopLeft.position.y - transform.position.y) > 0.0f))
        {
            transform.Translate((cameraFollowSpeedY * Time.deltaTime) * new Vector3(0.0f, (TopLeft.position.y - transform.position.y), 0f));

         }
        else if ((_pivot.position.y < BottomRight.position.y) && ((BottomRight.position.y - transform.position.y) < 0.0f))
        {
            transform.Translate((cameraFollowSpeedY * Time.deltaTime) * new Vector3(0.0f, (BottomRight.position.y - transform.position.y), 0f));

         }



    }

    private IEnumerator cameraShake()
    {
        //float a = _shakeFrequency * Time.deltaTime;
        this.transform.Translate(new Vector3(0.0f, _shakeFrequency, 0.0f));
        yield return new WaitForSeconds(_shakeFrequency);

        this.transform.Translate(new Vector3(0.0f, -_shakeAmplitude * _shakeFrequency, 0.0f));
        yield return new WaitForSeconds(_shakeFrequency);


        this.transform.Translate(new Vector3(0.0f, _shakeFrequency, 0.0f));

    }

    private IEnumerator cameraZoom(float destSize,float time)
    {
        float currSize = this.GetComponent<Camera>().orthographicSize;
        float t = 0.0f;
        while (t < time)
        {
            this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(currSize, destSize, t / time);
            t += t + Time.deltaTime > time ? time - t : Time.deltaTime;
            yield return null;
        }

    }

    #endregion


}
