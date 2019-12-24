using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    MoveNearDistance,
    MoveFarDistance,
    Cooldown,
}

public class CameraBehaviour : MonoBehaviour
{
    public Transform CameraAnchor1;
    public Transform CameraAnchor2;
    public Camera MainCamera;

    private float cameraAngleY = 0;
    private float cameraAngleSpeed = 0.02f;

    private Vector3 cameraStartPosition;
    private Vector3 cameraOffsetPosition;

    private bool isMoveFar = false;

    private float cameraCurrentRadius = 0;
    private float cameraCurrentPosY = 0;
    private CameraState cameraState = CameraState.Cooldown;
    private CameraState cameraStateAfterCooldown = CameraState.MoveNearDistance;
    private float lastChangeStateTime = 0;
    private float stateDurationTime = 0;
    private float stateCurrentTime = 0f;

    private CameraModel cameraSettings = null;
    public CameraModel CameraSettings
    {
        get
        {
            return cameraSettings;
        }
        set
        {
            cameraSettings = value;
            Init();
        }
    }

    private void Init()
    {
        cameraAngleY = MainCamera.transform.localRotation.eulerAngles.y;
        MainCamera.transform.localPosition = new Vector3(0, cameraSettings.height, 0);
        cameraStartPosition = new Vector3(CameraAnchor1.position.x, MainCamera.transform.position.y, CameraAnchor1.position.z);

        lastChangeStateTime = Time.time;

        cameraAngleSpeed = (float)(360 / cameraSettings.roundDuration) * Mathf.Deg2Rad;
        stateDurationTime = cameraSettings.fovDelay;
        cameraCurrentRadius = cameraSettings.roundRadius;

        cameraState = CameraState.Cooldown;
        cameraStateAfterCooldown = CameraState.MoveNearDistance;
    }

    private void Update()
    {
        if (cameraSettings == null)
            return;

        stateCurrentTime = Time.time - lastChangeStateTime;

        switch (cameraState)
        {
            case CameraState.Cooldown:
                if (stateCurrentTime >= stateDurationTime)
                {
                    cameraState = cameraStateAfterCooldown;
                    lastChangeStateTime = Time.time;
                    stateDurationTime = cameraSettings.fovDuration;
                }
                break;

            case CameraState.MoveNearDistance:
                if (stateCurrentTime >= stateDurationTime)
                {
                    cameraState = CameraState.Cooldown;
                    cameraStateAfterCooldown = CameraState.MoveFarDistance;
                    lastChangeStateTime = Time.time;
                    stateDurationTime = cameraSettings.fovDelay;
                }
                else
                {
                    //cameraCurrentPosY = Mathf.Lerp(cameraSettings.height, cameraSettings.lookAtHeight, stateCurrentTime / stateDurationTime);
                    MainCamera.fieldOfView = Mathf.Lerp(cameraSettings.fovMax, cameraSettings.fovMin, stateCurrentTime / stateDurationTime);
                }
                break;

            case CameraState.MoveFarDistance:
                if (stateCurrentTime >= stateDurationTime)
                {
                    cameraState = CameraState.Cooldown;
                    cameraStateAfterCooldown = CameraState.MoveNearDistance;
                    lastChangeStateTime = Time.time;
                    stateDurationTime = cameraSettings.fovDelay;
                }
                else
                {
                    //cameraCurrentPosY = Mathf.Lerp(cameraSettings.lookAtHeight, cameraSettings.height, stateCurrentTime / stateDurationTime);
                    MainCamera.fieldOfView = Mathf.Lerp(cameraSettings.fovMin, cameraSettings.fovMax, stateCurrentTime / stateDurationTime);
                }
                break;
        }

        cameraAngleY += cameraAngleSpeed * Time.deltaTime;

        cameraOffsetPosition = new Vector3(Mathf.Cos(cameraAngleY) * cameraCurrentRadius, cameraCurrentPosY, Mathf.Sin(cameraAngleY) * cameraCurrentRadius);
        MainCamera.transform.position = cameraStartPosition + cameraOffsetPosition;

        MainCamera.transform.LookAt(CameraAnchor1, Vector3.up);
    }
}
