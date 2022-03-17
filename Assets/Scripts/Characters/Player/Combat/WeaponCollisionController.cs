using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionController : MonoBehaviour
{

    public event Action<GameObject> onHit;
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.TryGetComponent<Damagable>(out Damagable damagable)) != false)
        {
            GameObject target = other.gameObject;
            Debug.Log("Colliding with: " + target.name);
            onHit?.Invoke(target);
        }
    }
}
