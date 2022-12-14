using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RaycastController : MonoBehaviour
{
    RaycastHit[] hitInfo;
    RaycastHit ray;
    public float rayDistance;
    public LayerMask layerMask;

    public GameObject f_Alarm;
    public GameObject c_Alarm;
    //[SerializeField]
    Animator mokiAnim;

    public AudioSource FlyAudio;
    public AudioSource SuckAudio;

    GameObject subCamera;

    public GameManger gameManager;

    [SerializeField]
    Rigidbody moki;

    [SerializeField]
    CharacterController charScript;

    public bool clickF = false;
    bool humanDeath = false;

    int loopNum = 0;
    float sTime = 0f;

    public float eatSpeed =2f;
    private void Start()
    {
        mokiAnim = GetComponent<Animator>();      
    }

    void Update()
    {
        //if(!clickF)
        //    hitInfo = Physics.RaycastAll(transform.position, transform.forward, rayDistance);

        if (Physics.Raycast(transform.position, transform.forward, out ray, rayDistance, layerMask)
            &&clickF ==false)
        {
            //if(ray.collider.tag=="Human")
            //{
            //    Debug.Log("hit human");
            //    Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
            //}
            f_Alarm.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) && clickF == false)
            {
                for(int i =0; i<ray.collider.transform.childCount; i++ )
                {
                    if(ray.collider.transform.GetChild(i).name =="HCamera")
                    {
                        subCamera = ray.collider.transform.GetChild(i).gameObject;
                        subCamera.SetActive(true);
                    }
                }
                //camera2.gameObject.SetActive(true);
                FlyAudio.Stop();
                SuckAudio.Play();
                mokiAnim.SetTrigger("Attack");
                f_Alarm.SetActive(false);
                c_Alarm.SetActive(true);
                clickF = true;
                ray.collider.GetComponent<MoveHuman>().isSucked = true;

                sTime = 0;


            }
            Debug.Log("hit human");
            //Debug.Log(Vector3.Distance(this.transform.position, ray.transform.position));
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);

        }
        else
        {
            f_Alarm.SetActive(false);
            //Debug.Log("nothing");
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green);
        }

        if(clickF == true)
        {
            if (Input.GetKeyDown(KeyCode.C) || humanDeath)
            {
                if(humanDeath ==true)
                {
                    StartCoroutine(Death(ray.collider.gameObject));
                    humanDeath = false;
                }
                //camera2.gameObject.SetActive(false);
                Debug.Log("detach");
                FlyAudio.Play();
                SuckAudio.Stop();
                c_Alarm.SetActive(false);
                mokiAnim.SetTrigger("Idle");
                moki.transform.parent = null;
                clickF = false;
                charScript.eatBlood = false;
                subCamera.SetActive(false);
  
            }

            gameManager.curHP += Time.deltaTime * eatSpeed;
            
            if(!humanDeath)
                sTime += Time.deltaTime;

            if (sTime > 6f)
            {
                humanDeath = true;      
                sTime = 0f;
            }
        }
        
     
    }

    private void FixedUpdate()
    {
        if (clickF == true)
        {

            moki.transform.parent = ray.collider.transform;

            if (Vector3.Distance(moki.transform.position, ray.collider.transform.position) > 1.23f)
            {
                Debug.Log("sound");
                
                //clickF = false;
                //moki.transform.parent = ray.collider.transform;
                moki.transform.position = Vector3.MoveTowards(moki.transform.position,
                    ray.collider.transform.position + new Vector3(0, 1.2f, 0), 1f * Time.deltaTime * 2f);
                //moki.MovePosition(ray.collider.transform.position.normalized * Time.deltaTime * 2f);
                charScript.eatBlood = true;

            }

           
        }

    }

    IEnumerator Death(GameObject rayHit)
    {
        ray.collider.GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        Destroy(rayHit);
    }
}
