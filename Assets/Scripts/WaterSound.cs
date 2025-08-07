using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class WaterSound : MonoBehaviour
{
    [SerializeField] AudioClip WaterSFX;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(WaterSFX, Camera.main.transform.position);
        }
    }
    
}
