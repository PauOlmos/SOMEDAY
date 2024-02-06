using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    public int playerHp;
    private float invencibilityCooldown = 0.75f;
    private bool isInvencible;
    private float invencibilityTimer;
    private bool gotDamaged = false;
    private float damagedTime = 0.25f;
    private float cameraShakeTimer;

    GameObject GameCamera;
    CameraBehaviour cameraBehaviour;

    PassiveAbility passiveAbility;
    // Start is called before the first frame update
    void Start()
    {
        playerHp = 3;
        GameCamera = GameObject.Find("Game Camera");
        cameraBehaviour = GameCamera.GetComponent<CameraBehaviour>();
        passiveAbility = gameObject.GetComponent<PassiveAbility>();
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
        if( gotDamaged )
        {
            cameraShakeTimer += Time.deltaTime;
            if(cameraShakeTimer > damagedTime )
            {
                gotDamaged = false;
                cameraShakeTimer = 0;
                cameraBehaviour.CameraShake(0.0f, 0.0f);
            }
            else
            {
                cameraBehaviour.CameraShake(3.0f, 0.25f);
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
        passiveAbility.canCharge = false;
        passiveAbility.canChargeTimer = 0.0f;
        playerHp--;
        isInvencible = true;
        invencibilityTimer = 0;
        gotDamaged = true;
    }
}
