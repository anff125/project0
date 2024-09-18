using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCamera : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;

    [SerializeField] private Transform camFollowTarget;

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        // Rotate target itself based on the camera's x-axis and y-axis input
        camFollowTarget.rotation = Quaternion.Euler(yAxis.Value, xAxis.Value, 0);
    }
}