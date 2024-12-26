using Unity.Netcode;

namespace Silk.Api.Player {
    public class PlayerUtils {
        public static bool IsLocalPlayer() {
            return NetworkManager.Singleton.LocalClient.PlayerObject != null;
        }

        public static bool IsServer() {
            return NetworkManager.Singleton.IsServer;
        }
    }
}