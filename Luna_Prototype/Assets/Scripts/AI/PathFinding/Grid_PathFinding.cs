using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class Grid_PathFinding
    {
        public World GameWorld;
        int playerNodeId = -1;

        public class Node
        {
            public int id = -1;
            public Platform platform;
            public Vector3 pos = new Vector3();
            public List<int> childrenID = new List<int>();

            public float gCost = 0.0f;
            public float hCost = 0.0f;
            public bool open = false;
            public bool closed = false;
            public bool blocked = false;

            public int parentID = -1;
        }
        public class Plat
        {
            public Node Left = new Node();
            public Node Right = new Node();

            public Plat(Node l, Node r)
            {
                Left = l;
                Right = r;
            }
        }

        public List<Node> mNodes = new List<Node>();
        List<Plat> mPlats = new List<Plat>();

        private List<Vector3> openList = new List<Vector3>();
        private List<Vector3> closedList = new List<Vector3>();

        private List<int> nodesComparer = new List<int>();

        public void Initialize(float length = 1.8f)
        {
            Generate(length);
        }
        void ResetNodes()
        {
            for(int i = 0; i < mNodes.Count; ++i)
            {
                mNodes[i].gCost = 0.0f;
                mNodes[i].hCost = 0.0f;
                mNodes[i].open = false;
                mNodes[i].closed = false;
                mNodes[i].parentID = -1;
            }
            openList.Clear();
            closedList.Clear();
        }

        private float CostFunction(Vector3 start, Vector3 current)
        {
            return Mathf.Abs(Vector3.SqrMagnitude(current - start));
        }
        private float HeuristicFunction(Vector3 current, Vector3 end)
        {
            return Mathf.Abs(Vector3.SqrMagnitude(end - current));
        }


        void Generate(float length)
        {
            //platform nodes
            for (int i = 0; i < GameWorld.mPlatforms.Count; ++i)
            {
                Node left = new Node();
                left.pos = GameWorld.mPlatforms[i].transform.position;
                left.pos.x -= GameWorld.mPlatforms[i].Width * 0.5f;
                left.pos.y += GameWorld.mPlatforms[i].Height;
                left.id = mNodes.Count;
                left.platform = GameWorld.mPlatforms[i];
                mNodes.Add(left);

                Node right = new Node();
                right.pos = left.pos;
                right.pos.x += GameWorld.mPlatforms[i].Width;
                right.id = mNodes.Count;
                right.platform = left.platform;
                mNodes.Add(right);

                right.childrenID.Add(left.id);
                left.childrenID.Add(right.id);
                mPlats.Add(new Plat(left, right));

                

            }
            //Jump Nodes
            for (int i = 0; i < mPlats.Count; ++i)
            {
                float xLeft = mPlats[i].Left.pos.x - length;
                float xRight = mPlats[i].Right.pos.x + length;


                List<Plat> possibleLeft = new List<Plat>();
                List<Plat> possibleRight = new List<Plat>();

                for (int j = 0; j < mPlats.Count; ++j)
                {
                    if (i == j)
                        continue;

                    if (mPlats[j].Left.pos.x < xLeft && mPlats[j].Right.pos.x > xLeft && mPlats[j].Right.pos.y < mPlats[i].Right.pos.y)
                    {
                        possibleLeft.Add(mPlats[j]);
                    }
                    if (mPlats[j].Left.pos.x < xRight && mPlats[j].Right.pos.x > xRight && mPlats[j].Right.pos.y < mPlats[i].Right.pos.y)
                    {
                        possibleRight.Add(mPlats[j]);
                    }

                }

                //get the highest platform
                int leftTop = -1;
                for(int j = 0; j < possibleLeft.Count; ++j)
                {
                    if (leftTop == -1)
                        leftTop = 0;
                    else if(leftTop > -1)
                    {
                        if(possibleLeft[leftTop].Left.pos.y < possibleLeft[j].Left.pos.y)
                        {
                            leftTop = j;
                        }
                    }
                }
                int rightTop = -1;
                for (int j = 0; j < possibleRight.Count; ++j)
                {
                    if (rightTop == -1)
                        rightTop = 0;
                    else if (rightTop > -1)
                    {
                        if (possibleRight[rightTop].Left.pos.y < possibleRight[j].Left.pos.y)
                        {
                            rightTop = j;
                        }
                    }
                }

                if(leftTop > -1)
                {
                    Node n = new Node();
                    n.pos = possibleLeft[leftTop].Left.pos;
                    n.pos.x = xLeft;
                    n.platform = possibleLeft[leftTop].Left.platform;
                    n.id = mNodes.Count;
                    n.childrenID.Add(mPlats[i].Left.id);
                    mPlats[i].Left.childrenID.Add(n.id);
                    mNodes.Add(n);
                }
                if(rightTop > -1)
                {
                    Node n = new Node();
                    n.pos = possibleRight[rightTop].Left.pos;
                    n.pos.x = xRight;
                    n.platform = possibleRight[rightTop].Left.platform;
                    n.id = mNodes.Count;
                    n.childrenID.Add(mPlats[i].Right.id);
                    mPlats[i].Right.childrenID.Add(n.id);
                    mNodes.Add(n);
                }

            }

            //Connect other jumpnodes that belong in the platform
            for(int i = 0; i < mNodes.Count; ++i)
            {
                for(int j = 0; j < mNodes.Count; ++j)
                {
                    if (i == j)
                        continue;

                    if (mNodes[i].platform != mNodes[j].platform)
                    {
                        continue;
                    }

                    bool existAsChild = false;
                    //if the node exist as its child already leave it
                    for(int k = 0; k < mNodes[i].childrenID.Count; ++k)
                    {
                        if(mNodes[i].childrenID[k] == mNodes[j].id)
                        {
                            existAsChild = true;
                            break;
                        }
                    }
                    if (existAsChild)
                        continue;
                    //now add
                    mNodes[i].childrenID.Add(mNodes[j].id);
                    mNodes[j].childrenID.Add(mNodes[i].id);
                }
            }

        }

        public void Generate(List<Platform> platformsAcc, float length = 1.8f)
        {
            //platform nodes
            for (int i = 0; i < platformsAcc.Count; ++i)
            {
                Node left = new Node();
                left.pos = platformsAcc[i].transform.position;
                left.pos.x -= platformsAcc[i].Width * 0.5f;
                left.pos.y += platformsAcc[i].Height;
                left.id = mNodes.Count;
                left.platform = platformsAcc[i];
                mNodes.Add(left);

                Node right = new Node();
                right.pos = left.pos;
                right.pos.x += platformsAcc[i].Width;
                right.id = mNodes.Count;
                right.platform = left.platform;
                mNodes.Add(right);

                right.childrenID.Add(left.id);
                left.childrenID.Add(right.id);
                mPlats.Add(new Plat(left, right));



            }
            //Jump Nodes
            for (int i = 0; i < mPlats.Count; ++i)
            {
                float xLeft = mPlats[i].Left.pos.x - length;
                float xRight = mPlats[i].Right.pos.x + length;


                List<Plat> possibleLeft = new List<Plat>();
                List<Plat> possibleRight = new List<Plat>();

                for (int j = 0; j < mPlats.Count; ++j)
                {
                    if (i == j)
                        continue;

                    if (mPlats[j].Left.pos.x < xLeft && mPlats[j].Right.pos.x > xLeft && mPlats[j].Right.pos.y < mPlats[i].Right.pos.y)
                    {
                        possibleLeft.Add(mPlats[j]);
                    }
                    if (mPlats[j].Left.pos.x < xRight && mPlats[j].Right.pos.x > xRight && mPlats[j].Right.pos.y < mPlats[i].Right.pos.y)
                    {
                        possibleRight.Add(mPlats[j]);
                    }

                }

                //get the highest platform
                int leftTop = -1;
                for (int j = 0; j < possibleLeft.Count; ++j)
                {
                    if (leftTop == -1)
                        leftTop = 0;
                    else if (leftTop > -1)
                    {
                        if (possibleLeft[leftTop].Left.pos.y < possibleLeft[j].Left.pos.y)
                        {
                            leftTop = j;
                        }
                    }
                }
                int rightTop = -1;
                for (int j = 0; j < possibleRight.Count; ++j)
                {
                    if (rightTop == -1)
                        rightTop = 0;
                    else if (rightTop > -1)
                    {
                        if (possibleRight[rightTop].Left.pos.y < possibleRight[j].Left.pos.y)
                        {
                            rightTop = j;
                        }
                    }
                }

                if (leftTop > -1)
                {
                    Node n = new Node();
                    n.pos = possibleLeft[leftTop].Left.pos;
                    n.pos.x = xLeft;
                    n.platform = possibleLeft[leftTop].Left.platform;
                    n.id = mNodes.Count;
                    n.childrenID.Add(mPlats[i].Left.id);
                    mPlats[i].Left.childrenID.Add(n.id);
                    mNodes.Add(n);
                }
                if (rightTop > -1)
                {
                    Node n = new Node();
                    n.pos = possibleRight[rightTop].Left.pos;
                    n.pos.x = xRight;
                    n.platform = possibleRight[rightTop].Left.platform;
                    n.id = mNodes.Count;
                    n.childrenID.Add(mPlats[i].Right.id);
                    mPlats[i].Right.childrenID.Add(n.id);
                    mNodes.Add(n);
                }

            }

            //Connect other jumpnodes that belong in the platform
            for (int i = 0; i < mNodes.Count; ++i)
            {
                for (int j = 0; j < mNodes.Count; ++j)
                {
                    if (i == j)
                        continue;

                    if (mNodes[i].platform != mNodes[j].platform)
                    {
                        continue;
                    }

                    bool existAsChild = false;
                    //if the node exist as its child already leave it
                    for (int k = 0; k < mNodes[i].childrenID.Count; ++k)
                    {
                        if (mNodes[i].childrenID[k] == mNodes[j].id)
                        {
                            existAsChild = true;
                            break;
                        }
                    }
                    if (existAsChild)
                        continue;
                    //now add
                    mNodes[i].childrenID.Add(mNodes[j].id);
                    mNodes[j].childrenID.Add(mNodes[i].id);
                }
            }
        }

        public int GetNearestNodeID(Vector3 pos)
        {
            int nID = 0;
            Node nearest = mNodes[nID];

            float minDisSq = Vector3.Distance(pos , nearest.pos);

            for (int i = 1; i < mNodes.Count; ++i)
            {
                float disSq = Vector3.Distance(pos, mNodes[i].pos);
                if (disSq < minDisSq && mNodes[i].pos.y <= pos.y)
                {
                    minDisSq = disSq;
                    nearest = mNodes[i];
                    nID = i;
                }
            }

            return nID;
        }

        private int GetNodeIDFrom(Vector3 pos)
        {
            for (int i = 0; i < mNodes.Count; ++i)
            {
                if (pos == mNodes[i].pos)
                {
                    return i;
                }
            }

            return -1;
        }

        void UninformedChecker(Node tempID, int parentID)
        {
            if (!tempID.open && !tempID.closed)
            {
                tempID.open = true;
                openList.Add(tempID.pos);
                tempID.parentID = parentID;
            }
        }

        private bool FindPath(Vector3 enemyPos)     //uses BFS can be switched to a*
        {
            ResetNodes();
            var startNode= mNodes[GetNearestNodeID(enemyPos)];
            openList.Add(startNode.pos);
            startNode.open = true;
            startNode.parentID = -1;

            bool found = false;

            while (!found && openList.Count > 0)
            {
                var current = mNodes[GetNearestNodeID(openList[0])];
                openList.RemoveAt(0);

                if (current.id == playerNodeId)
                    found = true;
                else
                    for (int i = 0; i < current.childrenID.Count; ++i)
                    {
                        var child = mNodes[current.childrenID[i]];

                        UninformedChecker(child, current.id);

                        /*float newG = CostFunction(current.pos, child.pos);

                        if (!child.open)
                        {
                            child.gCost = newG;
                            child.hCost = HeuristicFunction(child.pos, GameWorld.mPlayer.transform.position);

                            child.parentID = current.id;
                            int c = -1;
                            for (int k = 1; k < openList.Count; ++k)
                            {
                                var thisNode = mNodes[GetNearestNodeID(openList[k])];
                                var prvNode = mNodes[GetNearestNodeID(openList[k - 1])];
                                if (thisNode.gCost + thisNode.hCost > prvNode.gCost + prvNode.hCost)
                                {
                                    c = k;
                                    child.open = true;
                                    break;
                                }
                            }
                            if (c > -1)
                            {
                                openList.Insert(c, child.pos);
                                nodesComparer.Insert(c, child.id);
                            }
                        }
                        else if(!child.closed)
                        {
                            if(newG < child.gCost)
                            {
                                child.parentID = current.id;
                                child.gCost = newG;

                                 
                                //mOpenList.sort([this](const Coord&first, const Coord&second)
                                //{
                                //     float fOne = GetNode(first).g + GetNode(first).h;
                                //     float fSecond = GetNode(second).g + GetNode(second).h;
                                //     return fOne < fSecond;
                                // });


                                openList.Sort((first, second) => { return mNodes[GetNodeIDFrom(first)].gCost + mNodes[GetNodeIDFrom(first)].hCost < mNodes[GetNodeIDFrom(second)].gCost + mNodes[GetNodeIDFrom(second)].hCost ? 1 : 0; });
                            }
                        }

                        //UninformedChecker(mNodes[mNodes[current].childrenID[i]], current);*/
                    }

                closedList.Add(current.pos);
                current.closed = true;
            }

            return found;
        }

        public List<Vector3> GetPath()
        {
            List<Vector3> path = new List<Vector3>();

            Vector3 current = closedList[closedList.Count - 1];
            Vector3 start = closedList[0];

            path.Add(current);

            int currentID = GetNodeIDFrom(current);
            int startID = GetNodeIDFrom(start);

            while (current != start)
            {
                currentID = mNodes[currentID].parentID;
                current = mNodes[currentID].pos;
                path.Add(current);
            }

            path.Reverse();

            return path;
        }

        public void Calculate(Vector3 pos)
        {
            playerNodeId = GetNearestNodeID(GameWorld.mPlayer.transform.position);
            FindPath(pos);
        }
        public void Calculate(Vector3 Start, Vector3 end)
        {
            playerNodeId = GetNearestNodeID(GameWorld.mPlayer.transform.position);
            int endNodeId = GetNearestNodeID(end);
            FindPath(mNodes[endNodeId].pos);
        }

        public void FindPath(Vector3 enemyPos, int endID)
        {
            playerNodeId = endID;
            int endNodeId = GetNearestNodeID(enemyPos);
            FindPath(mNodes[endNodeId].pos);
        }

    }
}
