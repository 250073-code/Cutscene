using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControls : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private GameObject _shipModel;
    [SerializeField] private CamManager _camManager;

    // Start is called before the first frame update
    public void Start()
    {
        _currentSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        ShipMovement();
    }

    public void ShipMovement()
    {
        float pitch = Input.GetAxis("Pitch") * _rotSpeed * Time.deltaTime;
        float yaw = Input.GetAxis("Horizontal") * _rotSpeed * Time.deltaTime;
        float roll = Input.GetAxis("Roll") * _rotSpeed * Time.deltaTime;

        transform.Rotate(pitch, yaw, roll, Space.Self);

        if (Input.GetKeyDown(KeyCode.T))
        {
            _currentSpeed++;
            if (_currentSpeed > 5)
            {
                _currentSpeed = 5;
            }
        }//increase speed

        if (Input.GetKeyDown(KeyCode.G))
        {
            _currentSpeed--;
            if (_currentSpeed < 1)
            {
                _currentSpeed = 1;
            }
        }//decrease speed

        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Untagged")
        {
            _camManager.CutSceneCam();
            _currentSpeed = 0;
        }
    }
}
