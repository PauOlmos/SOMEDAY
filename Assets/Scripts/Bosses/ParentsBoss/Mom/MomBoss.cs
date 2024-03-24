using UnityEngine;
using UnityEngine.AI;

public class MomBoss : MonoBehaviour
{

    public GameObject player;

    public int phase = 0;

    public bool canAttack = false;

    public enum AttackType
    {
        circle, area, spikes, greatArea
    }

    public AttackType attackType;

    public float attackCooldownTimer = 0.0f;
    public float attackCooldownTime = 2.0f;

    public GameObject circularArea;
    public GameObject coneArea;
    public float delayTimer = 0.0f;
    public float delayTime = 2.5f;
    public int numberOfCircles = 5;
    public float spikeRadius = 5.0f;
    public GameObject spikePrefab;

    public float movSpeed = 4.0f;

    public int maxNumAttacks = 3;

    public GameObject greatAttackArea;
    public GameObject greatAttackArea1;
    public GameObject greatAttackArea2;

    public bool teleportingToPlayer = false;
    public bool startTeleporting = false;

    public enum TeleportingState
    {
        reduce, move, expand
    }

    public TeleportingState telState = TeleportingState.reduce;
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
        gameObject.GetComponent<EnemyHP>().canBeDamaged = true;


        if (canAttack == true)
        {
            switch (attackType)
            {
                case AttackType.circle:
                    delayTimer += Time.deltaTime;
                    if (delayTimer > delayTime)
                    {

                        if (delayTimer > (delayTime + delayTime / 10))
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
                        if (delayTimer > (delayTime - delayTime / 20)) gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                        Color auxColor = Color.Lerp(Color.green, Color.red, delayTimer / 1.5f);
                        auxColor.a = 0.3f;
                        circularArea.GetComponentInChildren<Renderer>().material.color = auxColor;
                    }

                    break;
                case AttackType.area:

                    delayTimer += Time.deltaTime;
                    if (delayTimer > delayTime)
                    {
                        if (delayTimer > (delayTime + delayTime / 10))
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
                        if (delayTimer > (delayTime - delayTime / 20))
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
                    if (delayTimer < 3)
                    {
                        gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                    }
                    else
                    {
                        delayTimer = 0.0f;
                        canAttack = false;
                    }

                    break;
                case AttackType.greatArea:

                    gameObject.GetComponent<NavMeshAgent>().speed = 0;
                    gameObject.GetComponent<NavMeshAgent>().angularSpeed = 0;
                    delayTimer += Time.deltaTime;
                    if (delayTimer > delayTime)
                    {
                        if (delayTimer > (delayTime * 4))
                        {
                            greatAttackArea1.tag = "Untagged";
                            greatAttackArea2.tag = "Untagged";
                            greatAttackArea1.layer = 0;
                            greatAttackArea2.layer = 0;
                            greatAttackArea1.GetComponent<Renderer>().material.color = Color.green;
                            greatAttackArea2.GetComponent<Renderer>().material.color = Color.green;
                            greatAttackArea.SetActive(false);
                            delayTimer = 0.0f;
                            gameObject.GetComponent<NavMeshAgent>().angularSpeed = 120.0f;
                            canAttack = false;
                        }
                        else
                        {
                            greatAttackArea1.layer = 7;
                            greatAttackArea2.layer = 7;
                            greatAttackArea.transform.RotateAround(gameObject.transform.position, Vector3.up, Time.deltaTime * 50);
                            greatAttackArea1.tag = "NonParryable";
                            greatAttackArea2.tag = "NonParryable";
                        }
                    }
                    else
                    {
                        if (delayTimer > (delayTime - delayTime / 20))
                        {
                            gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                            gameObject.GetComponent<NavMeshAgent>().angularSpeed = 0.0f;
                        }
                        Color auxColor = Color.Lerp(Color.green, Color.red, delayTimer / 1.5f);
                        auxColor.a = 0.3f;
                        greatAttackArea1.GetComponent<Renderer>().material.color = auxColor;
                        greatAttackArea2.GetComponent<Renderer>().material.color = auxColor;
                    }

                    break;
            }
        }
        else
        {
            attackCooldownTimer += Time.deltaTime;
            if (attackCooldownTimer >= attackCooldownTime)
            {
                gameObject.GetComponent<NavMeshAgent>().speed = movSpeed;
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 3.0f && teleportingToPlayer == false)
                {
                    startTeleporting = false;
                    attackCooldownTimer = 0;
                    canAttack = true;
                    int value = Random.Range(phase, maxNumAttacks);
                    switch (value)
                    {

                        case 2:

                            attackType = AttackType.circle;
                            circularArea.SetActive(true);

                            break;
                        case 1:

                            attackType = AttackType.area;
                            coneArea.SetActive(true);

                            break;
                        case 0:

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

                        default:

                            attackType = AttackType.greatArea;
                            greatAttackArea.SetActive(true);

                            break;
                    }

                }
                else if (phase == 1)
                {
                    if ((Vector3.Distance(gameObject.transform.position, player.transform.position) > 15.0f) || startTeleporting == true){
                        startTeleporting = true;
                        teleportingToPlayer = TeleportToPlayer();
                    }
                    
                }
            }
        }
    }

    bool TeleportToPlayer()
    {

        switch (telState)
        {
            case TeleportingState.reduce:

                gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                if (gameObject.transform.localScale.x > 0) gameObject.transform.localScale -= Vector3.one * Time.deltaTime;

                if(gameObject.transform.localScale.x <= 0)
                {
                    gameObject.transform.localScale = Vector3.zero;
                    telState = TeleportingState.move;
                }

                break;
            case TeleportingState.move:

                gameObject.transform.position = GeneratePointWithoutCollision();
                telState = TeleportingState.expand;

                break;
            case TeleportingState.expand:

                if(gameObject.transform.localScale.x < 1.0f)
                {
                    gameObject.transform.localScale += Vector3.one * Time.deltaTime * 2;
                }
                else
                {
                    gameObject.transform.localScale = Vector3.one;
                    telState = TeleportingState.reduce;
                    return false;
                }

                break;
        }

        return true;
    }

    Vector3 GeneratePointWithoutCollision()
    {
        Vector3 center = player.transform.position;
        float radius = player.GetComponentInChildren<BoxCollider>().bounds.size.magnitude;

        Vector3 randomPoint = Vector3.zero;
        bool pointFound = false;

        while (!pointFound)
        {
            randomPoint = center + GetRandomPointInSphere(3.0f);
            Collider[] colliders = Physics.OverlapSphere(randomPoint, radius);

            bool collisionDetected = false;
            foreach (Collider col in colliders)
            {
                if (col.tag == ("World"))
                {
                    collisionDetected = true;
                    break;
                }
            }

            if (!collisionDetected)
            {
                pointFound = true;
            }
        }
        return randomPoint;
    }

    Vector3 GetRandomPointInSphere(float radius)
    {
        float angle = Random.value * Mathf.PI * 2;
        float distance = Mathf.Sqrt(Random.value) * radius;

        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;

        return new Vector3(x, 0, y);
    }

}
