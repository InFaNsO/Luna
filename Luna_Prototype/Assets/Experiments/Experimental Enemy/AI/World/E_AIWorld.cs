using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_AIWorld : MonoBehaviour
{
    public List<E_AI_Zone> mZones = new List<E_AI_Zone>();

    public List<E_Enemy> mEnemies = new List<E_Enemy>();
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
        var zone = GetComponent<E_AI_Zone>();
        if(zone != null)
        {
            mZones.Add(zone);
        }
    }
}
