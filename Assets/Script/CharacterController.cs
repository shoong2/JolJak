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

    GameObject[] Human;
    public GameObject f_Alarm;

    public float moveSpeed = 3f;
    Animator animator;

    Rigidbody playerRid;
    float distance;
    GameObject colObject;

    public bool eatBlood = false;

    //RaycastHit hit;
    private void Start()
    {
        animator = characterBody.GetComponent<Animator>();
        Human = GameObject.FindGameObjectsWithTag("Human");
        playerRid = GetComponent<Rigidbody>();
        colObject = Human[0];
       
    }

    private void Update()
    {
        playerRid.velocity = Vector3.zero;
        if (!eatBlood)
        {
            LookAround();
            Move();
        }

        

        
        //foreach (GameObject mob in Human)
        //{
        //    if (Vector3.Distance(this.transform.position, mob.transform.position) <= 2)
        //    {
        //        colObject = mob;
        //        f_Alarm.SetActive(true);

        //        if (Input.GetKeyDown(KeyCode.F))
        //        {
        //            //if(transform.position.z > colObject.transform.position.z)
        //            //{
        //            //    distance = colObject.transform.position.z - transform.position.z;
        //            //}

        //            distance = colObject.transform.position.z - transform.position.z;
        //            //transform.position += new Vector3(0, 0, distance);
        //            //transform.Translate(new Vector3(0, 0, distance));
        //            animator.SetTrigger("Attack");
        //            Debug.Log("attack");

        //        }
             
        //    }

        //    if (Vector3.Distance(this.transform.position, colObject.transform.position) > 2)
        //    {
        //        f_Alarm.SetActive(false);
        //    }

        //}


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
        bool isMove = moveInput.magnitude != 0; //magnitude ????????
        //animator.SetBool("isMove", isMove);
        if(isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            //Vector3 goHeight = new Vector3(0f, height, 0f).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            //Vector3 goHeight = new Vector3(0, height, 0);

            
            characterBody.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }
    }
    void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if(x < 180f) //x?? 180???? ???? ????
        {
            /*clamp???? = ????1 ?????? 2 ?????? 3 ??????
             1?? 2???? ?????? 1???? 3???? ???? 3????*/
            x = Mathf.Clamp(x, -1f, 70f); //-1???? 70???? ???? 0???? ???? ?????????? ???????? ????

        }
        else //?????????? ????
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y +mouseDelta.x, camAngle.z);
    }


    void Attack()
    {

    }


}
