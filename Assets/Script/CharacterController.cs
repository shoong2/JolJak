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

    public GameObject Human;
    public GameObject f_Alarm;

    public float moveSpeed = 3f;
    Animator animator;

    private void Start()
    {
        animator = characterBody.GetComponent<Animator>();
    }

    private void Update()
    {
        LookAround();
        Move();
        float distance = Vector3.Distance(this.transform.position, Human.transform.position);
        if(distance <1f)
        {
            f_Alarm.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                Debug.Log("attack");
            }
        }
        else
        {
            f_Alarm.SetActive(false);
        }
        //if (Input.GetKey(KeyCode.F))
        //{
        //    if (distance <= 1f)
        //    {
                
        //        f_Alarm.SetActive(true);
        //    }
        //    else
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
        bool isMove = moveInput.magnitude != 0; //magnitude ���ͱ���
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

        if(x < 180f) //x�� 180���� ���� ���
        {
            /*clamp�Լ� = �Ű�1 �˻簪 2 �ּҰ� 3 �ִ밪
             1�� 2���� ������ 1��ȯ 3���� ũ�� 3��ȯ*/
            x = Mathf.Clamp(x, -1f, 70f); //-1���� 70���� ���� 0���� �θ� ��������� �������� ����

        }
        else //�Ʒ������� ȸ��
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y +mouseDelta.x, camAngle.z);
    }
}
