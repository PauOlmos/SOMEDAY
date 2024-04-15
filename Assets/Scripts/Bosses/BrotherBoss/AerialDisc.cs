using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialDisc : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform pointArea;
    public GameObject[] rays;
    public Vector3 actualSeekingPosition;
    public Vector3 randomRotations;
    Vector3 distanceToPoint;
    void Start()
    {
        actualSeekingPosition = GenerarPosiciones(pointArea);
        distanceToPoint = actualSeekingPosition - gameObject.transform.position;
        randomRotations.x = Random.Range(25, 50);
        randomRotations.y = Random.Range(25, 50);
        randomRotations.z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(randomRotations * Time.deltaTime * 2);

        for(int i = 0; i < rays.Length; i++)
        {
            if(rays[i].transform.localScale.z < 50) rays[i].transform.localScale += Vector3.forward * Time.deltaTime * 10;

        }

        if (Vector3.Distance(gameObject.transform.position, actualSeekingPosition) < 1.0f)
        {
            actualSeekingPosition = GenerarPosiciones(pointArea);
            distanceToPoint = actualSeekingPosition - gameObject.transform.position;

        }
        else
        {
            transform.position += distanceToPoint * Time.deltaTime / 5;
        }
    }
    Vector3 GenerarPosiciones(Transform area)
    {

        // Generar posiciones aleatorias dentro del volumen
        Vector3 posicion = new Vector3(Random.Range(-area.transform.localScale.x / 2, area.transform.localScale.x / 2),
                                       Random.Range(-area.transform.localScale.y / 2, area.transform.localScale.y / 2),
                                       Random.Range(-area.transform.localScale.z / 2, area.transform.localScale.z / 2));

        // Ajustar la posición al espacio del volumen
        return posicion += area.transform.position;

    }
}
