using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectLevel : MonoBehaviour
{
    // Start is called before the first frame update

    private DataToStore dataToStore;

    public int maxLevel = 0;

    public GameObject[] GoLevelArray;

    public int actualPosition;
    levels[] levelArray;

    public GameObject player;
    public NavMeshAgent agent;

    public bool canMove = true;

    public bool justOnce = false;
    public int scene;
    public bool collidingWithLevel = true;
    public struct levels
    {
        public GameObject level;
        public int levelIndex;
        public bool canAccess;
    }

    void Start()
    {
        dataToStore = LoadPlayerData(1);
        maxLevel = dataToStore.maxLevel + 1;
        levelArray = new levels[maxLevel];
        for(int i = 0; i < levelArray.Length; i++)
        {
            levelArray[i].level = GoLevelArray[i];
            levelArray[i].level.SetActive(true);
            levelArray[i].levelIndex = i;
            levelArray[i].canAccess = true;
        }
        
        actualPosition = levelArray[maxLevel-1].levelIndex;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump") && collidingWithLevel)
        {
            collidingWithLevel = true;
            SceneManager.LoadScene(scene);
        }

        if (justOnce == false)
        {
            player.transform.position = levelArray[maxLevel - 1].level.transform.position;
            agent.transform.position = levelArray[maxLevel - 1].level.transform.position;
        }

        if (Input.GetAxis("HorizontalArrows") < 0)//Left
        {
            if (actualPosition - 1 >= 0 && canMove == true)
            {
                agent.destination = (levelArray[actualPosition - 1].level.transform.position);
                Debug.Log(agent.destination);
                justOnce = true;
                actualPosition--;
                canMove = false;
            }
        }
        if (Input.GetAxis("HorizontalArrows") > 0)//Right
        {

            if (actualPosition + 1 <= maxLevel && actualPosition + 1 < levelArray.Length && canMove == true)
            {
                agent.destination = (levelArray[actualPosition + 1].level.transform.position);
                Debug.Log(agent.destination);
                justOnce = true;
                actualPosition++;
                canMove = false;
            }
        }
        if (Input.GetAxis("HorizontalArrows") == 0)//Idle
        {
            canMove = true;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Level")
        {

            collidingWithLevel = true;
            scene = other.gameObject.GetComponent<SceneToLoad>().sceneNum;

        }
        else
        {
            collidingWithLevel = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Level")
        {
            collidingWithLevel = false;
        }
    }

}
