using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FogSpawn : MonoBehaviour
{
    ParticleSystem particle;
    public Transform targetPos;
    public float range = 15f;
    Vector3 point;
    public int startSpawnFogNum = 5;
    public GameObject fogPrefab;

    public float sizeIncreaseRate = 0.5f;

    private void Start()
    {
        //particle = fogPrefab.GetComponent<ParticleSystem>();

        for (int i = 0; i < startSpawnFogNum; i++)
        {
            if (RandomPoint(targetPos.position, range, out point))
            {
                targetPos.position = point;
                GameObject fog = Instantiate(fogPrefab, targetPos.position, targetPos.rotation);
                particle = fog.GetComponent<ParticleSystem>();
               // ParticleSystem.ShapeModule shapeModule = particle.shape;
                StartCoroutine(ResizeFog(fog));
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    IEnumerator ResizeFog(GameObject particle)
    {
        yield return new WaitForSeconds(3f);
        //while (particle.startSize < 1000)
        //{
        //    Debug.Log("?");
        //    // particle.scale += new Vector3(10f, 10f, 10f);
        //    particle.startSize += 10f;
        //    yield return new WaitForSeconds(0.5f);
        //}
        while(particle.transform.localScale.x<10f)
        {
            particle.transform.localScale += new Vector3(sizeIncreaseRate, sizeIncreaseRate, sizeIncreaseRate)*Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("ll");
    }
}
