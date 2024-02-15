using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour
{
    public GameObject Boss;
    public CinemachineFreeLook Camera;
    public Transform initialPosition;

    public LayerMask Ground;
    public bool spawningBoss = false;
    public float spawningBossTimer = 0.0f;
    public float spawningBossTime = 4.0f;

    public float blackTimer = 0.0f;
    public float blackDuration = 5.0f;
    public Image black;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
        Boss.gameObject.GetComponent<Rigidbody>().useGravity = false;
        Boss.gameObject.GetComponent<Collider>().isTrigger = true;
        Camera.LookAt = initialPosition;
        Camera.Follow = initialPosition;

    }

    // Update is called once per frame
    void Update()
    {
        blackTimer += Time.deltaTime;
        if (blackTimer > blackDuration)
        {
            Debug.Log("!!!!!");
            gameObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
            if (Physics.Raycast(transform.position, Vector3.down, 1.5f + 0.2f, Ground))
            {
                spawningBoss = true;
            }
            if (spawningBoss)
            {
                spawningBossTimer += Time.deltaTime;
                if (spawningBossTimer < spawningBossTime)
                {
                    SpawnBoss();
                }
                else
                {
                    BeginTutorial();
                    Destroy(black.gameObject);
                    Destroy(gameObject.GetComponent<StartTutorial>());
                }
            }

        }
        else black.CrossFadeAlpha(0.0f, blackDuration, false);
        
        
    }

    public void BeginTutorial()
    {
        ActivateMovement();
        ActivateBoss();
        ActivateCamera();
    }
    public void SpawnBoss()
    {
        Boss.gameObject.GetComponent<Rigidbody>().transform.Translate(Vector3.up * Time.deltaTime * 2.0f);
    }

    public void ActivateMovement()
    {
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        gameObject.GetComponent<PassiveAbility>().enabled = true;
    }
    public void ActivateBoss()
    {
        Boss.GetComponent<Rigidbody>().useGravity = true;
        Boss.gameObject.GetComponent<Collider>().isTrigger = false;
    }
    public void ActivateCamera()
    {
        Camera.LookAt = gameObject.transform;
        Camera.Follow = gameObject.transform;
    }
}
