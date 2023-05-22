using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    float secondPerRealTimeSecond;

    public static bool isNight = false;

    [SerializeField]
    float nightFogDensity;
    float dayFogDensity;

    [SerializeField]
    float fogDensityCalc;
    float currentFogDensity;

    public GameObject playerLight;

    private void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    private void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            isNight = true;

        else if (transform.eulerAngles.x <= 10)
            isNight = false;

        if (isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
            playerLight.SetActive(true);
        }
        else
        {
            playerLight.SetActive(false);
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
