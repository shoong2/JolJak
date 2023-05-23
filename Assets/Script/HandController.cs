using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    Rigidbody mokiRigd;

    public GameManger gm;
    public AudioSource clap;
    //public AudioSource fly;
    //public AudioSource suck;
    
    bool oneTimeCor = true;

    private void Start()
    {
        mokiRigd = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManger>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("yes");
            //fly.Stop();
            //suck.Stop();
            clap.Play();
            mokiRigd.constraints = RigidbodyConstraints.None;
            mokiRigd.useGravity = true;
            mokiRigd.gameObject.transform.GetChild(0).GetComponent<Animator>().speed = 0f;
            if (oneTimeCor)
            {
                gm.hitHand = true;
                Debug.Log(gm.hitHand+ " a");
                oneTimeCor = false;
            }
            //mokiRigd.mass = 10f;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("yes");
    //        collision.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
    //        collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
    //    }
    //}
}
