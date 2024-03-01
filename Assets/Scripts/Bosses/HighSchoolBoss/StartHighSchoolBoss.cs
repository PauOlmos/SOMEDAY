using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class StartHighSchoolBoss : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject TutorialWalls;

    public float destroyWallsTimer = 0.0f;
    public bool destroyedWalls = false;
    public bool classRoomCreated = false;
    public GameObject teacherTable;

    public GameObject[] tables;
    public GameObject allTables;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;
    public NavMeshAgent agent;
    public GameObject proximityArea;
    public GameObject floor;

    public GameObject armari1;
    public GameObject armari2;

    public GameObject armariResetPos1;
    public GameObject armariResetPos2;
    public GameObject armariPos1;
    public GameObject armariPos2;
    public GameObject[] tableAttackPositions;
    public GameObject[] tableRestPositions;
    public bool highSchoolCreationCompleted = false;


    public GameObject bossShield;
    public GameObject portalSpawnArea;

    public GameObject portalPrefab;


    public GameObject corridorPos;
    public GameObject door1;
    public GameObject door2;


    public GameObject doorPos1;
    public GameObject doorPos2;
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

    public GameObject[] monolithWeakPoints;

    void Start()
    {
        TutorialWalls.isStatic = false;
        tables = GameObject.FindGameObjectsWithTag("Table");
        wall1.SetActive(true);
        wall2.SetActive(true);
        wall3.SetActive(true);
        wall4.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if(destroyedWalls == false) destroyedWalls = DestroyWalls();
        else
        {
            classRoomCreated = CreateClassRoom();
        }
        if (classRoomCreated == true)
        {

            if (allTables.transform.position.y < 0) allTables.transform.Translate(0.0f, Time.deltaTime * 4, 0.0f);
            else
            {
                classRoomCreated = false;
                allTables.transform.localPosition = new Vector3(allTables.transform.position.x, 0.0f, allTables.transform.position.z);
                highSchoolCreationCompleted = true;
            }
        }

        if(highSchoolCreationCompleted == true)
        {
            gameObject.AddComponent<HighSchoolBoss>();
            gameObject.GetComponent<HighSchoolBoss>().player = player;
            gameObject.GetComponent<HighSchoolBoss>().agent = agent;
            gameObject.GetComponent<HighSchoolBoss>().proximityArea = proximityArea;
            gameObject.GetComponent<HighSchoolBoss>().floor = floor;

            gameObject.GetComponent<HighSchoolBoss>().tables = tables;
            gameObject.GetComponent<HighSchoolBoss>().teacherTable = teacherTable;
            gameObject.GetComponent<HighSchoolBoss>().tableAttackPositions = tableAttackPositions;
            gameObject.GetComponent<HighSchoolBoss>().tableRestPositions = tableRestPositions;

            gameObject.GetComponent<HighSchoolBoss>().armari1 = armari1;
            gameObject.GetComponent<HighSchoolBoss>().armari2 = armari2;
            gameObject.GetComponent<HighSchoolBoss>().armariPos1 = armariPos1;
            gameObject.GetComponent<HighSchoolBoss>().armariPos2 = armariPos2;
            gameObject.GetComponent<HighSchoolBoss>().armariResetPos1 = armariResetPos1;
            gameObject.GetComponent<HighSchoolBoss>().armariResetPos2 = armariResetPos2;

            gameObject.GetComponent<HighSchoolBoss>().portalSpawnArea = portalSpawnArea;
            gameObject.GetComponent<HighSchoolBoss>().portalPrefab = portalPrefab;
            gameObject.GetComponent<HighSchoolBoss>().bossShield = bossShield;


            gameObject.GetComponent<HighSchoolBoss>().corridorPos = corridorPos;
            gameObject.GetComponent<HighSchoolBoss>().door1 = door1;
            gameObject.GetComponent<HighSchoolBoss>().door2 = door2;
            gameObject.GetComponent<HighSchoolBoss>().wall4 = wall4;

            gameObject.GetComponent<HighSchoolBoss>().doorPos1 = doorPos1;
            gameObject.GetComponent<HighSchoolBoss>().doorPos2 = doorPos2;
            gameObject.GetComponent<HighSchoolBoss>().projectileSource = projectileSource;
            gameObject.GetComponent<HighSchoolBoss>().projectilePrefab = projectilePrefab;


            gameObject.GetComponent<HighSchoolBoss>().hand1 = hand1;
            gameObject.GetComponent<HighSchoolBoss>().hand2 = hand2;
            gameObject.GetComponent<HighSchoolBoss>().hand3 = hand3;
            gameObject.GetComponent<HighSchoolBoss>().handPos1 = handPos1;
            gameObject.GetComponent<HighSchoolBoss>().handPos2 = handPos2;
            gameObject.GetComponent<HighSchoolBoss>().handPos3 = handPos3;


            gameObject.GetComponent<HighSchoolBoss>().handDamage1 = handDamage1;
            gameObject.GetComponent<HighSchoolBoss>().handDamage2 = handDamage2;
            gameObject.GetComponent<HighSchoolBoss>().handDamage3 = handDamage3;

            gameObject.GetComponent<HighSchoolBoss>().weakPoint1 = weakPoint1;
            gameObject.GetComponent<HighSchoolBoss>().weakPoint2 = weakPoint2;
            gameObject.GetComponent<HighSchoolBoss>().weakPoint3 = weakPoint3;

            gameObject.GetComponent<HighSchoolBoss>().shadowDogPortalPos1 = shadowDogPortalPos1;
            gameObject.GetComponent<HighSchoolBoss>().shadowDogPortalPos2 = shadowDogPortalPos2;
            gameObject.GetComponent<HighSchoolBoss>().shadowDogPortalPos3 = shadowDogPortalPos3;

            gameObject.GetComponent<HighSchoolBoss>().shadowDogPortalPrefab = shadowDogPortalPrefab;
            gameObject.GetComponent<HighSchoolBoss>().corridorFloor = corridorFloor;
            gameObject.GetComponent<HighSchoolBoss>().scenarioFloor = scenarioFloor;

            gameObject.GetComponent<HighSchoolBoss>().lightTarget1 = lightTarget1;
            gameObject.GetComponent<HighSchoolBoss>().lightTarget2 = lightTarget2;

            gameObject.GetComponent<HighSchoolBoss>().foco1 = foco1;
            gameObject.GetComponent<HighSchoolBoss>().foco2 = foco2;
            gameObject.GetComponent<HighSchoolBoss>().monolithWeakPoints = monolithWeakPoints;

            Destroy(gameObject.GetComponent<StartHighSchoolBoss>());
        }
    }

    public bool CreateClassRoom()
    {
        int wallsInPosition = 0;
        if (wall1.transform.position.z > 21.3f) wall1.transform.Translate(Time.deltaTime * 4,0.0f,0.0f);
        else
        {
            wallsInPosition++;
            wall1.isStatic = true;

            wall1.transform.localPosition = new Vector3(wall1.transform.position.x, wall1.transform.position.y, 21.3f);
        }
        if (wall2.transform.position.x > 12.0f)
        {
            wall2.transform.Translate(-Time.deltaTime * 4, 0.0f, 0.0f);
            armari1.transform.Translate(-Time.deltaTime * 4, 0.0f, 0.0f);

            hand1.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
            hand2.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
            hand3.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
            handPos1.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
            handPos2.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
            handPos3.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
            weakPoint1.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
            weakPoint2.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
            weakPoint3.transform.Translate(-Time.deltaTime * 4.0f, 0, 0);
        }
        else
        {
            armari1.transform.localPosition = new Vector3(11.120f, armari1.transform.position.y, armari1.transform.position.z);
            wallsInPosition++;
            wall2.transform.localPosition = new Vector3(12.0f, wall2.transform.position.y, wall2.transform.position.z);
            wall2.isStatic = true;
        }
        if (wall3.transform.position.z < -21.3f) wall3.transform.Translate(-Time.deltaTime * 4,0.0f, 0.0f);
        else
        {
            wallsInPosition++;
            wall3.isStatic = true;

            wall3.transform.localPosition = new Vector3(wall3.transform.position.x, wall3.transform.position.y, -21.3f);
        }
        if (wall4.transform.position.x < -12.0f)
        {
            wall4.transform.Translate(Time.deltaTime * 4, 0.0f, 0);
            armari2.transform.Translate(Time.deltaTime * 4, 0.0f, 0);
        }
        else
        {

            wallsInPosition++;
            wall4.isStatic = true;
            wall4.transform.localPosition = new Vector3(-12.0f, wall4.transform.position.y, wall4.transform.position.z);
            armari2.transform.localPosition = new Vector3(-11.120f, armari2.transform.position.y, armari2.transform.position.z);
        }

        if (wallsInPosition == 4) return true;
        else return false;
    }

    public bool DestroyWalls()
    {
        destroyWallsTimer += Time.deltaTime;
        if (destroyWallsTimer < 4.0f)
        {
            TutorialWalls.transform.Translate(Vector3.down * Time.deltaTime * 3.0f);
            return false;
        }
        else
        {
            Destroy(TutorialWalls);
            return true;
        }
    }
}
