using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Agent //: MonoBehaviour
{
    [SerializeField] protected Vector2 mPosition = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 mDestination = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 mVelocity = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 mHeading = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 mFinalDestination = new Vector2(0.0f, 0.0f);
    [SerializeField] protected float mMass = 100.0f;
    [SerializeField] protected float mMaxSpeed = 10.0f;
    [SerializeField] protected float mSafeDistance = 10.0f;
    [SerializeField] protected float mRadius = 0.75f;
    [SerializeField] protected World world;

    public Vector2 GetPosition() { return mPosition; }
    public Vector2 GetDestination() { return mDestination; }
    public Vector2 GetVelocity () { return mVelocity; }
    public Vector2 GetHeading () { return mHeading; }
    public Vector2 GetFinalDestination () { return mFinalDestination; }
    public float GetMass() { return mMass; }
    public float GetMaxSpeed() { return mMaxSpeed; }
    public float GetSafeDistance() { return mSafeDistance; }
    public World GetWorld() { return world; }
    public float GetRadius() { return mRadius; }

    public void SetPosition(Vector2 pos) { mPosition = pos; }
    public void SetHeading(Vector2 pos) { mHeading = pos; }
    public void SetVelocity(Vector2 pos) { mVelocity = pos; }
    public void SetDestination(Vector2 pos) { mDestination = pos; }
    public void SetFinalDestination(Vector2 pos) { mFinalDestination = pos; }
    public void SetWorld(World w) { world = w; }
}
