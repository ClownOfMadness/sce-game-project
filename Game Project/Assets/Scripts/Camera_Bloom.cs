using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Bloom : MonoBehaviour
{
    private Camera camera;
    private Camera thisCamera;

    private void Awake()
    {
        camera = Camera.main;
        thisCamera = this.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        thisCamera.orthographicSize = camera.orthographicSize;
    }
}
