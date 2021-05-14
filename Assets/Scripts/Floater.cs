using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]

public class Floater : MonoBehaviour
{
    private MeshFilter meshFilter;

    public Rigidbody rigidbody;
    public float displacementWeight = 3f;
    public float drag = 0.001f;
    public float angularDrag = 0.5f;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void FixedUpdate()
    {
        BoxCollider b = GetComponent<BoxCollider>();
        Vector3 cornerOne = transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f);
        Vector3 cornerTwo = transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f);
        Vector3 cornerThree = transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f);
        Vector3 cornerFour = transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f);
        Vector3 bCenter = transform.TransformPoint(b.center);
        ApplyCorner(cornerOne);
        ApplyCorner(cornerTwo);
        ApplyCorner(cornerThree);
        ApplyCorner(cornerFour);
        //ApplyCorner(bCenter);

        Vector3 v1 = Vector3.Cross(rigidbody.velocity, Quaternion.Euler(45, 45, 45) * rigidbody.velocity);
        v1.Normalize();
        Vector3 v2 = Vector3.Cross(rigidbody.velocity, v1);
        v2.Normalize();
        Debug.DrawRay(bCenter, rigidbody.velocity, Color.green);
        Debug.DrawRay(bCenter + rigidbody.velocity, v2, Color.blue);
        Debug.DrawRay(bCenter + rigidbody.velocity, v1, Color.blue);

        float width = 10f;
        float length = 24f;
        float height = 2f;
        Vector3 vel = rigidbody.velocity;
        Vector3 dragForce = vel * -1f * drag;
        
        for (float x = -width; x <= width; x += 3)
        {
            for (float z = -length; z <= length; z += 3)
            {

                Vector3 castPoint = bCenter + (vel * 5f) + (v1 * x) + (v2 * z);
                RaycastHit hit;
                int mask = 1 << LayerMask.NameToLayer("Boat");

                if (Physics.Raycast(castPoint, vel * -1f, out hit, Mathf.Infinity, mask))
                {
                    int oceanMask = 1 << LayerMask.NameToLayer("Ocean");
                    RaycastHit waveHit;
                    bool rayHit = Physics.Raycast(new Vector3(hit.point.x, 25f, hit.point.z), -Vector3.up, out waveHit, Mathf.Infinity, oceanMask);
                    if (rayHit)
                    {
                        float waveHeight = waveHit.point.y;
                        if (hit.point.y - height <= waveHeight) {
                            float displacement = Mathf.Clamp01((waveHeight - (hit.point.y - height))) * displacementWeight;
                            Debug.DrawRay(hit.point, dragForce * displacement, Color.red);
                            rigidbody.AddForceAtPosition(dragForce * displacement, hit.point);
                        }
                    }
                    
                }
            }
        } 
        
    }

    private void ApplyCorner(Vector3 cornerPos)
    {
        rigidbody.AddForceAtPosition(Physics.gravity / 4, cornerPos, ForceMode.Acceleration);
        float waveHeight;

        int mask = 1 << LayerMask.NameToLayer("Ocean");
        RaycastHit hit;

        bool rayHit = Physics.Raycast(new Vector3(cornerPos.x, 25f, cornerPos.z), -Vector3.up, out hit, Mathf.Infinity, mask);
        if (rayHit)
        {
            waveHeight = hit.point.y;
            Debug.Log(waveHeight);
            Debug.DrawRay(new Vector3(cornerPos.x, 25f, cornerPos.z), -Vector3.up * hit.distance, Color.blue);
            float displacement = Mathf.Clamp01((waveHeight - cornerPos.y) * displacementWeight);
            
            rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacement, 0f), cornerPos, ForceMode.Acceleration);
            rigidbody.AddTorque(displacement * -rigidbody.angularVelocity * angularDrag, ForceMode.VelocityChange);

            Debug.DrawRay(cornerPos, new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacement, 0f), Color.blue);
        } 

    }
}
