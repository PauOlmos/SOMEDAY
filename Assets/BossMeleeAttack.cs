using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "ParriedAttack")
        {
            gameObject.GetComponentInParent<EnemyHP>().canBeDamaged = true;
            gameObject.GetComponentInParent<EnemyHP>().stun = true;
            gameObject.SetActive(false);
        }
    }
}
