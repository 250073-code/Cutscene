using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private CamManager _mainCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _mainCam._canSwitchCams = true;
            _mainCam.SetCamPriorities();
            _mainCam._currentCam = 1;
            _mainCam.MainCamera();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _mainCam._canSwitchCams = false;
            _mainCam.SetCamPriorities();
            _mainCam._currentCam = 0;
            _mainCam.MainCamera();
        }
    }
}
