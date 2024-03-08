using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int projectileType = 3;

    public GameObject pencil;
    public GameObject paperPlane;
    public GameObject schoolBag;

    public bool createdProjectile = false;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Random Projectile is " + projectileType);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localScale.x > 1.0f && createdProjectile == false)
        {
            gameObject.transform.localScale = Vector3.one;
            createdProjectile = true;
            if (projectileType != 3)
            {
                CreateProjectile();
            }
            else
            {
                projectileType = Random.Range(0, 3);
                CreateProjectile();
            }
            

        }
        else if(createdProjectile == false) gameObject.transform.localScale += Vector3.one * Time.deltaTime;

        if(createdProjectile == true)
        {
            if (gameObject.transform.localScale.x > 0.0f) gameObject.transform.localScale -= Vector3.one * Time.deltaTime;
            else Destroy(gameObject);
        }
    }

    public void CreateProjectile()
    {
        switch (projectileType)
        {
            case 0:
                GameObject proj1 = Instantiate(pencil, gameObject.transform.position, Quaternion.identity);
                proj1.AddComponent<SeekingProjectile>();
                proj1.GetComponent<SeekingProjectile>().canFail = true;
                proj1.GetComponent<SeekingProjectile>().shotByPlayer = false;
                proj1.GetComponent<SeekingProjectile>().seekingTime = 0.01f;
                proj1.GetComponent<SeekingProjectile>().target = player;
                proj1.GetComponent<SeekingProjectile>().speed = 12.5f;
                proj1.transform.LookAt(player);
                proj1.tag = "BasicProjectile";
                proj1.layer = 7;
                proj1.AddComponent<DieByTime>();
                break;
            case 1:
                GameObject proj2 = Instantiate(paperPlane, gameObject.transform.position, Quaternion.identity);
                proj2.AddComponent<SeekingProjectile>();
                proj2.GetComponent<SeekingProjectile>().canFail = true;
                proj2.GetComponent<SeekingProjectile>().shotByPlayer = false;
                proj2.GetComponent<SeekingProjectile>().seekingTime = 4.0f;
                proj2.GetComponent<SeekingProjectile>().target = player;
                proj2.GetComponent<SeekingProjectile>().speed = 7.0f;
                proj2.transform.Rotate(0, 180, 0);
                proj2.transform.LookAt(player);
                proj2.tag = "BasicProjectile";
                proj2.layer = 7;
                proj2.AddComponent<DieByTime>();
                proj2.GetComponent<DieByTime>().deathTime = 10.0f;
                break;
            case 2:
                GameObject proj3 = Instantiate(schoolBag, gameObject.transform.position, Quaternion.identity);
                proj3.tag = "BasicProjectile";
                proj3.layer = 7;
                proj3.AddComponent<DieByTime>();
                proj3.AddComponent<SchoolBag>();

                break;
        }
    }
}
