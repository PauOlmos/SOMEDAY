using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public float timer = 0.0f;
    public GameObject footTrigger1;
    public GameObject footTrigger2;
    void Start()
    {
        CameraBehaviour.ActivateCameraShake(4.0f, 15.0f);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameObject.transform.Translate(Vector3.up * Time.deltaTime * 22.5f);
        footTrigger1.transform.Translate(Vector3.up * Time.deltaTime * 22.5f);
        footTrigger2.transform.Translate(Vector3.up * Time.deltaTime * 22.5f);
        if(timer > 15.0f)
        {
            gameObject.GetComponent<FinalBoss>().enabled = true;
            Destroy(gameObject.GetComponent<StartFinalBoss>());
        }
    }
}
