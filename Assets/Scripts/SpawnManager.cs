using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] SpawnPoints;

    private List<GameObject> AvailableSpawns = new List<GameObject>();

    public static SpawnManager Instance {
        get {
            return GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }
    }

    public void Awake() {
        AvailableSpawns.AddRange(SpawnPoints);
    }

    public GameObject TakeRandomSpawn() {
        int idx = Random.Range(0, AvailableSpawns.Count);
        GameObject spawn = AvailableSpawns[idx];
        AvailableSpawns.RemoveAt(idx);
        return spawn;
    }
}
