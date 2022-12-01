using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class MoveHuman : MonoBehaviour
{
    float x, y, z, wait;
    Vector3 pos;

    Transform humnaTransform;
    NavMeshAgent nav;
    Animator anim;
    GameObject moki;

    public enum humanState
    {
        idle,
        walk,
        run,
        trace
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
        //Debug.Log(nav.destination);
        //if(!nav.pathPending)
        //{
        //    if (nav.remainingDistance <= nav.stoppingDistance)
        //    {
        //        if(!nav.hasPath||nav.velocity.sqrMagnitude ==0)
        //        {
        //            currentState = humanState.idle;
        //            StartCoroutine(Move());
        //        }

        //    }
        //}

    }

    IEnumerator Move()
    {
        x = UnityEngine.Random.Range(0.6f, 100f);
        y = 0.115f;
        z = UnityEngine.Random.Range(0.6f, 100f);
        pos = new Vector3(x, y, z);
        Debug.Log(pos);
        switch (currentState)
        {
            case humanState.idle:
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                nav.Stop();
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
            case humanState.trace:
                nav.Resume();
                if(Vector3.Distance(this.transform.position, moki.transform.position) <10f)
                {
                    anim.SetBool("Walk", true);
                    anim.SetBool("Run", false);
                    nav.destination = moki.transform.position;
                    nav.speed = 1f;
                }
                else
                {
                    StartCoroutine(RandomState());
                }
                break;
        }
        //wait = UnityEngine.Random.Range(6f, 15f);

        yield return null;
        //StartCoroutine(Move());
    }

    IEnumerator RandomState()
    {
        currentState = (humanState)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(humanState)).Length));
        StartCoroutine(Move());
        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
        StartCoroutine(RandomState());
    }

}
