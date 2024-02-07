using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject currentSelected;
    public GameObject currentMenu;
    public GameObject backMenu;
    public bool canMoveHorizontally = true;
    public bool canMoveVertically = true;

    public enum Menus
    {
        main,options,
    }
    void Start()
    {
        currentSelected.GetComponentInChildren<TextMeshProUGUI>().alpha = 1.0f;

    }

    public Menus switchFromMenu;
    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetButtonUp("Back") && backMenu != null)
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
                default: break;
            }

        }
    }
}
