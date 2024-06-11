using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
public static class Settings
{
    // Ejemplo de variable global
    public static int archiveNum;
    public static bool predetSettings = true;
    public static float volume = 0.50f;
    public static float sensitivity = 1.0f;
    public static float fov = 70.0f;
    public static bool tutorialMessages = true;
    public static bool subtitles = true;
    public static int subtitlesSize = 2;
    public static bool healthBar = false;
    public static bool VSync = true;
    public static int actualBoss;
}

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    public GameObject currentSelected;
    public GameObject currentMenu;
    public GameObject backMenu;

    public GameObject mainMenu;
    public GameObject archivesMenu;
    public GameObject difficultyMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject Volume;
    public GameObject Size;
    public GameObject ChangeSettings;
    public GameObject SubtitlesSettings;
    public GameObject Resume;

    public bool canMoveHorizontally = true;
    public bool canMoveVertically = true;

    public bool archive1Renamed = false;
    public bool archive2Renamed = false;
    public bool archive3Renamed = false;

    public int whichArchiveIsBeingCreated;

    private float deleteTimer = 0.0f;

    public bool paused = false;
    public bool playing = false;

    public AudioSource menuAudioSource;
    public AudioClip selectButtonAudio;
    public AudioClip pressButtonAudio;
    public AudioClip changeMenuAudio;
    public enum Menus
    {
        main, options, archives, difficulty, pause,
    }
    void Start()
    {
        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
        playing = GameObject.Find("Player");
    }

    public Menus switchFromMenu;

    public Sprite check;
    public Sprite empty;

    // Update is called once per frame
    void Update()
    {
        CheckCreatedArchives();
        DeleteArchives();
        if (InputManager.GetAxis("HorizontalArrows") == 0) canMoveHorizontally = true;
        if (InputManager.GetAxis("VerticalArrows") == 0) canMoveVertically = true;
        if (InputManager.GetAxis("HorizontalArrows") < 0 && canMoveHorizontally)
        {
            if (player == null)
            {
                canMoveHorizontally = false;
                if (currentSelected.GetComponent<MenuButton>().leftButton != null)
                {
                    menuAudioSource.PlayOneShot(selectButtonAudio);
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                    currentSelected = currentSelected.GetComponent<MenuButton>().leftButton;
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                }
            }
            else if(Time.timeScale == 0)
            {
                canMoveHorizontally = false;
                if (currentSelected.GetComponent<MenuButton>().leftButton != null)
                {
                    menuAudioSource.PlayOneShot(selectButtonAudio);
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                    currentSelected = currentSelected.GetComponent<MenuButton>().leftButton;
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                }
            }
            
        }
        if (InputManager.GetAxis("HorizontalArrows") > 0 && canMoveHorizontally)
        {
            if (player == null)
            {
                canMoveHorizontally = false;
                if (currentSelected.GetComponent<MenuButton>().rightButton != null)
                {
                    menuAudioSource.PlayOneShot(selectButtonAudio);
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                    currentSelected = currentSelected?.GetComponent<MenuButton>().rightButton;
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                }
            }
            else if(Time.timeScale == 0)
            {
                canMoveHorizontally = false;
                if (currentSelected.GetComponent<MenuButton>().rightButton != null)
                {
                    menuAudioSource.PlayOneShot(selectButtonAudio);
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                    currentSelected = currentSelected?.GetComponent<MenuButton>().rightButton;
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                }
            }
            
        }
        if (InputManager.GetAxis("VerticalArrows") > 0 && canMoveVertically)
        {
            if (player == null)
            {
                canMoveVertically = false;
                if (currentSelected.GetComponent<MenuButton>().upButton != null)
                {
                    menuAudioSource.PlayOneShot(selectButtonAudio);

                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                    currentSelected = currentSelected?.GetComponent<MenuButton>().upButton;
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                }
            }
            else if(Time.timeScale == 0)
            {
                canMoveVertically = false;
                if (currentSelected.GetComponent<MenuButton>().upButton != null)
                {
                    menuAudioSource.PlayOneShot(selectButtonAudio);

                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                    currentSelected = currentSelected?.GetComponent<MenuButton>().upButton;
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                }
            }
        }
        float klk = InputManager.GetAxis("VerticalArrows");
        if (klk != 0)
        {
            int klkklk = 0;
        }
        if (InputManager.GetAxis("VerticalArrows") < 0 && canMoveVertically)
        {
            if (player == null)
            {
                canMoveVertically = false;
                if (currentSelected.GetComponent<MenuButton>().downButton != null)
                {
                    menuAudioSource.PlayOneShot(selectButtonAudio);

                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                    currentSelected = currentSelected?.GetComponent<MenuButton>().downButton;
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                }
            }
            else if(Time.timeScale == 0)
            {
                canMoveVertically = false;
                if (currentSelected.GetComponent<MenuButton>().downButton != null)
                {
                    menuAudioSource.PlayOneShot(selectButtonAudio);

                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                    currentSelected = currentSelected?.GetComponent<MenuButton>().downButton;
                    currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                }
            }
            
        }
        if (InputManager.GetButtonUp("Jump"))
        {
            if (currentSelected != null)
            {
                currentSelected.GetComponent<MenuButton>().PressButton();

            }
        }
        if (InputManager.GetButtonUp("Back"))
        {
            if (backMenu != null)
            {
                backMenu.SetActive(true);
                currentMenu.SetActive(false);
                menuAudioSource.PlayOneShot(changeMenuAudio);
                switch (switchFromMenu)
                {
                    case Menus.options:
                        backMenu = null;
                        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                        currentSelected = GameObject.Find("START");
                        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;

                        break;
                    case Menus.archives:
                        backMenu = null;
                        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                        currentSelected = GameObject.Find("START");
                        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
                        break;
                    case Menus.difficulty:
                        switchFromMenu = Menus.archives;
                        currentMenu = archivesMenu;
                        backMenu = mainMenu;
                        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                        currentSelected = GameObject.Find("Archive1");
                        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;

                        break;

                    case Menus.pause:
                        currentMenu = pauseMenu;
                        backMenu = null;
                        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                        currentSelected = GameObject.Find("Resume");
                        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;

                        DataToStore data = LoadPlayerData(Settings.archiveNum);
                        data.charge = LoadPlayerData(Settings.archiveNum).charge;
                        data.difficulty = LoadPlayerData(Settings.archiveNum).difficulty;
                        data.maxHp = LoadPlayerData(Settings.archiveNum).maxHp;
                        data.maxLevel = LoadPlayerData(Settings.archiveNum).maxLevel;
                        data.predetSettings = Settings.predetSettings;
                        data.volume = Settings.volume;
                        data.sensitivity = Settings.sensitivity;
                        data.FOV = Settings.fov;
                        data.tutorialMessages = Settings.tutorialMessages;
                        data.subtitles = Settings.subtitles;
                        data.subtitlesSize = Settings.subtitlesSize;
                        data.healthBar = Settings.healthBar;
                        data.VSync = Settings.VSync;
                        SavePlayerData(data,Settings.archiveNum);
                        break;
                    default: break;
                }
            }
            else if(mainMenu != null)
            {
                Application.Quit();
            }
            StopPhysics();

        }
        if (InputManager.GetButtonUp("Pause") && playing)
        {
            if(!paused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void StopPhysics()
    {
        if (paused) player.GetComponent<Rigidbody>().Sleep();
    }

    void DeleteArchives()
    {
        if (InputManager.GetButton("SwapAbilities") && player == null)
        {
            if (currentSelected.name == "Archive1" && currentSelected.GetComponentInChildren<TextMeshProUGUI>().text == "Continue")
            {
                deleteTimer += Time.deltaTime;
                if (deleteTimer > 3.0f)
                {
                    deleteTimer = 0;

                    DataToStore auxData = LoadPlayerData(1);
                    auxData.maxLevel = 0;
                    auxData.charge = 0.0f;
                    auxData.maxHp = 0;
                    File.Delete(Application.streamingAssetsPath + "/Archive1.json");
                }
            }
            if (currentSelected.name == "Archive2" && currentSelected.GetComponentInChildren<TextMeshProUGUI>().text == "Continue")
            {
                deleteTimer += Time.deltaTime;
                if (deleteTimer > 3.0f)
                {
                    DataToStore auxData = LoadPlayerData(2);
                    auxData.maxLevel = 0;
                    auxData.charge = 0.0f;
                    auxData.maxHp = 0;
                    deleteTimer = 0;
                    File.Delete(Application.streamingAssetsPath + "/Archive2.json");
                }
            }
            if(currentSelected.name == "Archive3" && currentSelected.GetComponentInChildren<TextMeshProUGUI>().text == "Continue")
            {
                deleteTimer += Time.deltaTime;
                if (deleteTimer > 3.0f)
                {
                    DataToStore auxData = LoadPlayerData(3);
                    auxData.maxLevel = 0;
                    auxData.charge = 0.0f;
                    auxData.maxHp = 0;
                    deleteTimer = 0;

                    File.Delete(Application.streamingAssetsPath + "/Archive3.json");
                }
            }
            
        }
        else
        {
            deleteTimer = 0;
        }
    }

    void CheckCreatedArchives()
    {
        if (archivesMenu != null)
        {
            if (archivesMenu.activeInHierarchy == true)
            {
                if (File.Exists(Application.streamingAssetsPath + "/Archive1.json"))
                {
                    GameObject.Find("Archive1").GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
                }
                else
                {
                    GameObject.Find("Archive1").GetComponentInChildren<TextMeshProUGUI>().text = "Create Archive 1";
                }
                if (File.Exists(Application.streamingAssetsPath + "/Archive2.json"))
                {
                    GameObject.Find("Archive2").GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
                }
                else
                {
                    GameObject.Find("Archive2").GetComponentInChildren<TextMeshProUGUI>().text = "Create Archive 2";
                }
                if (File.Exists(Application.streamingAssetsPath + "/Archive3.json"))
                {
                    GameObject.Find("Archive3").GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
                }
                else
                {
                    GameObject.Find("Archive3").GetComponentInChildren<TextMeshProUGUI>().text = "Create Archive 3";
                }
            }
        }
    }

    void PauseGame()
    {
        player.GetComponent<PlayerMovement>().canJump = false;
        paused = true;
        Time.timeScale = 0.0f;
        currentMenu = pauseMenu;
        currentMenu.SetActive(true);
        currentSelected = Resume;
        backMenu = null;
    }
    public void ResumeGame()
    {
        paused = false;
        Time.timeScale = 1.0f;
        currentMenu.SetActive(false);
        currentSelected = null;
        currentMenu = null;
        backMenu = null;
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
    public void SavePlayerData(DataToStore data, int num)
    {
        string json = JsonUtility.ToJson(data);
        string archiveNum = "Archive" + num.ToString(); ;
        File.WriteAllText(Application.streamingAssetsPath + "/" + archiveNum + ".json", json);
        //Debug.Log("Archive " + num.ToString() + " Created");
        //Comença Partida
    }

}
