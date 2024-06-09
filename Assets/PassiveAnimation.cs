using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAnimation : MonoBehaviour
{

    public GameObject otherPassive;

    public float baseMultiplier = 0.5f;
    public float auxMultiplier = 0.0f;

    public enum State
    {
        expanding, shrinking
    }

    public State state = State.expanding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        baseMultiplier += auxMultiplier;
        auxMultiplier = 0;

        switch (state)
        {
            case State.expanding:

                gameObject.GetComponent<RectTransform>().localScale += Time.deltaTime * Vector3.one * baseMultiplier;
                //otherPassive.GetComponent<RectTransform>().localScale += Time.deltaTime * Vector3.one * baseMultiplier;

                if (gameObject.GetComponent<RectTransform>().localScale.x > 1.1f)
                {
                    state = State.shrinking;
                }

                break;
            case State.shrinking:

                gameObject.GetComponent<RectTransform>().localScale -= Time.deltaTime * Vector3.one * baseMultiplier;
                //otherPassive.GetComponent<RectTransform>().localScale -= Time.deltaTime * Vector3.one * baseMultiplier;

                if (gameObject.GetComponent<RectTransform>().localScale.x < 0.9f)
                {
                    state = State.expanding;
                }

                break;
        
        }
        baseMultiplier = 0.75f;

    }
}
