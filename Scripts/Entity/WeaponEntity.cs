using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEntity : MonoBehaviour
{
    public Transform RightHoldPos;

    void Update()
    {
        transform.position = RightHoldPos.position;
    }
}
