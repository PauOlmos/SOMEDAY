using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    public int playerHp;
    private float invencibilityCooldown = 0.75f;
    private bool isInvencible;
    private float invencibilityTimer;
    // Start is called before the first frame update
    void Start()
    {
        playerHp = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(isInvencible)
        {
            invencibilityTimer += Time.deltaTime;
            if(invencibilityTimer > invencibilityCooldown )
            {
                isInvencible = false;
                invencibilityTimer = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)//7 = Attack
        {
            Debug.Log("Hit");
            if(!isInvencible && collision.gameObject.tag != "ParriedAttack") TakeDamage();
            if(collision.gameObject.tag == "BasicProjectile") Destroy(collision.gameObject);
        }
    }

    void TakeDamage()
    {
        Debug.Log("Damaged");
        playerHp--;
        isInvencible = true;
        invencibilityTimer = 0;
    }
}
