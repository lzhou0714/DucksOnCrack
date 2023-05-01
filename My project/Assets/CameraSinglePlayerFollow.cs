using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSinglePlayerFollow : MonoBehaviour
{
    [SerializeField] Transform target, trfm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trfm.position = target.position + Vector3.forward * -10;
    }
}
