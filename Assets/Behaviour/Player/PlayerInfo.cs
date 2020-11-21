using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IDamageable
{ 
    [SerializeField] private float health = 100;
    public float hp { get => health; }
    public List<(GameObject, float)> damageHistory = new List<(GameObject, float)>();
    public void damage(float amount, GameObject offender)
    {
        health -= amount > 0f ? amount : 0f;
        damageHistory.Add((offender, amount));
    }
    public float getDamageByPlayer(GameObject player)
    {
        float damage = 0;
        foreach (var item in damageHistory) if (item.Item1 == player) damage += item.Item2;
        return damage;
    }
}
