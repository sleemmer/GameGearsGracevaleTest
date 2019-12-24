using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Animator Animator;
    public Transform HealthbarHolder;
    public void Attack()
    {
        Animator.SetTrigger("Attack");
    }

    public void Death()
    {
        Animator.SetInteger("Health", 0);
    }

    public void Idle()
    {
        Animator.SetInteger("Health", 100);
    }

    public Vector3 GetHealthbarHolderPosition()
    {
        return HealthbarHolder.position;
    }
}
