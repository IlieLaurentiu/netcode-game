using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    private NetworkVariable<Vector3> _netPos = new(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Quaternion> _netRot = new(writePerm: NetworkVariableWritePermission.Owner);
}
