using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour
{
    public GameObject Boss;
    public CinemachineFreeLook Camera;
    public Transform initialPosition;
    public Transform initialPlayerPosition;
    public bool klk = true;
    public BossManager BossManager;
    public LayerMask Ground;
    public bool spawningBoss = false;
    public float spawningBossTimer = 0.0f;
    public float spawningBossTime = 4.0f;
    public GameObject floor;
    public float blackTimer = 0.0f;
    public float blackInitialTimer = 0.0f;
    public float blackDuration = 13.0f;
    public float blackInitialDuration = 13.0f;
    public Image black;
    public AudioClip intro;
    // Start is called before the first frame update
    void Start()
    {
        Boss.gameObject.GetComponent<Rigidbody>().useGravity = false;
        Boss.gameObject.GetComponent<Collider>().isTrigger = true;
        Camera.LookAt = initialPosition;
        Camera.Follow = initialPosition;
        BossManager.enabled = false;
        transform.position = initialPlayerPosition.position;
        transform.rotation = initialPlayerPosition.rotation;
        BossManager.bossAudioSource.clip = intro;
        BossManager.bossAudioSource.Play();
        spawningBossTime = BossManager.audioTransitions[0].length + 0.5f;
        Camera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0;
        Camera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Settings.actualBoss == 0)
        {
            if (klk)
            {
                transform.position = initialPlayerPosition.position;
                transform.rotation = initialPlayerPosition.rotation;
            }
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Back"))
            {
                if (blackInitialTimer > 1.35f)
                {
                    BossManager.bossAudioSource.Stop();
                    blackInitialTimer = blackInitialDuration;
                }
                
            }
            blackInitialTimer += Time.deltaTime;
            if (blackInitialTimer > blackInitialDuration)
            {
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Back"))
                {
                    BossManager.bossAudioSource.Stop();
                    blackTimer = blackDuration;
                    black.CrossFadeAlpha(0.0f, 0.0f, true);

                }
                blackTimer += Time.deltaTime;
                if (blackTimer > blackDuration)
                {
                    
                    klk = false;
                   // Debug.Log("!!!!!");
                    gameObject.GetComponentInChildren<Rigidbody>().useGravity = true;

                    if (Physics.Raycast(transform.position, Vector3.down, 1.5f + 0.2f, Ground))
                    {
                        spawningBoss = true;
                    }
                    if (spawningBoss)
                    {
                       // Debug.Log("???????????");
                        spawningBossTimer += Time.deltaTime;
                        if (spawningBossTimer < spawningBossTime)
                        {
                            SpawnBoss();
                        }
                        else
                        {
                            BeginGame();
                            //Destroy(black.gameObject);
                            Destroy(gameObject.GetComponent<StartTutorial>());
                        }
                    }

                }
                else
                {
                    black.CrossFadeAlpha(0.0f, blackDuration, false);
                }
            }


        }
        else
        {
            blackTimer += Time.deltaTime;
            if (blackTimer < blackDuration / 10)
            {
                black.CrossFadeAlpha(0.0f, blackDuration / 10, false);
            }
            else
            {
                BeginGame();
                Destroy(gameObject.GetComponent<StartTutorial>());

            }
        }
    }

    public void BeginGame()
    {
        BossManager.bossAudioSource.Stop();
        ActivateMovement();
        ActivateBoss();
        ActivateCamera();
    }
    public void SpawnBoss()
    {
        Boss.gameObject.GetComponent<Rigidbody>().transform.Translate(Vector3.up * Time.deltaTime * 2.0f);
        BossManager.bossAudioSource.PlayOneShot(BossManager.audioTransitions[0]);
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
        BossManager.enabled = true;
        floor.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    public void ActivateCamera()
    {
        Camera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2;
        Camera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300;
        Camera.GetComponent<CameraBehaviour>().readyBoss = true;
        Camera.LookAt = gameObject.transform;
        Camera.Follow = gameObject.transform;
    }
}
