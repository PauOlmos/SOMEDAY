using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CrownActive
{
    public static bool collector = false; //Change for collector's edition
    public static bool permaCompleted = false;
}

public class CrownManager : MonoBehaviour
{

    public GameObject crown;

    // Start is called before the first frame update
    void Start()
    {

        CheckCrown();
    }

    // Update is called once per frame
    void Update()
    {
        CrownActive.permaCompleted = File.Exists(Application.streamingAssetsPath + "/PermaCompleted.json");
        CheckCrown();

    }

    public void CheckCrown()
    {
        if (CrownActive.collector == true || CrownActive.permaCompleted == true) crown.SetActive(true);
        if (CrownActive.collector == false && CrownActive.permaCompleted == false) crown.SetActive(false);
    }

}
