using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public enum Action
    {
        start,options,exit,test1,test2,test3,
    }

    public Action action;   
    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
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
                Debug.Log("Start");
                break;
            case Action.exit:
                Debug.Log("Exit");
                Application.Quit();

                break;
            case Action.options:
                optionsMenu.SetActive(true);
                mainMenu.SetActive(false);
                menuManager.currentSelected.GetComponentInChildren<TMPro.TextMeshProUGUI>().alpha = 0.5f;
                menuManager.currentSelected = GameObject.Find("TEST1");
                menuManager.currentSelected.GetComponentInChildren<TMPro.TextMeshProUGUI>().alpha = 1.0f;
                menuManager.currentMenu = optionsMenu;
                menuManager.backMenu = mainMenu;
                menuManager.switchFromMenu = MenuManager.Menus.options;
                
                Debug.Log("Options");

                break;
                case Action.test1:
                Debug.Log("Test1");
                break;
            case Action.test2:
                Debug.Log("Test2");

                break;
            case Action.test3:
                Debug.Log("Test3");

                break;
                default: break;
        }
    }
}
