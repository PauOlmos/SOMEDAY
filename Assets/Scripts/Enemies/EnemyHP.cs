using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int hp = 10;
    public bool canBeDamaged = true;
    public bool stun = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEnemy(int damage)
    {
        if (canBeDamaged)
        {
            hp -= damage;
            canBeDamaged = false;
        }
    }
}
