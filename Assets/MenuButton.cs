using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    DataToStore dataToStore;
    public int creatingArchiveNum;
    public enum Action
    {
        start,options,exit,archive1,archive2,archive3,easy,hard,nightmare
    }

    public Action action;   
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        dataToStore = GameObject.Find("DataToStore").GetComponent<DataToStore>();
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
                /*archivesMenu.SetActive(true);
                menuManager.currentMenu = archivesMenu;
                menuManager.backMenu = mainMenu;
                menuManager.currentSelected.GetComponentInChildren<TMPro.TextMeshProUGUI>().alpha = 0.5f;
                menuManager.currentSelected = GameObject.Find("Archive1");
                menuManager.currentSelected.GetComponentInChildren<TMPro.TextMeshProUGUI>().alpha = 1.0f;
                mainMenu.SetActive(false);
                menuManager.switchFromMenu = MenuManager.Menus.archives;*/

                break;
            case Action.exit:
                Debug.Log("Exit");
                Application.Quit();

                break;
            case Action.options:
                //ChangeMenu(mainMenu, archivesMenu, MenuManager.Menus.archives, "Archive1");
                /*optionsMenu.SetActive(true);
                mainMenu.SetActive(false);
                menuManager.currentSelected.GetComponentInChildren<TMPro.TextMeshProUGUI>().alpha = 0.5f;
                menuManager.currentSelected = GameObject.Find("TEST1");
                menuManager.currentSelected.GetComponentInChildren<TMPro.TextMeshProUGUI>().alpha = 1.0f;
                menuManager.currentMenu = optionsMenu;
                menuManager.backMenu = mainMenu;
                menuManager.switchFromMenu = MenuManager.Menus.options;*/

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
                    //Anar al selector de nivells
                }
                break;
            case Action.easy:
                dataToStore.dificulty = DataToStore.Dificulty.easy;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
                break;
            case Action.hard:
                dataToStore.dificulty = DataToStore.Dificulty.hard;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
                break;
            
            case Action.nightmare:
                dataToStore.dificulty = DataToStore.Dificulty.nightmare;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
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
        //Comen�a Partida

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
