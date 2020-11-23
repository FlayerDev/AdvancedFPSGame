using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTracer : MonoBehaviour
{
    public float minimumDamageSpeed = 15f;
    public float fallDamageMultiplier = 1f;
    public bool isGrounded = false;

    private void OnTriggerExit(Collider other) => isGrounded = false;
    private void OnTriggerStay(Collider other) => isGrounded = true;
    private void OnTriggerEnter(Collider other) {
        if (transform.parent.GetComponent<PlayerMovement>().velocity.y < -minimumDamageSpeed)
            damagePlayer((Mathf.Abs(transform.parent.GetComponent<PlayerMovement>().velocity.y) - minimumDamageSpeed) * fallDamageMultiplier);
    }
    void damagePlayer(float amount)
    {
        transform.parent.GetComponent<IDamageable>().damage(amount, transform.parent.gameObject);
    }
}
