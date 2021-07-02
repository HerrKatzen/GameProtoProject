using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ParticleCollision : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    public void OnParticleCollision() {
        StartCoroutine(OnHitCO());
    }

    IEnumerator OnHitCO()
    {
        yield return new WaitForSeconds(0.2f);
        cam.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }
}
