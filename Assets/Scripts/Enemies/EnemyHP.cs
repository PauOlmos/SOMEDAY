using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int hp = 10;
    public bool canBeDamaged = true;
    public bool stun = false;
    public BossManager bossManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Enemy" && hp <= 0)
        {
            Destroy(gameObject);
        } 
    }

    public void DamageEnemy(int damage, bool weakPoint)
    {
        if (canBeDamaged || weakPoint)
        {
            if (hp > 0)
            {
                if(gameObject.GetComponent<HighSchoolBoss>() != null)
                {
                    gameObject.GetComponent<HighSchoolBoss>().damaged = true;
                    if (gameObject.GetComponent<HighSchoolBoss>().phase == 0)
                    {
                        gameObject.GetComponent<HighSchoolBoss>().hitsToSuperAttack -= damage;
                    }
                }
                else if(gameObject.GetComponent<DadBoss>() != null)
                {
                    gameObject.GetComponent<DadBoss>().damaged = true;
                }
                else if(gameObject.GetComponent<MomBoss>() != null)
                {
                    gameObject.GetComponent<MomBoss>().damaged = true;
                }
                else if(gameObject.GetComponent<BrotherBoss>() != null)
                {
                    gameObject.GetComponent<BrotherBoss>().damaged = true;
                }
                hp -= damage;
                canBeDamaged = false;
                DamageSound();
                
            }
        }
    }

    public void DamageSound()
    {
        if(gameObject.GetComponent<TutorialBoss>() != null)
        {
            gameObject.GetComponent<TutorialBoss>().bossAudioSource.PlayOneShot(gameObject.GetComponent<TutorialBoss>().tutorialBossAudios[2]);
        }
        if(gameObject.GetComponent<HighSchoolBoss>() != null)
        {
            gameObject.GetComponent<HighSchoolBoss>().bossAudioSource.PlayOneShot(gameObject.GetComponent<HighSchoolBoss>().highSchoolBossAudios[11]);
        }
        if(gameObject.GetComponent<DadBoss>() != null || gameObject.GetComponent<MomBoss>()!= null)
        {
            bossManager.bossAudioSource.PlayOneShot(bossManager.highSchoolBossAudios[11]);
        }
    }
}
