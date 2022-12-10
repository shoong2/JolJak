using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditor;

public class MoveHuman : MonoBehaviour
{
    float x, y, z, wait;
    public float angleRange = 50f;
    public float radius = 10f;
    bool isCollision = false;
    bool canTrace = true;
    bool canClap = true;

    Vector3 pos;

    Transform humnaTransform;
    NavMeshAgent nav;
    Animator anim;
    GameObject moki;


    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);


    public enum humanState
    {
        idle,
        walk,
        run,
        //trace
    };

    public humanState currentState = humanState.idle;
    private void Start()
    {
        humnaTransform = gameObject.GetComponent<Transform>();
        nav = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        moki = GameObject.FindWithTag("Player");

        //x = Random.Range(0.6f, 60f);
        //y = 0.115f;
        //z = Random.Range(0.6f, 60f);
        //pos = new Vector3(x, y, z);

        StartCoroutine(RandomState());
    }

    private void Update()
    {
 
        Vector3 interV = moki.transform.position - transform.position;

        if (interV.magnitude <= radius)
        {
            //������κ��� ����� ���Ϳ� ��� ���麤�͸� ����
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            //cos������ ���� ���ϱ�(��Ÿ)
            float theta = Mathf.Acos(dot);
            //degree�� ��ȯ
            float degree = Mathf.Rad2Deg * theta;


            if (degree <= angleRange / 2f)
            {

                if (interV.magnitude <= 3)
                {
                    //Debug.Log("hit");
                    if (canClap)
                    {
                        anim.SetTrigger("Clap");
                        canClap = false;
                    }
           
                }
                else
                    canClap = true;
                //StopCoroutine(RandomState());
                StopCoroutine(Move());
                isCollision = true;
                nav.Resume();
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                nav.destination = moki.transform.position;
                nav.speed = 2f;



            }          

            else
            {
                isCollision = false;

            }
        }

        else
        {
            isCollision = false;     
        }

        //Debug.Log(Vector3.Distance(transform.position, moki.transform.position));
        //if (Vector3.Distance(transform.position, moki.transform.position) <= 3f)
        //{
        //    Debug.Log("hit");
        //    if (canClap)
        //    {
        //        anim.SetTrigger("Clap");
        //        canClap = false;
        //    }


        //}
        //else
        //    canClap = true;
    }



    IEnumerator Move()
    {
        x = UnityEngine.Random.Range(0.6f, 100f);
        y = 0.115f;
        z = UnityEngine.Random.Range(0.6f, 100f);
        pos = new Vector3(x, y, z);
        switch (currentState)
        {
            case humanState.idle:
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                nav.Stop();
                //nav.speed = 0f;
                break;
            case humanState.walk:
                nav.Resume();
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                nav.destination = pos;
                nav.speed = 1f;
                break;
            case humanState.run:
                nav.Resume();
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                nav.destination = pos;
                nav.speed = 3f;
                break;
            //case humanState.trace:
            //    nav.Resume();
            //    //if(Vector3.Distance(this.transform.position, moki.transform.position) <10f)
            //    //{
            //    //    anim.SetBool("Walk", true);
            //    //    anim.SetBool("Run", false);
            //    //    nav.destination = moki.transform.position;
            //    //    nav.speed = 1f;
            //    //}
            //    //else
            //    //{
            //    //    StartCoroutine(RandomState());
            //    //}
            //    //break;
            //    isTrace = true;
            //    break;
        }
        //wait = UnityEngine.Random.Range(6f, 15f);

        yield return null;
        //StartCoroutine(Move());
    }

    IEnumerator RandomState()
    {
        if(!isCollision) //������ �ƴҶ��� ���� ��ġ�� �ٰų� �����̰ų� ���߱�
        {
            currentState = (humanState)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(humanState)).Length));
            StartCoroutine(Move());
        }
        
        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
        //isTrace = false;
        StartCoroutine(RandomState());
    }

    private void OnDrawGizmos()
    {
        Handles.color = isCollision ? _red : _blue;
        // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
    }

}
