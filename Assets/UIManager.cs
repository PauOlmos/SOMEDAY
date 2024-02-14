using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerHp pHp;
    public PassiveAbility ability;
    public GameObject[] hearts;
    public Image chargedPassive;
    public Image actualPassive;

    public Sprite activePassive;
    public Sprite nonActivePassive;
    public Sprite distanceAttack;
    public Sprite restoreHp;

    public Color auxColor;
    public float klk;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        klk = ability.passiveCharge / 100;
        GetHp();
        GetChargedAbility();
        GetActualAbility();
    }

    public void GetHp()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < pHp.playerHp) hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
        }
    }
    public void GetChargedAbility()
    {
        if (ability.passiveCharge < 100.0f)
        {
            chargedPassive.sprite = nonActivePassive;
            auxColor.r = ability.passiveCharge / 100;
            auxColor.g = ability.passiveCharge / 100;
            auxColor.b = ability.passiveCharge / 100;
            auxColor.a = ability.passiveCharge / 50;
            chargedPassive.color = auxColor;
        }
        else
        {
            chargedPassive.sprite = activePassive;
        }
    }

    public void GetActualAbility()
    {
        if (ability.passive == PassiveAbility.passiveType.shoot) actualPassive.sprite = distanceAttack;
        else actualPassive.sprite = restoreHp;
    }
}
