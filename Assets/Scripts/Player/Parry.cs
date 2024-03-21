using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    // Start is called before the first frame update

    private float parryTimer;
    private float parryCooldown = 3.0f;
    private float parryDuration = 0.42f;
    private bool parryActive = true;
    
    PassiveAbility passiveAbility;
    public bool parrying = false;
    GameObject player;
    PlayerMovement pMov;
    PlayerAttack pAttack;
    PlayerHp pHp;

    public LayerMask attack;
    public LayerMask nothing;

    public Transform parryPos;

    public GameObject shield;

    public AudioSource playerAudioSource;
    public AudioClip successfulParry;
    void Start()
    {
        player = GameObject.Find("Player");
        pMov = player.GetComponent<PlayerMovement>();
        passiveAbility = player.GetComponent<PassiveAbility>();
        pAttack = GameObject.Find("Sword").GetComponent<PlayerAttack>();
        pHp = player.GetComponent<PlayerHp>();
    }

    // Update is called once per frame
    void Update()
    {//Que no estigui invencible
        if(Input.GetButtonDown("Parry") && parryActive == true && pMov.grounded == true && pMov.canParry == true && pAttack.attacking == false && pHp.isInvencible == false)
        {
            UseParry();
        }
        if(parryActive == false)
        { 
            parryTimer += Time.deltaTime;
            shield.transform.localScale += Vector3.one * Time.deltaTime* 0.55f;
            if(parryTimer > parryDuration)
            {
                SetParry(false);
                //pMov.canAttack = true;
                parrying = false;
                player.GetComponentInChildren<BoxCollider>().excludeLayers = nothing;

            }
            else
            {
                pMov.canAttack = false;
                if (pMov.pStatus != PlayerMovement.playerState.dashing)
                {

                    gameObject.transform.position = parryPos.position;
                    gameObject.transform.rotation = parryPos.rotation;
                }
                else
                {
                    gameObject.transform.position = new Vector3(0, -30000, 0);

                }
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
                playerAudioSource.loop = false;
                playerAudioSource.PlayOneShot(successfulParry);
                passiveAbility.passiveCharge += 2.5f;
                other.gameObject.transform.localScale = Vector3.zero;
                Destroy(other.gameObject);
                Debug.Log(other.gameObject.transform.position);
            }
            if (other.gameObject.tag == "Parryable")
            {

                playerAudioSource.loop = false;
                playerAudioSource.PlayOneShot(successfulParry);

                passiveAbility.passiveCharge += 5.0f;
                other.gameObject.tag = "ParriedAttack";
            }
            if(other.gameObject.tag == "ReturnableProjectile")
            {
                other.gameObject.GetComponent<SeekingProjectile>().shotByPlayer = true;
                other.gameObject.GetComponent<SeekingProjectile>().canFail = false;
                other.gameObject.GetComponent<SeekingProjectile>().target = GameObject.Find("Boss").transform;
                other.gameObject.GetComponent<DieByTime>().deathTime = 20.0f;
                other.gameObject.tag = "Untagged";
                other.gameObject.layer = 0;
                other.gameObject.AddComponent<Rigidbody>();
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                other.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)//7 = Attack
        {
            if (collision.gameObject.tag == "BasicProjectile")
            {
                Debug.Log(collision.gameObject.name);
                playerAudioSource.loop = false;
                playerAudioSource.PlayOneShot(successfulParry);
                passiveAbility.passiveCharge += 2.5f;
                collision.gameObject.transform.Translate(Vector3.up * 10000.0f);
                Destroy(collision.gameObject);
                Debug.Log(collision.gameObject.transform.position);

            }
            if (collision.gameObject.tag == "Parryable")
            {
                Debug.Log(collision.gameObject.name);
                playerAudioSource.loop = false;
                playerAudioSource.PlayOneShot(successfulParry);
                passiveAbility.passiveCharge += 5.0f;
                collision.gameObject.tag = "ParriedAttack";
            }
            if (collision.gameObject.tag == "ReturnableProjectile")
            {
                collision.gameObject.GetComponent<SeekingProjectile>().shotByPlayer = true;
                collision.gameObject.GetComponent<SeekingProjectile>().canFail = false;
                collision.gameObject.GetComponent<SeekingProjectile>().target = GameObject.Find("Boss").transform;
                collision.gameObject.GetComponent<DieByTime>().deathTime = 20.0f;
                collision.gameObject.layer = 0;
                collision.gameObject.tag = "Untagged";
                collision.gameObject.AddComponent<Rigidbody>();
                collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
                collision.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            }
        }
    }

    void UseParry()
    {
        SetParry(true);
        parryActive = false;
        parrying = true;
        player.GetComponentInChildren<BoxCollider>().excludeLayers = attack;
    }

    void SetParry(bool active)
    {
        if (active == false)
        {
            shield.transform.localScale = new Vector3(0, 0, 0);
            gameObject.transform.position = new Vector3(0, -30000, 0);
        }

        //gameObject.GetComponent<MeshRenderer>().enabled = active;
        gameObject.GetComponent<BoxCollider>().enabled = active;

    }
}
