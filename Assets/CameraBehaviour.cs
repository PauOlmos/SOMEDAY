using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public Transform boss;

    public float rotationSpeed;

    public float righthorizontal;
    public float rightvertical;

    public bool lockOnBoss = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("LeftHorizontal");
        float verticalInput = Input.GetAxis("LeftVertical");
        righthorizontal = Input.GetAxis("RightHorizontal");
        rightvertical = Input.GetAxis("RightVertical");

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        if (Input.GetButtonDown("LockBoss"))
        {
            lockOnBoss = !lockOnBoss;
        }

        if (lockOnBoss)
        {
            Debug.Log(Angulo(player, boss));
            gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxValue = Angulo(player, boss) + 45;
            gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_MinValue = Angulo(player, boss) - 45;
            gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_Wrap = false;
        }
        else
        {
            gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxValue = 180;
            gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_MinValue = -180;
            gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_Wrap = true;
        }

    }

    float Angulo(Transform player, Transform Boss)
    {

        Vector3 direccion = player.forward;

        // Vector que va desde el objeto que mira hacia el otro objeto
        Vector3 vectorAOtroObjeto = Boss.position - player.position;

        // Calcula el ángulo entre los dos vectores
        return Vector3.Angle(direccion, vectorAOtroObjeto);

    }
}
