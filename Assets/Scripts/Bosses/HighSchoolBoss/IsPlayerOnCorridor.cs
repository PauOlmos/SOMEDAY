using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerOnCorridor : MonoBehaviour
{
    public GameObject boss;
    public PlayerAttack pAttack;

    public bool moveTexture = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(moveTexture) gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, gameObject.GetComponent<Renderer>().material.mainTextureOffset.y - Time.deltaTime);
    }

    private void OnCollisionStay(Collision other)
    {

        if(other.gameObject.name == "Player"&& boss.GetComponent<HighSchoolBoss>() != null)
        {
            if(pAttack.attacking == false) boss.GetComponent<HighSchoolBoss>().touchingGround = true;
            else boss.GetComponent<HighSchoolBoss>().touchingGround = false;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.name == "Player" && boss.GetComponent<HighSchoolBoss>() != null)
        {
            boss.GetComponent<HighSchoolBoss>().touchingGround = false;
        }
    }
}
