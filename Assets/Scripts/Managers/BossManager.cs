using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BossManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Positions")]
    public Transform[] playerSpawnPositions;
    public Transform[] bossSpawnPositions;
    public GameObject floor;

    [Header("TutorialBoss")]
    public int currentBoss;

    public int phase = 0;

    public int bossAttack;

    public int bossMovement;

    public bool canMove = true;
    public bool canAttack = true;

    public GameObject player;
    public GameObject boss;
    public GameObject auxiliarBoss;

    public Transform tutorialMap;
    public float radius;

    public GameObject[] circlesPrefabs = new GameObject[3];
    public NavMeshAgent agent;
    public float transfromTimer = 0.0f;
    public GameObject proximityArea;
    public GameObject weakPoint;
    public LayerMask Ground;

    public GameObject Sword1;
    public GameObject Sword2;

    [Header("HighSchool Boss")]

    public GameObject TutorialWalls;

    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;

    public GameObject allTables;
    public GameObject teacherTable;
    public GameObject[] tableAttackPositions;
    public GameObject[] tableRestPositions;

    public GameObject armari1;
    public GameObject armari2;

    public GameObject armariPos1;
    public GameObject armariPos2;

    public GameObject armariResetPos1;
    public GameObject armariResetPos2;


    public GameObject bossShield;
    public GameObject portalSpawnArea;

    public GameObject portalPrefab;


    public GameObject corridorPos;
    public GameObject door1;
    public GameObject door2;

    public GameObject doorPos1;
    public GameObject doorPos2;
    public GameObject checkPlayerOnCorridor;

    public GameObject projectileSource;
    public GameObject projectilePrefab;

    public GameObject hand1;
    public GameObject hand2;
    public GameObject hand3;
    public GameObject handPos1;
    public GameObject handPos2;
    public GameObject handPos3;

    public GameObject handDamage1;
    public GameObject handDamage2;
    public GameObject handDamage3;

    public GameObject weakPoint1;
    public GameObject weakPoint2;
    public GameObject weakPoint3;

    public GameObject shadowDogPortalPos1;
    public GameObject shadowDogPortalPos2;
    public GameObject shadowDogPortalPos3;

    public GameObject shadowDogPortalPrefab;
    public GameObject corridorFloor;
    public GameObject scenarioFloor;

    public GameObject lightTarget1;
    public GameObject lightTarget2;

    public GameObject foco1;
    public GameObject foco2;

    public GameObject projectileBossSource;
    public GameObject[] monolithWeakPoints;

    void Start()
    {
        currentBoss = Settings.actualBoss;
        Debug.Log(currentBoss);
        ActivateBoss(currentBoss);
    }

    // Update is called once per frame
    void Update()
    {
        CheckBossHp(currentBoss);
    }

    public void ActivateBoss(int nBoss)
    {
        switch (nBoss)
        {
            case 0:
                boss = GameObject.Find("Boss");
                boss.AddComponent<TutorialBoss>();
                boss.GetComponent<TutorialBoss>().player = player;
                boss.GetComponent<TutorialBoss>().tutorialMap = tutorialMap;
                boss.GetComponent<TutorialBoss>().radius = radius;
                boss.GetComponent<TutorialBoss>().circlesPrefabs = circlesPrefabs;
                boss.GetComponent<TutorialBoss>().Ground = 6;
                boss.GetComponent<TutorialBoss>().agent = agent;
                boss.GetComponent<TutorialBoss>().proximityArea = proximityArea;
                boss.GetComponent<TutorialBoss>().weakPoint = weakPoint;
                boss.GetComponent<TutorialBoss>().Sword1 = Sword1;
                boss.GetComponent<TutorialBoss>().Sword2 = Sword2;
                break;

            case 1:

                if(player.GetComponent<PlayerHp>().lifeTime < 15.0f)
                {
                    player.transform.position = playerSpawnPositions[nBoss].position;
                    boss.transform.position = bossSpawnPositions[nBoss].position;
                    boss.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                }
                Destroy(Sword1);
                Destroy(Sword2);
                Destroy(weakPoint);
                boss.AddComponent<StartHighSchoolBoss>();
                boss.GetComponent<StartHighSchoolBoss>().TutorialWalls = TutorialWalls;
                boss.GetComponent<StartHighSchoolBoss>().wall1 = wall1;
                boss.GetComponent<StartHighSchoolBoss>().wall2 = wall2;
                boss.GetComponent<StartHighSchoolBoss>().wall3 = wall3;
                boss.GetComponent<StartHighSchoolBoss>().wall4 = wall4;
                boss.GetComponent<StartHighSchoolBoss>().allTables = allTables;
                boss.GetComponent<StartHighSchoolBoss>().player = player;
                boss.GetComponent<StartHighSchoolBoss>().agent = agent;
                boss.GetComponent<StartHighSchoolBoss>().proximityArea = proximityArea;
                boss.GetComponent<StartHighSchoolBoss>().floor = floor;
                boss.GetComponent<StartHighSchoolBoss>().teacherTable = teacherTable;
                boss.GetComponent<StartHighSchoolBoss>().tableAttackPositions = tableAttackPositions;
                boss.GetComponent<StartHighSchoolBoss>().tableRestPositions = tableRestPositions;
                boss.GetComponent<StartHighSchoolBoss>().armari1 = armari1;
                boss.GetComponent<StartHighSchoolBoss>().armari2 = armari2;
                boss.GetComponent<StartHighSchoolBoss>().armariPos1 = armariPos1;
                boss.GetComponent<StartHighSchoolBoss>().armariPos2 = armariPos2;
                boss.GetComponent<StartHighSchoolBoss>().armariResetPos1 = armariResetPos1;
                boss.GetComponent<StartHighSchoolBoss>().armariResetPos2 = armariResetPos2;
                boss.GetComponent<StartHighSchoolBoss>().portalSpawnArea = portalSpawnArea;
                boss.GetComponent<StartHighSchoolBoss>().portalPrefab = portalPrefab;
                boss.GetComponent<StartHighSchoolBoss>().bossShield = bossShield;
                boss.GetComponent<StartHighSchoolBoss>().corridorPos = corridorPos;
                boss.GetComponent<StartHighSchoolBoss>().door1 = door1;
                boss.GetComponent<StartHighSchoolBoss>().door2 = door2;
                boss.GetComponent<StartHighSchoolBoss>().doorPos1 = doorPos1;
                boss.GetComponent<StartHighSchoolBoss>().doorPos2 = doorPos2;
                boss.GetComponent<StartHighSchoolBoss>().projectileSource = projectileSource;
                boss.GetComponent<StartHighSchoolBoss>().projectilePrefab = projectilePrefab;
                boss.GetComponent<StartHighSchoolBoss>().hand1 = hand1;
                boss.GetComponent<StartHighSchoolBoss>().hand2 = hand2;
                boss.GetComponent<StartHighSchoolBoss>().hand3 = hand3;
                boss.GetComponent<StartHighSchoolBoss>().handPos1 = handPos1;
                boss.GetComponent<StartHighSchoolBoss>().handPos2 = handPos2;
                boss.GetComponent<StartHighSchoolBoss>().handPos3 = handPos3;

                boss.GetComponent<StartHighSchoolBoss>().handDamage1 = handDamage1;
                boss.GetComponent<StartHighSchoolBoss>().handDamage2 = handDamage2;
                boss.GetComponent<StartHighSchoolBoss>().handDamage3 = handDamage3;

                boss.GetComponent<StartHighSchoolBoss>().weakPoint1 = weakPoint1;
                boss.GetComponent<StartHighSchoolBoss>().weakPoint2 = weakPoint2;
                boss.GetComponent<StartHighSchoolBoss>().weakPoint3 = weakPoint3;

                boss.GetComponent<StartHighSchoolBoss>().shadowDogPortalPos1 = shadowDogPortalPos1;
                boss.GetComponent<StartHighSchoolBoss>().shadowDogPortalPos2 = shadowDogPortalPos2;
                boss.GetComponent<StartHighSchoolBoss>().shadowDogPortalPos3 = shadowDogPortalPos3;

                boss.GetComponent<StartHighSchoolBoss>().shadowDogPortalPrefab = shadowDogPortalPrefab;
                boss.GetComponent<StartHighSchoolBoss>().corridorFloor = corridorFloor;
                boss.GetComponent<StartHighSchoolBoss>().scenarioFloor = scenarioFloor;

                boss.GetComponent<StartHighSchoolBoss>().lightTarget1 = lightTarget1;
                boss.GetComponent<StartHighSchoolBoss>().lightTarget2 = lightTarget2;

                boss.GetComponent<StartHighSchoolBoss>().foco1 = foco1;
                boss.GetComponent<StartHighSchoolBoss>().foco2 = foco2;
                boss.GetComponent<StartHighSchoolBoss>().monolithWeakPoints = monolithWeakPoints;
                boss.GetComponent<StartHighSchoolBoss>().projectileBossSource = projectileBossSource;


                break;
            case 2:

                if (player.GetComponent<PlayerHp>().lifeTime < 15.0f)
                {
                    player.transform.position = playerSpawnPositions[nBoss].position;
                    boss.transform.position = bossSpawnPositions[nBoss].position;
                    boss.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                }

                break;
                default: break;
        }
    }

    public void CheckBossHp(int nBoss)
    {
        switch (nBoss)
        {
            case 0:

                switch (boss.GetComponent<TutorialBoss>().phase)
                {
                    case 0:
                        if (boss.GetComponent<EnemyHP>().hp < 6)
                        {
                            boss.GetComponent<EnemyHP>().canBeDamaged = false;
                            transfromTimer += Time.deltaTime;
                            if(transfromTimer > 3.0f)
                            {
                                boss.GetComponent<TutorialBoss>().phase++;
                                boss.GetComponent<TutorialBoss>().canAttack = false;
                                boss.GetComponent<TutorialBoss>().canMove = true;
                                transfromTimer = 0.0f;
                                boss.GetComponent<NavMeshAgent>().enabled = true;

                            }
                            else
                            {
                                boss.transform.localScale += Vector3.one * Time.deltaTime / 2;
                            }
                            
                        }
                        break;
                    case 1:
                        if (boss.GetComponent<EnemyHP>().hp < 3)
                        {
                            boss.GetComponent<TutorialBoss>().weakPoint.SetActive(false);
                            transfromTimer += Time.deltaTime;
                            if (transfromTimer > 3.0f)
                            {
                                transfromTimer = 0.0f;
                                boss.GetComponent<TutorialBoss>().phase++;
                                boss.GetComponent<TutorialBoss>().proximityArea.SetActive(false);
                                boss.GetComponent<NavMeshAgent>().enabled = false;
                                boss.GetComponent<TutorialBoss>().movementState = TutorialBoss.MovementState.startSpinning;
                                boss.GetComponent<TutorialBoss>().Ground = Ground;
                                boss.GetComponent<TutorialBoss>().Sword1.SetActive(true);
                                boss.GetComponent<TutorialBoss>().Sword2.SetActive(true);

                            }
                            else
                            {
                                if (boss.transform.localScale.x > 1.8f) boss.transform.localScale -= Vector3.one * Time.deltaTime / 4;
                                else boss.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                            }
                        }
                        break;
                    case 2:
                        if (boss.GetComponent<EnemyHP>().hp <= 0)
                        {
                            NextBoss();
                            Destroy(boss.GetComponent<TutorialBoss>());
                        }
                        break;
                }

                break;
            case 1:
                if (boss.GetComponent<HighSchoolBoss>() != null)
                {
                    switch (boss.GetComponent<HighSchoolBoss>().phase)
                    {
                        case 0:
                            if (boss.GetComponent<EnemyHP>().hp < 85)
                            {
                                boss.GetComponent<EnemyHP>().canBeDamaged = false;
                                boss.GetComponent<HighSchoolBoss>().phase++;
                            }
                            break;
                        case 1:
                            if (boss.GetComponent<EnemyHP>().hp < 70)
                            {
                                boss.GetComponent<EnemyHP>().canBeDamaged = true;
                                boss.GetComponent<HighSchoolBoss>().phase++;
                            }
                            break;
                        case 2:
                            if(boss.GetComponent<HighSchoolBoss>().dead == true)
                            {
                                NextBoss();
                                Destroy(boss.GetComponent<HighSchoolBoss>());
                            }
                            break;
                    }
                    break;
                }
                break;
        }

    }

    public void NextBoss()
    {
        currentBoss++;
        DataToStore data = new DataToStore();
        if (currentBoss >= LoadPlayerData(Settings.archiveNum).maxLevel) data.maxLevel = currentBoss;
        else data.maxLevel = LoadPlayerData(Settings.archiveNum).maxLevel;
        if (currentBoss >= LoadPlayerData(Settings.archiveNum).maxLevel) data.maxHp = player.GetComponent<PlayerHp>().playerHp;
        else data.maxHp = LoadPlayerData(Settings.archiveNum).maxHp;
        if (currentBoss >= LoadPlayerData(Settings.archiveNum).maxLevel) data.charge = player.GetComponent<PassiveAbility>().passiveCharge;
        else data.charge = LoadPlayerData(Settings.archiveNum).charge;
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
        ActivateBoss(currentBoss);
    }
    public void SavePlayerData(DataToStore data, int num)
    {
        string json = JsonUtility.ToJson(data);
        string archiveNum = "Archive" + num.ToString(); ;
        File.WriteAllText(Application.streamingAssetsPath + "/" + archiveNum + ".json", json);
        Debug.Log("Archive " + num.ToString() + " Created");
        //Comen�a Partida
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
}

