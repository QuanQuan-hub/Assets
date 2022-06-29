using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComtroller : MonoBehaviour
{
    private Animator animator;
    private InputManager inputManager;
    private float forwardVector;
    private float rightVector;
    private bool isCrouch;

    public GameObject ViewObj;
    public GameObject AimObj;
    private Vector3 standViewPos;
    private Vector3 crouchViewPos;

    public bool RunMode_Hold = true;//temp

    void Start()
    {
        inputManager = InputManager.Instance;
        animator = transform.GetChild(0).GetComponent<Animator>();
        forwardVector = 0f;
        rightVector = 0f;
        isCrouch = false;
        standViewPos = ViewObj.transform.localPosition;
        crouchViewPos = standViewPos / 2;
    }

    private int forwardIndex = 0;
    float forwardSpeed = 6f; 
    
    private int rightIndex = 0;
    float rightSpeed = 6f;

    void Update()
    {
        KeyHold();
        SetCrouch();
        SetRun();
        SetForwardVector();
        SetRightVector();
        SetAnimator();
    }

    float clampIndex = 1f;
    bool isPressRun = false;
    private void SetRun()
    {
        if (RunMode_Hold)
        {
            isPressRun = false;
            clampIndex = isHoldRun ? 2f : 1f;
        }
        else
        {
            isPressRun = inputManager.IsKeyDown(ActionEnum.run) ? !isPressRun : isPressRun;
            clampIndex = isPressRun ? 2f : 1f;
        }
    }

    private void SetCrouch()
    {
        if (inputManager.IsKeyDown(ActionEnum.crouch_press))
        {
            isCrouch = !isCrouch;
            isPressRun = false;
        }
        if (!isCrouch)
        {
            isCrouch = isHoldCrouch;
        }
        if (inputManager.IsKeyUp(ActionEnum.crouch_hold))
        {
            isCrouch = isHoldCrouch;
        }
        //设置视点位置
        ViewObj.transform.localPosition = isCrouch ? crouchViewPos : standViewPos;
    }

    bool isHoldForward = false;
    bool isHoldBack = false;
    bool isHoldRight = false;
    bool isHoldLeft = false;
    bool isHoldCrouch = false;
    bool isHoldRun = false;
    private void KeyHold()
    {
        isHoldForward = inputManager.IsKeyHold(ActionEnum.forward);
        isHoldBack = inputManager.IsKeyHold(ActionEnum.back);
        isHoldRight = inputManager.IsKeyHold(ActionEnum.right);
        isHoldLeft = inputManager.IsKeyHold(ActionEnum.left);
        isHoldCrouch = inputManager.IsKeyHold(ActionEnum.crouch_hold);
        isHoldRun = inputManager.IsKeyHold(ActionEnum.run);
    }
    private void SetRightVector()
    {
        if (isHoldRight ^ isHoldLeft)
        {
            if (isHoldRight)
            {
                rightVector += Time.deltaTime * rightSpeed;
            }
            else
            {
                rightVector -= Time.deltaTime * rightSpeed;
            }
        }
        else
        {
            if (Mathf.Abs(rightVector) <= 0.1f)
            {
                rightVector = 0;
                isPressRun = false;
            }
            else
            {
                rightIndex = rightVector > 0 ? -1 : 1;
                rightVector += rightIndex * Time.deltaTime * rightSpeed;
            }
        }
        if (Mathf.Abs(rightVector) >= clampIndex + 0.5f)
        {
            rightIndex = rightVector > 0 ? -1 : 1;
            rightVector += rightIndex * Time.deltaTime * rightSpeed * 2;
        }
        else
        {
            rightVector = Mathf.Clamp(rightVector, -clampIndex, clampIndex);
        }
    }

    private void SetForwardVector()
    {
        if (isHoldForward ^ isHoldBack)
        {
            if (isHoldForward)
            {
                forwardVector += Time.deltaTime * forwardSpeed;
            }
            else
            {
                forwardVector -= Time.deltaTime * forwardSpeed;
            }
        }
        else
        {
            if (Mathf.Abs(forwardVector) <= 0.1f) 
            {
                forwardVector = 0;
                isPressRun = false;
            }
            else
            {
                forwardIndex = forwardVector > 0 ? -1 : 1;
                forwardVector += forwardIndex * Time.deltaTime * forwardSpeed;
            }
        }
        if (Mathf.Abs(forwardVector) >= clampIndex + 0.5f)
        {
            forwardIndex = forwardVector > 0 ? -1 : 1;
            forwardVector += forwardIndex * Time.deltaTime * forwardSpeed * 2;
        }
        else
        {
            forwardVector = Mathf.Clamp(forwardVector, -clampIndex, clampIndex);
        }
    }
    private void SetAnimator()
    {
        animator.SetBool("isCrouch", isCrouch);
        animator.SetFloat("forwardVector", forwardVector);
        animator.SetFloat("rightVector", rightVector);
    }
}
