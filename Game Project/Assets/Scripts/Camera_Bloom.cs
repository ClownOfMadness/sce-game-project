using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Bloom : MonoBehaviour
{
    private Camera cameraObject;
    private Camera thisCamera;

    private void Awake()
    {
        cameraObject = Camera.main;
        thisCamera = this.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        thisCamera.orthographicSize = cameraObject.orthographicSize;
    }
}
