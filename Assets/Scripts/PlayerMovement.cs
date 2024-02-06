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
    
    GameObject parry;
    Parry p;

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
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = groundDrag / 3.0f;
        }

        Inputs();
        SpeedCap();

        switch (pStatus)
        {
            case playerState.moving:
                canParry = true;
                canAttack = true;
                if(p.parrying == false) rb.AddForce(player.transform.forward.normalized * moveSpeed, ForceMode.Force);
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
    }

    private void Inputs()
    {
        float horizontalInput = Input.GetAxis("LeftHorizontal");
        float verticalInput = Input.GetAxis("LeftVertical");

        if (horizontalInput != 0.0f || verticalInput != 0.0f)
        {

            if (pStatus != playerState.dashing) pStatus = playerState.moving;

        }
        else if (horizontalInput == 0.0f && verticalInput == 0.0f && pStatus == playerState.moving)
        {
            pStatus = playerState.stand;
        }

        if (Input.GetAxis("R2") > -1 && pStatus != playerState.dashing)
        {
            pStatus = playerState.shooting;
        }
        if (Input.GetAxis("L2") > -1 && pStatus != playerState.dashing)
        {
            pStatus = playerState.charging;
        }
        if (pStatus == playerState.charging && Input.GetAxis("L2") == -1) pStatus = playerState.moving;


        if (Input.GetButtonDown("Dash") && pStatus != playerState.charging && pStatus != playerState.shooting) {
            Dash();
        }
        
        if(Input.GetButtonDown("Jump") && grounded) {
            Jump();
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
