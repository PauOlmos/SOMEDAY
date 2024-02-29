using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject targetObject;
    Vector3 randomPosition;
    void Start()
    {
        GenerateRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<NavMeshAgent>().destination = randomPosition;
        if (Vector3.Distance(gameObject.transform.position, randomPosition) < 1.0f)
        {
            GenerateRandomPosition();
        }
    }
    void GenerateRandomPosition()
    {
        if (targetObject != null)
        {
            // Obtenemos el tama�o del objeto
            Vector3 objectSize = targetObject.GetComponent<Renderer>().bounds.size;

            // Generamos una posici�n aleatoria dentro de los l�mites del objeto
            randomPosition = targetObject.transform.position + new Vector3(
                Random.Range(-objectSize.x / 2f, objectSize.x / 2f),
                Random.Range(-objectSize.y / 2f, objectSize.y / 2f),
                Random.Range(-objectSize.z / 2f, objectSize.z / 2f)
            );

            // Utilizamos la posici�n generada
            Debug.Log("Posici�n aleatoria dentro del objeto: " + randomPosition);
        }
        else
        {
            Debug.LogError("El GameObject de destino no est� asignado.");
        }
    }
}
