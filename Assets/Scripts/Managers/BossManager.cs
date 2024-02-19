using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BossManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int currentBoss = 0;

    public int phase = 0;

    public int bossAttack;

    public int bossMovement;

    public bool canMove = true;
    public bool canAttack = true;

    public GameObject player;
    public GameObject boss;
    public GameObject auxiliarBoss;

    public Transform tutorialMap;
    public float radius;

    public GameObject[] circlesPrefabs = new GameObject[3];
    public NavMeshAgent agent;
    public float transfromTimer = 0.0f;
    public GameObject proximityArea;
    public GameObject weakPoint;
    public LayerMask Ground;

    public GameObject Sword1;
    public GameObject Sword2;

    void Start()
    {
        ActivateBoss(currentBoss);
    }

    // Update is called once per frame
    void Update()
    {
        CheckBossHp(currentBoss);
    }

    public void ActivateBoss(int nBoss)
    {
        switch (nBoss)
        {
            case 0:
                boss = GameObject.Find("Boss");
                boss.AddComponent<TutorialBoss>();
                boss.GetComponent<TutorialBoss>().player = player;
                boss.GetComponent<TutorialBoss>().tutorialMap = tutorialMap;
                boss.GetComponent<TutorialBoss>().radius = radius;
                boss.GetComponent<TutorialBoss>().circlesPrefabs = circlesPrefabs;
                boss.GetComponent<TutorialBoss>().Ground = 6;
                boss.GetComponent<TutorialBoss>().agent = agent;
                boss.GetComponent<TutorialBoss>().proximityArea = proximityArea;
                boss.GetComponent<TutorialBoss>().weakPoint = weakPoint;
                boss.GetComponent<TutorialBoss>().Sword1 = Sword1;
                boss.GetComponent<TutorialBoss>().Sword2 = Sword2;
                break;
                default: break;
        }
    }

    public void CheckBossHp(int nBoss)
    {
        switch (nBoss)
        {
            case 0:

                switch (boss.GetComponent<TutorialBoss>().phase)
                {
                    case 0:
                        if (boss.GetComponent<EnemyHP>().hp < 6)
                        {
                            transfromTimer += Time.deltaTime;
                            if(transfromTimer > 3.0f)
                            {
                                boss.GetComponent<TutorialBoss>().phase++;
                                boss.GetComponent<TutorialBoss>().canAttack = false;
                                boss.GetComponent<TutorialBoss>().canMove = true;
                                boss.GetComponent<EnemyHP>().canBeDamaged = false;
                                transfromTimer = 0.0f;
                                boss.GetComponent<NavMeshAgent>().enabled = true;

                            }
                            else
                            {
                                boss.transform.localScale += Vector3.one * Time.deltaTime / 2;
                            }
                            
                        }
                        break;
                    case 1:
                        if (boss.GetComponent<EnemyHP>().hp < 3)
                        {
                            transfromTimer += Time.deltaTime;
                            if (transfromTimer > 3.0f)
                            {
                                transfromTimer = 0.0f;
                                boss.GetComponent<TutorialBoss>().phase++;
                                boss.GetComponent<TutorialBoss>().weakPoint.SetActive(false);
                                boss.GetComponent<TutorialBoss>().proximityArea.SetActive(false);
                                boss.GetComponent<NavMeshAgent>().enabled = false;
                                boss.GetComponent<TutorialBoss>().movementState = TutorialBoss.MovementState.startSpinning;
                                boss.GetComponent<TutorialBoss>().Ground = Ground;
                                boss.GetComponent<TutorialBoss>().Sword1.SetActive(true);
                                boss.GetComponent<TutorialBoss>().Sword2.SetActive(true);

                            }
                        }
                        break;
                    case 2:
                        if (boss.GetComponent<EnemyHP>().hp <= 0); //Kill&SwitchBoss boss.GetComponent<TutorialBoss>().phase++;
                        break;
                }

                break;
        }
    }
}