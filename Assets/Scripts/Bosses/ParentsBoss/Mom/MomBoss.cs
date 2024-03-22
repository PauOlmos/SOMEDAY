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
                            value = 1;
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
                                case 2: attackType = AttackType.spikes; break;

                            }

                        }
                    }
                }
                break;
        }
    }
}
