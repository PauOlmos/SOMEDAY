using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class TutorialBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public Transform tutorialMap;
    public float radius;

    public bool canMove = true;
    public bool canAttack = false;
    public bool canDamageOnContact = false;

    public float circlesAttackCooldown = 0.0f;
    //
    public float circlesAttackChargeTimer = 0.0f;
    public float circlesAttackCharge = 0.5f;
    public LayerMask Ground;

    public GameObject[] circlesPrefabs = new GameObject[3];

    public int circlesCount = 0;

    public int phase = 0;
    public int maxPhases = 3;

    public NavMeshAgent agent;

    public GameObject proximityArea;
    public float proximityAreaTimer = 0.0f;

    public float farTimer = 0.0f;

    public float stunTimer = 0.0f;
    
    
    public enum MovementState
    {
        none, jump,
    }
    public enum AttackType
    {
        none, circles, proximity,distance,
    }

    public MovementState movementState = MovementState.none;
    public AttackType attackType = AttackType.circles;
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case 0:
                gameObject.GetComponent<EnemyHP>().canBeDamaged = true;
                if (canMove)
                {

                    switch (movementState)
                    {
                        case MovementState.none:

                            circlesAttackChargeTimer += Time.deltaTime;
                            if (circlesAttackChargeTimer > circlesAttackCharge)
                            {
                                circlesAttackChargeTimer = 0;
                                movementState = MovementState.jump;
                            }

                            break;
                        case MovementState.jump:
                            JumpMovement();
                            break;
                        default: break;
                    }
                }
                if (canAttack)
                {
                    switch (attackType)
                    {
                        case AttackType.none:

                            break;
                        case AttackType.circles:
                            int value = Random.Range(3, 6);
                            canAttack = CirclesAttack(value);

                            break;
                        default: break;
                    }
                }
                break;

            case 1:

                if (canMove && gameObject.GetComponent<EnemyHP>().stun == false)
                {
                    agent.destination = player.transform.position;
                    if(IsNear(3.0f))
                    {
                        agent.destination = gameObject.transform.position;
                        canAttack = true;
                        canMove = false;
                        attackType = AttackType.proximity;
                        farTimer = 0;

                        agent.speed = 3.5f;
                    }
                    else
                    {
                        farTimer += Time.deltaTime;
                        if(farTimer > 7.5f) agent.speed = 7;
                    }
                }
                if (canAttack && gameObject.GetComponent<EnemyHP>().stun == false)
                {
                    switch (attackType)
                    {
                        case AttackType.proximity:
                            proximityAreaTimer += Time.deltaTime;
                            if (proximityAreaTimer > 1.0f && proximityArea.activeInHierarchy == false)
                            {
                                proximityArea.SetActive(true);
                                proximityArea.tag = "Parryable";
                            }
                            if (proximityAreaTimer > 1.5f)
                            {
                                proximityArea.SetActive(false);
                                proximityArea.tag = "Parryable";
                            }
                            if (proximityAreaTimer > 2.5f)
                            {
                                proximityAreaTimer = 0.0f;
                                canAttack = false;
                                canMove = true;
                            }
                            break;

                        case AttackType.distance:

                            //if(Physics.Raycast(transform.position, Vector3.down, 2f + 0.2f, Ground));

                            break;
                    }
                }
               

                if(gameObject.GetComponent<EnemyHP>().stun == true)
                {
                    stunTimer += Time.deltaTime;
                    if(stunTimer > 3.0f)
                    {
                        stunTimer = 0.0f;
                        gameObject.GetComponent<EnemyHP>().stun = false;
                        gameObject.GetComponent<EnemyHP>().canBeDamaged = false;
                        canMove = true;
                    }
                    else
                    {
                        proximityAreaTimer = 0.0f;
                        canAttack = false;
                        canMove = false;
                    }
                }

                break;
        }
        
    }


    public bool IsNear(float distanceToAttack)
    {
        if(Vector3.Distance(player.transform.position, proximityArea.transform.position) < distanceToAttack) { return true; }
        else { return false; }
    }

    public bool CirclesAttack(int numCircles)
    {
        circlesAttackCooldown += Time.deltaTime;
        if(circlesCount == numCircles)
        {
            circlesCount = 0;
            movementState = MovementState.none; 
            circlesAttackCooldown = 0;
            canAttack = false;
            canMove = true;
            return false;
        }
        if (circlesAttackCooldown >= 2.5f)
        {
            int value = Random.Range(0, 4);
            Debug.Log(value);
            switch (value)
            {
                case 0:
                    GameObject circle = Instantiate(circlesPrefabs[value], gameObject.transform.position, Quaternion.identity);
                    circle.transform.Rotate(new Vector3(-90, 0, 0));
                    break;
                case 1:

                    CreateCircles(10, 360, circlesPrefabs[value], 2, 10, true);

                    break;
                case 2:
                    CreateCircles(10, 360, circlesPrefabs[value], 2, 10,false);
                    break;
            }
            circlesCount++;
            circlesAttackCooldown = 0.0f;
        }
        return true;
    }
    void CreateCircles(int numberOfCircles, float angularSeparation,GameObject circlePrefab, float radius, float force, bool discontinious)
    {
        for (int i = 0; i < numberOfCircles; i++)
        {
            // Calcular la posición alrededor del objeto principal
            float angle = i * (angularSeparation / numberOfCircles);
            Vector3 position = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;

            // Instanciar el objeto en la posición calculada
            GameObject circle = Instantiate(circlePrefab, position, Quaternion.identity);
            Quaternion forwardRotation = Quaternion.LookRotation(position - transform.position, Vector3.up);
           
            // Aplicar la rotación al objeto
            circle.transform.rotation = forwardRotation;
            // Aplicar fuerza hacia adelante al objeto
            Rigidbody rb = circle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (discontinious)
                {
                    circle.transform.Translate(new Vector3(0, (4.0f - 0.15f), 0));
                    rb.freezeRotation = true;
                }
                rb.AddForce(circle.transform.forward * force, ForceMode.Impulse);
            }
        }
    }
    public void JumpMovement()
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 10)
        {
            Debug.Log("Near");
            Vector3 posicionSalto = RandomPositionInCircle(tutorialMap.transform.position, radius);
            JumpToPosition(posicionSalto, Vector3.Distance(posicionSalto, gameObject.transform.position),0.5f);
            canMove = false;    
        }
        else
        {
            Debug.Log("Far" + Vector3.Distance(player.transform.position, gameObject.transform.position));
            JumpToPosition(player.transform.position, Vector3.Distance(player.transform.position, gameObject.transform.position), 0.5f);
            canMove = false;
        }
    }
    private Vector3 RandomPositionInCircle(Vector3 center, float radius)
    {
        // Calcula una posición aleatoria dentro de un círculo en el plano XZ
        float angulo = Random.value * Mathf.PI * 2;
        float distance = Mathf.Sqrt(Random.value) * radius;
        float x = center.x + distance * Mathf.Cos(angulo);
        float z = center.z + distance * Mathf.Sin(angulo);

        // Devuelve la posición aleatoria dentro del círculo, manteniendo la altura
        return new Vector3(x, center.y, z);
    }

    private void JumpToPosition(Vector3 targetPosition, float jumpDistance, float jumpForceMultiplier)
    {
        // Calcula la dirección hacia la posición objetivo
        Vector3 direccionSalto = targetPosition - transform.position;
        direccionSalto.y = 0f; // Asegura que el salto sea en el mismo plano horizontal

        // Normaliza la dirección del salto para mantener la misma velocidad
        Vector3 direccionNormalizada = direccionSalto.normalized;

        // Calcula la fuerza del salto utilizando la dirección normalizada y la distancia del salto
        Vector3 fuerzaSalto = direccionNormalizada * jumpDistance/ 2;

        // Aplica una fuerza hacia arriba para simular el salto
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpDistance * jumpForceMultiplier, ForceMode.VelocityChange);

        // Aplica la fuerza del salto en la dirección calculada
        gameObject.GetComponent<Rigidbody>().AddForce(fuerzaSalto, ForceMode.VelocityChange);
        Debug.Log("JumpToPlayer");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si colisiona con el suelo, detiene todas las fuerzas
        if (collision.gameObject.layer == 6)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            canAttack = true;
        }
    }

}
