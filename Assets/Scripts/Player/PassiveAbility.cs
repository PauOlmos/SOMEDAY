using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PassiveAbility : MonoBehaviour
{

    public float passiveCharge = 0.0f;
    public bool canCharge = true;
    public float canChargeTimer;
    public float chargeTime = 10.0f;

    private float waitTime = 0.2f;
    private float waitTimer = 0.0f;

    public bool isCharged = false;

    public float necessaryCharge;
    public enum passiveType
    {
        shoot,hp
    }

    public passiveType passive = passiveType.shoot;
    PlayerHp playerHp;
    public Transform boss;
    public GameObject playerProjectile;
    GameObject cam;
    CameraBehaviour camBehaviour;
    public bool shootNow = false;
    public bool healNow = false;
    public BossManager bossManager;

    public AudioSource playerAudioSource;
    public AudioClip swapAbilities;

    public int[] difficultyBasedHP = { 10, 5, 3 };
    public int[] difficultyBasedRestoredHp = { 5, 3, 1 };
    public int[] difficultyBasedNecessaryCharge = { 45, 90, 120 };
    public int difficulty;
    public AudioSource chargeAudioSource;
    public AudioClip chargedSound;
    public bool hasSounded = false;
    // Start is called before the first frame update
    void Start()
    {
        difficulty = LoadPlayerData(Settings.archiveNum).difficulty;
        playerHp = GetComponent<PlayerHp>();
        cam = GameObject.Find("Game Camera");
        camBehaviour = cam.GetComponent<CameraBehaviour>();
        necessaryCharge = difficultyBasedNecessaryCharge[difficulty];
        if (bossManager.currentBoss < LoadPlayerData(Settings.archiveNum).maxLevel) passiveCharge = LoadPlayerData(Settings.archiveNum).charge;
        else passiveCharge = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCharge)
        {
            if (passiveCharge < necessaryCharge) passiveCharge += Time.deltaTime;
            else if (hasSounded == false)
            {
                hasSounded = true;
                chargeAudioSource.PlayOneShot(chargedSound);
            }
        }
        else
        {
            canChargeTimer += Time.deltaTime;
            if(canChargeTimer > chargeTime)
            {
                canChargeTimer = 0.0f;
                canCharge = true;
            }
        }

        if (InputManager.GetButtonDown("SwapAbilities") && Time.timeScale != 0)
        {
            SwapAbilities();
        }

        if(gameObject.GetComponent<PlayerMovement>().pStatus == PlayerMovement.playerState.charging || gameObject.GetComponent<PlayerMovement>().pStatus == PlayerMovement.playerState.shooting)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > waitTime)
            {
                switch (gameObject.GetComponent<PlayerMovement>().pStatus)
                {
                    case PlayerMovement.playerState.charging:
                        passiveCharge += Time.deltaTime;
                        break;
                    case PlayerMovement.playerState.shooting:
                        if (passiveCharge >= necessaryCharge)
                        {
                            switch (passive)
                            {
                                case passiveType.shoot:

                                    GameObject passiveProjectile = Instantiate(playerProjectile, transform.position, Quaternion.identity);
                                    passiveProjectile.GetComponent<SeekingProjectile>().target = bossManager.boss.transform;
                                    passiveProjectile.GetComponent<SeekingProjectile>().speed = 10.0f;
                                    passiveProjectile.GetComponent<SeekingProjectile>().canFail = false;
                                    passiveProjectile.GetComponent<SeekingProjectile>().shotByPlayer = true;
                                    isCharged = false;
                                    passiveCharge = 0;
                                    hasSounded = false;
                                    shootNow = true;
                                    break;
                                case passiveType.hp:
                                    //Debug.Log(difficulty);
                                        if (playerHp.playerHp < difficultyBasedHP[difficulty] + 1)
                                        {
                                            RestoreHp(difficultyBasedRestoredHp[difficulty]);
                                            isCharged = false;
                                        hasSounded = false;

                                        passiveCharge = 0;
                                            healNow = true;
                                            break;
                                        }

                                    break;

                            }
                            break;
                        }
                        else break;
                        
                }
            }
            
        }

    }

    private void SwapAbilities()
    {
        BossManager.SoundEffect(swapAbilities);
        switch (passive)
        {
            case passiveType.shoot:
                passive = passiveType.hp;
                break;
            case passiveType.hp:
                passive = passiveType.shoot;
                break;

        }
    }

    private void RestoreHp(int hp)
    {
        playerHp.playerHp += hp;
        if (playerHp.playerHp > difficultyBasedHP[difficulty] + 1) playerHp.playerHp = difficultyBasedHP[difficulty] + 1;
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
}
