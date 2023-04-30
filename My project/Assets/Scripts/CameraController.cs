using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Transform cameraTransform;
    // Start is called before the first frame update
    void Awake()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
