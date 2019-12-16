﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : Enemy
{

    private new void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < pathFinder.mNodes.Count; ++i)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(pathFinder.mNodes[i].coord, 0.3f);

            Gizmos.color = Color.cyan;

            for(int j = 0; j < pathFinder.mNodes[i].children.Count; ++j)
            {
                Gizmos.DrawLine(pathFinder.mNodes[i].coord, pathFinder.mNodes[pathFinder.mNodes[i].children[j]].coord);
            }
        }

    }
}
