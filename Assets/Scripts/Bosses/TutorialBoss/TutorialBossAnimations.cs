using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossAnimations : MonoBehaviour
{
    // Start is called before the first frame update

    public enum AnimationsState
    {
        idle, attack, recieveDamage, stun
    }

    public AnimationsState animState = AnimationsState.idle;

    public Material yellow;
    public Material red;
    public Material black;
    public Material white;

    public GameObject orbe1;
    public GameObject orbe2;
    public GameObject orbe3;
    public GameObject orbe4;

    public GameObject boss;

    public GameObject hand1;
    public GameObject hand2;

    public GameObject handPosition1;
    public GameObject handPosition2;

    public GameObject sword1;
    public GameObject sword2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.GetComponent<TutorialBoss>() != null)
        {
            switch (boss.GetComponent<TutorialBoss>().phase)
            {
                case 0:

                    switch (animState)
                    {
                        case AnimationsState.idle:

                            orbe1.GetComponent<Renderer>().material = black;
                            orbe2.GetComponent<Renderer>().material = black;
                            orbe3.GetComponent<Renderer>().material = black;
                            orbe4.GetComponent<Renderer>().material = black;

                            break;
                        case AnimationsState.attack:

                            orbe1.GetComponent<Renderer>().material = red;
                            orbe2.GetComponent<Renderer>().material = red;
                            orbe3.GetComponent<Renderer>().material = red;
                            orbe4.GetComponent<Renderer>().material = red;

                            break;

                        case AnimationsState.recieveDamage:

                            orbe1.GetComponent<Renderer>().material = white;
                            orbe2.GetComponent<Renderer>().material = white;
                            orbe3.GetComponent<Renderer>().material = white;
                            orbe4.GetComponent<Renderer>().material = white;

                            break;
                    }

                    break;

                case 1:

                    switch (animState)
                    {
                        case AnimationsState.idle:
                            orbe1.GetComponent<Renderer>().material = black;
                            orbe2.GetComponent<Renderer>().material = black;
                            orbe3.GetComponent<Renderer>().material = black;
                            orbe4.GetComponent<Renderer>().material = black;

                            break;
                        case AnimationsState.attack:

                            orbe1.GetComponent<Renderer>().material = red;
                            orbe2.GetComponent<Renderer>().material = red;
                            orbe3.GetComponent<Renderer>().material = red;
                            orbe4.GetComponent<Renderer>().material = red;

                            if (hand1.transform.eulerAngles.x < 100 || hand1.transform.eulerAngles.x > 359) 
                            { 
                                hand1.transform.Rotate(new Vector3(Time.deltaTime * 90.0f, 0, 0));
                                hand2.transform.Rotate(new Vector3(Time.deltaTime * 90.0f, 0, 0));
                            }


                            break;

                        case AnimationsState.recieveDamage:

                            orbe1.GetComponent<Renderer>().material = white;
                            orbe2.GetComponent<Renderer>().material = white;
                            orbe3.GetComponent<Renderer>().material = white;
                            orbe4.GetComponent<Renderer>().material = white;

                            break;
                        case AnimationsState.stun:


                            hand1.transform.localEulerAngles = Vector3.zero;
                            hand2.transform.localEulerAngles = Vector3.zero;

                            hand1.transform.Rotate(-66, 0, 0);
                            hand2.transform.Rotate(66, 0, 0);

                            orbe1.GetComponent<Renderer>().material = yellow;
                            orbe2.GetComponent<Renderer>().material = yellow;
                            orbe3.GetComponent<Renderer>().material = yellow;
                            orbe4.GetComponent<Renderer>().material = yellow;

                            break;
                    }


                    break;
                case 2:



                    break;
            }
        }
        
        
    }
}
