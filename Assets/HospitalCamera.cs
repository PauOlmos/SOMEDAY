using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HospitalCamera : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform LookAtHeadPlayer;

    public Transform[] camPath;

    public int currentCamPos = 0;

    public Image white;

    public float transitionTimer = 1;
    void Start()
    {
        gameObject.transform.position = camPath[0].position;
        gameObject.transform.LookAt(LookAtHeadPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(LookAtHeadPlayer);

        transitionTimer -= Time.deltaTime /5;
        white.color = new Color(1, 1, 1, transitionTimer);
        if (currentCamPos + 1 < camPath.Length)
        {
            Vector3 direction = camPath[currentCamPos + 1].localPosition - gameObject.transform.localPosition;
            gameObject.transform.position += direction.normalized * Time.deltaTime;

            if (Vector3.Distance(camPath[currentCamPos + 1].localPosition, gameObject.transform.localPosition) < 0.55f) currentCamPos++;
        }
    }
}