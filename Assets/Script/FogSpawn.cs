using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FogSpawn : MonoBehaviour
{
    ParticleSystem particle;
    public Transform targetPos;
    public Transform test;

    public float range = 5f;
    Vector3 point;
    public static int startSpawnFogNum = 5;
    public GameObject coilPrefab;

    public float sizeIncreaseRate = 0.5f;

    private void Start()
    {
        startSpawnFogNum = 5;
        //particle = fogPrefab.GetComponent<ParticleSystem>();

        for (int i = 0; i < startSpawnFogNum; i++)
        {
            if (RandomPoint(targetPos.position, range, out point))
            {
                point.y = 0.68f;
                test.position = point;
                Debug.Log(point);
                GameObject coil = Instantiate(coilPrefab, test.position, test.rotation);
                //particle = fog.GetComponent<ParticleSystem>();
                //StartCoroutine(ResizeFog(fog));
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;

            //randompoint가 범위 안에 있는지 체크
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
