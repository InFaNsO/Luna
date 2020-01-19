using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class Grid_PathFinding
    {
        public World GameWorld = new World();
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
        class Plat
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

        public void Initialize(float length = 3.0f)
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
                for(int j = 0; j < mPlats.Count; ++j)
                {
                    if (i == j)
                        continue;

                    var points = IntersectionPoint(mPlats[j].Left.pos, mPlats[j].Right.pos, mPlats[i].Left.pos, length);
                    if(points.Count > 0)
                    {
                        //add the points as nodes and mark children
                        for(int k = 0; i < points.Count; ++k)
                        {
                            if(points[k].x < mPlats[i].Left.pos.x)
                            {
                                Node n = new Node();
                                n.pos = points[k];
                                n.id = mNodes.Count;
                                n.platform = GameWorld.mPlatforms[j];
                                n.childrenID.Add(mPlats[i].Left.id);
                                n.childrenID.Add(mPlats[j].Left.id);
                                n.childrenID.Add(mPlats[j].Right.id);
                                mNodes.Add(n);
                                continue;
                            }
                        }
                        continue;
                    }
                    points = IntersectionPoint(mPlats[j].Left.pos, mPlats[j].Right.pos, mPlats[i].Right.pos, length);
                    if(points.Count > 0)
                    {
                        for (int k = 0; i < points.Count; ++k)
                        {
                            if (points[k].x > mPlats[i].Right.pos.x)
                            {
                                Node n = new Node();
                                n.pos = points[k];
                                n.id = mNodes.Count;
                                n.platform = GameWorld.mPlatforms[j];
                                n.childrenID.Add(mPlats[i].Right.id);
                                n.childrenID.Add(mPlats[j].Left.id);
                                n.childrenID.Add(mPlats[j].Right.id);
                                mNodes.Add(n);
                                continue;
                            }
                        }
                    }
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

        public List<Vector3> IntersectionPoint(Vector3 p1, Vector3 p2, Vector3 center, float radius)
        {
            Vector3 dp = new Vector3();
            List<Vector3> sect = new List<Vector3>();
            float a, b, c;
            float bb4ac;
            float mu1;
            float mu2;

            //  get the distance between X and Z on the segment
            dp.x = p2.x - p1.x;
            dp.z = p2.z - p1.z;
            //   I don't get the math here
            a = dp.x * dp.x + dp.z * dp.z;
            b = 2 * (dp.x * (p1.x - center.x) + dp.z * (p1.z - center.z));
            c = center.x * center.x + center.z * center.z;
            c += p1.x * p1.x + p1.z * p1.z;
            c -= 2 * (center.x * p1.x + center.z * p1.z);
            c -= radius * radius;
            bb4ac = b * b - 4 * a * c;
            if (Mathf.Abs(a) < float.Epsilon || bb4ac < 0)
            {
                //  line does not intersect
                return new List<Vector3>();
            }
            mu1 = (-b + Mathf.Sqrt(bb4ac)) / (2 * a);
            mu2 = (-b - Mathf.Sqrt(bb4ac)) / (2 * a);
            //sect = new Vector3[2];
            sect.Add(new Vector3(p1.x + mu1 * (p2.x - p1.x), 0, p1.z + mu1 * (p2.z - p1.z)));
            sect.Add(new Vector3(p1.x + mu2 * (p2.x - p1.x), 0, p1.z + mu2 * (p2.z - p1.z)));

            return sect;
        }

        public int GetNearestNodeID(Vector3 pos)
        {
            int nID = 0;
            Node nearest = mNodes[nID];

            float minDisSq = (pos - nearest.pos).sqrMagnitude;

            for (int i = 1; i < mNodes.Count; ++i)
            {
                float disSq = (pos - mNodes[i].pos).sqrMagnitude;
                if (disSq < minDisSq)
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
            openList.Add(enemyPos);

            mNodes[GetNearestNodeID(enemyPos)].open = true;
            bool found = false;

            while (!found && openList.Count > 0)
            {
                int current = GetNearestNodeID(openList[0]);
                openList.RemoveAt(0);

                if (current == playerNodeId)
                    found = true;
                else
                    for (int i = 0; i < mNodes[current].childrenID.Count; ++i)
                    {
                        UninformedChecker(mNodes[mNodes[current].childrenID[i]], current);
                    }

                closedList.Add(mNodes[current].pos);
                mNodes[current].closed = true;
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

    }
}
