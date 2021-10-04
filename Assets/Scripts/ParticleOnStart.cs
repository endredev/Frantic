using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnStart : MonoBehaviour
{
    ParticleSystem particles;
    bool isStopped;
    bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        {
            GetComponent<ParticleSystem>().Stop();
            isStopped = true;
        }
        else if (!isStarted && GameSession.GetGameStarted())
        {
            GetComponent<ParticleSystem>().Play();
            isStarted = true;
        }
    }
}
