using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePortal : MonoBehaviour
{
    public SreetPortal portal;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "PlayerModel")
        {
            portal.enabled = true;
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.name == "PlayerModel")
        {
            portal.enabled = true;
            Destroy(gameObject);

        }
    }
}
