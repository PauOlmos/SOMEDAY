using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject projectilePrefab;
    public Transform mainProjectileSource;
    public Transform target;
    public float timer = 0;
    public int shotCounter = 0;
    public bool right;
    public int maxShots;
    public int gunNum = -1;
    public enum ShotgunState
    {
        movingf,shooting,movingb
    }

    public ShotgunState state = ShotgunState.movingf;

    void Start()
    {
        if (!right) gameObject.transform.Rotate(Vector3.up, 180);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case ShotgunState.movingf:
                timer += Time.deltaTime;
                gameObject.transform.Translate(Vector3.right * Time.deltaTime);
                if(timer >= 3.0f)
                {
                    timer = 0;
                    state = ShotgunState.shooting;
                }
                break;
            case ShotgunState.shooting:
                
                timer += Time.deltaTime;
                if (shotCounter < maxShots)
                {
                    if(gunNum == 0) gameObject.GetComponent<AudioSource>().Play();
                    if (timer >= 0.35f)
                    {
                        CreateProjectile(target, 17.5f);
                        timer = 0.0f;
                        shotCounter++;
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }
                else
                {
                    timer = 0;
                    state = ShotgunState.movingb;
                }

                break;

            case ShotgunState.movingb:

                timer += Time.deltaTime;
                gameObject.transform.Translate(Vector3.left * Time.deltaTime);
                if (timer >= 3.0f)
                {
                    Destroy(gameObject);
                }

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
        projectile.tag = "BasicProjectile";
        projectile.layer = 7;
        projectile.AddComponent<DieByTime>();
        projectile.GetComponent<DieByTime>().deathTime = 10.0f;
    }
}
