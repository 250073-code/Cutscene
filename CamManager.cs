using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _cam;
    [SerializeField] private CinemachineVirtualCamera _mainCam;
    [SerializeField] private CinemachineBlendListCamera[] _cutsceneCam;
    public int _currentCam;
    public bool _canSwitchCams;
    public float idleTimeThreshold = 5f;
    private bool _isCinematic;
    private float _lastInputTime;
    private Vector3 _lastMousePosition;
    private Coroutine _cinematicRoutine;

    void Start()
    {
        _mainCam = GameObject.Find("POVcam").GetComponent<CinemachineVirtualCamera>();
        
        _canSwitchCams = false;
        _lastInputTime = Time.time;
        _lastMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //switch between cams
            _mainCam.Priority = 0;
            StopCinematic();
            CurrentCam();
        }

        // detect any key or mouse input (movement or button)
        bool inputDetected = Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        if (!inputDetected)
        {
            // detect mouse movement
            if ((Input.mousePosition - _lastMousePosition).sqrMagnitude > 0.0f)
                inputDetected = true;
        }

        if (inputDetected)
        {
            _lastInputTime = Time.time;
            _lastMousePosition = Input.mousePosition;
            if (_isCinematic)
                StopCinematic();
        }

        if (!_isCinematic && Time.time - _lastInputTime >= idleTimeThreshold)
        {
            StartCinematic();
        }

        if (Input.GetKeyDown(KeyCode.R) && _canSwitchCams)
        {
            CurrentCam();
        }
    }

    public void SetCamPriorities()
    {
        foreach (var c in _cam)
        {
            if (c.GetComponent<CinemachineVirtualCamera>())
            {
                c.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            }

            if (c.GetComponent<CinemachineBlendListCamera>())
            {
                c.GetComponent<CinemachineBlendListCamera>().Priority = 10;
            }
        }
    }

    public void MainCamera()
    {
        if (_cam[_currentCam].GetComponent<CinemachineVirtualCamera>())
        {
            _cam[_currentCam].GetComponent<CinemachineVirtualCamera>().Priority = 15;
        }

        if (_cam[_currentCam].GetComponent<CinemachineBlendListCamera>())
        {
            _cam[_currentCam].GetComponent<CinemachineBlendListCamera>().Priority = 15;
        }
    }

    public void CurrentCam()
    {
        _currentCam++;
        if (_currentCam >= _cam.Length)
        { 
            _currentCam = 0;
        }
        SetCamPriorities();
        MainCamera();
    }


    private void StartCinematic()
    {
        if (_isCinematic) return;
        _mainCam.Priority = 0;
        _isCinematic = true;
        _canSwitchCams = true;
        SetCamPriorities();
        MainCamera();
        if (_cinematicRoutine != null)
            StopCoroutine(_cinematicRoutine);
        _cinematicRoutine = StartCoroutine(CinematicLoop());
    }

    private void StopCinematic()
    {
        if (!_isCinematic) return;
        _isCinematic = false;
        _canSwitchCams = false;
        if (_cinematicRoutine != null)
        {
            StopCoroutine(_cinematicRoutine);
            _cinematicRoutine = null;
        }
        // restore main camera priority
        SetCamPriorities();
        if (_mainCam) _mainCam.Priority = 20;
    }

    IEnumerator CinematicLoop()
    {
        // first immediate switch when cinematic starts
        CurrentCam();
        while (_isCinematic)
        {
            yield return new WaitForSeconds(idleTimeThreshold);
            if (!_isCinematic) break;
            CurrentCam();
        }
    }

    public void CutSceneCam()
    {
        StopCinematic();
        foreach (var c in _cutsceneCam)
        {
            c.GetComponent<CinemachineBlendListCamera>().Priority = 15;
        }        
    }
}
