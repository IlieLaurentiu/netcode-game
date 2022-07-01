using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Camera Rotation")]
    [SerializeField] private GameObject _player;
    [SerializeField] private float _minAngle, _maxAngle, _mouseSensitivity;
    private bool _foundParentPlayer;
    private float _xRot, _yRot;

    private void Update()
    {
        if (!_foundParentPlayer)
        {
            FindParentPlayer();
        }

        Vector3 _playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _xRot -= _playerMouseInput.y * _mouseSensitivity;
        _yRot += _playerMouseInput.x * _mouseSensitivity;
        _player.transform.Rotate(0f, _playerMouseInput.x * _mouseSensitivity, 0f);
        transform.localRotation = Quaternion.Euler(Mathf.Clamp(_xRot, _minAngle, _maxAngle), 90f, 0f);
        transform.RotateAround(_player.transform.position, new Vector3(0f, _yRot, 0f), 0);
    }

    GameObject FindParentPlayer()
    {
        // as Find() and GetComponent() methods become pretty performance-expensive, we use a boolean to make sure this method only runs once
        _foundParentPlayer = false;
        _player = GameObject.Find("GameManager").GetComponent<GameManager>().playerList[GameObject.Find("GameManager").GetComponent<GameManager>().playerCount - 1];
        _foundParentPlayer = true;
        return _player;
    }
}