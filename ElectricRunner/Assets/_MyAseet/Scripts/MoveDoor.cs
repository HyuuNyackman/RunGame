using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : GimmickManager
{
    [SerializeField,Range(0.1f,10)] float speed;

    [SerializeField] Animator animator;

    // Start is called before the first frame update
   protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        float anitime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (anitime > 1)
        {
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 1);
        }
        if (anitime < 0)
        {
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0);
        }
    }

    protected override void Current()
    {
        animator.SetBool("OpenDoor", true);
        animator.SetFloat("DoorSpeed", speed);
    }
    protected override void CurrentOff()
    {
        animator.SetBool("OpenDoor", false);
        animator.SetFloat("DoorSpeed", -speed);
    }
}
