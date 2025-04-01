using DG.Tweening;
using UnityEngine;

public class roate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DORotate(new Vector3(0, 180, 0), 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
