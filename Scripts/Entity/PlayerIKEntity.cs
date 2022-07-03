using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIKEntity : MonoBehaviour
{
    private Animator animator;
    public float weightIK = 1f;
    public GameObject WeaponObj;
    public Transform LeftHoldPos;
    public Transform RightHoldPos;
    public Transform AimPos;
    private bool isAim = false;
    private float AimX = 0;
    private float AimY = 0;
    public float leftHandIKIndexX;
    public float leftHandIKIndexY;
    public float leftHandIKIndexZ;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        leftHandIKIndexX = ConstValue.leftHandIKIndexX;
        leftHandIKIndexY = ConstValue.leftHandIKIndexY;
        leftHandIKIndexZ = ConstValue.leftHandIKIndexZ;
        EventManager.AddListener<GameObject>(EventID.InitWeaponObj, InitWeaponObj);
        EventManager.AddListener<bool>(EventID.SetAimAnimation, SetAimAinmation);
        EventManager.AddListener<Transform>(EventID.InitAimObj, InitAimObj);
        EventManager.AddListener<float, float>(EventID.OnAimXYChange, OnAimXYChange);
    }
    private float index = -35;
    private void OnAimXYChange(float x, float y)
    {
        Debug.Log(x+" "+y);
        AimX = x;
        AimY = y;
        //视角如果高于35度取消左手的旋转，如果低于35度，即35~-60度，线性改变左手旋转量，越靠近35度越趋于0
        if (y > index) 
        {
            leftHandIKIndexX = ConstValue.leftHandIKIndexX + (y - index) / (ConstValue.ViewMaximumVert - index) * 30;
        }
        else
        {
            leftHandIKIndexX = ConstValue.leftHandIKIndexX;
        }
    }

    private void InitAimObj(Transform value)
    {
        AimPos = value;
    }
    private void SetAimAinmation(bool value)
    {
        isAim = value;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameObject>(EventID.InitWeaponObj, InitWeaponObj);
        EventManager.RemoveListener<bool>(EventID.SetAimAnimation, SetAimAinmation);
        EventManager.RemoveListener<Transform>(EventID.InitAimObj, InitAimObj);
        EventManager.RemoveListener<float, float>(EventID.OnAimXYChange, OnAimXYChange);
    }
    private void InitWeaponObj(GameObject value)
    {
        WeaponObj = value;
        if (WeaponObj != null)
        {
            LeftHoldPos = WeaponObj.transform.GetChild(0).Find("LeftHold_Pos");
            RightHoldPos = WeaponObj.transform.GetChild(0).Find("RightHold_Pos");
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weightIK);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weightIK);
            //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weightIK);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weightIK);

            animator.SetLookAtWeight(weightIK * 0.6f, weightIK * 0.7f, weightIK * 0.6f);

            if (LeftHoldPos != null)
            {
                //animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHoldPos.rotation);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, WeaponObj.transform.rotation*Quaternion.Euler(leftHandIKIndexX, leftHandIKIndexY, leftHandIKIndexZ));
                animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHoldPos.position);
            }
            if (RightHoldPos != null)
            {
                animator.SetIKRotation(AvatarIKGoal.RightHand, RightHoldPos.rotation);
                //animator.SetIKPosition(AvatarIKGoal.RightHand, RightHoldPos.position);
            }
            if (AimPos != null)
            {
                animator.SetLookAtPosition(AimPos.position);
            }
        }
    }
}
