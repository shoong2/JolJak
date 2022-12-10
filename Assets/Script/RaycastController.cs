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

    [SerializeField]
    Animator mokiAnim;

    [SerializeField]
    GameObject moki;

    bool clickF = false;
    int loopNum = 0;

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
            f_Alarm.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F))
            {
                clickF = true;
                f_Alarm.SetActive(false);
                mokiAnim.SetTrigger("Attack");
                //while(Vector3.Distance(this.transform.position, ray.transform.position) >= 2f)
                //{
                //    moki.transform.position = Vector3.MoveTowards(moki.transform.position,
                //    ray.collider.transform.position, 0.5f * Time.deltaTime);

                //    if (loopNum++ > 10000)
                //        throw new System.Exception("Infinite Loop");
                //}

                //while(Vector3.Distance(this.transform.position, ray.transform.position)>=1.89f)
                //{
                //    moki.transform.Translate(Vector3.forward*Time.deltaTime*0.5f);

                //    if (loopNum++ > 10000)
                //        throw new System.Exception("Infinite Loop");
                //}


            }
            Debug.Log("hit human");
            //Debug.Log(Vector3.Distance(this.transform.position, ray.transform.position));
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
        }
        else
        {
            f_Alarm.SetActive(false);
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

        if(clickF == true)
        {
            moki.transform.position = Vector3.MoveTowards(moki.transform.position,
                ray.collider.transform.position+new Vector3(0,1.2f,0), 1f * Time.deltaTime*2f);
            Debug.Log(Vector3.Distance(moki.transform.position, ray.collider.transform.position));

            if (Vector3.Distance(moki.transform.position, ray.collider.transform.position) < 1.23f)
            {
                clickF = false;
                moki.transform.parent = ray.collider.transform;

            }


            //moki.transform.Translate(Vector3.forward * Time.deltaTime);
        }
    }

    
}
