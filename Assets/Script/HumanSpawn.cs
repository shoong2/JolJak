using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanSpawn : MonoBehaviour
{

    public GameObject humanPrefab;
    public float range = 10.0f;
    public Transform targetPos;
    public Transform randomPos;
    public int startSpawnHumanNum = 5;
    Vector3 point;

    float time = 0;
    public float spawnTime = 8;

    private void Start()
    {
        for(int i=0; i<startSpawnHumanNum; i++)
        {
            if (RandomPoint(targetPos.position, range, out point))
            {
                randomPos.position = point;
                GameObject human = Instantiate(humanPrefab, randomPos.position, randomPos.rotation);
            }
        }
    }
    private void Update()
    {
        time += Time.deltaTime;
        if(time> spawnTime)
        {
            time = 0;
            if (RandomPoint(targetPos.position, range, out point))
            {
                randomPos.position = point;
                GameObject human = Instantiate(humanPrefab, randomPos.position, randomPos.rotation);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

}
