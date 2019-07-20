using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleToCrouch : MonoBehaviour
{
    private Animator mAnimator;
    private float mInputX;
    private float mInputY;

    // Start is called before the first frame update
    void Start()
    {
        //Get the Animator
        mAnimator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mInputY = Input.GetAxis("Vertical");
        mAnimator.SetFloat("InputY", mInputY);

        mInputX = Input.GetAxis("Horizontal");
        mAnimator.SetFloat("InputX", mInputX);
    }
}
