using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://wergia.tistory.com/230
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    Transform characterBody;
    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    GameObject mokiOrigin;

    //GameObject[] Human;
    public GameObject f_Alarm;

    public float moveSpeed = 3f;
    Animator animator;

    Rigidbody playerRid;
    float distance;
    //GameObject colObject;

    public bool eatBlood = false;

    public ParticleSystem blood;
    bool stopBlood = true;
    bool startShot = false;
    bool isShotBlood = false;

    public float shotBlood = 1f;
    private void Start()
    {
        animator = characterBody.GetComponent<Animator>();
        //Human = GameObject.FindGameObjectsWithTag("Human");
        playerRid = GetComponent<Rigidbody>();
        //colObject = Human[0];
       
    }

    private void Update()
    {
        playerRid.velocity = Vector3.zero;
        if (!eatBlood)
        {
            LookAround();
            Move();
        }

        if (Input.GetMouseButtonDown(0))
        {
            stopBlood = false;
            startShot = true;
            isShotBlood = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            stopBlood = true;
            isShotBlood = false;
        }

        if (startShot)
        {
            blood.Play();
            startShot = false;
        }
        else if(stopBlood ==true)
        {
            blood.Stop();
            stopBlood = false;
        }
       
        if(isShotBlood && !startShot)
        {
            Debug.Log("shot");
            GameManger.curHP -= shotBlood*Time.deltaTime;
        }
    }

    void Move()
    {
        //float height = 0f;
        if(Input.GetKey(KeyCode.E))
        {
            transform.position += new Vector3(0,1,0).normalized *Time.deltaTime*moveSpeed;
            //height = 1f;
        }

        if(Input.GetKey(KeyCode.Q))
        {
            transform.position -= new Vector3(0, 1, 0).normalized * Time.deltaTime * moveSpeed;
        }
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0; //magnitude 벡터길이
        if(isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            
            characterBody.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }

       
    }
    void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if(x < 180f) //x가 180보다 작은 경우
        {
            /*clamp함수 = 매개1 검사값 2 최소값 3 최대값
             1이 2보다 작으면 1반환 3보다 크면 3반환*/
            x = Mathf.Clamp(x, -1f, 70f); //-1에서 70도로 제한 0으로 두면 수평면으로 내려가지 않음

        }
        else //아래쪽으로 회전
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y +mouseDelta.x, camAngle.z);
    }


    void Attack()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        if (!RaycastController.maskOn)
        {
            GameManger.curHP -= 0.3f;
            Debug.Log("hit fog");
            Debug.Log(other);
        }
    }


}
