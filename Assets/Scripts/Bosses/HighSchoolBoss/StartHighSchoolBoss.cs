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
    public NavMeshSurface floor;

    public GameObject armari1;
    public GameObject armari2;

    public GameObject armariPos1;
    public GameObject armariPos2;
    public GameObject[] tableAttackPositions;
    public GameObject[] tableRestPositions;
    public bool highSchoolCreationCompleted = false;
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
            //floor.BuildNavMesh();
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
