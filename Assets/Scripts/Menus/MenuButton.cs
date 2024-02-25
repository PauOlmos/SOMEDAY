using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]

public class DataToStore
{
    public int numArchive;
    public int maxLevel;
    public int maxHp;
    public float charge;
    public int difficulty;

    public bool predetSettings = Settings.predetSettings;
    public float volume = Settings.volume;
    public float sensitivity = Settings.sensitivity;
    public float FOV = Settings.fov;
    public bool tutorialMessages = Settings.tutorialMessages;
    public bool subtitles = Settings.subtitles;
    public int subtitlesSize = Settings.subtitlesSize;
    public bool healthBar = Settings.healthBar;
    public bool VSync = Settings.VSync;
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

    public DataToStore dataToStore;
    public int creatingArchiveNum;
    public enum Action
    {
        start,options,exit,archive1,archive2,archive3,easy,hard,nightmare,resume,mainmenu,none,
        settingsPreferences,tutorial,subtitles,subtitlesSize1,subtitlesSize2,subtitlesSize3,healthBar,VSync
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
                    dataToStore.numArchive = 1;
                    ChangeMenu(archivesMenu, difficultyMenu, MenuManager.Menus.difficulty, "Hard");
                }
                else
                {
                    dataToStore.numArchive = 1;
                    Settings.archiveNum = 1;
                    LoadSettings(1);
                    SceneManager.LoadScene(3);
                    //Anar al selector de nivells

                }

                break;
            case Action.archive2:
                if (!File.Exists(Application.streamingAssetsPath + "/Archive2.json"))
                {
                    creatingArchiveNum = 2;
                    Debug.Log(creatingArchiveNum);
                    dataToStore.numArchive = 2;
                    ChangeMenu(archivesMenu, difficultyMenu, MenuManager.Menus.difficulty, "Hard");
                }
                else
                {
                    Settings.archiveNum = 2;

                    dataToStore.numArchive = 2;
                    LoadSettings(2);
                    SceneManager.LoadScene(3); 

                    //Anar al selector de nivells

                }
                break;
            case Action.archive3:
                if (!File.Exists(Application.streamingAssetsPath + "/Archive3.json"))
                {
                    creatingArchiveNum = 3;
                    Debug.Log(creatingArchiveNum);
                    ChangeMenu(archivesMenu, difficultyMenu, MenuManager.Menus.difficulty, "Hard");
                    dataToStore.numArchive = 3;

                }
                else
                {
                    dataToStore.numArchive = 3;
                    Settings.archiveNum = 3;
                    LoadSettings(3);
                    SceneManager.LoadScene(3);
                    //Anar al selector de nivells
                }
                break;
            case Action.easy:
                dataToStore.difficulty = 0;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
                Settings.archiveNum = menuManager.whichArchiveIsBeingCreated;
                LoadSettings(menuManager.whichArchiveIsBeingCreated);
                SceneManager.LoadScene(0);
                break;
            case Action.hard:
                dataToStore.difficulty = 1;
                Settings.archiveNum = menuManager.whichArchiveIsBeingCreated;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
                LoadSettings(menuManager.whichArchiveIsBeingCreated);

                SceneManager.LoadScene(0);
                break;
            case Action.nightmare:
                dataToStore.difficulty = 2;
                Settings.archiveNum = menuManager.whichArchiveIsBeingCreated;
                CreateArchive(menuManager.whichArchiveIsBeingCreated);
                LoadSettings(menuManager.whichArchiveIsBeingCreated);

                SceneManager.LoadScene(0);
                break;
            case Action.options:
                ChangeMenu(pauseMenu, optionsMenu, MenuManager.Menus.pause, "Predetermined");
                if (Settings.predetSettings == true)
                {
                    menuManager.ChangeSettings.SetActive(false);
                }
                else
                {
                    menuManager.ChangeSettings.SetActive(true);
                    if(Settings.subtitles == true)
                    {
                        menuManager.SubtitlesSettings.SetActive(true);
                    }
                    else
                    {
                        menuManager.SubtitlesSettings.SetActive(false);
                    }
                
                }
                break;
            case Action.resume:
                if (menuManager.paused)
                {
                    menuManager.ResumeGame();
                }
                
                break;
            case Action.mainmenu:
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(2);
                break;
            case Action.settingsPreferences:

                if(Settings.predetSettings == false)
                {
                    Settings.predetSettings = true;
                    downButton = null;
                    menuManager.ChangeSettings.SetActive(false);
                    Settings.volume = 0.5f;
                    Settings.sensitivity = 1.0f;
                    Settings.fov = 50.0f;
                    Settings.tutorialMessages = true;
                    Settings.subtitles = true;
                    Settings.subtitlesSize = 2;
                    Settings.healthBar = false;
                    Settings.VSync = true;
                    menuManager.SubtitlesSettings.SetActive(true);
                    SavePlayerData(dataToStore,Settings.archiveNum);

                }
                else
                {
                    menuManager.ChangeSettings.SetActive(true);
                    Settings.predetSettings = false;
                    downButton = menuManager.Volume;

                }

                break;
            case Action.tutorial:
                
                Settings.tutorialMessages = !Settings.tutorialMessages;
                
                break;
            case Action.subtitles:
                
                Settings.subtitles = !Settings.subtitles;
                menuManager.SubtitlesSettings.SetActive(Settings.subtitles);

                if(Settings.subtitles == true)
                {
                    rightButton = menuManager.Size;
                }
                else
                {
                    rightButton = null;
                }

                break;
            case Action.subtitlesSize1:

                Settings.subtitlesSize = 1;

                break;
            case Action.subtitlesSize2:

                Settings.subtitlesSize = 2;

                break;
            case Action.subtitlesSize3:

                Settings.subtitlesSize = 3;

                break;
            case Action.healthBar:
                
                Settings.healthBar = !Settings.healthBar;

                break;
            case Action.VSync:
                
                Settings.VSync = !Settings.VSync;
                if (Settings.VSync) QualitySettings.vSyncCount = 1;
                else QualitySettings.vSyncCount = 0;
                

                break;
            default: break;
        }
    }

    public void LoadSettings(int num)
    {
        DataToStore data;
        data = LoadPlayerData(num);
        Settings.predetSettings = data.predetSettings;
        Settings.volume = data.volume;
        Settings.sensitivity = data.sensitivity;
        Settings.fov = data.FOV;
        Settings.tutorialMessages = data.tutorialMessages;
        Settings.subtitles = data.subtitles;
        Settings.subtitlesSize = data.subtitlesSize;
        Settings.healthBar = data.healthBar;
        Settings.VSync = data.VSync;
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
        dataToStore.predetSettings = true;
        dataToStore.volume = 0.5f;
        dataToStore.sensitivity = 1;
        dataToStore.FOV = 50.0f;
        dataToStore.tutorialMessages = true;
        dataToStore.subtitles = true;
        dataToStore.subtitlesSize = 1;
        dataToStore.healthBar = false;
        dataToStore.VSync = true;
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
