using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMusic : MonoBehaviour
{
    [SerializeField] public AudioSource musicSource;
    void Awake()
    {
        int numBackMusic = FindObjectsOfType<BackMusic>().Length;
        if (numBackMusic > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        musicSource.loop = true;
        musicSource.Play();

    }
    
    public void ResetBackMusic()
    {
        Destroy(gameObject);
    }
}
