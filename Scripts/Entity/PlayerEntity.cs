using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    private Animator animator;

    public GameObject ViewObj;
    public GameObject AimObj;

    private Vector3 standViewPos;
    private Vector3 crouchViewPos;

    private void Awake()
    {
        EventManager.AddListener<bool, float, float>(EventID.SetPlayerAin, SetAnimator);
        EventManager.AddListener<float, float>(EventID.SetAim, SetAim);
    }
    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        standViewPos = ViewObj.transform.localPosition;
        crouchViewPos = standViewPos / 2;
    }

    private void SetAnimator(bool isCrouch,float forwardVector,float rightVector)
    {
        animator.SetBool("isCrouch", isCrouch);
        animator.SetFloat("forwardVector", forwardVector);
        animator.SetFloat("rightVector", rightVector);

        //设置视点位置
        ViewObj.transform.localPosition = isCrouch ? crouchViewPos : standViewPos;
    }
    private void SetAim(float x,float y)
    {
        Debug.Log(x + " " + y);
        AimObj.transform.Rotate(y , x, 0);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<bool, float, float>(EventID.SetPlayerAin, SetAnimator);
        EventManager.RemoveListener<float, float>(EventID.SetAim, SetAim);
    }
}
