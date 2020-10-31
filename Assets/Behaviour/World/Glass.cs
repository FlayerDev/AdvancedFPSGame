using UnityEngine;

public class Glass : MonoBehaviour, IDamageable
{
    [SerializeField]private float health = 50;
    public float hp { get => health; }

    public void damage(float amount)
    {
        health -= amount > 0f ? amount : 0f;
        if (health < 0f) shutter();
    }
    void shutter()
    {
    
    }
}
