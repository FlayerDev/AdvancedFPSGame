using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomPlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 10f;
    public float gravity = -25f;
    public float jumpForce = 2f;

    //[SerializeField]float previousY = 0;
    public bool isGrounded = false;
    public Vector3 velocity;
    public GameObject groundCheckSphere;
    public float AnimationSpeedMultiplier = 1f;

    Vector2 LastLocation = new Vector2(0,0);
    void Update()
    {
        float x = Input.GetAxis("Horizontal");      // X- = A ,X+ = D
        float z = Input.GetAxis("Vertical");        // Z- = S ,Z+ = W

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(LocalInfo.KeyBinds.Jump) && isGrounded && !LocalInfo.IsPaused)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (LocalInfo.ctrlAnimSpeedSingleton != null)
        {
            var vec = (Vector2)gameObject.transform.position - LastLocation;
            LocalInfo.ctrlAnimSpeedSingleton.MoveDirection = vec.normalized;
            LocalInfo.ctrlAnimSpeedSingleton.MoveSpeed = vec.magnitude * AnimationSpeedMultiplier;
        }
    }
    private void FixedUpdate()
    {
        if (groundCheckSphere.GetComponent<GroundTracer>().isGrounded)
        {
            velocity.y /= 1.1f ;
            isGrounded = true;
        } else
        {
            velocity.y += gravity * Time.fixedDeltaTime;
            isGrounded = false;
        }
        //if (!isGrounded) velocity.y += gravity * Time.fixedDeltaTime;
    }
}
