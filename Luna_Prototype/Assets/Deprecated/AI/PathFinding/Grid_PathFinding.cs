using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class Grid_PathFinding
    {
        public class Node
        {
            public Vector3 coord = new Vector3();
            public float gCost = 0.0f;
            public float hCost = 0.0f;
            public bool open = false;
            public bool closed = false;
            public bool blocked = false;

            public int parentID = -1;

            public List<int> children = new List<int>();

            public Node(Node n)
            {
                coord = n.coord;
                gCost = n.gCost;
                hCost = n.hCost;
                open = n.open;
                closed = n.closed;
                blocked = n.blocked;
                children = new List<int>();
                children = n.children;
            }
            public Node()
            {

            }
        }

        public class Platform_Line
        {
            public Node left = new Node();
            public int leftID = -1;
            public Node right = new Node();
            public int rightID = -1;
        }

        private class IntersectionData
        {
            public int platformID = -1;
            public Vector3 coordinate = new Vector3();
        }

        public List<int> PlatformsAccecible = new List<int>();
        public int groundID = -1;


        public List<Node> mNodes = new List<Node>();
        [SerializeField] public World gameWorld;
        //private LayerMask mask;
        //private List<LayerMask> toBlock = new List<LayerMask>();          Could be used to set spme paths as blocked

        private List<Platform_Line> platformNodes = new List<Platform_Line>();
        private List<Platform> platforms = new List<Platform>();

        private List<IntersectionData> platformIntersectionData = new List<IntersectionData>();

        private List<Vector3> openList = new List<Vector3>();
        private List<Vector3> closedList = new List<Vector3>();

        int PlayeNodeID = -1;

        [SerializeField] private float jumpHeight = 3.0f;

        private float sgn(float f)
        {
            if (f < 0.0f)
                return -1.0f;
            else
                return 1.0f;
        }

        private void CheckInterseactions(Platform_Line self, Vector3 coord)
        {
            bool isLeftCoord = false;
            if (coord == self.left.coord)
                isLeftCoord = true;


            for (int i = 0; i < platformNodes.Count; ++i)
            {
                if (platformNodes[i].left.coord == self.left.coord)
                {
                    //its the same so go to next one
                    continue;
                }

                IntersectionData id = new IntersectionData();
                id.platformID = i;

                float lenLefSq = (platformNodes[i].left.coord - coord).sqrMagnitude;
                float lenRigSq = (platformNodes[i].right.coord - coord).sqrMagnitude;

                if (lenLefSq < (jumpHeight * jumpHeight) && lenRigSq < (jumpHeight * jumpHeight))    //the platform is inside the circle
                {
                    if (platformNodes[i].left.coord.y > coord.y)     //check if above then leave if below then add
                        continue;   //above
                    else
                    {
                        float difL = platformNodes[i].left.coord.x - coord.x;
                        float difR = platformNodes[i].right.coord.x - coord.x;
                        if (coord == self.left.coord)
                            if (difL < difR && difL < 0.0f) //add left
                            { id.coordinate = new Vector3(platformNodes[i].left.coord.x + 0.01f, platformNodes[i].left.coord.y, platformNodes[i].left.coord.z); platformIntersectionData.Add(id); }
                            else //add right
                            { id.coordinate = new Vector3(platformNodes[i].right.coord.x - 0.01f, platformNodes[i].right.coord.y, platformNodes[i].right.coord.z); platformIntersectionData.Add(id); }
                        else    //get the closest point on its right
                            if (difL < difR && difL > 0.0f) //add left
                        { id.coordinate = new Vector3(platformNodes[i].left.coord.x + 0.01f, platformNodes[i].left.coord.y, platformNodes[i].left.coord.z); platformIntersectionData.Add(id); }
                        else //add right
                        { id.coordinate = new Vector3(platformNodes[i].right.coord.x - 0.01f, platformNodes[i].right.coord.y, platformNodes[i].right.coord.z); }
                    }
                    continue;
                }

                //left side is inside
                if (lenLefSq < jumpHeight * jumpHeight)
                {
                    if (platformNodes[i].left.coord.y > coord.y)     //ITS ABOVE
                    {
                        //if the platforms left node is on its right then it is a node 
                        if (platformNodes[i].left.coord.x > coord.x)
                        {
                            //it is add
                            Vector3 jpN = platformNodes[i].left.coord;
                            jpN.x += 0.1f;
                            id.coordinate = jpN;
                            platformIntersectionData.Add(id);
                        }
                        continue;
                    }

                    if (isLeftCoord)
                    {
                        //Its Below
                        if (platformNodes[i].left.coord.x < coord.x)
                        {
                            //it is add
                            Vector3 jpN = platformNodes[i].left.coord;
                            jpN.x += 0.1f;
                            id.coordinate = jpN;
                            platformIntersectionData.Add(id);
                        }
                    }
                    else
                    {
                        //Its Below
                        if (platformNodes[i].left.coord.x > coord.x)
                        {
                            //it is add
                            Vector3 jpN = platformNodes[i].left.coord;
                            jpN.x += 0.1f;
                            id.coordinate = jpN;
                            platformIntersectionData.Add(id);
                        }

                    }
                    continue;
                }
                //right side is inside
                if (lenRigSq < jumpHeight * jumpHeight)
                {
                    if (platformNodes[i].right.coord.y > coord.y)     //ITS ABOVE
                    {
                        //if the platforms right node is on its left then it is a node 
                        if (platformNodes[i].right.coord.x < coord.x)
                        {
                            //it is add
                            Vector3 jpN = platformNodes[i].right.coord;
                            jpN.x += 0.1f;
                            id.coordinate = jpN;
                            platformIntersectionData.Add(id);

                        }
                        continue;
                    }
                    if (isLeftCoord)                                            //Its Below
                    {
                        if (platformNodes[i].right.coord.x < coord.x)
                        {
                            //it is add
                            Vector3 jpN = platformNodes[i].left.coord;
                            jpN.x += 0.1f;
                            id.coordinate = jpN;
                            platformIntersectionData.Add(id);
                        }
                    }
                    else
                    {
                        if (platformNodes[i].right.coord.x > coord.x)
                        {
                            //it is add
                            Vector3 jpN = platformNodes[i].left.coord;
                            jpN.x += 0.1f;
                            id.coordinate = jpN;
                            platformIntersectionData.Add(id);
                        }
                    }
                    continue;
                }


                if (platformNodes[i].left.coord.y > coord.y - jumpHeight && platformNodes[i].left.coord.y < coord.y)
                {
                    //the platform might be in the range but the points are outside of the circle
                    if (platformNodes[i].left.coord.x < coord.x)
                    {
                        if (platformNodes[i].right.coord.x > coord.x)
                        {
                            //the platform is in the range find the point
                            float x2 = Mathf.Sqrt((jumpHeight * jumpHeight) - ((platformNodes[i].left.coord.y - coord.y) * (platformNodes[i].left.coord.y - coord.y))) + coord.x;

                            Vector3 jpn = platformNodes[i].right.coord;
                            jpn.x = x2;
                            id.coordinate = jpn;
                            platformIntersectionData.Add(id);
                            continue;

                        }
                    }
                }
            }
        }

        private void GenerateJumpPoints()
        {
            //check intersection and get all the points and connect them
            for (int i = 0; i < platformNodes.Count; ++i)
            {
                //left
                CheckInterseactions(platformNodes[i], platformNodes[i].left.coord);

                for (int j = 0; j < platformIntersectionData.Count; ++j)
                {
                    Node n = new Node();
                    n.coord = platformIntersectionData[j].coordinate;
                    float dX = (n.coord.x - platformNodes[i].left.coord.x) * 2.0f;
                    n.coord.x -= dX;
                    mNodes.Add(n);

                    platformNodes[i].left.children.Add(mNodes.Count - 1);
                    platformNodes[platformIntersectionData[j].platformID].left.children.Add(mNodes.Count - 1);
                    platformNodes[platformIntersectionData[j].platformID].right.children.Add(mNodes.Count - 1);

                    mNodes[mNodes.Count - 1].children.Add(platformNodes[i].leftID);
                    mNodes[mNodes.Count - 1].children.Add(platformNodes[platformIntersectionData[j].platformID].leftID);
                    mNodes[mNodes.Count - 1].children.Add(platformNodes[platformIntersectionData[j].platformID].rightID);
                }

                platformIntersectionData.Clear();

                //repeat for right
                CheckInterseactions(platformNodes[i], platformNodes[i].right.coord);

                for (int j = 0; j < platformIntersectionData.Count; ++j)
                {
                    Node n = new Node();
                    n.coord = platformIntersectionData[j].coordinate;
                    mNodes.Add(n);

                    platformNodes[i].right.children.Add(mNodes.Count - 1);
                    platformNodes[platformIntersectionData[j].platformID].left.children.Add(mNodes.Count - 1);
                    platformNodes[platformIntersectionData[j].platformID].right.children.Add(mNodes.Count - 1);

                    mNodes[mNodes.Count - 1].children.Add(platformNodes[i].rightID);
                    mNodes[mNodes.Count - 1].children.Add(platformNodes[platformIntersectionData[j].platformID].leftID);
                    mNodes[mNodes.Count - 1].children.Add(platformNodes[platformIntersectionData[j].platformID].rightID);
                }
                platformIntersectionData.Clear();
            }
        }

        private void GetAllPLatforms()
        {
            //platforms
            if(PlatformsAccecible.Count == 0)
                for (int i = 0; i < gameWorld.mPlatforms.Count; ++i)
                {
                    Platform_Line pl = new Platform_Line();
                    pl.left.coord = gameWorld.mPlatforms[i].transform.position;
                    pl.right.coord = gameWorld.mPlatforms[i].transform.position;

                    pl.left.coord.x -= gameWorld.mPlatforms[i].Width * 0.5f;
                    pl.right.coord.x += gameWorld.mPlatforms[i].Width * 0.5f;

                    pl.left.coord.y += gameWorld.mPlatforms[i].Height * 0.5f;
                    pl.right.coord.y += gameWorld.mPlatforms[i].Height * 0.5f;

                    mNodes.Add(pl.left);
                    pl.leftID = mNodes.Count - 1;
                    mNodes.Add(pl.right);
                    pl.rightID = mNodes.Count - 1;

                    //add each other as a child
                    mNodes[pl.leftID].children.Add(pl.rightID);
                    mNodes[pl.rightID].children.Add(pl.leftID);

                    platformNodes.Add(pl);
                }
            else
                for (int i = 0; i < PlatformsAccecible.Count; ++i)
                {
                    Platform_Line pl = new Platform_Line();
                    pl.left.coord = gameWorld.mPlatforms[PlatformsAccecible[i]].transform.position;
                    pl.right.coord = gameWorld.mPlatforms[PlatformsAccecible[i]].transform.position;

                    pl.left.coord.x -= gameWorld.mPlatforms[PlatformsAccecible[i]].Width * 0.5f;
                    pl.right.coord.x += gameWorld.mPlatforms[PlatformsAccecible[i]].Width * 0.5f;

                    pl.left.coord.y += gameWorld.mPlatforms[PlatformsAccecible[i]].Height * 0.5f;
                    pl.right.coord.y += gameWorld.mPlatforms[PlatformsAccecible[i]].Height * 0.5f;

                    mNodes.Add(pl.left);
                    pl.leftID = mNodes.Count - 1;
                    mNodes.Add(pl.right);
                    pl.rightID = mNodes.Count - 1;

                    //add each other as a child
                    mNodes[pl.leftID].children.Add(pl.rightID);
                    mNodes[pl.rightID].children.Add(pl.leftID);

                    platformNodes.Add(pl);
                }

            //ground
            if(groundID == -1)
                for (int i = 0; i < gameWorld.mGround.Count; ++i)
            {
                Platform_Line pl = new Platform_Line();
                pl.left.coord = gameWorld.mGround[i].transform.position;
                pl.right.coord = gameWorld.mGround[i].transform.position;

                pl.left.coord.x -= gameWorld.mGround[i].Width * 0.5f;
                pl.right.coord.x += gameWorld.mGround[i].Width * 0.5f;

                pl.left.coord.y += gameWorld.mGround[i].Height * 0.5f;
                pl.right.coord.y += gameWorld.mGround[i].Height * 0.5f;

                mNodes.Add(pl.left);
                pl.leftID = mNodes.Count - 1;
                mNodes.Add(pl.right);
                pl.rightID = mNodes.Count - 1;

                //add each other as a child
                mNodes[pl.leftID].children.Add(pl.rightID);
                mNodes[pl.rightID].children.Add(pl.leftID);

                platformNodes.Add(pl);
            }
            else
            {
                Platform_Line pl = new Platform_Line();
                pl.left.coord = gameWorld.mGround[groundID].transform.position;
                pl.right.coord = gameWorld.mGround[groundID].transform.position;

                pl.left.coord.x -= gameWorld.mGround[groundID].Width * 0.5f;
                pl.right.coord.x += gameWorld.mGround[groundID].Width * 0.5f;

                pl.left.coord.y += gameWorld.mGround[groundID].Height * 0.5f;
                pl.right.coord.y += gameWorld.mGround[groundID].Height * 0.5f;

                mNodes.Add(pl.left);
                pl.leftID = mNodes.Count - 1;
                mNodes.Add(pl.right);
                pl.rightID = mNodes.Count - 1;

                //add each other as a child
                mNodes[pl.leftID].children.Add(pl.rightID);
                mNodes[pl.rightID].children.Add(pl.leftID);

                platformNodes.Add(pl);
            }
            GenerateJumpPoints();
        }

        public void Initialize()
        {
            GetAllPLatforms();
        }
        public void Initialize(ref List<Platform> platforms)
        {
            for (int i = 0; i < platforms.Count; ++i)
            {
                Platform_Line pl = new Platform_Line();
                pl.left.coord = platforms[i].transform.position;
                pl.right.coord = platforms[i].transform.position;

                pl.left.coord.x -= platforms[i].Width * 0.5f;
                pl.right.coord.x += platforms[i].Width * 0.5f;

                pl.left.coord.y += platforms[i].Height * 0.5f;
                pl.right.coord.y += platforms[i].Height * 0.5f;

                mNodes.Add(pl.left);
                pl.leftID = mNodes.Count - 1;
                mNodes.Add(pl.right);
                pl.rightID = mNodes.Count - 1;

                //add each other as a child
                mNodes[pl.leftID].children.Add(pl.rightID);
                mNodes[pl.rightID].children.Add(pl.leftID);

                platformNodes.Add(pl);
            }

            GenerateJumpPoints();
        }

        private void Reset()
        {
            for (int i = 0; i < mNodes.Count; ++i)
            {
                mNodes[i].closed = false;
                mNodes[i].open = false;
                mNodes[i].gCost = 0.0f;
                mNodes[i].hCost = 0.0f;
                mNodes[i].parentID = -1;
            }

            openList.Clear();
            closedList.Clear();
        }

        public int GetNearestNodeID(Vector3 pos)
        {
            int nID = 0;
            Node nearest = mNodes[nID];

            float minDisSq = (pos - nearest.coord).sqrMagnitude;

            for (int i = 1; i < mNodes.Count; ++i)
            {
                float disSq = (pos - mNodes[i].coord).sqrMagnitude;
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
                if (pos == mNodes[i].coord)
                {
                    return i;
                }
            }

            return -1;
        }

        void UninformedChecker(int tempID, int parentID)
        {
            if (!mNodes[tempID].open && !mNodes[tempID].closed)
            {
                mNodes[tempID].open = true;
                openList.Add(mNodes[tempID].coord);
                mNodes[tempID].parentID = parentID;
            }
        }

        private bool FindPath(Vector3 enemyPos)     //uses BFS can be switched to a*
        {
            Reset();
            openList.Add(enemyPos);

            mNodes[GetNearestNodeID(enemyPos)].open = true;
            bool found = false;

            while (!found && openList.Count > 0)
            {
                int current = GetNearestNodeID(openList[0]);
                openList.RemoveAt(0);

                if (current == PlayeNodeID)
                    found = true;
                else
                    for (int i = 0; i < mNodes[current].children.Count; ++i)
                    {
                        UninformedChecker(mNodes[current].children[i], current);
                    }

                closedList.Add(mNodes[current].coord);
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
                current = mNodes[currentID].coord;
                path.Add(current);
            }

            path.Reverse();

            return path;
        }

        // Update is called once per frame
        public void Calculate(Vector3 pos)
        {
            PlayeNodeID = GetNearestNodeID(gameWorld.mPlayer.transform.position);
            FindPath(pos);
        }

        public void Calculate(Vector3 start, Vector3 end)
        {
            PlayeNodeID = GetNodeIDFrom(end);
            FindPath(start);
        }
    }
}
