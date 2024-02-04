using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum playerState
    {
        moving,stand,charging,shooting,attacking,jumping
    }
    public float moveSpeed;

    public Transform player;

    public playerState pStatus;

    public bool grounded;

    public LayerMask Ground;
    public float playerHeight;
    public float groundDrag;

    public float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        pStatus = playerState.stand;
        groundDrag = gameObject.GetComponent<Rigidbody>().drag;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        if (grounded)
        {
            gameObject.GetComponent<Rigidbody>().drag = groundDrag;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().drag = groundDrag / 3.0f;
        }

        Inputs();
        SpeedCap();

        switch (pStatus)
        {
            case playerState.moving:
               
                gameObject.GetComponent<Rigidbody>().AddForce(player.transform.forward.normalized * moveSpeed, ForceMode.Force);
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                
                break;
            case playerState.stand:
                
                break;

            default: break;
        }

    }

    private void SpeedCap()
    {
        Vector3 flatvel = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, 0.0f, gameObject.GetComponent<Rigidbody>().velocity.z);

        if(flatvel.magnitude > moveSpeed * 2f)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed * 2f;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(limitedVel.x, gameObject.GetComponent<Rigidbody>().velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, 0.0f, gameObject.GetComponent<Rigidbody>().velocity.z);
        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Inputs()
    {
        float horizontalInput = Input.GetAxis("LeftHorizontal");
        float verticalInput = Input.GetAxis("LeftVertical");

        if (horizontalInput != 0.0f || verticalInput != 0.0f)
        {
            pStatus = playerState.moving;

        }
        else if (horizontalInput == 0.0f && verticalInput == 0.0f)
        {
            pStatus = playerState.stand;
        }

        if (Input.GetAxis("R2") > -1)
        {
            pStatus = playerState.attacking;
        }
        if (Input.GetAxis("L2") > -1)
        {
            pStatus = playerState.charging;

        }

        if(Input.GetButtonDown("Jump") && grounded) {
            Jump();
            Debug.Log("Jump");
        }

    }
}
