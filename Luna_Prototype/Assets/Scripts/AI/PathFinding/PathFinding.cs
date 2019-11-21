using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    public Transform startPosition;
    public Transform endPosition;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void FindPathAstar(Vector3 startWorld, Vector3 endWorld)
    {
        Node start = grid.NodeFromWorldPoint(startWorld);
        Node end = grid.NodeFromWorldPoint(endWorld);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for(int i = 1; i < openList.Count; ++i)
            {
                if(openList[i].FCost < currentNode.FCost ||
                    openList[i].FCost == currentNode.FCost &&
                    openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == end)//If the current node is the same as the target node
            {
                GetFinalPath(start, end);//Calculate the final path
            }

            foreach (Node NeighborNode in grid.GetNeighbours(currentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.isWall || closedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                float MoveCost = currentNode.gCost + GetManhattenDistance(currentNode, NeighborNode);//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.gCost || !openList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.gCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, end);//Set the h cost
                    NeighborNode.parentNode = currentNode;//Set the parent of the node for retracing steps

                    if (!openList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        openList.Add(NeighborNode);//Add it to the list
                    }
                }
            }
        }
    }


    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.parentNode;//Move onto its parent node
        }

        FinalPath.Reverse();//Reverse the path to get the correct order

        grid.finalPath = FinalPath;//Set the final path

    }

    float GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);//y1-y2

        return ix + iy;//Return the sum
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
