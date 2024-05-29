using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementLimbo : MonoBehaviour
{
    // Start is called before the first frame update
    public enum playerState
    {
        moving, stand
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

    public Animator animation;
    public AnimationClip[] animations;

    public AudioSource playerAudioSource;
    public AudioClip playerRunAudio;
    public enum AnimationState
    {
        idle, run
    }

    public AnimationState animState = AnimationState.idle;

    // Start is called before the first frame update
    void Start()
    {
        pStatus = playerState.stand;
        groundDrag = gameObject.GetComponent<Rigidbody>().drag;
        rb = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
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
        Animations();

        switch (pStatus)
        {
            case playerState.moving:
                rb.AddForce(player.transform.forward.normalized * moveSpeed, ForceMode.VelocityChange);
                rb.freezeRotation = true;
                animState = AnimationState.run;
                break;
            case playerState.stand:
                animState = AnimationState.idle;
                rb.freezeRotation = true;

                break;

            default:

                break;
        }
    }

    void Animations()
    {
        if (Time.timeScale != 0)
        {
            switch (animState)
            {
                case AnimationState.idle:
                    playerAudioSource.Stop();
                    //Debug.Log(animations[0].name);
                    animation.Play(animations[0].name);

                    break;
                case AnimationState.run:
                    playerAudioSource.clip = playerRunAudio;
                    playerAudioSource.loop = true;
                    if (playerAudioSource.isPlaying == false) playerAudioSource.Play();
                    //Debug.Log(animations[1].name);
                    animation.Play(animations[1].name);
                    break;
            }
        }
    }
    private void SpeedCap()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        if (flatvel.magnitude > moveSpeed * 2f)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed * 2f;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Inputs()
    {
        float horizontalInput = InputManager.GetAxis("LeftHorizontal");
        float verticalInput = InputManager.GetAxis("LeftVertical");

        if (horizontalInput != 0.0f || verticalInput != 0.0f)
        {

            pStatus = playerState.moving;

        }
        else if (horizontalInput == 0.0f && verticalInput == 0.0f && pStatus == playerState.moving)
        {
            pStatus = playerState.stand;
        }


    }
}
