using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    // Start is called before the first frame update

    private float parryTimer;
    private float parryCooldown = 3.0f;
    private float parryDuration = 0.25f;
    private bool parryActive = true;
    
    PassiveAbility passiveAbility;
    public bool parrying = false;
    GameObject player;
    PlayerMovement pMov;
    PlayerAttack pAttack;
    void Start()
    {
        player = GameObject.Find("Player");
        pMov = player.GetComponent<PlayerMovement>();
        passiveAbility = player.GetComponent<PassiveAbility>();
        pAttack = GameObject.Find("Sword").GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Parry") && parryActive == true && pMov.grounded == true && pMov.canParry == true && pAttack.attacking == false)
        {
            UseParry();
        }
        if(parryActive == false)
        { 
            parryTimer += Time.deltaTime;
            
            if(parryTimer > parryDuration)
            {
                SetParry(false);
                //pMov.canAttack = true;
                parrying = false;
            }
            else
            {
                pMov.canAttack = false;
            }

            if (parryTimer > parryCooldown){
                parryTimer = 0;
                parryActive = true;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)//7 = Attack
        {
            if (other.gameObject.tag == "BasicProjectile") 
            {
                passiveAbility.passiveCharge += 2.5f;
                Destroy(other.gameObject);
            }
            if (other.gameObject.tag == "Parryable")
            {
                Debug.Log("NiceParryG");
                passiveAbility.passiveCharge += 5.0f;
                other.gameObject.tag = "ParriedAttack";
            }
        }
    }


    void UseParry()
    {
        SetParry(true);
        parryActive = false;
        parrying = true;
    }

    void SetParry(bool active)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = active;
        gameObject.GetComponent<BoxCollider>().enabled = active;

    }
}
