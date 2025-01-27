using UnityEngine;

public interface IDamageble
{

    int GetHealth();
    bool IsAlive();
    void TakeDamage(int damage);





    public void DamageUnity(int damageAmount)
    {
       
    }
    public void HealUnity(int healAmount)
    {
       
    }
}
