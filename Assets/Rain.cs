using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject area;
    public GameObject lightning;
    public AudioClip thunder;
    public float globalTimer = 0.0f;
    public float globalCooldown;

    public float burstTimer = 0.0f;
    public bool readyToStorm = true;
    public int maxBurstNum = 0;
    public int burstNum = 0;
    float[] burstCooldown = { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f };
    void Start()
    {
        globalCooldown = Random.Range(15, 30);
    }

    // Update is called once per frame
    void Update()
    {
        globalTimer += Time.deltaTime;
        if (globalTimer > globalCooldown && readyToStorm)
        {
            readyToStorm = false;
            maxBurstNum = Random.Range(1, 5);
            for(int i = 0; i < maxBurstNum; i++)
            {
                burstCooldown[i] = Random.Range(i+3, i+4) + Random.Range(-1,1) / 100;
            }
        }
        if (!readyToStorm) { 
            RainEvent(maxBurstNum);
        } 
    }

    public void RainEvent(int numBursts)
    {
        burstTimer += Time.deltaTime;
        for(int i = 0; i < numBursts; i++)
        {
            if(i <= burstNum && burstTimer > burstCooldown[i])
            {
                Vector3 posicion = new Vector3(Random.Range(-area.transform.localScale.x / 2, area.transform.localScale.x / 2),
                                           Random.Range(-area.transform.localScale.y / 2, area.transform.localScale.y / 2),
                                           Random.Range(-area.transform.localScale.z / 2, area.transform.localScale.z / 2));

                // Ajustar la posición al espacio del volumen
                posicion += area.transform.position;
                GameObject light = Instantiate(lightning, area.transform.position, Quaternion.identity);
                light.GetComponent<DieByTime>().deathTime = Random.Range(0.15f, 0.5f);
                burstNum++;
            }
        }

        if(burstNum >= maxBurstNum)
        {
            burstNum = 0;
            burstTimer = 0.0f;
            readyToStorm = true;
            globalTimer = 0.0f;
            if(thunder != null)BossManager.SoundEffect(thunder);
        }

    }

}
