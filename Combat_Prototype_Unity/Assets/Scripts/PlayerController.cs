using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mSpeed = 4.0f;
    [SerializeField] private float mRotSpeed = 80.0f;
    private float mRotation = 0.0f;
    [SerializeField] private float mGravity = 8.0f;

    private Vector3 mMoveDir = Vector3.zero;

    CharacterController mCharachterController;
    Animator mAnimationController;

    // Start is called before the first frame update
    void Start()
    {
        mCharachterController = GetComponent<CharacterController>();
        mAnimationController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mCharachterController.isGrounded)
        {
            if(Input.GetKey(KeyCode.D))
            {
                mMoveDir = new Vector3(1, 0, 0);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                mMoveDir = new Vector3(0, 0, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                mMoveDir = new Vector3(-1, 0, 0);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                mMoveDir = new Vector3(0, 0, 0);
            }

            mMoveDir *= mSpeed;
        }



        //mMoveDir.y -= mGravity * Time.deltaTime;
        mCharachterController.Move(mMoveDir * Time.deltaTime);
    }
}
