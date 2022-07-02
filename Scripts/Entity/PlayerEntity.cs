using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    private Animator animator;

    public GameObject ViewObj;
    public GameObject AimTurnObj;
    private Transform AimElevationObj;
    private Transform AimPos;
    public GameObject WeaponObj;
    private Transform LeftHoldPos;

    private Vector3 standViewPos;
    private Vector3 crouchViewPos;

    private void Awake()
    {
        AimElevationObj = AimTurnObj.transform.GetChild(0);
        AimPos = AimElevationObj.GetChild(0);
        LeftHoldPos = WeaponObj.transform.GetChild(0).Find("LeftHold_Pos");

        EventManager.AddListener<bool, float, float>(EventID.SetPlayerAin, SetAnimator);
        EventManager.AddListener<float, float>(EventID.SetAim, SetAim);
        EventManager.AddListener<bool>(EventID.SetAimAnimation, SetAimAinmation);
    }

    private void SetAimAinmation(bool isAim)
    {
        if (isAim)
        {
            WeaponObj.transform.LookAt(AimPos);
        }
        animator.SetBool("isAim", isAim);
    }

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        standViewPos = ViewObj.transform.localPosition;
        crouchViewPos = standViewPos / 2;

        EventManager.ExecuteEvent(EventID.InitLeftHold, LeftHoldPos);
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
    private float minimumVert = -60;
    private float maximumVert = 60;
    private void SetAim(float x,float y)
    {
        AimTurnObj.transform.localRotation = Quaternion.AngleAxis(x, transform.up) * AimTurnObj.transform.localRotation;
        AimElevationX += y;
        AimElevationX = Mathf.Clamp(AimElevationX, minimumVert, maximumVert);
        AimElevationObj.localEulerAngles = new Vector3(AimElevationX, 0, 0);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<bool, float, float>(EventID.SetPlayerAin, SetAnimator);
        EventManager.RemoveListener<float, float>(EventID.SetAim, SetAim);
        EventManager.RemoveListener<bool>(EventID.SetAimAnimation, SetAimAinmation);
    }
}
