using UnityEngine;
using Unity.Netcode;

public class StartParrelSync : NetworkBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 30), "Start Host"))
        {
            NetworkManager.StartHost();
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(this);
        }
        else if (GUI.Button(new Rect(10, 50, 200, 30), "Join"))
        {
            NetworkManager.StartClient();
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(this);
        }
        else if (GUI.Button(new Rect(10, 90, 200, 30), "Start Server"))
        {
            NetworkManager.StartServer();
            Destroy(this);
        }
    }
}
