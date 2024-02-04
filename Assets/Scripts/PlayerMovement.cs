using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum playerState
    {
        moving,stand,charging,shooting,attacking
    }
    public float moveSpeed;

    public Transform player;

    public playerState pStatus;
    // Start is called before the first frame update
    void Start()
    {
        pStatus = playerState.stand;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("LeftHorizontal");
        float verticalInput = Input.GetAxis("LeftVertical");

        if (horizontalInput != 0.0f || verticalInput != 0.0f)
        {
            pStatus = playerState.moving;
            
        }
        else if(horizontalInput == 0.0f && verticalInput == 0.0f)
        {
            pStatus = playerState.stand;
        }

        switch (pStatus)
        {
            case playerState.moving:

                gameObject.GetComponent<Rigidbody>().AddForce(player.transform.forward.normalized * moveSpeed, ForceMode.Force);
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;

                break;
            case playerState.stand:
                
                break;
        }



    }
}
