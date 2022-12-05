using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    Rigidbody mokiRigd;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("yes");
            //mokiRigd.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            //mokiRigd.freezeRotation = false;
            //mokiRigd.constraints = ~RigidbodyConstraints.FreezePositionY;
            mokiRigd.constraints = RigidbodyConstraints.None;
            mokiRigd.useGravity = true;
            mokiRigd.gameObject.transform.GetChild(0).GetComponent<Animator>().speed = 0f;
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
