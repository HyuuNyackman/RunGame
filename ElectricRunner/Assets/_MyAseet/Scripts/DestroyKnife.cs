using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyKnife : MonoBehaviour
{
    //float変数
    public float time;
    public float Limit;

    //GameObject変数
    public GameObject Player;

    //RigidBody変数
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Player = GameObject.Find("Player");
    }

    //knife消失
    void Update()
    {
        //time += Time.deltaTime;
        //if (time >= Limit || Player.GetComponent<KnifeShot>().Return)
        //{
        //    Destroy(this.gameObject);
        //}
    }

    //刺さったときに止まる
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
       
    }
}
