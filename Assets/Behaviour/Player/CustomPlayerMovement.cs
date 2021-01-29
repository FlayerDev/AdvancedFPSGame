using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Flayer.InputSystem;

public class CustomPlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Movement")]
    public float speed = 10f;
    public float gravity = -25f;
    public float jumpForce = 2f;

    [Header("Crouch")]
    public float UprightHeight = 1.8f;
    public float CrouchedHeight = .9f;
    public float HeightTransitionSpeed = .1f;
    public float StaminaLossPerCrouch = .5f;
    public float StaminaGainPerSecond = .33f;

    [Header("Others")]
    public GameObject groundCheckSphere;
    public float AnimationSpeedMultiplier = 1f;

    [Header("Runtime")]
    public bool isGrounded = false;
    public Vector3 velocity;
    public float HeightBuffer = 1.8f;
    public float stamina = 1f;

    Vector3 LastLocation = new Vector2(0,0);

    void Update()
    {
        float x = Input.GetAxis("Horizontal");      // X- = A ,X+ = D
        float z = Input.GetAxis("Vertical");        // Z- = S ,Z+ = W

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move.normalized * speed * Time.deltaTime);

        if (InputManager.GetBindDown("Jump") && isGrounded && !LocalInfo.IsPaused)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
            CtrlAnimationMovementParameters.Singleton.Jump = true;
        }
        controller.Move(velocity * Time.deltaTime);

        if (InputManager.GetBindDown("Crouch")) stamina -= StaminaLossPerCrouch;

        if (InputManager.GetBind("Crouch"))
        {
            HeightBuffer = Mathf.Clamp(HeightBuffer - (AnimationSpeedMultiplier * Time.deltaTime * stamina), CrouchedHeight, UprightHeight);
        }
        else
        {
            HeightBuffer = Mathf.Clamp(HeightBuffer + (AnimationSpeedMultiplier * Time.deltaTime), CrouchedHeight, UprightHeight);
        }
    }

    private void LateUpdate()
    {
        if (CtrlAnimationMovementParameters.Singleton != null)
        {
            //Planar Movement
            var vec = ((transform.position - LastLocation) / Time.deltaTime);
            var vecmag = vec.magnitude / 10;
            var angle = (Quaternion.LookRotation(
                vec, Vector3.up).eulerAngles.y * Mathf.Deg2Rad) - (transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
            var movDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (vecmag > .1 ? vecmag : 0);
            /*
            var angle = Vector3.SignedAngle(transform.position, vec, Vector3.up);
            var movDir = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.zero;
            */
            CtrlAnimationMovementParameters.Singleton.MoveDirection = movDir;
            CtrlAnimationMovementParameters.Singleton.MoveSpeed = vec.magnitude * AnimationSpeedMultiplier * Time.deltaTime;
            CtrlAnimationMovementParameters.Singleton.PlayerAlt = GenericUtilities.ToPercent01(CrouchedHeight, UprightHeight, HeightBuffer);
            CtrlAnimationMovementParameters.Singleton.Grounded = isGrounded;

            LastLocation = transform.position;


        }
        else if (JumpParameterMachine.Singleton != null) JumpParameterMachine.Singleton.Grounded = isGrounded;
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
        stamina = Mathf.Clamp01(stamina + StaminaGainPerSecond * Time.fixedDeltaTime);
    }
}
