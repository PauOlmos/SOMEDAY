using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadBoss : MonoBehaviour
{
    public int phase = 0;
    public Transform mainProjectileSource;
    public Transform[] bulletHell1Directions;
    public Transform bulletHell2Direction;
    public Transform[] bulletHell3Directions;
    public bool canAttack = false;
    public float attackCooldownTime = 5.0f;
    public float attackCooldownTimer;

    public float bulletHellTimer = 0.0f;
    public float bulletHell2Timer = 0.0f;
    public int bulletHell1Count;
    public int bulletHell3Count = 0;
    public Vector3 originalBulletHell2DirectionPosition = Vector3.zero;

    public GameObject shotgun;
    public Transform[] rightShotgunPositions;
    public Transform[] leftShotgunPositions;
    public float shotgunTimer = 0.0f;

    public GameObject player;
    public enum AttackType
    {
        bullethell1,bullethell2,bullethell3,wallShotGuns,impulse
    }

    public AttackType attackType;

    public GameObject projectilePrefab;
    public Material returnableProjectileMaterial;

    public float delayTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(originalBulletHell2DirectionPosition == Vector3.zero && bulletHell2Direction != null) originalBulletHell2DirectionPosition = bulletHell2Direction.transform.position;
        //Debug.Log(originalBulletHell2DirectionPosition);
        switch(phase)
        {
            case 0:

                if(canAttack == true)
                {
                    switch (attackType)
                    {
                        case AttackType.bullethell1:
                            delayTimer += Time.deltaTime;
                            if(delayTimer >= 1.5f)
                            {
                                if (bulletHell1Count < 10)
                                {
                                    if (bulletHellTimer >= 0.15f)
                                    {
                                        for (int i = 0; i < bulletHell1Directions.Length; i++)
                                        {
                                            CreateProjectile(bulletHell1Directions[i], 15.0f);
                                        }
                                        bulletHellTimer = 0;
                                        bulletHell1Count++;
                                    }
                                    else
                                    {
                                        bulletHellTimer += Time.deltaTime;
                                    }
                                }
                                else
                                {
                                    canAttack = false;
                                    delayTimer = 0.0f;
                                    mainProjectileSource.GetComponentInChildren<Renderer>().material.color = Color.green;
                                    mainProjectileSource.gameObject.SetActive(false);
                                    bulletHell1Count = 0;
                                }

                            }
                            else
                            {
                                Color auxColor = Color.Lerp(Color.green, Color.red, delayTimer / 1.5f);
                                mainProjectileSource.GetComponentInChildren<Renderer>().material.color = auxColor;
                            }
                            break;
                        case AttackType.bullethell2:
                            delayTimer += Time.deltaTime;
                            if (delayTimer >= 1.5f)
                            {
                                if (bulletHell2Timer < 10)
                                {
                                    bulletHell2Timer += Time.deltaTime;
                                    if (bulletHell2Timer < 3 || (bulletHell2Timer > 6 && bulletHell2Timer < 9)) bulletHell2Direction.RotateAround(mainProjectileSource.transform.position, Vector3.up, -Time.deltaTime * 50.0f);
                                    else bulletHell2Direction.RotateAround(mainProjectileSource.transform.position, Vector3.up, Time.deltaTime * 50.0f);

                                    if (bulletHellTimer >= 0.15f)
                                    {
                                        CreateProjectile(bulletHell2Direction, 15.0f);
                                        bulletHellTimer = 0.0f;

                                    }
                                    else
                                    {
                                        bulletHellTimer += Time.deltaTime;
                                    }
                                }
                                else
                                {
                                    bulletHell2Direction.transform.position = originalBulletHell2DirectionPosition;
                                    delayTimer = 0.0f;
                                    mainProjectileSource.GetComponentInChildren<Renderer>().material.color = Color.green;
                                    mainProjectileSource.gameObject.SetActive(false);
                                    canAttack = false;
                                    bulletHell2Timer = 0;
                                }
                            }
                            else
                            {
                                Color auxColor = Color.Lerp(Color.green, Color.red, delayTimer / 1.5f);
                                mainProjectileSource.GetComponentInChildren<Renderer>().material.color = auxColor;
                            }
                            break;
                            
                        case AttackType.bullethell3:
                            delayTimer += Time.deltaTime;
                            if (delayTimer >= 1.5f)
                            {
                                if (bulletHell1Count < 1)
                                {
                                    if (bulletHellTimer >= 0.15f)
                                    {
                                        for (int i = 0; i < bulletHell1Directions.Length; i++)
                                        {
                                            CreateProjectile(bulletHell1Directions[i], 15.0f);
                                        }
                                        bulletHellTimer = 0;
                                        bulletHell1Count++;
                                    }
                                    else
                                    {
                                        bulletHellTimer += Time.deltaTime;
                                    }
                                }
                                else
                                {
                                    if (bulletHell3Count < 5)
                                    {
                                        if (bulletHellTimer >= 0.15f)
                                        {
                                            for (int i = 0; i < bulletHell3Directions.Length; i++)
                                            {
                                                CreateProjectile(bulletHell3Directions[i], 22.5f);
                                            }
                                            bulletHellTimer = 0;
                                            bulletHell3Count++;
                                        }
                                        else
                                        {
                                            bulletHellTimer += Time.deltaTime;
                                        }
                                    }
                                    else
                                    {
                                        bulletHellTimer = 0; 
                                        delayTimer = 0.0f;
                                        mainProjectileSource.GetComponentInChildren<Renderer>().material.color = Color.green;
                                        mainProjectileSource.gameObject.SetActive(false);
                                        canAttack = false;
                                        bulletHell1Count = 0;
                                        bulletHell3Count = 0;
                                    }

                                }
                            }
                            else
                            {
                                Color auxColor = Color.Lerp(Color.green, Color.red, delayTimer / 1.5f);
                                mainProjectileSource.GetComponentInChildren<Renderer>().material.color = auxColor;
                            }
                            break;
                        case AttackType.wallShotGuns:

                            if(shotgunTimer == 0)
                            {
                                for(int i = 0; i < rightShotgunPositions.Length; i++)
                                {
                                    GameObject gun = Instantiate(shotgun, rightShotgunPositions[i].position, Quaternion.identity);
                                    gun.GetComponent<Shotgun>().right = true;
                                }
                            }
                            else if(shotgunTimer > 7.0f)
                            {
                                for (int i = 0; i < leftShotgunPositions.Length; i++)
                                {
                                    GameObject gun = Instantiate(shotgun, leftShotgunPositions[i].position, Quaternion.identity);
                                    gun.GetComponent<Shotgun>().right = false;
                                }
                                shotgunTimer = 0;
                                canAttack = false;
                                break;
                            }


                            shotgunTimer += Time.deltaTime;

                            break;
                    }
                }
                else
                {
                    attackCooldownTimer += Time.deltaTime;
                    if(attackCooldownTimer >= attackCooldownTime)
                    {
                        attackCooldownTimer = 0;
                        canAttack = true;
                        int value = Random.Range(0, 4);
                        switch (value)
                        {
                            case 0:attackType = AttackType.bullethell1; break;
                            case 1:attackType = AttackType.bullethell2; break;
                            case 2:attackType = AttackType.bullethell3; break;
                            case 3:attackType = AttackType.wallShotGuns; break;
                        }
                        if (value != 3) mainProjectileSource.gameObject.SetActive(true);
                    }
                }

                break;
            case 1:

                break;
        }
    }

    void CreateProjectile(Transform projectileDirection, float speed)
    {
        GameObject projectile = Instantiate(projectilePrefab, mainProjectileSource.position, Quaternion.identity);
        projectile.AddComponent<SeekingProjectile>();
        projectile.GetComponent<SeekingProjectile>().canFail = true;
        projectile.GetComponent<SeekingProjectile>().shotByPlayer = false;
        projectile.GetComponent<SeekingProjectile>().seekingTime = 0.1f;
        projectile.GetComponent<SeekingProjectile>().target = projectileDirection;
        projectile.GetComponent<SeekingProjectile>().speed = speed;
        if (Random.Range(0, 20) == 0)
        {
            projectile.tag = "ReturnableProjectile";
            projectile.GetComponent<Renderer>().material = returnableProjectileMaterial;
        }
        else projectile.tag = "BasicProjectile";
        projectile.layer = 7;
        projectile.AddComponent<DieByTime>();
        projectile.GetComponent<DieByTime>().deathTime = 5.0f;
    }
}
