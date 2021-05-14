using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Range(0.0f, 1.0f)]
    public float speed = 1f;
    public float offset = 0f;
    public bool useGerstner = true;
    public bool useSecondWave = true;
    public bool useThirdWave = true;
    public Vector4 WaveA = new Vector4(1f, 1f, 0.5f, 5f);
    public Vector4 WaveB = new Vector4(1f, 0f, 0.25f, 4f);
    public Vector4 WaveC = new Vector4(0f, 1f, 0.25f, 5f);

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if(instance != this)
        {
            Debug.Log("Duplicate");
            Destroy(this);
        }
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }

    public Vector3 GetWave(float _x, float _z)
    {
        
        Vector2 XZ = new Vector2(WaveA[0], WaveA[1]);
        XZ.Normalize();
        Vector3 WaveOne = GetWaveHelper(XZ, WaveA[2], WaveA[3], _x, _z);

        Vector3 WaveTwo, WaveThree;
        if(useSecondWave) { 
            XZ = new Vector2(WaveB[0], WaveB[1]);
            XZ.Normalize();
            WaveTwo = GetWaveHelper(XZ, WaveB[2], WaveB[3], _x, _z);
        } else
        {
            WaveTwo = new Vector3(0f, 0f, 0f);
        }

        if (useThirdWave) { 
            XZ = new Vector2(WaveC[0], WaveC[1]);
            XZ.Normalize();
            WaveThree = GetWaveHelper(XZ, WaveC[2], WaveC[3], _x, _z);
        }
        else
        {
            WaveThree = new Vector3(0f, 0f, 0f);
        }

        return WaveOne + WaveTwo + WaveThree;
    }

    private Vector3 GetWaveHelper(Vector2 XZ, float steepness, float length, float _x, float _z)
    {
        float w = 2 * (float)Mathf.PI / length;
        float y = (steepness / w) * Mathf.Sin(w * ((XZ[0] * _x + XZ[1] * _z) - offset));
        float x, z;
        if (useGerstner)
        {
            x = XZ[0] * (steepness / w) * Mathf.Cos(w * ((XZ[0] * _x + XZ[1] * _z) - offset));
            z = XZ[1] * (steepness / w) * Mathf.Cos(w * ((XZ[0] * _x + XZ[1] * _z) - offset));
        } else
        {
            x = 0f;
            z = 0f;
        }
        return new Vector3(x, y, z);
    }
}
