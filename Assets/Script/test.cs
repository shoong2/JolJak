using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    Rigidbody myRigid;
    NavMeshAgent agent;
    Animator anim;

    public Transform[] arrWaypoint;
    //�������� �迭�� �־���

    private Vector3 destination;
    private Coroutine moveStop;

    void Start()
    {
        this.myRigid= this.GetComponent<Rigidbody>();
        this.anim = this.GetComponent<Animator>();

        this.agent = this.GetComponent<NavMeshAgent>();
        //agent�� AI�� ������

        Invoke("AiMove", 2);
    }

    private void AiMove()
    {
        int random = Random.Range(0, arrWaypoint.Length);
        Debug.LogFormat("random : {0}", random);
        // �������� ������ ����

        for (int i = 0; i < arrWaypoint.Length; i++)
        {
            if (i == random)
            {
                this.destination = this.arrWaypoint[i].position;
                // ������ �������� ���

                if (this.moveStop == null)
                {
                    Debug.Log("�ڷ�ƾ����");
                    this.moveStop = this.StartCoroutine(this.crAiMove());
                    // �������� AI�� �Ÿ��� ����ϴ� �޼��� --> �ִϸ��̼� ������ ����
                }

                this.agent.SetDestination(this.destination);
                // AI���� �����̷� �̵������ ����
                this.anim.Play("Walk");
                break;
            }
        }
    }

    IEnumerator crAiMove()
    {
        while (true)
        {
            var dis = Vector3.Distance(this.transform.position, this.destination);
            //�������� AI������ �Ÿ� ���
            if (dis <= 0.2f)
            {
                Debug.Log("������ ����");
                this.anim.Play("Idle");
                //�����ϸ� �ִϸ��̼� �ٲ�
                if (this.moveStop != null)
                {
                    this.StopCoroutine(this.moveStop);
                    this.moveStop = null;
                    Invoke("AiMove", 1.5f);
                    //1.5�ʵ� �ٸ� ������ �̵���Ŵ
                    break;
                }
            }
            yield return null;
        }
    }
}
