using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject empty;

    public GameObject projectilePrefab;

    public Transform[] directions;

    public float timer = 0.0f;
    public float lifeTime = 0.0f;

    Vector3 randomPosition;
    public Transform targetObject;

    public float delayTimer = 0.0f;
    public float delayTime = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer >= delayTime)
        {
            if (lifeTime == 0)
            {
                GenerateRandomPosition();
                empty.GetComponent<NavMeshAgent>().destination = randomPosition;
                empty.GetComponent<NavMeshAgent>().angularSpeed = 0.0f;
            }
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 55.0f);
            timer += Time.deltaTime;
            lifeTime += Time.deltaTime;
            if (timer > 0.25f && empty.activeInHierarchy == true)
            {
                for (int i = 0; i < 4; i++)
                {
                    CreateProjectile(directions[i], 10.0f);
                }
                timer = 0.0f;
            }
            if (lifeTime > 7)
            {
                empty.SetActive(false);
                if (lifeTime > 18)
                {
                    Destroy(empty);
                }
            }
        }
        else
        {
            Color auxColor = Color.Lerp(Color.green, Color.red, delayTimer / delayTime);
            gameObject.GetComponent<Renderer>().material.color = auxColor;
        }
    }

    void CreateProjectile(Transform projectileDirection, float speed)
    {
        GameObject projectile = Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
        projectile.transform.localScale = Vector3.one * 0.7f;
        projectile.AddComponent<SeekingProjectile>();
        projectile.GetComponent<SeekingProjectile>().canFail = true;
        projectile.GetComponent<SeekingProjectile>().shotByPlayer = false;
        projectile.GetComponent<SeekingProjectile>().seekingTime = 0.1f;
        projectile.GetComponent<SeekingProjectile>().target = projectileDirection;
        projectile.GetComponent<SeekingProjectile>().speed = speed;
        projectile.tag = "BasicProjectile";
        projectile.layer = 7;
        projectile.AddComponent<DieByTime>();
        projectile.GetComponent<DieByTime>().deathTime = 5.0f;
    }

    void GenerateRandomPosition()
    {
        if (targetObject != null)
        {
            // Obtenemos el tamaño del objeto
            Vector3 objectSize = targetObject.GetComponent<Renderer>().bounds.size;

            // Generamos una posición aleatoria dentro de los límites del objeto
            randomPosition = targetObject.transform.position + new Vector3(
                Random.Range(-objectSize.x / 2f, objectSize.x / 2f),
                Random.Range(-objectSize.y / 2f, objectSize.y / 2f),
                Random.Range(-objectSize.z / 2f, objectSize.z / 2f)
            );

            // Utilizamos la posición generada
            Debug.Log("Posición aleatoria dentro del objeto: " + randomPosition);
        }
        else
        {
            Debug.LogError("El GameObject de destino no está asignado.");
        }
    }
}
