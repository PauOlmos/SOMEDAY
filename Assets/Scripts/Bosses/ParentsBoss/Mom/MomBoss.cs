using UnityEngine;
using UnityEngine.AI;

public class MomBoss : MonoBehaviour
{

    public GameObject player;

    int phase = 0;

    public bool canAttack = false;

    public enum AttackType
    {
        circle, area, spikes, projectiles
    }

    public AttackType attackType;

    public float attackCooldownTimer = 0.0f;
    public float attackCooldownTime = 5.0f;

    public GameObject circularArea;
    public GameObject coneArea;
    public float delayTimer = 0.0f;
    public int numberOfCircles = 5;
    public float spikeRadius = 5.0f;
    public GameObject spikePrefab;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;

        switch (phase)
        {
            case 0:

                if (canAttack == true)
                {
                    switch (attackType)
                    {
                        case AttackType.circle:
                            delayTimer += Time.deltaTime;
                            if (delayTimer > 2.5f)
                            {

                                if (delayTimer > 2.75f)
                                {
                                    circularArea.tag = "Untagged";
                                    circularArea.layer = 0;
                                    circularArea.GetComponentInChildren<Renderer>().material.color = Color.green;
                                    circularArea.SetActive(false);
                                    delayTimer = 0.0f;
                                    canAttack = false;
                                }
                                else
                                {
                                    circularArea.layer = 7;
                                    circularArea.tag = "NonParryable";
                                }
                            }
                            else
                            {
                                if (delayTimer > 1.75f) gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                                Color auxColor = Color.Lerp(Color.green, Color.red, delayTimer / 1.5f);
                                auxColor.a = 0.3f;
                                circularArea.GetComponentInChildren<Renderer>().material.color = auxColor;
                            }

                            break;
                        case AttackType.area:

                            delayTimer += Time.deltaTime;
                            if (delayTimer > 2.5f)
                            {

                                if (delayTimer > 2.75f)
                                {
                                    coneArea.tag = "Untagged";
                                    coneArea.layer = 0;
                                    coneArea.GetComponentInChildren<Renderer>().material.color = Color.green;
                                    coneArea.SetActive(false);
                                    delayTimer = 0.0f;
                                    gameObject.GetComponent<NavMeshAgent>().angularSpeed = 120.0f;
                                    canAttack = false;
                                }
                                else
                                {
                                    coneArea.layer = 7;

                                    coneArea.tag = "NonParryable";
                                }
                            }
                            else
                            {
                                if (delayTimer > 1.75f)
                                {
                                    gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                                    gameObject.GetComponent<NavMeshAgent>().angularSpeed = 0.0f;
                                }
                                Color auxColor = Color.Lerp(Color.green, Color.red, delayTimer / 1.5f);
                                auxColor.a = 0.3f;
                                coneArea.GetComponentInChildren<Renderer>().material.color = auxColor;
                            }

                            break;
                        case AttackType.spikes:

                            delayTimer += Time.deltaTime;
                            if(delayTimer < 3)
                            {
                                gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                            }
                            else
                            {
                                delayTimer = 0.0f;
                                canAttack = false;
                            }

                            break;
                    }
                }
                else
                {
                    attackCooldownTimer += Time.deltaTime;
                    if (attackCooldownTimer >= attackCooldownTime)
                    {
                        gameObject.GetComponent<NavMeshAgent>().speed = 3.5f;
                        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 3.0f)
                        {
                            attackCooldownTimer = 0;
                            canAttack = true;
                            int value = Random.Range(0, 3);
                            switch (value)
                            {

                                case 0: 

                                    attackType = AttackType.circle;
                                    circularArea.SetActive(true);

                                    break;
                                case 1: 
                                    
                                    attackType = AttackType.area;
                                    coneArea.SetActive(true);

                                    break;
                                case 2: 
                                    attackType = AttackType.spikes;
                                    for (int i = 0; i < numberOfCircles; i++)
                                    {
                                        // Calcular la posición alrededor del objeto principal
                                        float angle = i * (360 / numberOfCircles);
                                        Vector3 position = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * spikeRadius;

                                        // Instanciar el objeto en la posición calculada
                                        Instantiate(spikePrefab, position, Quaternion.identity);
                                    }
                                    break;

                            }

                        }
                    }
                }
                break;
        }
    }
}
