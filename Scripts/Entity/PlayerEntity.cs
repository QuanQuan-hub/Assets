using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    private Animator animator;

    public GameObject ViewObj;
    public GameObject AimTurnObj;
    private GameObject AimElevationObj;

    private Vector3 standViewPos;
    private Vector3 crouchViewPos;

    private void Awake()
    {
        AimElevationObj = AimTurnObj.transform.GetChild(0).gameObject;
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
    private float AimElevationX = 0;
    private float minimumVert = -60;
    private float maximumVert = 60;
    private void SetAim(float x,float y)
    {
        AimTurnObj.transform.localRotation = Quaternion.AngleAxis(x, transform.up) * AimTurnObj.transform.localRotation;
        AimElevationX += y;
        AimElevationX = Mathf.Clamp(AimElevationX, minimumVert, maximumVert);
        AimElevationObj.transform.localEulerAngles = new Vector3(AimElevationX, 0, 0);
        /*if(AimElevationX >= 0.2f)
        {
            AimElevationObj.transform.localRotation = Quaternion.AngleAxis(y, transform.right) * AimElevationObj.transform.localRotation;
        }*/
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<bool, float, float>(EventID.SetPlayerAin, SetAnimator);
        EventManager.RemoveListener<float, float>(EventID.SetAim, SetAim);
    }
}
