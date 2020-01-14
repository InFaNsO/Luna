using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LAI
{
   /* public class GridNode
    {
        public Vector3 pos = new Vector3();
        public int ID = -1;
        public int parentID = -1;
        public List<int> neighbourIDs = new List<int>();
    }

    [System.Serializable]
    public class Grid
    {
        private List<GridNode> mNodes = new List<GridNode>();
        private List<World.Wall> mPlatforms = new List<World.Wall>();

        [SerializeField] private float jumpHeight = 2.5f;
        [SerializeField] private World mAIWorld;



        private bool IsInRange(ref CircleCollider2D circle, World.Wall platform)
        {
            float maxY = circle.transform.position.y + circle.radius;
            float minY = circle.transform.position.y - circle.radius;

            float maxX = circle.transform.position.x + circle.radius;
            float minX = circle.transform.position.x - circle.radius;

            //if both points are above return false || below
            if (platform.from.y > maxY || platform.from.y < minY)
                return false;

            //if both are on left or right they are far away
            if (platform.from.x > maxX && platform.to.x > maxX)
                return false;
            if (platform.from.x < minX && platform.to.x < minX)
                return false;

            return true;
        }

        public void BuildGrid()
        {
            //Make the nodes on platform
            for(int i = 0; i < mAIWorld.mPlatforms.Count; ++i)
            {
                Vector3 posL = new Vector3();
                Vector3 posR = new Vector3();

                posL = mAIWorld.mPlatforms[i].transform.position;
                posR = mAIWorld.mPlatforms[i].transform.position;

                posL.x += mAIWorld.mPlatforms[i].mCollider.offset.x;
                posL.y += mAIWorld.mPlatforms[i].mCollider.offset.y;

                posR.x += mAIWorld.mPlatforms[i].mCollider.offset.x;
                posR.y += mAIWorld.mPlatforms[i].mCollider.offset.y;

                float xShift = mAIWorld.mPlatforms[i].transform.localScale.x * mAIWorld.mPlatforms[i].mCollider.size.x * 0.5f;
                float yShift = mAIWorld.mPlatforms[i].transform.localScale.y * mAIWorld.mPlatforms[i].mCollider.size.y * 0.5f;

                posL.x -= xShift;
                posR.x += xShift;

                posL.y += yShift;
                posR.y += yShift;

                GridNode nL = new GridNode();
                GridNode nR = new GridNode();
                nL.pos = posL;
                nR.pos = posR;

                nL.ID = mNodes.Count;
                nR.ID = mNodes.Count + 1;

                nR.neighbourIDs.Add(nL.ID);
                nL.neighbourIDs.Add(nR.ID);
                mNodes.Add(nL);
                mNodes.Add(nR);
            }

            //Make the nodes on ground
            for (int i = 0; i < mAIWorld.mGround.Count; ++i)
            {
                Vector3 posL = new Vector3();
                Vector3 posR = new Vector3();

                posL = mAIWorld.mGround[i].transform.position;
                posR = mAIWorld.mGround[i].transform.position;

                posL.x += mAIWorld.mGround[i].mCollider.offset.x;
                posL.y += mAIWorld.mGround[i].mCollider.offset.y;

                posR.x += mAIWorld.mGround[i].mCollider.offset.x;
                posR.y += mAIWorld.mGround[i].mCollider.offset.y;

                float xShift = mAIWorld.mGround[i].transform.localScale.x * mAIWorld.mPlatforms[i].mCollider.size.x * 0.5f;
                float yShift = mAIWorld.mGround[i].transform.localScale.y * mAIWorld.mPlatforms[i].mCollider.size.y * 0.5f;

                posL.x -= xShift;
                posR.x += xShift;

                posL.y
 += yShift;
                posR.y += yShift;

                GridNode nL = new GridNode();
                GridNode nR = new GridNode();
                nL.pos = posL;
                nR.pos = posR;

                nL.ID = mNodes.Count;
                nR.ID = mNodes.Count + 1;

                nR.neighbourIDs.Add(nL.ID);
                nL.neighbourIDs.Add(nR.ID);
                mNodes.Add(nL);
                mNodes.Add(nR);
            }

            //make the platform walls
            for(int i = 0; i < mNodes.Count; )
            {
                Vector2 from = mNodes[i].pos;
                Vector2 to = mNodes[i + 1].pos;

                World.Wall platform = new World.Wall(from, to);
                mPlatforms.Add(platform);

                i += 2;
            }

            //Make jump Nodes
            List<GridNode> jumpNode = new List<GridNode>();

            for(int i = 0; i < mPlatforms.Count; ++i)
            {
                World.Obstacle jumpL;
                World.Obstacle jumpR;

                jumpL.center = mPlatforms[i].from;
                jumpR.center = mPlatforms[i].to;

                jumpL.radius = jumpHeight;
                jumpR.radius = jumpHeight;

                //do line and circle collision check
                CircleCollider2D cc = new CircleCollider2D();
                cc.enabled = true;
                cc.transform.position = mPlatforms[i].from;
                cc.radius = jumpHeight;

                //check jump point on left node
                for(int j = 0; j < mPlatforms.Count; ++j)
                {
                    if (j == i)
                        continue;
                    if (!IsInRange(ref cc, mPlatforms[j]))
                        continue;
                    RaycastHit2D hit = Physics2D.Raycast(mPlatforms[i].to, mPlatforms[i].from - mPlatforms[i].to, (mPlatforms[i].from - mPlatforms[i].to).magnitude, 26);
                    if (hit.collider)
                    {
                        GridNode n = new GridNode();
                        n.pos = hit.point;
                        n.neighbourIDs.Add(mNodes[j * 2].ID);
                        n.neighbourIDs.Add(mNodes[(j * 2) + 1].ID);

                        mNodes[j * 2].neighbourIDs.Add(mNodes.Count + jumpNode.Count);
                        mNodes[(j * 2) + 1].neighbourIDs.Add(mNodes.Count + jumpNode.Count);

                        jumpNode.Add(n);
                    }
                }

                //check jump point on Rigth node
                cc.transform.position = mPlatforms[i].to;
                for (int j = 0; j < mPlatforms.Count; ++j)
                {
                    if (j == i)
                        continue;
                    if (!IsInRange(ref cc, mPlatforms[j]))
                        continue;
                    RaycastHit2D hit = Physics2D.Raycast(mPlatforms[i].to, mPlatforms[i].from - mPlatforms[i].to, (mPlatforms[i].from - mPlatforms[i].to).magnitude, 26);
                    if (hit.collider)
                    {
                        GridNode n = new GridNode();
                        n.pos = hit.point;
                        n.neighbourIDs.Add(mNodes[j * 2].ID);
                        n.neighbourIDs.Add(mNodes[(j * 2) + 1].ID);

                        mNodes[j * 2].neighbourIDs.Add(mNodes.Count + jumpNode.Count);
                        mNodes[(j * 2) + 1].neighbourIDs.Add(mNodes.Count + jumpNode.Count);

                        jumpNode.Add(n);
                    }
                }
                cc.enabled = false;
            }

            //add the jump nodes in the main node list
            for(int i = 0; i < jumpNode.Count; ++i)
            {
                mNodes.Add(jumpNode[i]);
            }

        }


        Vector3 GetClosestNode(Vector3 position)
        {
            Vector3 pos = mNodes[0].pos; //new Vector3();
            float len = (pos - position).sqrMagnitude;

            for(int i = 0; i < mNodes.Count; ++i)
            {
                if (mNodes[i].pos.y > position.y + 1.0f)
                    continue;

                if((mNodes[i].pos - position).sqrMagnitude < len)
                {
                    len = (mNodes[i].pos - position).sqrMagnitude;
                    pos = mNodes[i].pos;
                }
            }

            return pos;
        }


        public void MakePath(Vector3 enemyPos)
        {
            MakePath(enemyPos, mAIWorld.mPlayer.transform.position);
        }

        public void MakePath(Vector3 enemyPos, Vector3 playerPos)
        {

        }
    }*/
}


