using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IDamageable
{ 
    [SerializeField] private float health = 100;
    public float hp { get => health; }

    public void damage(float amount)
    {
        health -= amount > 0f ? amount : 0f;
    }
}
