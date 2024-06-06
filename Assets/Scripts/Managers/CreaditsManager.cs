using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreaditsManager : MonoBehaviour
{

    public GameObject[] creditsArray;

    public int currentText = 0;

    public float creditsTimer = 0.0f;

    public bool ending = false;

    public float creditSpeed;

    public Image black;

    public Camera cam;

    public float camRotatingTimer = 0.0f;

    public int currentStatue = 1;

    public AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        currentText += ActivateCredit(currentText);
    }

    // Update is called once per frame
    void Update()
    {
        CamRotation();

        creditsTimer += Time.deltaTime;

        if (black.color.a > 0)  black.color = new Color(0, 0, 0, black.color .a - Time.deltaTime / 15);

        if (currentText >= creditsArray.Length) ending = true;

        if(creditsTimer > 5.0f && ending == false)
        {
            currentText += ActivateCredit(currentText);
            creditsTimer = 0.0f;
        }

        for(int i = 0; i < creditsArray.Length; i++)
        {
            if(creditsArray[i] != null) if (creditsArray[i].activeInHierarchy == true) creditsArray[i].transform.Translate(0, creditSpeed * Time.deltaTime, 0);
        }
        if (creditsTimer > 13.0f) music.volume -= Time.deltaTime / 1.5f;
        if (creditsTimer > 14.5f) SceneManager.LoadScene(0);

    }

    public void CamRotation()
    {
        camRotatingTimer += Time.deltaTime;

        if (cam.transform.eulerAngles.y < (currentStatue - 1) * 60) cam.transform.Rotate(0, Time.deltaTime * 100, 0);

        if(camRotatingTimer > 12.5f * currentStatue)
        {
            //cam.transform.Rotate(0, 60, 0);
            currentStatue++;
        }

    }

    public int ActivateCredit(int num)
    {
        creditsArray[num].SetActive(true);
        creditsArray[num].AddComponent<DieByTime>();
        creditsArray[num].GetComponent<DieByTime>().deathTime = 15;
        return 1;
    }

}
