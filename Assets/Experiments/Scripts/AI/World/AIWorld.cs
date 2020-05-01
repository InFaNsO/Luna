using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWorld : MonoBehaviour
{
    public List<AI_Zone> mZones = new List<AI_Zone>();

    public List<Enemy> mEnemies = new List<Enemy>();
    public Player mPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var zone = GetComponent<AI_Zone>();
        if(zone != null)
        {
            mZones.Add(zone);
        }
    }
}
