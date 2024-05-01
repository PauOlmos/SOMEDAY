using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] prefabs;
    public int whichObject;
    public Transform rubishWallPosition;
    public Transform[] containerPositions;
    public Transform[] carPositions;
    public Transform[] handPositions;

    void Start()
    {
        whichObject = Random.Range(0, prefabs.Length);
        switch (whichObject)
        {
            case 0:
                //RubishWalls
                Instantiate(prefabs[whichObject], rubishWallPosition.position, Quaternion.identity);

                break;

            case 1:
                //Containers
                Instantiate(prefabs[whichObject], containerPositions[Random.Range(0, containerPositions.Length)].position, Quaternion.identity);

                break;

            case 2:

                Instantiate(prefabs[whichObject], carPositions[Random.Range(0, carPositions.Length)].position, Quaternion.identity);

                break;

            case 3:
                int value = Random.Range(0, handPositions.Length);
                GameObject creatingHand = Instantiate(prefabs[whichObject], handPositions[value].position, Quaternion.identity);
                if(value == 0)
                {
                    creatingHand.GetComponentInChildren<ActivateHand>().handDamage.name = "HandDamage2";
                    creatingHand.GetComponentInChildren<ActivateHand>().speed = -creatingHand.GetComponentInChildren<ActivateHand>().speed;
                    creatingHand.transform.Rotate(0, 180, 0);
                }
                break;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
