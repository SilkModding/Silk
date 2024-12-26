using Unity.Netcode;
using UnityEngine;

namespace Silk.API {
    /// <summary>
    /// Provides player-related utilities.
    /// </summary>
    public static class Player {
        /// <summary>
        /// Checks if the local player is spawned.
        /// </summary>
        public static bool IsLocalPlayer() {
            return NetworkManager.Singleton.LocalClient.PlayerObject != null;
        }

        /// <summary>
        /// Checks if the current instance is the server.
        /// </summary>
        public static bool IsServer() {
            return NetworkManager.Singleton.IsServer;
        }

        /// <summary>
        /// Checks if the current instance is the client.
        /// </summary>
        public static bool IsClient() {
            return NetworkManager.Singleton.IsClient;
        }

        /// <summary>
        /// Checks if the current instance is the host.
        /// </summary>
        public static bool IsHost() {
            return NetworkManager.Singleton.IsHost;
        }

        /// <summary>
        /// Checks if the current instance is listening.
        /// </summary>
        public static bool IsListening() {
            return NetworkManager.Singleton.IsListening;
        }

        /// <summary>
        /// Checks if the current instance is a connected client.
        /// </summary>
        public static bool IsConnectedClient() {
            return NetworkManager.Singleton.IsConnectedClient;
        }

        /// <summary>
        /// Gets the position of a player.
        /// </summary>
        public static Vector3 GetPlayerPosition(GameObject player) {
            return player.transform.position;
        }

        /// <summary>
        /// Sets the position of a player.
        /// </summary>
        public static void SetPlayerPosition(GameObject player, Vector3 newPosition) {
            player.transform.position = newPosition;
        }

        /// <summary>
        /// Gets the rotation of a player.
        /// </summary>
        public static Quaternion GetPlayerRotation(GameObject player) {
            return player.transform.rotation;
        }

        /// <summary>
        /// Sets the rotation of a player.
        /// </summary>
        public static void SetPlayerRotation(GameObject player, Quaternion newRotation) {
            player.transform.rotation = newRotation;
        }

        /// <summary>
        /// Sets the active state of a player.
        /// </summary>
        public static void SetPlayerActive(GameObject player, bool isActive) {
            player.SetActive(isActive);
        }

        /// <summary>
        /// Checks if a player is active.
        /// </summary>
        public static bool IsPlayerActive(GameObject player) {
            return player.activeSelf;
        }

    }
}
