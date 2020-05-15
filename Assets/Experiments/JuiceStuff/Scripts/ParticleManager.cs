using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] GameObject EnemyDieParticles;
    
    
    private List<ParticleSystem> mySpawnedSystems = new List<ParticleSystem>();

    #region Event System Funstions
    //Note
    //Add these funstions in Awake to hook with event system
    public void OnEnemyDeath(Transform enemyPos)
    {
        var par = Instantiate(EnemyDieParticles, enemyPos.position, enemyPos.rotation);     //can be child of enemy to remove when the enemy is destroyed
        var sys = par.GetComponent<ParticleSystem>();
        sys.Play();
        mySpawnedSystems.Add(sys);
    }
    #endregion

    #region Mono Behavior Functions
    private void Awake()
    {
        GameEvents.current.EnemyDied += OnEnemyDeath;
    }

    private void Update()
    {
        for (int i = 0; i < mySpawnedSystems.Count; ++i)
        {
            if (mySpawnedSystems[i].particleCount < 1)
            {
                Destroy(mySpawnedSystems[i].gameObject);
            }
        }

        mySpawnedSystems = mySpawnedSystems.Where(x => x != null).ToList();
    }
    #endregion
}
