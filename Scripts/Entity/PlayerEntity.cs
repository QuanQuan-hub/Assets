using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家实体类
/// </summary>
public class PlayerEntity : MonoBehaviour
{
    private Animator animator;

    public GameObject ViewObj;
    public GameObject AimTurnObj;
    private Transform AimElevationObj;
    private Transform AimPos;
    public GameObject WeaponObj;

    private Vector3 standViewPos;
    private Vector3 crouchViewPos;

    private void Awake()
    {
        AimElevationObj = AimTurnObj.transform.GetChild(0);
        AimPos = AimElevationObj.GetChild(0);

        EventManager.AddListener<bool, float, float>(EventID.SetPlayerAin, SetAnimator);
        EventManager.AddListener<float, float>(EventID.SetAim, SetAim);
        EventManager.AddListener<bool>(EventID.SetAimAnimation, SetAimAinmation);
    }

    private void SetAimAinmation(bool isAim)
    {
        WeaponObj.transform.LookAt(AimPos);
        animator.SetBool("isAim", isAim);
    }

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        standViewPos = ViewObj.transform.localPosition;
        crouchViewPos = standViewPos / 2;
        characterForward = transform.forward;

        EventManager.ExecuteEvent(EventID.InitWeaponObj, WeaponObj);
        EventManager.ExecuteEvent(EventID.InitAimObj, AimPos);
    }

    private void SetAnimator(bool isCrouch,float forwardVector,float rightVector)
    {
        animator.SetBool("isCrouch", isCrouch);
        animator.SetFloat("forwardVector", forwardVector);
        animator.SetFloat("rightVector", rightVector);

        //设置视点位置
        ViewObj.transform.localPosition = isCrouch ? crouchViewPos : standViewPos;
    }
    private float AimElevationX = 0;
    private Vector3 characterForward;
    private Vector3 AimForward;
    private void SetAim(float x,float y)
    {
        AimForward = AimPos.position - transform.position;
        if (Vector3.Dot(characterForward, AimForward) > 4f) 
        {
            AimTurnObj.transform.localRotation = Quaternion.AngleAxis(x, transform.up) * AimTurnObj.transform.localRotation;
        }
        else
        {
            characterForward = Vector3.Lerp(characterForward, transform.forward, 2);
            if (Vector3.Dot(transform.forward,characterForward) > 0.5f)
            {
                if (Vector3.Dot(transform.right, AimForward) >0)
                {
                    Debug.Log("right");
                    if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "turn 90 right")
                    {
                        animator.SetTrigger("turn_right");
                    }
                }
                else
                {
                    Debug.Log("left");
                    if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "turn 90 left")
                    {
                        animator.SetTrigger("turn_left");
                    }
                }
            }
            transform.localRotation = Quaternion.AngleAxis(x, transform.up) * transform.localRotation;
        }
        

        AimElevationX += y;
        AimElevationX = Mathf.Clamp(AimElevationX, ConstValue.ViewMinimumVert, ConstValue.ViewMaximumVert);
        AimElevationObj.localEulerAngles = new Vector3(AimElevationX, 0, 0);
        EventManager.ExecuteEvent(EventID.OnAimXYChange, AimTurnObj.transform.localRotation.y, AimElevationX);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<bool, float, float>(EventID.SetPlayerAin, SetAnimator);
        EventManager.RemoveListener<float, float>(EventID.SetAim, SetAim);
        EventManager.RemoveListener<bool>(EventID.SetAimAnimation, SetAimAinmation);
    }
}
