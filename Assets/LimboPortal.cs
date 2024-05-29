using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LimboPortal : MonoBehaviour
{
    // Start is called before the first frame update

    public float transitionTimer = 0.0f;
    public Image white;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transitionTimer > 5.0f)
        {
            SceneManager.LoadScene(4);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "PlayerModel")
        {
            transitionTimer += Time.deltaTime;
            white.color = new Color(1, 1, 1, transitionTimer / 5);
            //CameraShake¿?
        }
    }
    private void OnTriggerExit(Collider other)
    {
        transitionTimer = 0.0f;
        white.color = new Color(1, 1, 1, 0);

    }
}
