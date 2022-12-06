using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    RaycastHit[] hitInfo;
    public float rayDistance;

    public GameObject f_Alarm;


    Animator mokiAnim;

    bool clickF = false;

    private void Start()
    {
        mokiAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if(!clickF)
            hitInfo = Physics.RaycastAll(transform.position, transform.forward, rayDistance);
        

        for(int i =0; i< hitInfo.Length; i++)
        {
            RaycastHit hit = hitInfo[i];

            if (hit.collider.tag == "Human")
            {
                clickF = true;
                Debug.Log("raycast");
                Debug.Log(clickF);
                f_Alarm.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    f_Alarm.SetActive(false);
                    //if(transform.position.z > colObject.transform.position.z)
                    //{
                    //    distance = colObject.transform.position.z - transform.position.z;
                    //}

                    //distance = colObject.transform.position.z - transform.position.z;
                    //transform.position += new Vector3(0, 0, distance);
                    //transform.Translate(new Vector3(0, 0, distance));
                    mokiAnim.SetTrigger("Attack");
                    Debug.Log("attack");



                }

            }
            else
            {
                f_Alarm.SetActive(false);
                clickF = false;
            }
                
        }
        Debug.DrawRay(transform.position, transform.forward*rayDistance, Color.red);
    }
}
