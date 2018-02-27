using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticleEffect : MonoBehaviour
{

    public GameObject particalPrefab;
    public float Rate = 4;
    float timeSpawnFromLastSpawn = 0;
    // Use this for initialization

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeSpawnFromLastSpawn += Time.deltaTime;
        float correctTimeBetweenSpawns = 1f / Rate;

        while (timeSpawnFromLastSpawn > correctTimeBetweenSpawns)
        {
            timeSpawnFromLastSpawn -= 1f / Rate;
            SpawnFireAlongOutLine();
        }
    }

        
    void SpawnFireAlongOutLine()
    {
        var col = GetComponentInParent<PolygonCollider2D>();

        //--コライダーパスをランダム選択
        var spawnPath = col.GetPath(Random.Range(0, col.pathCount));

        //---コライダーパスからポイントをランダム選択
        int pointIndex = Random.Range(0, spawnPath.Length);
        var spawnPointA = spawnPath[pointIndex];
        var spawnPointB = spawnPath[(pointIndex + 1) % spawnPath.Length];

        var spawnPointVec2 = Vector2.Lerp(spawnPointA, spawnPointB, Random.Range(0,1.0f));

        Vector3 spawnPoint = new Vector3(spawnPointVec2.x, spawnPointVec2.y, 0)*3;

        spawnPoint += transform.position;

        Instantiate(particalPrefab, spawnPoint, Quaternion.identity, this.transform.parent);

    }

}