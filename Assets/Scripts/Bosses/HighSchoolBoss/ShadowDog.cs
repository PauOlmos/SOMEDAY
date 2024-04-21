using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowDog : MonoBehaviour
{
    public GameObject player;
    public Animator animator;
    public AnimationClip walk;
    public AnimationClip attack;
    public float attackTimer = 0.0f;
    public bool attacking = false;
    // Start is called before the first frame update
    void Start()
    {
        if (animator != null) animator.Play(walk.name);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) < 1.5f && attacking == false)
        {
            attacking = true;
            if(animator != null) animator.Play(attack.name);
        }
        if (attacking == true)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attack.length)
            {
                attackTimer = 0.0f;
                attacking = false;
                if (animator != null) animator.Play(walk.name);

            }
        }
    }
}
