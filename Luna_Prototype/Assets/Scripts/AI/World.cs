using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [System.Serializable]
    public struct Wall
    {
        public Wall(Vector2 f, Vector2 t)
        {
            from = f;
            to = t;
        }
        public Vector2 from;
        public Vector2 to;
    }
    [System.Serializable]
    public struct Obstacle
    {
        public Vector2 center;
        public float radius;
    }
    private struct Line
    {
        public float m;
        public float b;

        public float findX(float y)
        {
            return (y - b) / m;
        }
        public float GetX(Line l)
        {
            return (l.b - b) / (m - l.m);
        }

        public float findY(float x)
        {
            return m * x + b;
        }

        public void SetMB(Vector2 p1, Vector2 p2)
        {
            m = (p2.y - p1.y) / (p2.x - p1.x);
            b = p1.y - m * p1.x;
        }

        public float GetInverseM()
        {
            return (1.0f / m) * -1.0f;
        }
        public void SetB(Vector2 p)
        {
            b = p.y - m * p.x;
        }
    }


    [SerializeField] private List<Wall> mWalls;
    [SerializeField] private List<Obstacle> mObstacles;
    [SerializeField] private List<Agent> mAgents;
    [SerializeField] public Agent mPlayer;

    private int orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        int val = (int)(q.y - p.y) * (int)(r.x - q.x) -
        (int)(q.x - p.x) * (int)(r.y - q.y);

        if (val == 0) return 0;
        if (val > 0) return 1;
        return 2;
    }

    private bool onSegment(Vector2 p, Vector2 q, Vector2 r)
    {
        if (q.x <=  Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) &&
            q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y))
        {
            return true;
        }
        return false;
    }

    public bool HasLineOfSight(Wall lineOfSight)
    {
        foreach (Wall line in mWalls)
        {
            // Find the four orientations needed for general and
            // special cases
            //p1 = line.from	q1 = lint.to	p2 = los.from	q2 = los.to
            int o1 = orientation(line.from, line.to, lineOfSight.from);
            int o2 = orientation(line.from, line.to, lineOfSight.to);
            int o3 = orientation(lineOfSight.from, lineOfSight.to, line.from);
            int o4 = orientation(lineOfSight.from, lineOfSight.to, line.to);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1
            if (o1 == 0 && onSegment(line.from, lineOfSight.from, line.to)) return true;

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1
            if (o2 == 0 && onSegment(line.from, lineOfSight.to, line.to)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2
            if (o3 == 0 && onSegment(lineOfSight.from, line.from, lineOfSight.to)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2
            if (o4 == 0 && onSegment(lineOfSight.from, line.to, lineOfSight.to)) return true;
        }
        Line l = new Line();
        l.SetMB(lineOfSight.from, lineOfSight.to);
        Line inv = l;
        inv.m = l.GetInverseM();


        foreach (Obstacle circle in mObstacles)
        {
            inv.SetB(circle.center);
            Vector2 pointOnLine = new Vector2();
            pointOnLine.x = l.GetX(inv);
            pointOnLine.y = l.findY(pointOnLine.x);

            if((lineOfSight.from - pointOnLine).SqrMagnitude() + (lineOfSight.to - pointOnLine).SqrMagnitude() == (lineOfSight.from - lineOfSight.to).SqrMagnitude())
            {
                //the point is on line
                if((pointOnLine - circle.center).SqrMagnitude() <= (circle.radius * circle.radius))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AddAgent(Agent a) { mAgents.Add(a); }

    public void Update()
    {
        
    }
    public void Start()
    {
        Wall w = new Wall();
        w.from = new Vector2(-7.0f, 3.0f);
        w.to = new Vector2(-7.0f, 2.0f);

        // mWalls.Add(new Wall(new Vector2(-7.0f, 3.0f), new Vector2(-7.0f, 2.0f)));
        // mWalls.Add(new Wall(new Vector2(-7.0f, 2.0f), new Vector2(-1.0f, 2.0f)));
        // mWalls.Add(new Wall(new Vector2(-1.0f, 2.0f), new Vector2(-1.0f, 1.0f)));
        // mWalls.Add(new Wall(new Vector2(-1.0f, 1.0f), new Vector2(3.0f, 1.0f)));
        // mWalls.Add(new Wall(new Vector2(1.0f, 0.0f), new Vector2(-8.0f, 0.0f)));
        // mWalls.Add(new Wall(new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1.0f)));
        // mWalls.Add(new Wall(new Vector2(-7.0f, 0.0f), new Vector2(-7.0f, 2.0f)));

        mWalls.Add(new Wall(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f)));
        mWalls.Add(new Wall(new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f)));
        mWalls.Add(new Wall(new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f)));
        mWalls.Add(new Wall(new Vector2(0.0f, 1.0f), new Vector2(1.0f, 1.0f)));

        mWalls.Add(new Wall(new Vector2(3.0f, 1.0f), new Vector2(4.0f, 1.0f)));
        mWalls.Add(new Wall(new Vector2(4.0f, 1.0f), new Vector2(4.0f, 2.0f)));
        mWalls.Add(new Wall(new Vector2(4.0f, 2.0f), new Vector2(3.0f, 2.0f)));
        mWalls.Add(new Wall(new Vector2(3.0f, 2.0f), new Vector2(3.0f, 1.0f)));

        mWalls.Add(new Wall(new Vector2(-2.0f, 1.0f), new Vector2(-3.0f, 1.0f)));
        mWalls.Add(new Wall(new Vector2(-3.0f, 1.0f), new Vector2(-3.0f, 2.0f)));
        mWalls.Add(new Wall(new Vector2(-3.0f, 2.0f), new Vector2(-2.0f, 2.0f)));
        mWalls.Add(new Wall(new Vector2(-2.0f, 2.0f), new Vector2(-2.0f, 1.0f)));

        mWalls.Add(new Wall(new Vector2(-6.0f, -2.0f), new Vector2(6.0f, 1.0f)));
    }

    public void AddWall(Wall w) { mWalls.Add(w); }
    public void AddObstacle(Obstacle o) { mObstacles.Add(o); }

    public List<Wall> GetWalls() { return mWalls; }
    public List<Obstacle> GetObstacles() { return mObstacles; }
}
