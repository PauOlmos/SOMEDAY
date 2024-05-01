using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float timer = 0.0f;
    public Material returnableProjectileMaterial;
    public GameObject bossProjectile;
    public Transform direction;
    public Vector3 randomSeed;
    public int seedDirection;
    public Vector3[] droneSeekingPositions;
    
    void Start()
    {
        Vector3[] droneSeekingPositionsAuxiliar = new Vector3[10];
        seedDirection = Random.Range(0, 10);
        for (int i = 0; i < 10; i++)
        {
            // Calcular la posición alrededor del objeto principal
            float angle = i * (360 / 10);
            Vector3 position = player.transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * 15;
            position.y += 3.0f;
            droneSeekingPositionsAuxiliar[i] = new Vector3(position.x,position.y,position.z);
        }
        droneSeekingPositions = droneSeekingPositionsAuxiliar;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
        timer += Time.deltaTime;
        if(Vector3.Distance(player.transform.position, gameObject.transform.position) > 15.0f)
        {
            Vector3 distanceToPlayer = player.transform.position - gameObject.transform.position;
            transform.position += distanceToPlayer * Time.deltaTime / 2.5f;
        }
        else
        {

            if (timer > 3.5f)
            {
                timer = 0.0f;
                CreateProjectile(direction, 30.0f);
            }
            else
            {
                Vector3 distanceToPlayer = Vector3.zero;

                distanceToPlayer = droneSeekingPositions[seedDirection] - gameObject.transform.position;

                //switch (seedDirection)
                //{
                //    case 0:
                //
                //       distanceToPlayer = player.transform.position + (player.transform.forward + randomSeed) * 100 - gameObject.transform.position;
                //
                //        break;
                //    case 1:
                //
                //        distanceToPlayer = player.transform.position + (player.transform.right + randomSeed) * 100 - gameObject.transform.position;
                //
                //        break;
                //    case 2:
                //
                //        distanceToPlayer = player.transform.position + (-player.transform.forward + randomSeed) * 100 - gameObject.transform.position;
                //
                //        break;
                //    case 3:
                //
                //        distanceToPlayer = player.transform.position + (-player.transform.right + randomSeed) * 100 - gameObject.transform.position;
                //
                //        break;
                //}
               
                transform.position += distanceToPlayer * Time.deltaTime / 2.5f;
            }
        }
    }

    void CreateProjectile(Transform projectileDirection, float speed)
    {
        GameObject projectile = Instantiate(bossProjectile, gameObject.transform.position + transform.forward, Quaternion.identity);
        projectile.AddComponent<SeekingProjectile>();
        projectile.GetComponent<SeekingProjectile>().canFail = true;
        projectile.GetComponent<SeekingProjectile>().shotByPlayer = false;
        projectile.GetComponent<SeekingProjectile>().seekingTime = 0.1f;
        projectile.GetComponent<SeekingProjectile>().target = projectileDirection;
        projectile.GetComponent<SeekingProjectile>().speed = speed;
        projectile.GetComponent<SeekingProjectile>().shotBy = gameObject.transform;
        if (Random.Range(0, 1) == 0)
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
