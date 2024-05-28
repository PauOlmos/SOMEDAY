using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum playerState
    {
        moving,stand,charging,shooting,attacking,dashing
    }
    public float moveSpeed;

    public Transform player;

    public playerState pStatus;

    public bool grounded;

    public LayerMask Ground;
    public float playerHeight;
    public float groundDrag;

    public float jumpForce;

    public Rigidbody rb;
    public float dashForce;
    public float dashTime;
    public float dashTimer;

    float playerMass;
    public bool canParry = true;
    public bool canAttack = true;
    public bool canJump = true;
    public float canJumpTimer = 0;

    public PlayerAttack pAttack;
    public ParticleSystem chargeParticles;
    GameObject parry;
    Parry p;

    public PlayerAnimations pAnim;
    // Start is called before the first frame update
    void Start()
    {
        pStatus = playerState.stand;
        groundDrag = gameObject.GetComponent<Rigidbody>().drag;
        rb = gameObject.GetComponent<Rigidbody>();
        playerMass = rb.mass;

        parry = GameObject.Find("Shield");
        p = parry.GetComponent<Parry>();
    }

    // Update is called once per frame
    void Update()
    {

        CheckCharging();

        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.1f + 0.35f, Ground)
        || Physics.Raycast(new Vector3(transform.position.x + 0.35f, transform.position.y, transform.position.z), Vector3.down, playerHeight * 0.5f + 0.2f, Ground)
        || Physics.Raycast(new Vector3(transform.position.x - 0.35f, transform.position.y, transform.position.z), Vector3.down, playerHeight * 0.5f + 0.2f, Ground)
        || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.35f), Vector3.down, playerHeight * 0.5f + 0.2f, Ground)
        || Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.35f), Vector3.down, playerHeight * 0.5f + 0.2f, Ground)
        )
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (grounded)
        {
            //gameObject.GetComponentInChildren<BoxCollider>().size = new Vector3(gameObject.GetComponentInChildren<BoxCollider>().size.x, 1.5f, gameObject.GetComponentInChildren<BoxCollider>().size.z);
            rb.drag = groundDrag;
        }
        else
        {
            //gameObject.GetComponentInChildren<BoxCollider>().size = new Vector3(gameObject.GetComponentInChildren<BoxCollider>().size.x, 0.5f, gameObject.GetComponentInChildren<BoxCollider>().size.z);
            rb.drag = groundDrag / 1.1f;
        }

        //if(rb.velocity.y < 0) gameObject.GetComponentInChildren<BoxCollider>().size = new Vector3(gameObject.GetComponentInChildren<BoxCollider>().size.x, 1.5f, gameObject.GetComponentInChildren<BoxCollider>().size.z);

        if (Time.timeScale != 0) Inputs();
        SpeedCap();

        switch (pStatus)
        {
            case playerState.moving:
                canParry = true;
                canAttack = true;
                if(p.parrying == false && pAttack.attackActive == true) rb.AddForce(player.transform.forward.normalized * moveSpeed, ForceMode.VelocityChange);
                rb.freezeRotation = true;
                
                break;
            case playerState.stand:
                canParry = true;
                canAttack = true;

                rb.freezeRotation = true;

                break;
            
            case playerState.dashing:
                canParry = false;
                canAttack = false;


                dashTimer += Time.deltaTime;
                if (dashTimer > dashTime)
                {
                    pStatus = playerState.stand;
                    dashTimer = 0;
                    rb.mass = playerMass;

                }
                else
                {
                    rb.drag = 0.0f;
                    rb.freezeRotation = true;
                }
                rb.freezeRotation = true;

                break;

            default:
                canParry = false;
                canAttack = false;


                rb.freezeRotation = true;

                break;
        }

    }

    private void CheckCharging()
    {
        if(pStatus == playerState.charging)
        {
            chargeParticles.gameObject.SetActive(true);
        }
        else
        {
            chargeParticles.gameObject.SetActive(false);
        }
    }

    private void SpeedCap()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        if(flatvel.magnitude > moveSpeed * 2f && pStatus != playerState.dashing)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed * 2f;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        pAnim.animState = PlayerAnimations.AnimationState.jump;
    }

    private void Inputs()
    {
        float horizontalInput = InputManager.GetAxis("LeftHorizontal");
        float verticalInput = InputManager.GetAxis("LeftVertical");

        if (horizontalInput != 0.0f || verticalInput != 0.0f)
        {

            if (pStatus != playerState.dashing) pStatus = playerState.moving;

        }
        else if (horizontalInput == 0.0f && verticalInput == 0.0f && pStatus == playerState.moving)
        {
            //Debug.Log("Standddddddd");
            pStatus = playerState.stand;
        }

        if (InputManager.GetAxis("R2") > -1 && pStatus != playerState.dashing)
        {
            pStatus = playerState.shooting;
        }
        if (InputManager.GetAxis("L2") > -1 && pStatus != playerState.dashing)
        {
            //Debug.Log("Charge");
            pStatus = playerState.charging;
        }
        if (pStatus == playerState.charging && InputManager.GetAxis("L2") == -1) pStatus = playerState.moving;


        if (InputManager.GetButtonDown("Dash") && pStatus != playerState.charging && pStatus != playerState.shooting) {
            Dash();
        }
        
        if(InputManager.GetButtonDown("Jump") && grounded && canJump == true && pStatus != playerState.charging && pStatus != playerState.dashing) {
            Jump();
        }
        else
        {
            canJumpTimer += Time.deltaTime;
            if(canJumpTimer > 0.05f)
            {
                canJumpTimer = 0;
                canJump = true;
            }
        }


    }

    private void Dash()
    {
        pStatus = playerState.dashing;
        rb.drag = 0;
        rb.AddForce(player.transform.forward * dashForce, ForceMode.Impulse);
        rb.mass = playerMass/3;
    }
}
