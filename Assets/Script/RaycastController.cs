using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    RaycastHit[] hitInfo;
    RaycastHit ray;
    public float rayDistance;
    public LayerMask layerMask;
    public GameObject f_Alarm;


    Animator mokiAnim;

    bool clickF = false;

    private void Start()
    {
        mokiAnim = GetComponent<Animator>();
    }

    void Update()
    {
        //if(!clickF)
        //    hitInfo = Physics.RaycastAll(transform.position, transform.forward, rayDistance);
        
        if(Physics.Raycast(transform.position, transform.forward,out ray, rayDistance, layerMask))
        {
            //if(ray.collider.tag=="Human")
            //{
            //    Debug.Log("hit human");
            //    Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
            //}

            Debug.Log("hit human");
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
        }
        else
        {
            Debug.Log("nothing");
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green);
        }
        //for(int i =0; i< hitInfo.Length; i++)
        //{
        //    Debug.Log("Check");
        //    RaycastHit hit = hitInfo[i];
        //    Debug.Log(hitInfo[i]);
        //    if (hit.collider.tag == "Human")
        //    {
        //        Debug.Log("raycast");
        //        Debug.Log(clickF);
        //        f_Alarm.SetActive(true);
        //        if (Input.GetKeyDown(KeyCode.F))
        //        {
        //            clickF = true;
        //            f_Alarm.SetActive(false);
        //            //if(transform.position.z > colObject.transform.position.z)
        //            //{
        //            //    distance = colObject.transform.position.z - transform.position.z;
        //            //}

        //            //distance = colObject.transform.position.z - transform.position.z;
        //            //transform.position += new Vector3(0, 0, distance);
        //            //transform.Translate(new Vector3(0, 0, distance));
        //            mokiAnim.SetTrigger("Attack");
        //            Debug.Log("attack");



        //        }

        //    }
        //    else
        //    {
        //        Debug.Log("ddasdsa");
        //        f_Alarm.SetActive(false);
        //        clickF = false;
        //    }
                
        //}
        //Debug.DrawRay(transform.position, transform.forward*rayDistance, Color.red);
    }
}
