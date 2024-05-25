using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTerrain : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject continousCircle;
    public FinalBoss finalBoss;
    public float cooldown = 0.0f;
    public bool hasTouchedGround = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = Vector3.zero;
        if (hasTouchedGround) cooldown += Time.deltaTime;
        if(cooldown > 45.0f)
        {
            cooldown = 0.0f;
            hasTouchedGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6 && finalBoss.phase2State != FinalBoss.Phase2State.climb && hasTouchedGround == false)
        {
            hasTouchedGround = true;
            Debug.Log("patata");
            CameraBehaviour.ActivateCameraShake(6.0f, 3.5f);
            GameObject impactArea = Instantiate(continousCircle, new Vector3(gameObject.transform.position.x, -220, gameObject.transform.position.z), Quaternion.identity);
            impactArea.transform.Rotate(new Vector3(-90, 0, 0));
            impactArea.GetComponent<Expand>().expandMultiplier = 15.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && finalBoss.phase2State != FinalBoss.Phase2State.climb && hasTouchedGround == false)
        {
            hasTouchedGround = true;

            Debug.Log("patata");

            CameraBehaviour.ActivateCameraShake(6.0f, 3.5f);
            GameObject impactArea = Instantiate(continousCircle, new Vector3 (gameObject.transform.position.x, -220, gameObject.transform.position.z), Quaternion.identity);
            impactArea.transform.Rotate(new Vector3(-90, 0, 0));
            impactArea.GetComponent<Expand>().expandMultiplier = 15.0f;
        }
    }

}
