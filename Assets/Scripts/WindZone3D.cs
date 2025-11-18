using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class WindZone3D : MonoBehaviour
{
    public float upwardForce = 10f; // strength (see ForceMode notes)
    public bool useImpulse = false; // true = one-off impulse per entry
    public bool massIndependent = true; // use ForceMode.Acceleration when false use ForceMode.Force
    public LayerMask affectedLayers = ~0;

    HashSet<Rigidbody> bodies = new HashSet<Rigidbody>();

    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & affectedLayers) == 0) return;
        var rb = other.attachedRigidbody;
        if (rb != null) bodies.Add(rb);
    }

    void OnTriggerExit(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null) bodies.Remove(rb);
    }

    void OnTriggerStay(Collider other)
    {
        // CharacterController receiver support
        var receiver = other.GetComponent<WindReceiverCC>();
        if (receiver != null)
        {
            // pass an upward velocity (m/s). Adjust scale as needed.
            receiver.AddWind(upwardForce * (useImpulse ? 1f : Time.fixedDeltaTime));
        }
    }

    void FixedUpdate()
    {
        foreach (var rb in bodies)
        {
            if (rb == null) continue;
            if (useImpulse)
            {
                rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
            }
            else if (massIndependent)
            {
                rb.AddForce(Vector3.up * upwardForce, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(Vector3.up * upwardForce * Time.fixedDeltaTime, ForceMode.Force);
            }
        }
    }
}