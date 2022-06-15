using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    new ParticleSystem particleSystem;
    void OnEnable() => particleSystem.Play();
    public void DisableGameObject() => gameObject.SetActive(false);
}
