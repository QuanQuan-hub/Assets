using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComtroller : MonoBehaviour
{
    private InputManager inputManager;
    private float forwardVector;
    private float rightVector;
    private bool isCrouch;

    #region 设置Setting，都是暂时这样，到时候应该有设置中心控制
    public bool RunMode_Hold = true;//temp
    public bool IsInvertYAxis = false;
    private int YAxisIndex = 1;
    [Range(1f, 10f)]
    public float mouseXSensitivity = 1f;
    [Range(1f, 10f)]
    public float mouseYSensitivity = 1f;
    #endregion
    private void Awake()
    {
        EventManager.AddListener<bool>(EventID.SetInvertYAxis, SetInvertYAxis);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<bool>(EventID.SetInvertYAxis, SetInvertYAxis);
    }
    void Start()
    {
        inputManager = InputManager.Instance;
        forwardVector = 0f;
        rightVector = 0f;
        isCrouch = false;
        SetInvertYAxis(IsInvertYAxis);
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
        SetAim();
    }

    private void SetAnimator()
    {
        EventManager.ExecuteEvent(EventID.SetPlayerAin, isCrouch, forwardVector, rightVector);
    }

    private void SetAim()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            EventManager.ExecuteEvent(EventID.SetAim,
                Input.GetAxis("Mouse X") * mouseXSensitivity, Input.GetAxis("Mouse Y") * mouseYSensitivity * YAxisIndex);
        }
        
        //TODO:鼠标控制转向抬头
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
    private void SetInvertYAxis(bool isInvert)
    {
        IsInvertYAxis = isInvert;
        YAxisIndex = IsInvertYAxis ? 1 : -1;
    }
}
