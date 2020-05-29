    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWater : MonoBehaviour
{
    [SerializeField] Vector2 LW = new Vector2();
    [SerializeField] Vector2 TB = new Vector2();

    float[] xPositions;
    float[] yPositions;
    float[] velocities;
    float[] accelerations;
    LineRenderer Body;
    public Material material;

    GameObject[] MeshObjects;
    Mesh[] meshes;
    GameObject[] colliders;

    float springConstant = 0.02f;
    float damping = 0.04f;
    float spread = 0.05f;
    float z = -1f;

    float baseHeight;
    float left;
    float bottom;

    public GameObject waterMesh;
    public GameObject splash;//might not be used

    MeshFilter meshFilter;

    List<GameObject> spawnedOBJ = new List<GameObject>();
    public float maxH = 5f;
    public float waveSpeed = 3f;

    int mEdgeCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < spawnedOBJ.Count; ++i)
            Destroy(spawnedOBJ[i]);
        spawnedOBJ.Clear();

        SpawnWater(LW.x, LW.y, TB.x, TB.y);
    }

    private void OnValidate()
    {
        for (int i = 0; i < spawnedOBJ.Count; ++i)
            Destroy(spawnedOBJ[i]);
        spawnedOBJ.Clear();
        
        SpawnWater(LW.x, LW.y, TB.x, TB.y);
        
        SetSineValues();
    }

    public int numSineWaves = 5;
    float[] sineOffset;// = new float[5] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f};
    float[] sineAmplitude;// = new float[5] { 1f, 2f, 3f, 4f, 5f };
    float[] sineStreach;// = new float[5] { 1f, 1.5f, 2f, 2.5f, 3f };
    float[] sineStreatchOffset;// = new float[5] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f };

    public void Splash(float xpos, float velocity)
    {
        return;

        //If the position is within the bounds of the water:
        if (xpos >= xPositions[0] && xpos <= xPositions[xPositions.Length - 1])
        {
            //Offset the x position to be the distance from the left side
            xpos -= xPositions[0];

            //Find which spring we're touching
            int index = Mathf.RoundToInt((xPositions.Length - 1) * (xpos / (xPositions[xPositions.Length - 1] - xPositions[0])));

            //Add the velocity of the falling object to the spring
            velocities[index] += velocity;

            //Set the lifetime of the particle system.
            float lifetime = 0.93f + Mathf.Abs(velocity) * 0.07f;

            //Set the splash to be between two values in Shuriken by setting it twice.
            splash.GetComponent<ParticleSystem>().startSpeed = 8 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
            splash.GetComponent<ParticleSystem>().startSpeed = 9 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
            splash.GetComponent<ParticleSystem>().startLifetime = lifetime;

            //Set the correct position of the particle system.
            Vector3 position = new Vector3(xPositions[index], yPositions[index] - 0.35f, 5);

            //This line aims the splash towards the middle. Only use for small bodies of water:
            Quaternion rotation = Quaternion.LookRotation(new Vector3(xPositions[Mathf.FloorToInt(xPositions.Length / 2)], baseHeight + 8, 5) - position);

            //Create the splash and tell it to destroy itself.
            GameObject splish = Instantiate(splash, position, rotation) as GameObject;
            Destroy(splish, lifetime + 0.3f);
        }
    }

    public void SpawnWater(float Left, float Width, float Top, float Buttom)
    {
        ////Add a box collider to the water that will allow things to float in it.
        //gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(Left + Width / 2, (Top + Buttom) / 2);
        //gameObject.GetComponent<BoxCollider2D>().size = new Vector2(Width, Top - Buttom);
        //gameObject.GetComponent<BoxCollider2D>().isTrigger = true;


        //increase here for more smoothness
        int edgeCount = Mathf.RoundToInt(Width) * 5;
        int nodeCount = edgeCount + 1;

        mEdgeCount = edgeCount;

        meshFilter = GetComponent<MeshFilter>();

        //Body = gameObject.GetComponent<LineRenderer>();
        //Body.material = material;
        //Body.material.renderQueue = 1000;
        //Body.positionCount = nodeCount;
        //Body.startWidth = 0.1f;
        //Body.endWidth = 0.1f;

        xPositions = new float[nodeCount];
        yPositions = new float[nodeCount];
        velocities = new float[nodeCount];
        accelerations = new float[nodeCount];

        MeshObjects = new GameObject[edgeCount];
        meshes = new Mesh[edgeCount];
        colliders = new GameObject[edgeCount];

        baseHeight = Top;
        bottom = Buttom;
        left = Left;

        for (int i = 0; i < nodeCount; i++)
        {
            yPositions[i] = Top;
            xPositions[i] = Left + Width * i / edgeCount;
            accelerations[i] = 0;
            velocities[i] = 0;
            //Body.SetPosition(i, new Vector3(xPositions[i], yPositions[i], z));
        }

        SetupMesh(edgeCount);

        SetSineValues();
        GetComponent<MeshRenderer>().sharedMaterial = material;
        return;
        /*
                 //triangle = i,i+1,i+3 & i+3, i+2,0


        for (int i = 0; i < edgeCount; ++i)
        {
            meshes[i] = new Mesh();
            Vector3[] vertices = new Vector3[4];
            //vertices[0] = new Vector3(xPositions[i], yPositions[i], z);
            //vertices[1] = new Vector3(xPositions[i + 1], yPositions[i + 1], z);
            //vertices[2] = new Vector3(xPositions[i], bottom, z);
            //vertices[3] = new Vector3(xPositions[i + 1], bottom, z);

            Vector2[] uvs = new Vector2[4];
            uvs[0] = new Vector2(0, 1);
            uvs[1] = new Vector2(1, 1);
            uvs[2] = new Vector2(0, 0);
            uvs[3] = new Vector2(1, 0);
            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

            meshes[i].vertices = vertices;
            meshes[i].uv = uvs;
            meshes[i].triangles = tris;

            MeshObjects[i] = Instantiate(waterMesh, transform);
            MeshObjects[i].GetComponent<MeshFilter>().mesh = meshes[i];

            spawnedOBJ.Add(MeshObjects[i]);

            colliders[i] = new GameObject();
            colliders[i].name = "Trigger";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;
            colliders[i].transform.position = new Vector3(Left + Width * (i + 0.5f) / edgeCount, Top - 0.5f, 0);
            colliders[i].transform.localScale = new Vector3(Width / edgeCount, 1, 1);
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].AddComponent<WaterDetector>();
        }

        SetSineValues();*/
    }

    void SetupMesh(int edgeCount)
    {
        Vector3[] vertices = new Vector3[2 * edgeCount];
        Vector2[] uvs = new Vector2[2 * edgeCount];

        int[] triangles = new int[6 * edgeCount];

        vertices[0] = new Vector3(xPositions[0], yPositions[0], z);
        vertices[1] = new Vector3(xPositions[0], bottom, z);

        uvs[0] = new Vector2(0, 1);
        uvs[0] = new Vector2(0, 0);

        for (int i = 1; i < edgeCount; ++i)
        {
            vertices[i * 2 + 0] = new Vector3(xPositions[i], yPositions[i], z);
            vertices[i * 2 + 1] = new Vector3(xPositions[i], bottom, z);

            uvs[i * 2 + 0] = new Vector2((1f / (float)edgeCount) * (float)i, 1f);
            uvs[i * 2 + 1] = new Vector2((1f / (float)edgeCount) * (float)i, 0f);

            triangles[(i - 1) * 6 + 0] = (i - 1) * 2 + 0;
            triangles[(i - 1) * 6 + 1] = (i - 1) * 2 + 2;
            triangles[(i - 1) * 6 + 2] = (i - 1) * 2 + 1;
            triangles[(i - 1) * 6 + 3] = (i - 1) * 2 + 1;
            triangles[(i - 1) * 6 + 4] = (i - 1) * 2 + 2;
            triangles[(i - 1) * 6 + 5] = (i - 1) * 2 + 3;
        }

        meshFilter.sharedMesh = new Mesh();
        meshFilter.sharedMesh.vertices = vertices;
        meshFilter.sharedMesh.uv = uvs;
        meshFilter.sharedMesh.triangles = triangles;
    }

    void UpdateMeshVertex()
    {
        Vector3[] vertices = new Vector3[2 * mEdgeCount];
        vertices[0] = new Vector3(xPositions[0], yPositions[0], z);
        vertices[1] = new Vector3(xPositions[0], bottom, z);

        for (int i = 1; i < mEdgeCount; ++i)
        {
            vertices[i * 2 + 0] = new Vector3(xPositions[i], yPositions[i], z);
            vertices[i * 2 + 1] = new Vector3(xPositions[i], bottom, z);
        }

        meshFilter.mesh.vertices = vertices;
    }

    void SetSineValues()
    {
        float maxCompression = 1 / maxH;

        sineOffset = new float[numSineWaves];
        sineAmplitude = new float[numSineWaves];
        sineStreach = new float[numSineWaves];
        sineStreatchOffset = new float[numSineWaves];
        
        for (int i = 0; i < numSineWaves; ++i)
        {
            sineOffset[i] = (Random.value * 2f) - 1f;
            sineAmplitude[i] = Random.value * maxH;
            sineStreach[i] = Random.value * maxCompression;
            sineStreatchOffset[i] = Random.value * maxCompression;
        }
    }

    float GetSine(float x)
    {
        float res = 0f;
        for(int i = 0; i < numSineWaves; ++i)
        {
            res += sineOffset[i] + sineAmplitude[i] * (Mathf.Sin(x * sineStreach[i] + sineStreatchOffset[i] * sineStreach[i]));
        }
        return res;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < xPositions.Length; i++)
        {
            float force = springConstant * (yPositions[i] - baseHeight) + velocities[i] * damping;
            accelerations[i] = -force;
            yPositions[i] += velocities[i] + GetSine(Time.time * waveSpeed + xPositions[i]);     //add sine wave
            velocities[i] += accelerations[i];
            //Body.SetPosition(i, new Vector3(xPositions[i], yPositions[i], z));
        }
        //Now we store the difference in heights:
        float[] leftDeltas = new float[xPositions.Length];
        float[] rightDeltas = new float[xPositions.Length];

        //We make 8 small passes for fluidity:
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < xPositions.Length; i++)
            {
                //We check the heights of the nearby nodes, adjust velocities accordingly, record the height differences
                if (i > 0)
                {
                    leftDeltas[i] = spread * (yPositions[i] - yPositions[i - 1]);
                    velocities[i - 1] += leftDeltas[i];
                }
                if (i < xPositions.Length - 1)
                {
                    rightDeltas[i] = spread * (yPositions[i] - yPositions[i + 1]);
                    velocities[i + 1] += rightDeltas[i];
                }
            }

            //Now we apply a difference in position
            for (int i = 0; i < xPositions.Length; i++)
            {
                if (i > 0)
                    yPositions[i - 1] += leftDeltas[i];
                if (i < xPositions.Length - 1)
                    yPositions[i + 1] += rightDeltas[i];
            }
        }

        UpdateMeshVertex();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Use this to add floating ability by adding boyancy factor
    }

    private void OnDestroy()
    {
        for(int i = 0; i < spawnedOBJ.Count; ++i)
        {
            Destroy(spawnedOBJ[i]);
        }

        spawnedOBJ.Clear();
    }
}
