using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;


public class Spinning : MonoBehaviour
{

    private float[] wheelIconPositions = new float[]
    {
        0, 45, 90, 135, 180, 225, 270, 315
    };

    public enum wheelIcons
    {
        Penguin = 2,
        IceCube = 3,
        Coin = 4,
        Fish = 0,
        Seven = 1,
        Dollar = 5,
        IceBerg = 6,
        Ball = 7
    }

    private Quaternion initialRotation;
    
    public bool IsSpinning;
    
    void Start()
    {
        IsSpinning = false;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSpinning)
        {
            transform.Rotate(Vector3.forward,  Time.deltaTime * 1500f);
        }
    }

    public void StartSpinning()
    {
        IsSpinning = true;
    }

    public void StopSpinning(int index)
    {
        IsSpinning = false;
        StartCoroutine(StopWheelCoroutine(wheelIconPositions[index], 0.1f));
    }

    private IEnumerator StopWheelCoroutine(float targetZRotation, float duration)
    {
        var elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        float startZRotation = transform.eulerAngles.z;

        float initialX = transform.eulerAngles.x;
        float intialY = transform.eulerAngles.y;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float newZrotation = Mathf.Lerp(startZRotation, targetZRotation, t);
            transform.rotation = Quaternion.Euler(initialX, intialY, newZrotation);
            yield return null;
        }
    }
}
