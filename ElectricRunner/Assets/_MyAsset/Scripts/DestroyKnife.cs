using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyKnife : MonoBehaviour
{
    //float変数
    public float speed = 10;
    public bool Stop;

    //GameObject変数
    [SerializeField] GameObject player;
    [SerializeField] Transform mainCamera;
    [SerializeField] ParticleSystem hitLightning;
    [SerializeField] ParticleSystem hitSpark;

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
            //対象の位置の方向を向く
            transform.LookAt(mainCamera.transform);

            //自分自身の位置から相対的に移動する
            //Vector3 hookPoint = player.transform.position;
            //float flyingSpeed = Vector3.Distance(transform.position, hookPoint) * 2f;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    //刺さったときに止まる
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target" || other.gameObject.tag == "Ground" || other.gameObject.tag == "Wall")
        {

            if (!player.GetComponent<KnifeShot>().Return)
            {
                if (player.GetComponent<PlayerController>().HoldHand == false)
                {
                    hitLightning.gameObject.SetActive(true);
                    hitLightning.Play();

                    hitSpark.gameObject.SetActive(true);
                    hitSpark.Play();
                }

                rb.velocity = Vector3.zero;
                Stop = true;
            }
        }
    }
}
