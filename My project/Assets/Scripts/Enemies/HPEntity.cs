using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    public int HP, maxHP;
    [SerializeField] bool customDeath;

    protected void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Debug.Log("Died");
        }
        if (!customDeath)
        {
            //INstantiate death FX
            Destroy(gameObject);
        }
    } 

    public void Heal(int amount, bool allowOverheal = false)
    {
        HP += amount;
        if (HP > maxHP && !allowOverheal)
        {
            HP = maxHP;
        }
    }

}
