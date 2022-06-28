using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMove : MonoBehaviour
{
    private Animator animator;
    private float x;
    private float y;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        if (InputManager.Instance.IsKeyHold(ActionEnum.run ))
        {
            x *= 2;
            y *= 2;
        }
        animator.SetBool("isCrouch", InputManager.Instance.IsKeyHold(ActionEnum.crouch_hold));
        animator.SetFloat("rightVector", x);
        animator.SetFloat("forwardVector", y);
    }
}
