using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyIsGroundChecker : MonoBehaviour
{
    Enemy mEnemy;
    // Start is called before the first frame update
    void Awake()
    {
        mEnemy = GetComponent<Enemy>();
        Assert.IsNotNull(mEnemy, "GameObject dont have Enemy script componet");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            mEnemy.isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            mEnemy.isGrounded = false;
        }
    }
}
