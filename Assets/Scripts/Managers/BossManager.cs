using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    void Start()
    {
        ActivateBoss(currentBoss);
    }

    // Update is called once per frame
    void Update()
    {
        
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
                break;
                default: break;
        }
    }
}
