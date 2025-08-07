using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    public void Shake()
    {
        impulseSource.GenerateImpulse();
    }


}
