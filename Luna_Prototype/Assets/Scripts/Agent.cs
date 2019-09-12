using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Agent //: MonoBehaviour
{
    [SerializeField] private Vector2 mPosition = new Vector2(0.0f, 0.0f);
    [SerializeField] private Vector2 mDestination = new Vector2(0.0f, 0.0f);
    [SerializeField] private Vector2 mVelocity = new Vector2(0.0f, 0.0f);
    [SerializeField] private Vector2 mHeading = new Vector2(0.0f, 0.0f);
    [SerializeField] private Vector2 mFinalDestination = new Vector2(0.0f, 0.0f);
    [SerializeField] private float mMass = 100.0f;
    [SerializeField] private float mMaxSpeed = 10.0f;
    [SerializeField] private float mSafeDistance = 10.0f;
    [SerializeField] private World world;

    public Vector2 GetPosition() { return mPosition; }
    public Vector2 GetDestination() { return mDestination; }
    public Vector2 GetVelocity () { return mVelocity; }
    public Vector2 GetHeading () { return mHeading; }
    public Vector2 GetFinalDestination () { return mFinalDestination; }
    public float GetMass() { return mMass; }
    public float GetMaxSpeed() { return mMaxSpeed; }
    public float GetSafeDistance() { return mSafeDistance; }
    public World GetWorld() { return world; }

    public void SetPosition(Vector2 pos) { mPosition = pos; }
    public void SetHeading(Vector2 pos) { mHeading = pos; }
    public void SetVelocity(Vector2 pos) { mVelocity = pos; }
    public void SetDestination(Vector2 pos) { mDestination = pos; }
    public void SetFinalDestination(Vector2 pos) { mFinalDestination = pos; }
    public void SetWorld(World w) { world = w; }
}
