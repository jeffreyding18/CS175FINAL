using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 100.0f;
    public float rotationSpeed = 100.0f;

    // Update is called once per frame

    void Update()
    {
        float t = Input.GetAxis("Vertical") * speed;
        float r = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        t *= Time.deltaTime;
        r *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, t);

        // Rotate around our y-axis
        transform.Rotate(0, r, 0);
    }
}
