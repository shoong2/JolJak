using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class RaycastController : MonoBehaviour
{
    RaycastHit[] hitInfo;
    RaycastHit ray;
    public float rayDistance;
    public LayerMask layerMask;
    LayerMask mask;
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

    [Header("마스크 아이템 속성")]
    public GameObject gasMask;
    public float maskYPos = 3f;
    public GameObject r_Alarm;
    public GameObject applyGasMask;
    public GameObject gasCamera;
    bool clickR = false;
    public static bool maskOn = false;
    public float maskTime = 10f;
    public float nowMaskTime;
    [Header("Mask UI")]
    public GameObject maskUI;
    public TMP_Text maskCount;


    private void Start()
    {  
        mokiAnim = GetComponent<Animator>();
        mask = LayerMask.GetMask("Human") | LayerMask.GetMask("Item");
        nowMaskTime = maskTime;
    }


    void Update()
    {
        //if(!clickF)
        //    hitInfo = Physics.RaycastAll(transform.position, transform.forward, rayDistance);

        if (Physics.Raycast(transform.position, transform.forward, out ray, rayDistance, mask)
            &&!clickF &&!clickR)
        {
            if (ray.collider.tag == "Human") 
            {
                f_Alarm.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F) && clickF == false)
                {
                    for (int i = 0; i < ray.collider.transform.childCount; i++)
                    {
                        if (ray.collider.transform.GetChild(i).name == "HCamera")
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
                //Debug.Log("hit human");
                //Debug.Log(Vector3.Distance(this.transform.position, ray.transform.position));
                Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
            }
            
            else if(ray.collider.tag =="Item")
            {
                Debug.Log("item");
                f_Alarm.SetActive(false);
                r_Alarm.SetActive(true);
                if(Input.GetKeyDown(KeyCode.R))
                {
                    Destroy(ray.collider.gameObject);
                    if (!maskOn)
                        StartCoroutine(GasItem());
                    else
                        nowMaskTime += maskTime;
                    clickR = false;
                    
                }
            }

        }
        else
        {
            f_Alarm.SetActive(false);
            r_Alarm.SetActive(false);
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
                Debug.Log("detach");
                FlyAudio.Play();
                SuckAudio.Stop();
                c_Alarm.SetActive(false);
                mokiAnim.SetTrigger("Idle");
                charScript.eatBlood = false;
                subCamera.SetActive(false);
  
            }

            //GameManger.curHP += Time.deltaTime * eatSpeed;
            
            if(!humanDeath)
                sTime += Time.deltaTime;

            if (sTime > 6f)
            {
                humanDeath = true;
                f_Alarm.SetActive(false);
                sTime = 0f;
            }
        }

        if(maskOn)
        {
            nowMaskTime -= Time.deltaTime;
            maskCount.text = nowMaskTime.ToString("F0");
            if(nowMaskTime<0)
            {
                maskOn = false;
                applyGasMask.SetActive(false);
                maskUI.SetActive(false);
                nowMaskTime = maskTime;
            }
        }
        
     
    }

    private void FixedUpdate()
    {
        if (clickF == true)
        {
            //모기를 사람의 자식으로
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
        clickF = false;
        moki.transform.parent = null; //자식해제
        ray.collider.GetComponent<Animator>().SetTrigger("Death");
        GameObject mask = Instantiate(gasMask, rayHit.transform.position + new Vector3(0, maskYPos, 0), transform.rotation);
        mask.transform.LookAt(transform);
        yield return new WaitForSeconds(3f);     
        Destroy(rayHit);
        
    }

    IEnumerator GasItem()
    {
        DirectGasItem(true);
        yield return new WaitForSecondsRealtime(2.5f);
        DirectGasItem(false);
        
    }

    void DirectGasItem(bool a)
    {
        clickR = a;
        gasCamera.SetActive(a);
        if(a==true)
        {
            applyGasMask.SetActive(a);
            applyGasMask.GetComponent<Animator>().SetTrigger("Gas");
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            maskOn = true;
            maskUI.SetActive(true);
        }
    }

    //private void OnParticleCollision(GameObject other)
    //{
    //    if (!maskOn)
    //    {
    //        //GameManger.curHP -= Time.deltaTime;
    //        Debug.Log("hit fog");
    //    }
    //}
}
