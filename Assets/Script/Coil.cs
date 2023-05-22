using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coil : MonoBehaviour
{
    public Renderer coilLight;
    Color originalEmissionColor;

    public GameObject coilBody;
    Material coilMat;
    public GameObject pointLight;
    public ParticleSystem fog;
    private void Start()
    {
        originalEmissionColor = coilLight.material.GetColor("_EmissionColor");
        coilMat = coilBody.GetComponent<Material>();
        
    }
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("coil hit");
        if(originalEmissionColor.r>0)
        {
            originalEmissionColor -= new Color(0.1f, 0, 0.1f);
            coilLight.material.SetColor("_EmissionColor", originalEmissionColor);
        }
        else
        {
            pointLight.SetActive(false);
            //MeshCut.Cut(coilBody, transform.position, Vector3.right, coilMat);
            fog.Stop();
            FogSpawn.startSpawnFogNum--;
        }
    }
}
