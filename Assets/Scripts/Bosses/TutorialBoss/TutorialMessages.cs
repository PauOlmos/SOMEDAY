using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessages : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject empty;
    public TextMeshProUGUI whichControlToPress;
    public Image whichControlToShow;
    public string[] controlList;
    public Sprite[] controlImages;

    public int activeDisplay = 0;
    public float showMessagesTimer = 0.0f;
    public GameObject boss;
    public GameObject player;

    public bool readyToShow = true;
    void Start()
    {
        empty.SetActive(false);
        boss = GameObject.Find("Boss");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (boss != null)
        {
            if (boss.GetComponent<TutorialBoss>() != null || boss.GetComponent<StartHighSchoolBoss>() != null)
            {
                if (Settings.tutorialMessages == true && player.GetComponent<PlayerHp>().playerHp > 0)
                {
                    showMessagesTimer += Time.deltaTime;
                    if (boss.GetComponent<TutorialBoss>() != null)
                    {
                        if (boss.GetComponent<TutorialBoss>().phase == 0 && activeDisplay < 3) readyToShow = true;
                        if (boss.GetComponent<TutorialBoss>().phase == 1 && activeDisplay > 2 && activeDisplay < 5) readyToShow = true;
                        if (boss.GetComponent<TutorialBoss>().phase == 1 && activeDisplay > 4) readyToShow = false;
                        if (boss.GetComponent<TutorialBoss>().phase == 2 && activeDisplay > 4 && activeDisplay < 8) readyToShow = true;

                        if (boss.GetComponent<TutorialBoss>().phase == 1 && activeDisplay < 3) activeDisplay = 3;
                        if (boss.GetComponent<TutorialBoss>().phase == 2 && activeDisplay < 5) activeDisplay = 5;
                    }


                    if (showMessagesTimer > 7.5f && readyToShow == true)
                    {
                        if (Time.timeScale > 0.2) Time.timeScale -= Time.deltaTime * 10;
                        empty.SetActive(true);
                        whichControlToPress.text = controlList[activeDisplay];
                        whichControlToShow.sprite = controlImages[activeDisplay];
                        switch (activeDisplay)
                        {
                            case 0:

                                if (InputManager.GetButtonDown("Jump") && boss.GetComponent<TutorialBoss>().phase == 0)
                                {
                                    NextControl();
                                }

                                break;
                            case 1:

                                if (InputManager.GetButtonDown("Attack") && Time.timeScale > 0.0f && boss.GetComponent<TutorialBoss>().phase == 0)
                                {
                                    NextControl();
                                }

                                break;
                            case 2:

                                if (InputManager.GetButtonDown("Dash") && Time.timeScale > 0.0f && boss.GetComponent<TutorialBoss>().phase == 0)
                                {
                                    NextControl();
                                }

                                break;
                            case 3:

                                if (InputManager.GetButtonDown("Parry") && Time.timeScale > 0.0f && boss.GetComponent<TutorialBoss>().phase == 1)
                                {
                                    NextControl();
                                }

                                break;
                            case 4:

                                if (InputManager.GetButtonDown("LockBoss") && Time.timeScale > 0.0f && boss.GetComponent<TutorialBoss>().phase == 1)
                                {
                                    NextControl();
                                }

                                break;
                            case 5:

                                if (InputManager.GetAxis("L2") != -1 && Time.timeScale > 0.0f && boss.GetComponent<TutorialBoss>().phase == 2)
                                {
                                    NextControl();
                                }

                                break;
                            case 6:

                                if (player.GetComponent<PassiveAbility>().passiveCharge < player.GetComponent<PassiveAbility>().necessaryCharge)
                                {
                                    Time.timeScale = 1.0f;
                                    empty.SetActive(false);
                                }
                                else
                                {
                                    if (InputManager.GetAxis("R2") != -1 && Time.timeScale > 0.0f && boss.GetComponent<TutorialBoss>().phase == 2)
                                    {
                                        NextControl();
                                    }
                                }

                                break;
                            case 7:

                                if (InputManager.GetButtonDown("SwapAbilities") && Time.timeScale > 0.0f)
                                {
                                    Time.timeScale = 1.0f;
                                    Destroy(gameObject);
                                }

                                break;

                        }
                    }
                }
                else
                {
                    empty.SetActive(false);
                    if (Time.timeScale != 0.0f && Time.timeScale != 1.0f) Time.timeScale = 1.0f;
                }
            }
        }
    }
    public void NextControl()
    {
        Time.timeScale = 1.0f;
        showMessagesTimer = 0.0f;
        activeDisplay++;
        empty.SetActive(false);
        readyToShow = false;
    }
}
