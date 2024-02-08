using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataToStore : MonoBehaviour
{
    // Start is called before the first frame update

    public int numArchive;
    public int maxLevel;
    public int maxHp;
    public float charge;

    public enum Dificulty
    {
        easy = 0, hard = 1, nightmare = 2,
    } 

    public Dificulty dificulty;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }
}
