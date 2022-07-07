using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class DestroyKnife : MonoBehaviour
{
    //float変数

    public bool Stop;

    //GameObject変数
    [SerializeField] GameObject player;

    [SerializeField] ParticleSystem hitLightning;
    [SerializeField] ParticleSystem hitSprak;

    //RigidBody変数
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //knife消失
    void Update()
    {
        if (player.GetComponent<KnifeShot>().Return)
        {
            Stop = false;
        }
    }
    //刺さったときに止まる
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target" || other.gameObject.tag == "Ground" || other.gameObject.tag == "Wall")
        { 
            if(player.GetComponent<PlayerController>().HoldHand==false)
            {
                hitLightning.gameObject.SetActive(true);
                hitLightning.Play();

                hitSprak.gameObject.SetActive(true);
                hitSprak.Play();
            }
            
            
            rb.velocity = Vector3.zero;
            Stop = true;
        }
        
    }
}
