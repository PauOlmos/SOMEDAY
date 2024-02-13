using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UI;

public class SliderManager : MonoBehaviour
{
    private bool canMoveHorizontally = true;
    private bool canMoveVertically = true;

    public MenuManager menuManager;

    public enum SliderType
    {
        volume,sensitivity,fov
    }

    public SliderType sType;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetAxis("HorizontalArrows") == 0) canMoveHorizontally = true;
        if (Input.GetAxis("VerticalArrows") == 0) canMoveVertically = true;
        ChangeSettingsSlider();

    }

    public void ChangeSettingsSlider()
    {
        switch (sType)
        {
            case SliderType.volume:
                if(menuManager.currentSelected == gameObject)
                {
                    if (Input.GetAxis("HorizontalArrows") < 0 && canMoveHorizontally)
                    {
                        canMoveHorizontally = false;
                        gameObject.GetComponent<UnityEngine.UI.Slider>().value -= 0.05f;
                        Settings.volume = gameObject.GetComponent<UnityEngine.UI.Slider>().value;
                    }
                    if (Input.GetAxis("HorizontalArrows") > 0 && canMoveHorizontally)
                    {
                        canMoveHorizontally = false;
                        gameObject.GetComponent<UnityEngine.UI.Slider>().value += 0.05f;
                        Settings.volume = gameObject.GetComponent<UnityEngine.UI.Slider>().value;

                    }
                }
                
                gameObject.GetComponent<UnityEngine.UI.Slider>().value = Settings.volume;

                break;
            case SliderType.sensitivity:
                if (menuManager.currentSelected == gameObject)
                {
                    if (Input.GetAxis("HorizontalArrows") < 0 && canMoveHorizontally)
                    {
                        canMoveHorizontally = false;
                        gameObject.GetComponent<UnityEngine.UI.Slider>().value -= 0.05f;
                        Settings.sensitivity = gameObject.GetComponent<UnityEngine.UI.Slider>().value;
                    }
                    if (Input.GetAxis("HorizontalArrows") > 0 && canMoveHorizontally)
                    {
                        canMoveHorizontally = false;
                        gameObject.GetComponent<UnityEngine.UI.Slider>().value += 0.05f;
                        Settings.sensitivity = gameObject.GetComponent<UnityEngine.UI.Slider>().value;
                    }
                    gameObject.GetComponent<UnityEngine.UI.Slider>().value = Settings.sensitivity;
                }
                break;
            case SliderType.fov:
                if (menuManager.currentSelected == gameObject)
                {
                    if (Input.GetAxis("HorizontalArrows") < 0 && canMoveHorizontally)
                    {
                        canMoveHorizontally = false;
                        gameObject.GetComponent<UnityEngine.UI.Slider>().value -= 1.0f;
                        Settings.fov = gameObject.GetComponent<UnityEngine.UI.Slider>().value;
                    }
                    if (Input.GetAxis("HorizontalArrows") > 0 && canMoveHorizontally)
                    {
                        canMoveHorizontally = false;
                        gameObject.GetComponent<UnityEngine.UI.Slider>().value += 1.0f;
                        Settings.fov = gameObject.GetComponent<UnityEngine.UI.Slider>().value;
                    }
                }
                gameObject.GetComponent<UnityEngine.UI.Slider>().value = Settings.fov;

                break;
        }
    }

}
