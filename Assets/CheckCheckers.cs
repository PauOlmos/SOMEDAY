using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckCheckers : MonoBehaviour
{

    public enum SettingsState
    {
        predet, tutorial, subtitles,subtitlesSize,healthBar,VSync
    }
    public MenuManager menuManager;
    public SettingsState state;
    public GameObject Go;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case SettingsState.predet:
                Debug.Log(Settings.predetSettings);
                if (Settings.predetSettings == true) Go.GetComponent<Image>().sprite = menuManager.check;
                else Go.GetComponent<Image>().sprite = menuManager.empty;
                break;
            case SettingsState.tutorial:

                if (Settings.tutorialMessages) Go.GetComponent<Image>().sprite = menuManager.check;
                else Go.GetComponent<Image>().sprite = menuManager.empty;
                break;
            case SettingsState.subtitles:

                if (Settings.subtitles) Go.GetComponent<Image>().sprite = menuManager.check;
                else Go.GetComponent<Image>().sprite = menuManager.empty;
                break;
            case SettingsState.subtitlesSize:

                if (gameObject.name == "Small")
                {
                    if (Settings.subtitlesSize == 1)
                    {
                        Go.GetComponent<Image>().sprite = menuManager.check;
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    }
                    else
                    {
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "S";
                        Go.GetComponent<Image>().sprite = menuManager.empty;
                    }
                }
                if(gameObject.name == "Medium")
                {
                    if (Settings.subtitlesSize == 2)
                    {
                        Go.GetComponent<Image>().sprite = menuManager.check;
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    }
                    else
                    {
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "M";
                        Go.GetComponent<Image>().sprite = menuManager.empty;
                    }
                }
                if(gameObject.name == "Big")
                {
                    if (Settings.subtitlesSize == 3)
                    {
                        Go.GetComponent<Image>().sprite = menuManager.check;
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    }
                    else
                    {
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "L";
                        Go.GetComponent<Image>().sprite = menuManager.empty;
                    }
                }
                break;
            case SettingsState.healthBar:

                if (Settings.healthBar) Go.GetComponent<Image>().sprite = menuManager.check;
                else Go.GetComponent<Image>().sprite = menuManager.empty;
                break;
            case SettingsState.VSync:

                if (Settings.VSync) Go.GetComponent<Image>().sprite = menuManager.check;
                else Go.GetComponent<Image>().sprite = menuManager.empty;
                break;
            default: break;
        }
    }
}
