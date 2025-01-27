using UnityEngine;

public abstract class Unit : IDamageble
{
    protected int _currentHealth;

    protected int _currentMaxHealth;

    public Unit(int health, int maxHealth)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
    }

    public virtual void TakeDamage(int damageAmount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damageAmount;
        }
    }

    public bool IsAlive()
    {
        return _currentHealth > 0;
    }

    public int GetHealth()
    {
        return _currentHealth;
    }
}
