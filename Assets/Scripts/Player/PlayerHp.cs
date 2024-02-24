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
        if(dying == true)
        {
            dieTimer += Time.deltaTime;
            if(dieTimer > 5.0f)
            {
                if (LoadPlayerData(Settings.archiveNum).difficulty == 2) File.Delete(Application.streamingAssetsPath + "/Archive" + Settings.archiveNum.ToString() + ".json");
                else
                {
                    DataToStore data = new DataToStore();
                    data.maxLevel = bossManager.currentBoss;
                    data.maxHp = 3;
                    data.charge = 0;
                    data.difficulty = LoadPlayerData(Settings.archiveNum).difficulty;
                    data.predetSettings = Settings.predetSettings;
                    data.volume = Settings.volume;
                    data.sensitivity = Settings.sensitivity;
                    data.FOV = Settings.fov;
                    data.tutorialMessages = Settings.tutorialMessages;
                    data.subtitles = Settings.subtitles;
                    data.subtitlesSize = Settings.subtitlesSize;
                    data.healthBar = Settings.healthBar;
                    data.VSync = Settings.VSync;
                    SavePlayerData(data, Settings.archiveNum);
                }
                SceneManager.LoadScene(1);
            }
            else
            {
                black.CrossFadeAlpha(1.0f, 4.5f, false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)//7 = Attack
        {
            Debug.Log("Hit");
            if (!isInvencible && other.gameObject.tag != "ParriedAttack") TakeDamage();
            if (other.gameObject.tag == "BasicProjectile") Destroy(other.gameObject);
        }
    }

    public void TakeDamage()
    {
        Debug.Log("Damaged");
        passiveAbility.canCharge = false;
        passiveAbility.canChargeTimer = 0.0f;
        playerHp--;
        isInvencible = true;
        invencibilityTimer = 0;
        gotDamaged = true;
        if(playerHp <= 0)
        {
            pAttack.enabled = false;
            pMov.enabled = false;
            cameraBehaviour.enabled = false;
            parry.enabled = false;
            pAbility.enabled = false;
            dying = true;
            pAnim.animState = PlayerAnimations.AnimationState.die;

        }
        else
        {
            pAnim.animState = PlayerAnimations.AnimationState.takeDmg;
        }
    }
    public DataToStore LoadPlayerData(int numArchive)
    {
        string path = Application.streamingAssetsPath + "/Archive" + numArchive.ToString() + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log(json);
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
        Debug.Log("Archive " + num.ToString() + " Created");
        //Comença Partida
    }

    public void Die()
    {

    }
}
