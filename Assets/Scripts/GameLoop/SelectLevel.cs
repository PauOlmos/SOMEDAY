using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.AI.Navigation;
using Unity.IO.Archive;
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

    public int scene;
    public bool collidingWithLevel = true;

    public AnimationClip idle;
    public AnimationClip run;
    public AnimationClip actualAnimation;
    public Animator animator;
    public bool isIdle = true;
    public struct levels
    {
        public GameObject level;
        public int levelIndex;
        public bool canAccess;
    }

    void Start()
    {

        dataToStore = LoadPlayerData(Settings.archiveNum);
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
        player.SetActive(false);
        player.transform.position = levelArray[maxLevel - 1].level.transform.position;
        agent.transform.position = levelArray[maxLevel - 1].level.transform.position;
        player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown("Back")) SceneManager.LoadScene(0);
        isIdle = Vector3.Distance(gameObject.transform.position, agent.destination) < 1.0f;
        Animations();
        if (InputManager.GetButtonDown("Jump") && collidingWithLevel)
        {
            collidingWithLevel = true;
            Settings.actualBoss = scene;
            if (scene == 5) SceneManager.LoadScene(3);
            else if(Settings.actualBoss == scene) SceneManager.LoadScene(2);
        }

        if (InputManager.GetAxis("HorizontalArrows") < 0)//Left
        {
            if (actualPosition - 1 >= 0 && canMove == true)
            {
                agent.destination = (levelArray[actualPosition - 1].level.transform.position);
                //Debug.Log(agent.destination);
                actualPosition--;
                canMove = false;
            }
        }
        if (InputManager.GetAxis("HorizontalArrows") > 0)//Right
        {

            if (actualPosition + 1 <= maxLevel && actualPosition + 1 < levelArray.Length && canMove == true)
            {
                agent.destination = (levelArray[actualPosition + 1].level.transform.position);
                //Debug.Log(agent.destination);
                actualPosition++;
                canMove = false;
            }
        }
        if (InputManager.GetAxis("HorizontalArrows") == 0)//Idle
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
            //Debug.Log(json);
            return JsonUtility.FromJson<DataToStore>(json);
        }
        else
        {
            Debug.LogWarning("No se encontraron datos de jugador guardados.");
            return null;
        }
    }

    private void OnTriggerEnter(Collider other)
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
    private void Animations()
    {
        if (isIdle && actualAnimation != idle) animator.Play(idle.name); 
        if (!isIdle && actualAnimation != run) animator.Play(run.name); 
    }

}
