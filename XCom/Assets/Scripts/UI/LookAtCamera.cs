using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{ //Script to make the text look at the camera at all times
    [SerializeField] private bool invert;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate() //As Update, runs at every frame but after Update
    {
        if (invert)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera * -1);
        }
        else
        {
            transform.LookAt(cameraTransform); //Make this (the text) look at the camera
        }
    }
}
