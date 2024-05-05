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
using UnityEngine.UI;
using Random = UnityEngine.Random;
 
public class BossManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Positions")]
    public Transform[] playerSpawnPositions;
    public Transform[] bossSpawnPositions;
    public GameObject floor;
    public GameObject[] lights; 

    [Header("Animations")]

    public TutorialBossAnimations tutorialAnimations;
    public GameObject tutorialBossModel;
    public HighSchoolBossAnimations highSchoolBossAnimations;
    public GameObject highSchoolBossModel;
    public DadBossAnimations dadBossAnimations;
    public GameObject dadBossModel;
    public MomBossAnimations momBossAnimations;
    public GameObject momBossModel;
    public BrotherBossAnimations brotherBossAnimations;
    public GameObject brotherBossModel;

    [Header("Audio")]
    public AudioSource ambienceAudioSource;
    public AudioClip[] ambienceAudios;

    public AudioSource bossAudioSource;
    public AudioSource momAudioSource;
    public AudioSource bossDialogAudioSource;

    public AudioClip[] tutorialBossAudios;
    public AudioClip[] tutorialBossDialogAudios;
    public string[] tutorialBossDialogs;

    public AudioClip[] highSchoolBossAudios;
    public AudioClip[] highSchoolBossDialogAudios;
    public string[] highSchoolBossDialogs;
    
    public AudioClip[] parentsBossAudios;
    public AudioClip[] parentsBossDialogAudios;
    public string[] parentsBossDialogs;

    public SubtitleManager subtitleManagaer;

    public AudioClip[] audioTransitions;

    public GameObject tutorialMessagesManager;
    [Header("TutorialBoss")]
    public int currentBoss;

    public int phase = 0;

    public int bossAttack;

    public int bossMovement;

    public bool canMove = true;
    public bool canAttack = true;

    public GameObject player;
    public GameObject boss;

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
    public GameObject TutorialMessages;

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

    [Header("ParentsBoss")]

    public GameObject firstEnvironment;
    public Transform previousEnvironmentPosition;
    public GameObject parentsHouse;
    public Transform dadPosition;
    public Transform momPosition;

    public GameObject houseDoor1;
    public GameObject houseDoor2;
    public Transform houseDoorPosition1;
    public Transform houseDoorPosition2;

    public GameObject isPlayerAtHome;
    public NavMeshSurface houseFloor;
    public Transform houseCenter;

    public Transform mainDadProjectileSource;
    public Transform[] bulletHell1Directions;
    public Transform bulletHell2Direction;
    public Material returnableProjectileMaterial;

    public Transform[] bulletHell3Directions;

    public GameObject shotgun;
    public Transform[] rightShotgunPositions;
    public Transform[] leftShotgunPositions;

    public GameObject circularArea;
    public GameObject coneArea;
    public GameObject spikePrefab;
    public GameObject spinner;
    public Transform spinnerArea;
    public GameObject greatAttackArea;

    public GameObject greatAttackArea1;
    public GameObject greatAttackArea2;

    public GameObject auxiliarLight;

    [Header("BrotherBoss")]

    public GameObject auxiliarLight2;

    public GameObject ageCorridor;
    public Transform[] bossTrail;
    public GameObject streetFloor;
    public GameObject street;
    public GameObject car;
    public Transform carDriversPosition;
    public Transform[] carTrail;

    public Transform clone1Position;
    public Transform clone2Position;

    public GameObject clone;
    public GameObject cloneWall;

    public Transform[] randomMapPositions;

    public Transform aerialPosition;
    public Transform[] discMovementArea;
    public GameObject disc;
    public GameObject drone;
    public GameObject head;
    public Transform headPosition;
    public GameObject mainRoadBlock;
    public Transform endOfTheStreet;
    public GameObject brotherModel;

    public GameObject cityBarrier1;
    public GameObject cityBarrier2;

    public GameObject secondEnvironment;
    public int difficulty;
    public float dialogTimer = 0.0f;
    private int dialogNum;

    void Start()
    {
        currentBoss = Settings.actualBoss;
        //Debug.Log(currentBoss);
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
                lights[nBoss].SetActive(true);
                boss = GameObject.Find("Boss");
                tutorialAnimations.enabled = true;
                tutorialBossModel.SetActive(true);
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
                boss.GetComponent<TutorialBoss>().turoialAnimations = tutorialAnimations;
                boss.GetComponent<TutorialBoss>().floor = floor;
                boss.GetComponent<TutorialBoss>().subtitleManagaer = subtitleManagaer;
                boss.GetComponent<TutorialBoss>().bossAudioSource = bossAudioSource;
                boss.GetComponent<TutorialBoss>().bossDialogAudioSource = bossDialogAudioSource;
                boss.GetComponent<TutorialBoss>().tutorialBossAudios = tutorialBossAudios;
                boss.GetComponent<TutorialBoss>().tutorialBossDialogs = tutorialBossDialogs;
                boss.GetComponent<TutorialBoss>().tutorialBossDialogAudios = tutorialBossDialogAudios;
                boss.GetComponent<TutorialBoss>().difficulty = LoadPlayerData(Settings.archiveNum).difficulty;

                tutorialAnimations.enabled = true;
                break;

            case 1:
                Destroy(tutorialAnimations);
                Destroy(tutorialBossModel);
                highSchoolBossAnimations.enabled = true;
                highSchoolBossAnimations.animation = highSchoolBossAnimations.model.GetComponent<Animator>();
                highSchoolBossAnimations.animation.Play(highSchoolBossAnimations.animations[1].name);
                highSchoolBossAnimations.actualAnimation = highSchoolBossAnimations.animations[1];
                highSchoolBossModel.SetActive(true);
                lights[nBoss].SetActive(true);
                if (player.GetComponent<PlayerHp>().lifeTime < 15.0f)
                {
                    player.SetActive(false);
                    boss.SetActive(false);
                    Destroy(TutorialMessages);
                    player.transform.position = playerSpawnPositions[nBoss].position;
                    boss.transform.position = bossSpawnPositions[nBoss].position;
                    boss.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                    player.SetActive(true);
                    boss.SetActive(true);

                }
                Destroy(lights[0]);
                Destroy(Sword1);
                Destroy(Sword2);
                Destroy(weakPoint);
                if (tutorialMessagesManager != null) Destroy(tutorialMessagesManager);
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

                boss.GetComponent<StartHighSchoolBoss>().subtitleManagaer = subtitleManagaer;
                boss.GetComponent<StartHighSchoolBoss>().bossAudioSource = bossAudioSource;
                boss.GetComponent<StartHighSchoolBoss>().bossDialogAudioSource = bossDialogAudioSource;
                boss.GetComponent<StartHighSchoolBoss>().highSchoolBossAudios = highSchoolBossAudios;
                boss.GetComponent<StartHighSchoolBoss>().highSchoolBossDialogAudios = highSchoolBossDialogAudios;
                boss.GetComponent<StartHighSchoolBoss>().highSchoolBossDialogs = highSchoolBossDialogs;

                boss.GetComponent<StartHighSchoolBoss>().ambienceAudioSource = ambienceAudioSource;
                boss.GetComponent<StartHighSchoolBoss>().ambienceAudios = ambienceAudios;
                boss.GetComponent<StartHighSchoolBoss>().difficulty = LoadPlayerData(Settings.archiveNum).difficulty;

                break;
            case 2:
                ambienceAudioSource.clip = ambienceAudios[2];
                ambienceAudioSource.Play();
                lights[nBoss].SetActive(true);
                auxiliarLight.SetActive(true);
                if (player.GetComponent<PlayerHp>().lifeTime < 15.0f)
                {
                    player.transform.position = playerSpawnPositions[nBoss].position;
                    boss.transform.position = bossSpawnPositions[nBoss].position;
                    boss.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                    //Debug.Log("Player: " + player.transform.position + "Position: " + playerSpawnPositions[nBoss].position);
                }
                Destroy(highSchoolBossAnimations);
                Destroy(tutorialAnimations);
                Destroy(tutorialBossModel);
                //Destroy(highSchoolBossModel);
                highSchoolBossModel.SetActive(true); 
                dadBossAnimations.enabled = true;
                dadBossAnimations.animation = dadBossAnimations.model.GetComponent<Animator>();
                dadBossAnimations.animation.Play(dadBossAnimations.animations[0].name);
                dadBossAnimations.actualAnimation = dadBossAnimations.animations[0];
                
                momBossAnimations.enabled = true;
                momBossAnimations.animation = momBossAnimations.model.GetComponent<Animator>();
                //momBossAnimations.animation.Play(momBossAnimations.animations[0].name);
                //momBossAnimations.actualAnimation = dadBossAnimations.animations[0];
                boss.AddComponent<StartParentsBoss>();
                boss.GetComponent<StartParentsBoss>().previousEnvironment = firstEnvironment;
                boss.GetComponent<StartParentsBoss>().previousEnvironmentPosition = previousEnvironmentPosition;
                boss.GetComponent<StartParentsBoss>().player = player;
                boss.GetComponent<StartParentsBoss>().playerSpawnPosition = playerSpawnPositions[2];
                boss.GetComponent<StartParentsBoss>().bossSpawnPosition = bossSpawnPositions[2];
                boss.GetComponent<StartParentsBoss>().parentsHouse = parentsHouse;
                boss.GetComponent<StartParentsBoss>().dadPosition = dadPosition;
                boss.GetComponent<StartParentsBoss>().momPosition = momPosition;
                boss.GetComponent<StartParentsBoss>().houseDoor1 = houseDoor1;
                boss.GetComponent<StartParentsBoss>().houseDoor2 = houseDoor2;
                boss.GetComponent<StartParentsBoss>().houseDoorPosition1 = houseDoorPosition1;
                boss.GetComponent<StartParentsBoss>().houseDoorPosition2 = houseDoorPosition2;
                boss.GetComponent<StartParentsBoss>().isPlayerAtHome = isPlayerAtHome;
                boss.GetComponent<StartParentsBoss>().momBoss = momBossModel;
                boss.GetComponent<StartParentsBoss>().floor = houseFloor;
                boss.GetComponent<StartParentsBoss>().center = houseCenter;
                boss.GetComponent<StartParentsBoss>().mainDadProjectileSource = mainDadProjectileSource;
                boss.GetComponent<StartParentsBoss>().bulletHell1Directions = bulletHell1Directions;
                boss.GetComponent<StartParentsBoss>().returnableProjectileMaterial = returnableProjectileMaterial;
                boss.GetComponent<StartParentsBoss>().projectilePrefab = projectilePrefab;
                boss.GetComponent<StartParentsBoss>().bulletHell2Direction = bulletHell2Direction;
                boss.GetComponent<StartParentsBoss>().bulletHell3Directions = bulletHell3Directions;
                boss.GetComponent<StartParentsBoss>().shotgun = shotgun;
                boss.GetComponent<StartParentsBoss>().rightShotgunPositions = rightShotgunPositions;
                boss.GetComponent<StartParentsBoss>().leftShotgunPositions = leftShotgunPositions;
                boss.GetComponent<StartParentsBoss>().circularArea = circularArea;
                boss.GetComponent<StartParentsBoss>().coneArea = coneArea;
                boss.GetComponent<StartParentsBoss>().spikePrefab = spikePrefab;
                boss.GetComponent<StartParentsBoss>().spinner = spinner;
                boss.GetComponent<StartParentsBoss>().spinnerArea = spinnerArea;
                boss.GetComponent<StartParentsBoss>().greatAttackArea = greatAttackArea;
                boss.GetComponent<StartParentsBoss>().greatAttackArea1 = greatAttackArea1;
                boss.GetComponent<StartParentsBoss>().greatAttackArea2 = greatAttackArea2;
                boss.GetComponent<StartParentsBoss>().highSchoolBossModel = highSchoolBossModel;
                boss.GetComponent<StartParentsBoss>().dadBossModel = dadBossModel;
                boss.GetComponent<StartParentsBoss>().bossAudioSource = bossAudioSource;
                boss.GetComponent<StartParentsBoss>().momAudioSource = momAudioSource;
                boss.GetComponent<StartParentsBoss>().parentsBossAudios = parentsBossAudios;
                boss.GetComponent<StartParentsBoss>().difficulty = LoadPlayerData(Settings.archiveNum).difficulty;

                break;

            case 3:

                if (player.GetComponent<PlayerHp>().lifeTime < 15.0f)
                {
                    parentsHouse.transform.position = new Vector3(38.2999992f, -49.5260925f, -59.7000008f);
                    parentsHouse.SetActive(true);
                    Destroy(firstEnvironment);
                    player.SetActive(false);
                    boss.SetActive(false);
                    player.transform.position = playerSpawnPositions[nBoss].position;
                    player.SetActive(true);
                    boss.transform.position = bossSpawnPositions[nBoss].position;
                    boss.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                    boss.SetActive(true);


                    //Debug.Log("Player: " + player.transform.position + "Position: " + playerSpawnPositions[nBoss].position);
                }

                lights[nBoss].SetActive(true);
                auxiliarLight2.SetActive(true);
                ageCorridor.SetActive(true);
                Destroy(scenarioFloor);
                if(boss.name == "Boss")
                {
                    if (tutorialAnimations != null) Destroy(tutorialAnimations);
                    if (highSchoolBossAnimations != null) Destroy(highSchoolBossAnimations);
                    if (dadBossAnimations != null) Destroy(dadBossAnimations);
                    if (tutorialBossModel != null) Destroy(tutorialBossModel);
                    if (highSchoolBossModel != null) Destroy(highSchoolBossModel);
                    if (dadBossModel != null) Destroy(dadBossModel);
                    if (dadBossAnimations != null) Destroy(dadBossAnimations);
                    if (momBossModel != null) Destroy(momBossModel);
                }
                else if(boss.name == "Mom")
                {
                    if (boss.GetComponent<MomBossAnimations>() != null) Destroy(boss.GetComponent<MomBossAnimations>().model);
                    if (momBossAnimations != null) Destroy(momBossAnimations);
                    if (greatAttackArea != null) Destroy(greatAttackArea);
                    if (circularArea != null) Destroy(circularArea);
                    if (coneArea != null) Destroy(coneArea);
                    boss.name = "Boss";
                }
                
                brotherBossModel.SetActive(true);
                brotherBossAnimations.enabled = true;
                brotherBossAnimations.animation = brotherBossAnimations.model.GetComponent<Animator>();
                brotherBossAnimations.animation.Play(brotherBossAnimations.animations[0].name);
                brotherBossAnimations.actualAnimation = brotherBossAnimations.animations[0];

                boss.AddComponent<StartBrotherBoss>();
                boss.GetComponent<StartBrotherBoss>().player = player;
                boss.GetComponent<StartBrotherBoss>().houseDoor1 = houseDoor1;
                boss.GetComponent<StartBrotherBoss>().houseDoor2 = houseDoor2;
                boss.GetComponent<StartBrotherBoss>().houseDoorPosition1 = houseDoorPosition1;
                boss.GetComponent<StartBrotherBoss>().houseDoorPosition2 = houseDoorPosition2;
                boss.GetComponent<StartBrotherBoss>().bossTrail = bossTrail;
                boss.GetComponent<StartBrotherBoss>().isPlayerOnStreet = streetFloor.GetComponent<IsPlayerOnStreet>();
                boss.GetComponent<StartBrotherBoss>().streetsFloor = streetFloor.GetComponent<NavMeshSurface>();
                boss.GetComponent<StartBrotherBoss>().streets = street;
                boss.GetComponent<StartBrotherBoss>().ageCorridor = ageCorridor;
                boss.GetComponent<StartBrotherBoss>().car = car;
                boss.GetComponent<StartBrotherBoss>().carDriversPosition = carDriversPosition;
                boss.GetComponent<StartBrotherBoss>().carTrail = carTrail;

                boss.GetComponent<StartBrotherBoss>().clone = clone;
                boss.GetComponent<StartBrotherBoss>().cloneWall = cloneWall;
                boss.GetComponent<StartBrotherBoss>().clone1Position = clone1Position;

                boss.GetComponent<StartBrotherBoss>().clone2Position = clone2Position;
                boss.GetComponent<StartBrotherBoss>().proximityAreaAttack = proximityArea;
                boss.GetComponent<StartBrotherBoss>().brotherBossModel = brotherBossModel;
                boss.GetComponent<StartBrotherBoss>().randomMapPositions = randomMapPositions;
                
                boss.GetComponent<StartBrotherBoss>().aerialPosition = aerialPosition;
                boss.GetComponent<StartBrotherBoss>().disc = disc;
                boss.GetComponent<StartBrotherBoss>().discMovementArea = discMovementArea;
                boss.GetComponent<StartBrotherBoss>().drone = drone;
                boss.GetComponent<StartBrotherBoss>().continousCircle = circlesPrefabs[0];
                boss.GetComponent<StartBrotherBoss>().head = head;
                boss.GetComponent<StartBrotherBoss>().headPosition = headPosition;
                boss.GetComponent<StartBrotherBoss>().mainRoadBlock = mainRoadBlock;
                boss.GetComponent<StartBrotherBoss>().endOfTheStreet = endOfTheStreet;
                boss.GetComponent<StartBrotherBoss>().secondEnvironment = secondEnvironment;
                boss.GetComponent<StartBrotherBoss>().brotherModel = brotherModel;
                boss.GetComponent<StartBrotherBoss>().cityBarrier1 = cityBarrier1;
                boss.GetComponent<StartBrotherBoss>().cityBarrier2 = cityBarrier2;



                brotherBossAnimations.startBoss = boss.GetComponent<StartBrotherBoss>();

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
                                boss.GetComponent<TutorialBoss>().attackType = TutorialBoss.AttackType.proximity;
                                boss.GetComponent<TutorialBoss>().canMove = true;
                                transfromTimer = 0.0f;
                                boss.GetComponent<NavMeshAgent>().enabled = true;

                            }
                            else
                            {
                                boss.transform.localScale += Vector3.one * Time.deltaTime / 2;
                                boss.GetComponent<TutorialBossAnimations>().hand1.transform.localScale += Vector3.one * Time.deltaTime / 2;
                                boss.GetComponent<TutorialBossAnimations>().hand1.transform.position = boss.GetComponent<TutorialBossAnimations>().handPosition1.transform.position;
                                boss.GetComponent<TutorialBossAnimations>().hand2.transform.localScale -= Vector3.one * Time.deltaTime / 2;
                                boss.GetComponent<TutorialBossAnimations>().hand2.transform.position = boss.GetComponent<TutorialBossAnimations>().handPosition2.transform.position;


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
                                if (boss.transform.localScale.x > 1.8f)
                                {
                                    boss.transform.localScale -= Vector3.one * Time.deltaTime / 4;
                                    tutorialAnimations.sword1.transform.localScale += Vector3.one * Time.deltaTime * 2;
                                    tutorialAnimations.sword1.SetActive(true);
                                    tutorialAnimations.sword2.transform.localScale += Vector3.one * Time.deltaTime * 2;
                                    tutorialAnimations.sword2.SetActive(true);
                                    if (tutorialAnimations.hand1.transform.localScale.x > 0)
                                    {
                                        tutorialAnimations.hand1.transform.localScale -= Vector3.one * Time.deltaTime * 6;
                                        tutorialAnimations.hand2.transform.localScale += Vector3.one * Time.deltaTime * 6;
                                    }

                                }
                                else
                                {
                                    boss.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                                }
                            }
                        }
                        break;
                    case 2:
                        if (boss.GetComponent<EnemyHP>().hp <= 0)
                        {
                            NextBoss();
                            bossAudioSource.Stop();
                            bossAudioSource.clip = tutorialBossDialogAudios[tutorialBossDialogAudios.Length-1];
                            subtitleManagaer.subtitleText = tutorialBossDialogs[tutorialBossDialogAudios.Length-1];
                            subtitleManagaer.currentAudioClip = tutorialBossDialogAudios[tutorialBossDialogAudios.Length-1];
                            subtitleManagaer.canReproduceAudio = true;
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
                            if (boss.GetComponent<EnemyHP>().hp < 90)
                            {
                                boss.GetComponent<EnemyHP>().canBeDamaged = false;
                                boss.GetComponent<HighSchoolBoss>().phase++;
                            }
                            break;
                        case 1:
                            if (boss.GetComponent<EnemyHP>().hp < 80)
                            {
                                boss.GetComponent<EnemyHP>().canBeDamaged = true;
                                boss.GetComponent<HighSchoolBoss>().phase++;
                            }
                            break;
                        case 2:
                            if(boss.GetComponent<HighSchoolBoss>().dead == true)
                            {
                                NextBoss();
                                bossAudioSource.Stop();
                                bossAudioSource.clip = highSchoolBossDialogAudios[highSchoolBossDialogAudios.Length - 1];
                                subtitleManagaer.subtitleText = highSchoolBossDialogs[highSchoolBossDialogAudios.Length - 1];
                                subtitleManagaer.currentAudioClip = highSchoolBossDialogAudios[highSchoolBossDialogAudios.Length - 1];
                                subtitleManagaer.canReproduceAudio = true;
                                Destroy(boss.GetComponent<HighSchoolBoss>());
                            }
                            break;
                    }
                    break;
                }
                break;

            case 2:

                dialogTimer += Time.deltaTime;

                dialogTimer = Dialogs();

                if (momBossModel.GetComponent<EnemyHP>().hp <= 0 && boss.GetComponent<EnemyHP>().hp <= 0)
                {
                    if (momBossModel.activeInHierarchy == true)
                    {
                        Destroy(momBossModel.GetComponent<MomBoss>());
                        Destroy(boss);
                        boss = momBossModel;
                        brotherBossModel.transform.localPosition = Vector3.zero;
                        brotherBossModel.transform.SetParent(boss.transform);
                        brotherBossModel.transform.localPosition = Vector3.zero;
                        brotherBossModel.transform.Rotate(-brotherBossModel.transform.localEulerAngles.x, 0, -brotherBossModel.transform.localEulerAngles.z);
                        proximityArea.transform.SetParent(boss.transform);
                    }
                    else {

                        Destroy(boss.GetComponent<DadBoss>());
                        Destroy(momBossModel); 

                    }
                    NextBoss();
                    break;
                }

                if (momBossModel.GetComponent<EnemyHP>().hp <= 0 && boss.GetComponent<DadBoss>().phase == 0 && boss.activeInHierarchy == true)
                {
                    momBossModel.SetActive(false);
                    momBossModel.tag = "Untagged";
                    subtitleManagaer.subtitleText = parentsBossDialogs[9];
                    subtitleManagaer.currentAudioClip = parentsBossDialogAudios[9];
                    subtitleManagaer.canReproduceAudio = true;
                    dialogTimer = 0;
                    momAudioSource.Stop();
                    boss.GetComponent<DadBoss>().phase++;
                    boss.GetComponent<DadBoss>().shotgunTimer = 0.0f;
                    boss.GetComponent<DadBoss>().molotovTimer = 0.0f;
                    boss.GetComponent<DadBoss>().attackType = DadBoss.AttackType.spinners;
                    boss.GetComponent<DadBoss>().maxNumAttacks = 7;
                    boss.GetComponent<DadBoss>().attackCooldownTime = 4.0f;
                    boss.GetComponent<DadBoss>().delayTime = 1.20f;
                    mainDadProjectileSource.gameObject.SetActive(false);
                }
                if(boss.GetComponent<EnemyHP>().hp <= 0 && momBossModel.GetComponent<MomBoss>().phase == 0 && momBossModel.activeInHierarchy == true)
                {

                    bossAudioSource.transform.SetParent(momBossModel.transform);
                    bossDialogAudioSource.transform.SetParent(momBossModel.transform);
                    boss.SetActive(false);
                    boss.tag = "Untagged";
                    subtitleManagaer.subtitleText = parentsBossDialogs[8];
                    subtitleManagaer.currentAudioClip = parentsBossDialogAudios[8];
                    subtitleManagaer.canReproduceAudio = true;
                    dialogTimer = 0;
                    bossAudioSource.Stop();

                    momBossModel.GetComponent<MomBoss>().phase++;
                    momBossModel.GetComponent<MomBoss>().delayTime = 2.15f;
                    momBossModel.GetComponent<MomBoss>().maxNumAttacks = 5;
                    momBossModel.GetComponent<MomBoss>().movSpeed = 5.5f;
                }

                break;

            case 3:

                if(boss.GetComponent<BrotherBoss>() != null)
                {
                    switch (boss.GetComponent<BrotherBoss>().phase)
                    {
                        case 0:

                            if(boss.GetComponent<EnemyHP>().hp < 30)
                            {
                                boss.GetComponent<BrotherBoss>().phase++;
                                boss.GetComponent<BrotherBoss>().canAttack = false;
                                boss.GetComponent<BrotherBoss>().canMove = true;
                                boss.GetComponent<BrotherBoss>().cooldownTimer = 0.0f;
                                boss.GetComponent<BrotherBoss>().attackSelected = false;
                                boss.GetComponent<BrotherBoss>().mState = BrotherBoss.MovementState.aerial;
                                boss.GetComponent<CapsuleCollider>().isTrigger = false;
                                boss.GetComponent<NavMeshAgent>().enabled = false;
                            }

                            break;
                        case 1:

                            if (boss.GetComponent<EnemyHP>().hp < 10)
                            {
                                boss.GetComponent<BrotherBoss>().phase++;
                                boss.GetComponent<BrotherBoss>().canAttack = false;
                                boss.GetComponent<BrotherBoss>().canMove = true;
                                head.SetActive(true);
                                mainRoadBlock.SetActive(false);
                                boss.GetComponent<NavMeshAgent>().enabled = false;
                                boss.GetComponent<CapsuleCollider>().enabled = false;
                            }

                            break;

                        default:break;
                    }
                }

                break;
            default:break;
        }

    }

    private float Dialogs()
    {
        if (dialogTimer > 30.0f)
        {
            subtitleManagaer.subtitleText = parentsBossDialogs[dialogNum];
            subtitleManagaer.currentAudioClip = parentsBossDialogAudios[dialogNum];
            subtitleManagaer.canReproduceAudio = true;
            dialogTimer = 0;
            return dialogTimer;
        }
        else
        {

            dialogNum = Random.Range(0, parentsBossDialogAudios.Length - 3);
            return dialogTimer;
        }
    }

    public void NextBoss()
    {
        bossAudioSource.loop = false;
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
        //Debug.Log("Archive " + num.ToString() + " Created");
        //Comença Partida
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
            //Debug.LogWarning("No se encontraron datos de jugador guardados.");
            return null;
        }
    }
}

