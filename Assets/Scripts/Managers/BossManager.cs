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
    public NavMeshSurface floor;

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
        //Comença Partida
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

