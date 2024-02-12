using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataToStore
{
    public int numArchive;
    public int maxLevel;
    public int maxHp;
    public float charge;
    public int difficulty;
}

public class MenuButton : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject upButton;
    public GameObject downButton;
    public GameObject rightButton;
    public GameObject leftButton;

    public MenuManager menuManager;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject archivesMenu;
    public GameObject difficultyMenu;
    public GameObject pauseMenu;

    DataToStore dataToStore;
    public int creatingArchiveNum;
    public enum Action
    {
        start,options,exit,archive1,archive2,archive3,easy,hard,nightmare,resume,mainmenu,none
    }

    public Action action;   
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        dataToStore = new DataToStore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressButton()
    {
        switch (action)
        {
            case Action.start:
                ChangeMenu(mainMenu, archivesMenu, MenuManager.Menus.archives, "Archive1");

                break;
            case Action.exit:
                Debug.Log("Exit");
                Application.Quit();

                break;
            case Action.archive1:
                if (!File.Exists(Application.streamingAssetsPath + "/Archive1.json"))
                {
                    creatingArchiveNum = 1;
                    Debug.Log(creatingArchiveNum);
                    ChangeMenu(archivesMenu, difficultyMenu, MenuManager.Menus.difficulty, "Hard");
                }
                else
                {
                    dataToStore.numArchive = 1;

                    SceneManager.LoadScene(2);
                    //Anar al selector de nivells

                }

                break;
            case Action.archive2:
                if (!File.Exists(Application.streamingAssetsPath + "/Archive2.json"))
                {
                    creatingArchiveNum = 2;
                    Debug.Log(creatingArchiveNum);
                    ChangeMenu(archivesMenu, difficultyMenu, MenuManager.Menus.difficulty, "Hard");
                }
                else
                {
                    dataToStore.numArchive = 2;
                    SceneManager.LoadScene(2);
                    //Anar al selector de nivells

                }
                break;
            case Action.archive3:
                if (!File.Exists(Application.streamingAssetsPath + "/Archive3.json"))
                {
                    creatingArchiveNum = 3;
                    Debug.Log(creatingArchiveNum);
                    ChangeMenu(archivesMenu, difficultyMenu, MenuManager.Menus.difficulty, "Hard");
                }
                else
                {
                    dataToStore.numArchive = 3;

                    SceneManager.LoadScene(2);
                    //Anar al selector de nivells
                }
                break;
            case Action.easy:
                dataToStore.difficulty = 0;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
                SceneManager.LoadScene(0);
                break;
            case Action.hard:
                dataToStore.difficulty = 1;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
                SceneManager.LoadScene(0);
                break;
            case Action.nightmare:
                dataToStore.difficulty = 2;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
                SceneManager.LoadScene(0);
                break;
            case Action.options:
                ChangeMenu(pauseMenu, optionsMenu, MenuManager.Menus.pause, "Predetermined");
                break;
            case Action.resume:
                if (menuManager.paused)
                {
                    menuManager.ResumeGame();
                }
                
                break;
            case Action.mainmenu:
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(1);
                break;
            default: break;
        }
    }

    public void SavePlayerData(DataToStore data, int num)
    {
        string json = JsonUtility.ToJson(data);
        string archiveNum = "Archive" + num.ToString(); ;
        File.WriteAllText(Application.streamingAssetsPath + "/"+archiveNum+".json", json);
        Debug.Log("Archive " + num.ToString() + " Created");
        //Comença Partida
    }

    public void CreateArchive(int num)
    {
        dataToStore.numArchive = num;
        dataToStore.maxHp = 3;
        dataToStore.charge = 0.0f;
        dataToStore.maxLevel = 0;
        SavePlayerData(dataToStore, dataToStore.numArchive);
    }

    public void ChangeMenu(GameObject beforeChange, GameObject afterChange, MenuManager.Menus menuType, string nextSelectedButton)
    {
        afterChange.SetActive(true);
        menuManager.currentMenu = afterChange;
        menuManager.backMenu = beforeChange;
        menuManager.currentSelected.GetComponentInChildren<TMPro.TextMeshProUGUI>().alpha = 0.5f;
        menuManager.currentSelected = GameObject.Find(nextSelectedButton);
        menuManager.currentSelected.GetComponentInChildren<TMPro.TextMeshProUGUI>().alpha = 1.0f;
        beforeChange.SetActive(false);
        menuManager.switchFromMenu = menuType;
        menuManager.whichArchiveIsBeingCreated = creatingArchiveNum;
    }

}
