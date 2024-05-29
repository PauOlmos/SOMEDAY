using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LimboCamera : MonoBehaviour
{
    public Transform orientation;
    public Transform objective;
    public Transform objectiveObj;
    public Rigidbody rb;

    public float rotationSpeed;
    public static float sensitivity;

    public float righthorizontal;

    public float rightJoystick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sensitivity = Settings.sensitivity;

        Vector3 viewDir = objective.position - new Vector3(transform.position.x, objective.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = InputManager.GetAxis("LeftHorizontal");
        float verticalInput = InputManager.GetAxis("LeftVertical");
        rightJoystick = InputManager.GetAxis("RightVertical");
        if (Time.timeScale != 0 && rightJoystick > 0.15f) gameObject.GetComponent<CinemachineFreeLook>().m_Heading.m_Bias += rightJoystick + sensitivity / 5.0f;
        if (Time.timeScale != 0 && rightJoystick < -0.15f) gameObject.GetComponent<CinemachineFreeLook>().m_Heading.m_Bias += rightJoystick - sensitivity / 5.0f;

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        gameObject.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = Settings.fov;
        
        objectiveObj.forward = Vector3.Slerp(objectiveObj.forward, inputDir, Time.deltaTime * rotationSpeed * sensitivity);
        
    }
}
