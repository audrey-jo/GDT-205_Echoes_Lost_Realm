using UnityEngine;
using System.Collections.Generic;

public class MinionFactory : MonoBehaviour
{
    public GameObject minionPrefab;
    
    public int totalMinionCount = 10;
    public int activeMinionCount = 2;

    List<GameObject> spawnedMinions = new();
    List<GameObject> activeMinions = new();

    int deathCount;

    void Start()
    {
        for (int i = 0; i < totalMinionCount; i++)
        {
            GameObject obj = Instantiate(minionPrefab);
            spawnedMinions.Add(obj);
        }

        ActivateMinions();
    }

    void Update()
    {
        
    }

    void ActivateMinions()
    {
        for (int i = 0; i < activeMinionCount; i++)
        {
            spawnedMinions[i].GetComponent<Minion>().Activate();
            activeMinions.Add(spawnedMinions[i]);
        }
    }

    void ActivateAllMinions()
    {
        for (int i = 0; i < totalMinionCount; i++)
        {
            spawnedMinions[i].GetComponent<Minion>().Activate();
            activeMinions.Add(spawnedMinions[i]);
        }
    }

    void AddDeath() { deathCount++; }
}
