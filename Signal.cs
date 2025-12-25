using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
    [SerializeField] private GameObject _directorGameObject;
    [SerializeField] private float _timeToPlay = 0.1f;
    [SerializeField] private string _playerTag = "Player";
    private bool _isActived = false;
    private bool _canStartCounting = false;
    private float _timer;

    // Update is called once per frame
    public void Update()
    {
        if (!_canStartCounting)
            return;

        if (_isActived)
            return;

        if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _timer = 0f;
        }
        else
        {
            _timer += Time.deltaTime;
            if (_timer >= _timeToPlay)
            {
                ActivateTimeline();
            }
        }
    }

    private void ActivateTimeline()
    {
        if (_directorGameObject != null)
        {
            _directorGameObject.SetActive(true);
            _isActived = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            _canStartCounting = true;
        }
    }
}
