using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

//dont forget: id must be same on client and server
internal class WeaponNetworkPrefabInstanceHandler : INetworkPrefabInstanceHandler {

    public readonly uint Id;
    public NetworkObject No;

    public WeaponNetworkPrefabInstanceHandler(uint id, NetworkObject no) {
        Id = id;
        No = no;

        AccessTools.FieldRefAccess<NetworkObject, uint>("GlobalObjectIdHash")(No) = Id;
    }
    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation) {
        var tmp = NetworkObject.Instantiate(No, position, rotation);

        tmp.gameObject.SetActive(true);

        return tmp;
    }
    public void Destroy(NetworkObject networkObject) {
        GameObject.Destroy(networkObject.gameObject);
    }
}