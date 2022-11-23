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
    //목적지를 배열에 넣었음

    private Vector3 destination;
    private Coroutine moveStop;

    void Start()
    {
        this.myRigid= this.GetComponent<Rigidbody>();
        this.anim = this.GetComponent<Animator>();

        this.agent = this.GetComponent<NavMeshAgent>();
        //agent가 AI를 조종함

        Invoke("AiMove", 2);
    }

    private void AiMove()
    {
        int random = Random.Range(0, arrWaypoint.Length);
        Debug.LogFormat("random : {0}", random);
        // 랜덤으로 목적지 선정

        for (int i = 0; i < arrWaypoint.Length; i++)
        {
            if (i == random)
            {
                this.destination = this.arrWaypoint[i].position;
                // 선정한 목적지를 기억

                if (this.moveStop == null)
                {
                    Debug.Log("코루틴시작");
                    this.moveStop = this.StartCoroutine(this.crAiMove());
                    // 목적지와 AI의 거리를 계산하는 메서드 --> 애니매이션 변경을 위해
                }

                this.agent.SetDestination(this.destination);
                // AI에게 목적이로 이동명령을 내림
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
            //목적지와 AI사이의 거리 계산
            if (dis <= 0.2f)
            {
                Debug.Log("목적지 도착");
                this.anim.Play("Idle");
                //도착하면 애니메이션 바꿈
                if (this.moveStop != null)
                {
                    this.StopCoroutine(this.moveStop);
                    this.moveStop = null;
                    Invoke("AiMove", 1.5f);
                    //1.5초뒤 다른 곳으로 이동시킴
                    break;
                }
            }
            yield return null;
        }
    }
}
