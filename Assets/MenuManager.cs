using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject currentSelected;
    public GameObject currentMenu;
    public GameObject backMenu;

    public GameObject mainMenu;
    public GameObject archivesMenu;
    public GameObject difficultyMenu;

    public bool canMoveHorizontally = true;
    public bool canMoveVertically = true;

    public bool archive1Renamed = false;
    public bool archive2Renamed = false;
    public bool archive3Renamed = false;

    public int whichArchiveIsBeingCreated;

    private float deleteTimer = 0.0f;
    public enum Menus
    {
        main, options, archives, difficulty
    }
    void Start()
    {
        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
    }

    public Menus switchFromMenu;
    private bool isPressingTriangle;
    private float pressingStartTime;

    // Update is called once per frame
    void Update()
    {
        CheckCreatedArchives();
        DeleteArchives();
        if (Input.GetAxis("HorizontalArrows") == 0) canMoveHorizontally = true;
        if (Input.GetAxis("VerticalArrows") == 0) canMoveVertically = true;
        if (Input.GetAxis("HorizontalArrows") < 0 && canMoveHorizontally)
        {
            canMoveHorizontally = false;
            if (currentSelected.GetComponent<MenuButton>().leftButton != null)
            {
                currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                currentSelected = currentSelected.GetComponent<MenuButton>().leftButton;
                currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
            }
        }
        if (Input.GetAxis("HorizontalArrows") > 0 && canMoveHorizontally)
        {
            canMoveHorizontally = false;
            if (currentSelected.GetComponent<MenuButton>().rightButton != null)
            {
                currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                currentSelected = currentSelected?.GetComponent<MenuButton>().rightButton;
                currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
            }
        }
        if (Input.GetAxis("VerticalArrows") > 0 && canMoveVertically)
        {
            canMoveVertically = false;
            if (currentSelected.GetComponent<MenuButton>().upButton != null)
            {
                currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                currentSelected = currentSelected?.GetComponent<MenuButton>().upButton;
                currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
            }
        }
        if (Input.GetAxis("VerticalArrows") < 0 && canMoveVertically)
        {
            canMoveVertically = false;
            if (currentSelected.GetComponent<MenuButton>().downButton != null)
            {
                currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
                currentSelected = currentSelected?.GetComponent<MenuButton>().downButton;
                currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (currentSelected != null) currentSelected.GetComponent<MenuButton>().PressButton();
        }
        if (Input.GetButtonUp("Back"))
        {
            if (backMenu != null)
            {
                backMenu.SetActive(true);
                currentMenu.SetActive(false);

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
                    default: break;
                }
            }
            else
            {
                //Quit Game??
            }


        }
    }

    void DeleteArchives()
    {
        if (Input.GetButton("SwapAbilities"))
        {
            if (currentSelected.name == "Archive1")
            {
                deleteTimer += Time.deltaTime;
                if (deleteTimer > 3.0f)
                {
                    deleteTimer = 0;
                    File.Delete(Application.streamingAssetsPath + "/Archive1.json");
                }
            }
            if (currentSelected.name == "Archive2")
            {
                deleteTimer += Time.deltaTime;
                if (deleteTimer > 3.0f)
                {
                    deleteTimer = 0;

                    File.Delete(Application.streamingAssetsPath + "/Archive2.json");

                }
            }
            if(currentSelected.name == "Archive3")
            {
                deleteTimer += Time.deltaTime;
                if (deleteTimer > 3.0f)
                {
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
}
