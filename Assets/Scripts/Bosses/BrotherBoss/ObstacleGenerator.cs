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
    public GameObject street;

    void Start()
    {
        whichObject = Random.Range(0, prefabs.Length);
        switch (whichObject)
        {
            case 0:
                //RubishWalls
                GameObject obs = Instantiate(prefabs[whichObject], rubishWallPosition.position, Quaternion.identity);
                obs.transform.SetParent(street.transform);
                break;

            case 1:
                //Containers
                GameObject obs1 = Instantiate(prefabs[whichObject], containerPositions[Random.Range(0, containerPositions.Length)].position, Quaternion.identity);
                obs1.transform.SetParent(street.transform);

                break;

            case 2:

                GameObject obs2 = Instantiate(prefabs[whichObject], carPositions[Random.Range(0, carPositions.Length)].position, Quaternion.identity);
                obs2.transform.SetParent(street.transform);

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
                creatingHand.transform.SetParent(street.transform);

                break;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
