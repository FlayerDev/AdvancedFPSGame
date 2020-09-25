using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 10f;
    public float gravity = -25f;
    public float jumpForce = 2f;
    public float ovSphereRadius = .5f;

    //[SerializeField]float previousY = 0;
    public bool isGrounded = false;
    [SerializeField] Vector3 velocity;
    public Transform groundCheckSphere;

    void Update()
    {
        //this.previousY = transform.position.y;
        if(Physics.OverlapSphere(groundCheckSphere.position, ovSphereRadius).Length > 1 && velocity.y < 0)
        {
            velocity.y /= 1.1f;
            isGrounded = true;
        }
        else { isGrounded = false; }

        float x = Input.GetAxis("Horizontal");      // X- = A ,X+ = D
        float z = Input.GetAxis("Vertical");        // Z- = S ,Z+ = W

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        if (Input.GetKeyDown(LocalInfo.KeyBinds.Jump) && isGrounded && !LocalInfo.isPaused)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }
        controller.Move(velocity * Time.deltaTime);


        //if (transform.position.y == this.previousY) { velocity.y = 0f; isGrounded = true; }
        //else isGrounded = false;
    }
}
