using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    private Rigidbody _rb;
    private void Awake()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            _rb = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Failed to get Rigidbody on" + gameObject.name.ToString());
        }
    }

    void Start()
    {
        _rb.AddForce(transform.forward * 10.0f, ForceMode.Impulse);
        Destroy(gameObject, 3.0f);
    }

    private void OnDestroy()
    {
        Debug.Log(gameObject.name.ToString() + " has been destroyed!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // So we don't send the weapon itself flying into space
        // A simple string comparison, don't yell at me

        if (collision.gameObject.tag != "Weapon")
        {
            var colBody = collision.rigidbody;
            var contactPointList = new List<ContactPoint>();
            var contactPoints = collision.GetContacts(contactPointList);
            
            // Gather all the contact points on the hit object
            foreach (var point in contactPointList)
            {
                // Inverse the normal so the force is applied to the forward vector of the contact point
                var normal = point.normal.normalized * -1;

                // Apply force at each point
                colBody.AddForceAtPosition(normal * 1.0f, point.point, ForceMode.Impulse);
            }
        }
        

    }
}
