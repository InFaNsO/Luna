using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Agent : MonoBehaviour
{
   // [SerializeField] protected Vector2 mPosition = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 mDestination = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 mVelocity = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 mHeading = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 mFinalDestination = new Vector2(0.0f, 0.0f);
    [SerializeField] protected float mMass = 100.0f;
    [SerializeField] protected float mMaxSpeed = 10.0f;
    [SerializeField] protected float mSafeDistanceReduced = 5.0f;
    [SerializeField] protected float mSafeDistance = 10.0f;
    [SerializeField] protected float mSafeDistanceExtended = 15.0f;
    [SerializeField] protected float mRadius = 0.75f;
    [SerializeField] protected World world;

    public Vector2 GetPosition()                                    { return transform.position; }
    public Vector2 GetDestination()                                 { return mDestination; }
    public Vector2 GetVelocity ()                                   { return mVelocity; }
    public Vector2 GetHeading ()                                    { return mHeading; }
    public Vector2 GetFinalDestination ()                           { return mFinalDestination; }
    public World GetWorld()                                         { return world; }
    public float GetMass()                                          { return mMass; }
    public float GetMaxSpeed()                                      { return mMaxSpeed; }
    public float GetSafeDistanceReduced()                           { return mSafeDistanceReduced; }
    public float GetSafeDistance()                                  { return mSafeDistance; }
    public float GetSafeDistanceExtended()                          { return mSafeDistanceExtended; }
    public float GetRadius()                                        { return mRadius; }

    public void SetSafeDistanceReduced(float distance)              { mSafeDistanceReduced = distance; }
    public void SetSafeDistance(float distance)                     { mSafeDistance = distance; }
    public void SetSafeDistanceExtended(float distance)             { mSafeDistanceExtended = distance; }
    public void SetPosition(Vector2 pos)                            { transform.position = pos; }
    public void SetHeading(Vector2 pos)                             { mHeading = pos; }
    public void SetVelocity(Vector2 pos)                            { mVelocity = pos; }
    public void SetDestination(Vector2 pos)                         { mDestination = pos; }
    public void SetDestination(Vector3 pos)                         { mDestination = pos; }
    public void SetFinalDestination(Vector2 pos)                    { mFinalDestination = pos; }
    public void SetWorld(World w)                                   { world = w; }

    public void Update()
    {
        transform.position = new Vector2(transform.position.x + (mVelocity.x * Time.deltaTime), transform.position.y + (mVelocity.y * Time.deltaTime));
    }
}
