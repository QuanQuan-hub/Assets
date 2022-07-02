using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCameronEntity : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private Transform AimObj;
    private Transform FollowObj;
    private GameObject ControllerPlayer;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AimObj == null || FollowObj == null)
        {
            GetFollowAndAim();
        }
    }
    private void GetFollowAndAim()
    {
        if (ControllerPlayer == null)
        {
            ControllerPlayer = GameObject.FindGameObjectWithTag("ControllerPlayer");
        }
        if (AimObj == null)
        {
            AimObj = ControllerPlayer.transform.Find("ViewObj/Aim_Turn_Obj/Aim_Elevation_Obj/Aim_Pos");
            virtualCamera.LookAt = AimObj;
        }
        if (FollowObj == null)
        {
            FollowObj = ControllerPlayer.transform.Find("ViewObj/Head_Pos");
            virtualCamera.Follow = FollowObj;
        }
    }
}
