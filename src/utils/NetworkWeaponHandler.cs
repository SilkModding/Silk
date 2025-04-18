using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

// Don't forget: id must be same on client and server
internal class WeaponNetworkPrefabInstanceHandler : INetworkPrefabInstanceHandler {

    public readonly uint Id;
    public NetworkObject No;

    /// <summary>
    /// Creates a new <see cref="WeaponNetworkPrefabInstanceHandler"/> from a given <see cref="NetworkObject"/> and a unique identifier.
    /// </summary>
    /// <param name="id">A unique identifier for this network object.</param>
    /// <param name="no">The <see cref="NetworkObject"/> to create the handler for.</param>
    public WeaponNetworkPrefabInstanceHandler(uint id, NetworkObject no) {
        Id = id;
        No = no;

        AccessTools.FieldRefAccess<NetworkObject, uint>("GlobalObjectIdHash")(No) = Id;
    }
    /// <summary>
    /// Instantiates a network object and its associated game object at a given position and rotation.
    /// </summary>
    /// <param name="ownerClientId">The client ID of the owner of the network object.</param>
    /// <param name="position">The position of the network object.</param>
    /// <param name="rotation">The rotation of the network object.</param>
    /// <returns>The instantiated network object.</returns>
    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation) {
        var instantiatedObject = UnityEngine.Object.Instantiate(No, position, rotation);

        instantiatedObject.gameObject.SetActive(true);

        return instantiatedObject;
    }
    /// <summary>
    /// Destroys a network object and its associated game object.
    /// </summary>
    /// <param name="networkObject">The network object to destroy.</param>
    public void Destroy(NetworkObject networkObject) {
        UnityEngine.Object.Destroy(networkObject.gameObject);
    }
}
