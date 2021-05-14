using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Vector3[] origVertices;
    public bool useGerstner = false;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        origVertices = meshFilter.mesh.vertices;
    }

    void OnTriggerStay(Collider o)
    {
        Debug.Log("Wave Pushing Back");
        o.GetComponent<Rigidbody>().AddForce(new Vector3 (0f, 1f, 0f), ForceMode.Acceleration);
    }


    private void Update()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 newCoords = WaveManager.instance.GetWave(origVertices[i].x, origVertices[i].z);
            vertices[i].x = origVertices[i].x + newCoords[0];
            vertices[i].y = newCoords[1];
            vertices[i].z = origVertices[i].z + newCoords[2];
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;

    }
}
