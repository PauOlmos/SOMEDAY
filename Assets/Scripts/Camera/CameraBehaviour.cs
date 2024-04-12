using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;


public class CameraBehaviour : MonoBehaviour
{
    public Transform orientation;
    public Transform objective;
    public Transform objectiveObj;
    public Rigidbody rb;

    public GameObject boss;

    public float rotationSpeed;
    public static float sensitivity;

    public float righthorizontal;

    public Transform lockPosition;
    private float shakeTimer = 0.0f;
    public enum cameraState
    {
        onObjective, onBoss, onPosition,
    }

    public cameraState camState;
    public Image bosslockDot;
    public Camera mainCamera;

    public bool cameraShake = false;

    GameObject player;
    PlayerMovement pMov;
    PassiveAbility passiveAbility;

    private GameObject closestEnemy;
    private GameObject[] allBosses;
    public int numBosses;
    int currentLockedBoss = 0;

    public float rightJoystick;

    public bool readyBoss = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camState = cameraState.onObjective;
        bosslockDot.gameObject.SetActive(false);
        player = GameObject.Find("Player");
        pMov = player.GetComponent<PlayerMovement>();
        passiveAbility = player.GetComponent<PassiveAbility>();
        boss = FindClosestEnemy("Boss");
        numBosses = CountBosses();
        allBosses = AllBosses();
    }


    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 1;
        numBosses = CountBosses();
        allBosses = AllBosses();
        sensitivity = Settings.sensitivity;

        Vector3 viewDir = objective.position - new Vector3(transform.position.x, objective.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("LeftHorizontal");
        float verticalInput = Input.GetAxis("LeftVertical");
        righthorizontal = Input.GetAxis("L2");
        rightJoystick = Input.GetAxis("RightVertical");
        //Debug.Log("RightJoystick" + rightJoystick);
        if(Time.timeScale != 0 && rightJoystick > 0.15f && readyBoss) gameObject.GetComponent<CinemachineFreeLook>().m_Heading.m_Bias += rightJoystick + sensitivity / 15.0f; 
        if(Time.timeScale != 0 && rightJoystick < -0.15f && readyBoss) gameObject.GetComponent<CinemachineFreeLook>().m_Heading.m_Bias += rightJoystick - sensitivity / 15.0f; 

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        gameObject.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = Settings.fov;

        if (pMov.pStatus != PlayerMovement.playerState.dashing)
        {
            objectiveObj.forward = Vector3.Slerp(objectiveObj.forward, inputDir, Time.deltaTime * rotationSpeed * sensitivity);
        }

        if (Input.GetButtonDown("LockBoss"))
        {
            //CameraOnLock(lockPosition, boss);
            //cameraShake = true;
            if(numBosses == 1)
            {
                if (camState == cameraState.onBoss)
                {
                    camState = cameraState.onObjective;
                }
                else if (camState == cameraState.onObjective)
                {
                    camState = cameraState.onBoss;
                }
            }
            else if (numBosses > 1)
            {
                switch (currentLockedBoss)
                {
                    case 0:
                        boss = allBosses[0];
                        currentLockedBoss = 1;
                        camState = cameraState.onBoss;

                        break;
                        case 1:
                        boss = allBosses[1];
                        currentLockedBoss = 2;
                        camState = cameraState.onBoss;

                        break;
                        case 2:
                        camState = cameraState.onObjective;
                        currentLockedBoss = 0;
                        break;
                }
            }            

        }

        bosslockDot.gameObject.SetActive(camState == cameraState.onBoss);

        switch (camState)
        {
            case cameraState.onObjective:
                CameraLimits(180, 0, true);
                AbilityAttack();
                break;
            case cameraState.onBoss:
                AbilityAttack();
                if(boss != null) CameraLimits(45, Angulo(objective, boss.transform), false);
                if(boss != null) bosslockDot.transform.position = mainCamera.GetComponent<Camera>().WorldToScreenPoint(boss.transform.position);
                break;
            case cameraState.onPosition:

                CameraLimits(0, 0, false);

                gameObject.GetComponent<CinemachineFreeLook>().Follow = lockPosition;
                gameObject.GetComponent<CinemachineFreeLook>().LookAt = objective.transform;

                break;

        }

        //if (cameraShake) CameraShake(3.0f, 0.25f);

    }

    float Angulo(Transform objective, Transform Boss)
    {

        Vector3 direccion = objective.forward;

        // Vector que va desde el objeto que mira hacia el otro objeto
        Vector3 vecDic = Boss.position - objective.position;

        // Calcula el ángulo entre los dos vectores
        return Vector3.Angle(direccion, vecDic);

    }

    public void CameraOnLock(Transform position, Transform lookAt)
    {
        if (lookAt != null) objective = lookAt;
        if (position != null) lockPosition = position;
        camState = cameraState.onPosition;
    }

    private void CameraLimits(int maxmin, float direction, bool wrap)
    {
        gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxValue = direction + maxmin;
        gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_MinValue = direction - maxmin;
        gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_Wrap = wrap;
    }

    public void AbilityAttack()
    {
        if (pMov.grounded && pMov.pStatus != PlayerMovement.playerState.dashing)
        {
            if (Input.GetAxis("R2") > -1 && passiveAbility.isCharged && passiveAbility.passive == PassiveAbility.passiveType.shoot)
            {
                gameObject.GetComponent<CinemachineFreeLook>().m_YAxis.m_InputAxisName = "R2";
                camState = cameraState.onBoss;
            }
            else
            {
                gameObject.GetComponent<CinemachineFreeLook>().m_YAxis.m_InputAxisName = "L2";
            }
        }
        else
        {
            gameObject.GetComponent<CinemachineFreeLook>().m_YAxis.m_InputAxisName = null;
        }

    }

    public void CameraShake(float intensity, float time)
    {
        shakeTimer += Time.deltaTime;
        if (shakeTimer < time) gameObject.GetComponent<CinemachineFreeLook>().GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        else
        {
            gameObject.GetComponent<CinemachineFreeLook>().GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            shakeTimer = 0;
            cameraShake = false;
        }
    }

    public GameObject FindClosestEnemy(string enemyTag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float closestDistance = Mathf.Infinity;
        closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    public int CountBosses()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        return bosses.Length;
    }

    public GameObject[] AllBosses()
    {
        int numActiveBosses = 0;
        GameObject[] baux = GameObject.FindGameObjectsWithTag("Boss");
        GameObject[] b = new GameObject[2];
        for(int i = 0; i < baux.Length; i++)
        {
            if (baux[i].activeInHierarchy == true) b[i] = baux[i];
        }
        return b;
        
    }
}
