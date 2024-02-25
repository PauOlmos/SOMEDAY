using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHighSchoolBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject TutorialWalls;

    public float destroyWallsTimer = 0.0f;
    public bool destroyedWalls;
    void Start()
    {
        TutorialWalls.isStatic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyedWalls == false) destroyedWalls = DestroyWalls();
        else
        {

        }
    }

    public bool DestroyWalls()
    {
        destroyWallsTimer += Time.deltaTime;
        if (destroyWallsTimer < 12.0f)
        {
            TutorialWalls.transform.Translate(Vector3.down * Time.deltaTime * 3.0f);
            return false;
        }
        else
        {
            Destroy(TutorialWalls);
            return true;
        }
    }
}
