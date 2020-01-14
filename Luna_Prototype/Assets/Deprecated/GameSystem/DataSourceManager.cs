using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSourceManager : MonoBehaviour
{
    public class TowerData
    {
        public TowerData(float bltSpeed, float atkRange, float atkDamage, float retargettime, float reloadtime)
        {
            bulletSpeed = bltSpeed;
            attackRange = atkRange;
            attackDamage = atkDamage;
            retargetTime = retargettime;
            reloadTime = reloadtime;
        }
        public float bulletSpeed;
        public float attackRange;
        public float attackDamage;
        public float retargetTime;
        public float reloadTime;
    }

    public string towerDataSourceId = "Tower";

    private List<TowerData> towerDatas = new List<TowerData>();


    public void LoadDatas()
    {
        Debug.Log("[DataSourceManager]: start to read");

        JsonDataSource towerDataSource = ServiceLocator.Get<DataLoader>().GetDataSourceById(towerDataSourceId) as JsonDataSource;
        object towerData = towerDataSource.DataDictionary["TowerAttributes"];
        IEnumerable ITDO = towerData as IEnumerable;

        foreach (var data in ITDO)
        {
            TowerData td = new TowerData(
                (float)System.Convert.ToDouble(((Dictionary<string, object>)data)["BulletSpeed"]),
                (float)System.Convert.ToDouble(((Dictionary<string, object>)data)["AttackRange"]),
                (float)System.Convert.ToDouble(((Dictionary<string, object>)data)["AttackDamage"]),
                (float)System.Convert.ToDouble(((Dictionary<string, object>)data)["RetargetTime"]),
                (float)System.Convert.ToDouble(((Dictionary<string, object>)data)["ReloadTime"])
                );
            towerDatas.Add(td);
        }
        Debug.Log("[DataSourceManager]: done");

    }

    public TowerData GetTowerData(int index)
    {
        return towerDatas[index];
    }


}
