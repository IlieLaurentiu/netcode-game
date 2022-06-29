using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Camera Rotation")]
    [SerializeField] private GameObject _player;
    [SerializeField] private float _minAngle, _maxAngle, _mouseSensitivity;
    private float _xRot, _yRot;
    private bool foundPlayer;

    void Start()
    {
        _player = transform.GetComponentInParent<GameObject>();
    }

    private void Update()
    { 
        Vector3 _playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _xRot -= _playerMouseInput.y * _mouseSensitivity;
        _yRot += _playerMouseInput.x * _mouseSensitivity;
        _player.transform.Rotate(0f, _playerMouseInput.x * _mouseSensitivity, 0f);
        //transform.localRotation = Quaternion.Euler(Mathf.Clamp(_xRot, _minAngle, _maxAngle), 90f + _yRot, 0f);
        transform.RotateAround(_player.transform.position, new Vector3(0f, _yRot, 0f), 0);
    }
}