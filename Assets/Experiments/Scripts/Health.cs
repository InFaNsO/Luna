using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float health = 0.0f;
    [SerializeField] float MaxHealth = 30.0f;

    public Element myElement;

    private void Start()
    {
        health = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        ServiceLocator.Get<UIManager>().UpdateHPGauge(health / MaxHealth);
    }

    public void TakeHealth(float hp)
    {
        health += hp;
        ServiceLocator.Get<UIManager>().UpdateHPGauge(health / MaxHealth);

    }

    public bool IsAlive()
    {
        return health > 0.0f;
    }

    public void Respawn()
    {
        health = MaxHealth;
        ServiceLocator.Get<UIManager>().UpdateHPGauge(health / MaxHealth);

    }

    public float GetHealth() { return health; }
    public float GetMaxHealth() { return MaxHealth; }
}
