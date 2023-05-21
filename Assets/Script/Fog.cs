using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public float sizeIncreaseRate = 0.5f;
    public float waitTime = 5f;
    public float fogMaxSize = 15f;
    void Start()
    {
        StartCoroutine(SpreadFog());
    }

    IEnumerator SpreadFog()
    {
        yield return new WaitForSeconds(waitTime);
        while(gameObject.transform.localScale.x<fogMaxSize)
        {
            gameObject.transform.localScale += new Vector3(sizeIncreaseRate, sizeIncreaseRate, 
                sizeIncreaseRate) * Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }


 
}
