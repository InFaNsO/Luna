using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public int gridX;//X Position in the Node Array
    public int gridY;//Y Position in the Node Array

    public bool isWall;//Tells the program if this node is being obstructed.
    public Vector3 position;//The world position of the node.

    public Node parentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

    public float gCost;//The cost of moving to this square.
    public float hCost;//The distance to the goal from this node.

    public float FCost { get { return gCost + hCost; } }//Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.

    public Node(bool a_bIsWall, Vector3 a_vPos, int a_igridX, int a_igridY, float gcost)//Constructor
    {
        isWall = a_bIsWall;
        position = a_vPos;
        gridX = a_igridX;
        gridY = a_igridY;
        gCost = gcost;
    }

}
