using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class HPEntity : MonoBehaviour
{
    public float HP, maxHP;
    [SerializeField] bool customDeath;
    public UnityEvent Death;

    protected void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float TakeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Debug.Log("Died");

            if (!customDeath)
            {
                //INstantiate death FX
                Death.Invoke();
                PhotonNetwork.Destroy(gameObject);  // This assumes the current object is networked! Otherwise will not work!
                                                    // Destroy(gameObject);
            }
            return 0;
        }
        return ((float)HP) / maxHP;
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
