using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] float projectileSpeed = 10f;

    void Update()
    {
        if (target == null) return;

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.position;
        }

        // Get the middle point of the target
        return target.position + Vector3.up * targetCapsule.height / 2;
    }
}
