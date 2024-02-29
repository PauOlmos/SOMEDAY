using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerOnCorridor : MonoBehaviour
{
    public GameObject boss;
    public PlayerAttack pAttack;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay(Collision other)
    {

        Debug.Log(other.gameObject.name); 
        if(other.gameObject.name == "Player")
        {
            Debug.Log("KLK");
            if(pAttack.attacking == false) boss.GetComponent<HighSchoolBoss>().touchingGround = true;
            else boss.GetComponent<HighSchoolBoss>().touchingGround = false;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.name == "Player")
        {
            Debug.Log("DWWW");
            boss.GetComponent<HighSchoolBoss>().touchingGround = false;
        }
    }
}
