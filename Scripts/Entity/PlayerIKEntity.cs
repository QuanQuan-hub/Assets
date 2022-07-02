using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIKEntity : MonoBehaviour
{
    private Animator animator;
    public float weightIK = 1f;
    public Transform LeftHoldPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        EventManager.AddListener<Transform>(EventID.InitLeftHold, InitLeftHold);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<Transform>(EventID.InitLeftHold, InitLeftHold);
    }

    private void InitLeftHold(Transform value)
    {
        LeftHoldPos = value;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weightIK);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weightIK);

            if (LeftHoldPos != null)
            {
                animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHoldPos.rotation);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHoldPos.position);
            }
        }
    }
}
