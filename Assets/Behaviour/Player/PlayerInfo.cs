using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IDamageable
{
    public float hp { get => health; }
    [SerializeField] private float health = 100;
    public void damage(float amount)
    {
        health -= amount > 0f ? amount : 0f;
    }
}
