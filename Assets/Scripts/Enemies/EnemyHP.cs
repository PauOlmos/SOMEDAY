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
        if(gameObject.tag == "Enemy" && hp <= 0)
        {
            Destroy(gameObject);
        } 
    }

    public void DamageEnemy(int damage, bool weakPoint)
    {
        if (canBeDamaged || weakPoint)
        {
            if (hp > 0)
            {
                if(gameObject.GetComponent<HighSchoolBoss>() != null)
                {
                    if(gameObject.GetComponent<HighSchoolBoss>().phase == 0)
                    {
                        gameObject.GetComponent<HighSchoolBoss>().hitsToSuperAttack -= damage;
                    }
                }
                hp -= damage;
                canBeDamaged = false;
            }
        }
    }
}
