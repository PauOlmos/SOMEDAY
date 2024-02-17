using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
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
    public enum MovementState
    {
        none, jump,
    }
    public enum AttackType
    {
        none, circles
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
        if (canMove)
        {
            switch (movementState)
            {
                case MovementState.none:

                    circlesAttackChargeTimer += Time.deltaTime;
                    if(circlesAttackChargeTimer > circlesAttackCharge)
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

                    InstanciarObjetos(10, 360, circlesPrefabs[value], 2, 10, true);

                    break;
                case 2:
                    InstanciarObjetos(10, 360, circlesPrefabs[value], 2, 10,false);
                    break;
            }
            circlesCount++;
            circlesAttackCooldown = 0.0f;
        }
        return true;
    }
    /*void InstanciarObjetos(int numeroDeObjetos, float separacionAngular, GameObject objetoBase, float radio, float fuerzaHaciaAdelante)
    {
        for (int i = 0; i < numeroDeObjetos; i++)
        {
            // Calcular la posición alrededor del objeto principal
            float angulo = i * (separacionAngular / numeroDeObjetos);
            Vector3 posicion = transform.position + Quaternion.Euler(0, angulo, 0) * Vector3.forward * radio;

            // Instanciar el objeto en la posición calculada
            GameObject nuevoObjeto = Instantiate(objetoBase, posicion, Quaternion.identity);

            // Aplicar fuerza hacia adelante al objeto
            Rigidbody rb = nuevoObjeto.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(nuevoObjeto.transform.forward * fuerzaHaciaAdelante, ForceMode.Impulse);
            }
        }
    }*/
    void InstanciarObjetos(int numberOfCircles, float angularSeparation,GameObject circlePrefab, float radius, float force, bool discontinious)
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
            JumpToPosition(posicionSalto, Vector3.Distance(posicionSalto, gameObject.transform.position));
            canMove = false;    
        }
        else
        {
            Debug.Log("Far" + Vector3.Distance(player.transform.position, gameObject.transform.position));
            JumpToPosition(player.transform.position, Vector3.Distance(player.transform.position, gameObject.transform.position));
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

    private void JumpToPosition(Vector3 targetPosition, float jumpDistance)
    {
        // Calcula la dirección hacia la posición objetivo
        Vector3 direccionSalto = targetPosition - transform.position;
        direccionSalto.y = 0f; // Asegura que el salto sea en el mismo plano horizontal

        // Normaliza la dirección del salto para mantener la misma velocidad
        Vector3 direccionNormalizada = direccionSalto.normalized;

        // Calcula la fuerza del salto utilizando la dirección normalizada y la distancia del salto
        Vector3 fuerzaSalto = direccionNormalizada * jumpDistance/ 2;

        // Aplica una fuerza hacia arriba para simular el salto
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpDistance/ 2, ForceMode.VelocityChange);

        // Aplica la fuerza del salto en la dirección calculada
        gameObject.GetComponent<Rigidbody>().AddForce(fuerzaSalto, ForceMode.VelocityChange);
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
