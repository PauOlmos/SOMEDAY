using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CloneDash : MonoBehaviour
{
    public bool seeking = false;
    public float timer = 0.0f;
    public float timeToDash;
    public Vector3 direction;
    public GameObject player;
    public float speed;
    public Transform streets;
    public Animator animator;
    public AnimationClip dash;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > timeToDash && seeking == false)
        {
            seeking = true;
            animator.Play(dash.name);
            direction = (player.transform.position - gameObject.transform.position);
            gameObject.transform.SetParent(streets);
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }

        if (seeking) gameObject.GetComponent<NavMeshAgent>().destination = transform.position + direction.normalized * 4 * direction.magnitude;

    }
}
