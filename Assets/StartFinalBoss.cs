using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public float timer = 0.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameObject.transform.Translate(Vector3.up * Time.deltaTime * 22.5f);
        if(timer > 15.0f)
        {
            gameObject.GetComponent<FinalBoss>().enabled = true;
            Destroy(gameObject.GetComponent<StartFinalBoss>());
        }
    }
}
