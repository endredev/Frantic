using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab = null;
    [SerializeField] GameObject shield = null;
    [SerializeField] GameObject laserShotFrom = null;
    [SerializeField] int maxHealth = 100;
    [SerializeField] float moveSpeed = 14f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
