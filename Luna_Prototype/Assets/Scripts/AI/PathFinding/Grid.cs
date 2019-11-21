using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid : MonoBehaviour
{
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask[] costMasks;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private float cellRadius;
    [SerializeField] private float nodeDistance;

    private Node[,] mNodeArray;
    public List<Node> finalPath;

    private float cellDiameter;
    private Vector2Int gridSizeArray;

    void Start()
    {
        cellDiameter = cellRadius * 2.0f;
        gridSizeArray.x = Mathf.RoundToInt(gridSize.x / cellDiameter);
        gridSizeArray.y = Mathf.RoundToInt(gridSize.y / cellDiameter);
    }

    public void CreateGrid()
    {
        mNodeArray = new Node[gridSizeArray.x, gridSizeArray.y];
        Vector3 buttomLeft = transform.position;
        for(int i = 0; i < gridSizeArray.x; ++i)
        {
            for(int j = 0; j < gridSizeArray.y; ++j)
            {
                Vector3 worldPos = buttomLeft + Vector3.right * (i * cellDiameter + cellRadius) + Vector3.up * (j * cellDiameter + cellRadius);
                bool wall = true;

                if (Physics.CheckSphere(worldPos, cellRadius, wallMask))
                {
                    wall = false;
                }
                mNodeArray[i, j] = new Node(wall, worldPos, i, j, GetCost(worldPos));
            }
        }
    }

    private float GetCost(Vector3 pos)
    {
        for(int i = 0; i < costMasks.Length; ++i)
        {
            if (Physics.CheckSphere(pos, cellRadius, costMasks[i]))
            {
                if (costMasks[i] == 11)          //it is fire type
                    return 3.0f;
                else if (costMasks[i] == 12)     //it is water type
                    return 2.0f;
            }
        }

        return 0.0f;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbourList = new List<Node>();
        Vector2Int check = new Vector2Int();

        //check right side
        check.x = node.gridX + 1;
        check.y = node.gridY;
        if (check.x >= 0 && check.x < gridSizeArray.x)
            if (check.y >= 0 && check.y < gridSizeArray.y)
                neighbourList.Add(mNodeArray[check.x, check.y]);
        //check left side
        check.x = node.gridX - 1;
        check.y = node.gridY;
        if (check.x >= 0 && check.x < gridSizeArray.x)
            if (check.y >= 0 && check.y < gridSizeArray.y)
                neighbourList.Add(mNodeArray[check.x, check.y]);
        //check top side
        check.x = node.gridX;
        check.y = node.gridY + 1;
        if (check.x >= 0 && check.x < gridSizeArray.x)
            if (check.y >= 0 && check.y < gridSizeArray.y)
                neighbourList.Add(mNodeArray[check.x, check.y]);
        //check buttom side
        check.x = node.gridX;
        check.y = node.gridY - 1;
        if (check.x >= 0 && check.x < gridSizeArray.x)
            if (check.y >= 0 && check.y < gridSizeArray.y)
                neighbourList.Add(mNodeArray[check.x, check.y]);

        return neighbourList;
    }

    public Node NodeFromWorldPoint(Vector3 pos)
    {
        float xPos = ((pos.x + gridSize.x / 2.0f) / gridSize.x);
        float yPos = ((pos.y + gridSize.y / 2.0f) / gridSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x = Mathf.RoundToInt((gridSizeArray.x - 1) * xPos);
        int y = Mathf.RoundToInt((gridSizeArray.y - 1) * yPos);

        return mNodeArray[x, y];
    }

    void Update()
    {
        
    }
}
