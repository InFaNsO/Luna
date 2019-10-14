using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//========== [ISceneInfo] ===================
// 2019-10-12 [Rick H]
// [ISceneInfo] contains info about attributes that need to be reset
// Prefab gameobjects can be made from [ISceneInfo]
// [GameManager] will find [ISceneInfo] in current scene as a reference

// e.g.  items/enemies in the scene respawn or not?
// NPC dialog, quest progress reset or not ?

//=============================

public interface ISceneInfo
{
     void ResetScene();
 
}
