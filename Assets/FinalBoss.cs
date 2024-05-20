using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public bool canAttack = false;

    public float attackTimer = 0.0f;

    public enum AttackType
    {
        Projectiles, slash, sword
    }

    public AttackType attackType;

    public int numProjectiles = 0;

    public GameObject projectiles;

    public Transform projectileSource;

    public float projectilesCooldown = 0.0f;

    public Animator animator;
    public AnimationClip idle;
    public AnimationClip slashAttack;
    public float slashAttackTimer = 0.0f;
    public GameObject realSword;
    public GameObject auxiliarSword;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > 10.0f) SelectAttack();
        }
        else
        {
            switch (attackType)
            {
                case AttackType.Projectiles:

                    projectilesCooldown += Time.deltaTime;

                    if (projectilesCooldown > 0.25f)
                    {
                        GameObject p = Instantiate(projectiles, projectileSource.transform.position, Quaternion.identity);
                        Vector3 forceVector = new Vector3(Random.Range(-100, 100), Random.Range(100, 200), Random.Range(-400, -600));
                        p.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
                        p.GetComponent<FinalBossProjectile>().player = player;
                        numProjectiles++;
                        projectilesCooldown = 0.0f;
                    }

                    if (numProjectiles > 20)
                    {
                        projectilesCooldown = 0;
                        numProjectiles = 0;
                        canAttack = false;
                    }

                    break;

                case AttackType.slash:

                    slashAttackTimer += Time.deltaTime;

                    if (slashAttackTimer > 36.0f && slashAttackTimer < 42.5f)
                    {
                        auxiliarSword.SetActive(true);
                        auxiliarSword.transform.Rotate(Vector3.left * Time.deltaTime * 25.0f);

                    }

                    if(slashAttackTimer > 42.5f)
                    {
                        auxiliarSword.SetActive(false);
                    }

                    if (slashAttackTimer * 0.05f > slashAttack.length)
                    {
                        slashAttackTimer = 0.0f;
                        animator.Play(idle.name);
                        canAttack = false;
                        Vector3 currentRotation = auxiliarSword.transform.rotation.eulerAngles;
                        currentRotation.y = 180f;
                        auxiliarSword.transform.rotation = Quaternion.Euler(currentRotation);
                        auxiliarSword.transform.Rotate(180,0, 0);
                    }


                    break;

            }
            

            

        }
    }

    public void SelectAttack()
    {
        int value = Random.Range(0, 3);
        value = 1;
        switch (value)
        {
            case 0:

                attackType = AttackType.Projectiles;
                break;
            case 1:
                attackType = AttackType.slash;
                animator.Play(slashAttack.name);
                break;
        }
        canAttack = true;
        attackTimer = 0.0f;
    }
}
