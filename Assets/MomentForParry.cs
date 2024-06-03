using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentForParry : MonoBehaviour
{
    // Start is called before the first frame update

    public TutorialMessages tutorialMessages;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Parryable" && tutorialMessages != null) tutorialMessages.momentForParry = true;
    }

}
