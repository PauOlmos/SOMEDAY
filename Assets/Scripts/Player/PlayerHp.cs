using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public int playerHp;
    private float invencibilityCooldown = 0.75f;
    public bool isInvencible;
    private float invencibilityTimer;
    private bool gotDamaged = false;
    private float damagedTime = 0.25f;
    private float cameraShakeTimer;
    public bool dying = false;
    public float dieTimer = 0.0f;
    public BossManager bossManager;
    GameObject GameCamera;
    CameraBehaviour cameraBehaviour;

    public PlayerAnimations pAnim;
    public Image black;
    PassiveAbility passiveAbility;

    public PlayerAttack pAttack;
    public PlayerMovement pMov;
    public Parry parry;
    public PassiveAbility pAbility;

    public float lifeTime = 0.0f;
    public AudioSource playerAudioSource;
    public AudioClip takeDamageAudio;
    public AudioClip deathAudio;

    public int difficulty;

    public int[] difficultyBasedHPs = { 10, 5, 3 };

    // Start is called before the first frame update
    void Start()
    {
        difficulty = LoadPlayerData(Settings.archiveNum).difficulty;
        //Debug.Log("CurrentBoss = " + Settings.actualBoss + ". MaxLevel = " + LoadPlayerData(Settings.archiveNum).maxLevel);
        if (Settings.actualBoss == LoadPlayerData(Settings.archiveNum).maxLevel) playerHp = LoadPlayerData(Settings.archiveNum).maxHp;
        else
        {
            //Debug.Log(difficultyBasedHPs[0]);
            playerHp = difficultyBasedHPs[difficulty];
        }
        GameCamera = GameObject.Find("Game Camera");
        cameraBehaviour = GameCamera.GetComponent<CameraBehaviour>();
        passiveAbility = gameObject.GetComponent<PassiveAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
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
                //CameraBehaviour.ActivateCameraShake(0.0f, 0.0f);
            }
            else
            {
                //CameraBehaviour.CameraShake(3.0f, 0.25f);
            }
        }
        if(dying == true)
        {
            dieTimer += Time.deltaTime;
            if(dieTimer > 10.0f)
            {
                if (LoadPlayerData(Settings.archiveNum).difficulty == 2)
                {
                    File.Delete(Application.streamingAssetsPath + "/Archive" + Settings.archiveNum.ToString() + ".json");
                    SceneManager.LoadScene(0);//Main menu
                }
                else SceneManager.LoadScene(1);//LvlSelector
            }
            else
            {
                black.CrossFadeAlpha(1.0f, 4.5f, false);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)//7 = Attack
        {
            //Debug.Log("Hit");
            if (!isInvencible && collision.gameObject.tag != "ParriedAttack")
            {
                //Debug.Log("Damaged from " + collision.gameObject.name);

                TakeDamage();
            }
            if (collision.gameObject.tag == "BasicProjectile") Destroy(collision.gameObject);
            if (collision.gameObject.name == "HandDamage")
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * 150, ForceMode.Impulse);
            }
            if (collision.gameObject.name == "HandDamage2")
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * 150, ForceMode.Impulse);
            }
            if (collision.gameObject.name == "SpikesWall" && playerHp > 0)
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1) * 50, ForceMode.Impulse);
            }
            if (collision.gameObject.name == "BossFaceWithHoles" && playerHp > 0)
            {
                Debug.Log("HitHead");
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1) * 150, ForceMode.Impulse);
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * 20, ForceMode.Impulse);
            }
            if (collision.gameObject.name == "Car" && playerHp > 0)
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1) * 550, ForceMode.Impulse);
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * 30, ForceMode.Impulse);
            }
        }
        if (collision.gameObject.layer == 10)//7 = Attack
        {
            playerHp -= 20;
            TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)//7 = Attack
        {
            //Debug.Log("Hit");
            if (!isInvencible && other.gameObject.tag != "ParriedAttack")
            {
                //Debug.Log("Damaged from " + other.gameObject.name);

                TakeDamage();
            }
            if (other.gameObject.tag == "BasicProjectile") Destroy(other.gameObject);
            if (other.gameObject.name == "HandDamage")
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * 150, ForceMode.Impulse);
            }
            if (other.gameObject.name == "HandDamage2")
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * 150, ForceMode.Impulse);
            }
        }
        if (other.gameObject.layer == 10)//7 = Attack
        {
            playerHp -= 20;
            TakeDamage();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7)//7 = Attack
        {
            //Debug.Log("Hit");
            if (!isInvencible && other.gameObject.tag != "ParriedAttack")
            {
                //Debug.Log("Damaged from " + other.gameObject.name);

                TakeDamage();
            }
            if (other.gameObject.tag == "BasicProjectile") Destroy(other.gameObject);
            if (other.gameObject.name == "HandDamage")
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * 150, ForceMode.Impulse);
            }
            if (other.gameObject.name == "HandDamage2")
            {
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * 150, ForceMode.Impulse);
            }
        }

    }

    public void TakeDamage()
    {
        if(dying == false)
        {
            CameraBehaviour.ActivateCameraShake(9.0f, 0.5f);
            passiveAbility.canCharge = false;
            passiveAbility.canChargeTimer = 0.0f;
            if (bossManager.currentBoss != 4) playerHp--;
            else playerHp -= 2;
            isInvencible = true;
            invencibilityTimer = 0;
            gotDamaged = true;
            if (playerHp <= 0)
            {
                playerAudioSource.PlayOneShot(deathAudio);
                pAttack.enabled = false;
                pMov.enabled = false;
                cameraBehaviour.enabled = false;
                parry.enabled = false;
                pAbility.enabled = false;
                dying = true;
                bossManager.bossAudioSource.volume /= 2;
                pAnim.animState = PlayerAnimations.AnimationState.die;

            }
            else
            {
                playerAudioSource.PlayOneShot(takeDamageAudio);
                pAnim.animState = PlayerAnimations.AnimationState.takeDmg;
            }
        }
        
    }
    public DataToStore LoadPlayerData(int numArchive)
    {
        string path = Application.streamingAssetsPath + "/Archive" + numArchive.ToString() + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            //Debug.Log(json);
            return JsonUtility.FromJson<DataToStore>(json);
        }
        else
        {
            Debug.LogWarning("No se encontraron datos de jugador guardados.");
            return null;
        }
    }

    public void SavePlayerData(DataToStore data, int num)
    {
        string json = JsonUtility.ToJson(data);
        string archiveNum = "Archive" + num.ToString(); ;
        File.WriteAllText(Application.streamingAssetsPath + "/" + archiveNum + ".json", json);
        //Debug.Log("Archive " + num.ToString() + " Created");
        //Comença Partida
    }

}
