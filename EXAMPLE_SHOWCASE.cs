using Silk.API;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SilkExampleMods
{
    /// <summary>
    /// Comprehensive example showcasing all Silk API libraries.
    /// This mod demonstrates common use cases and best practices.
    /// </summary>
    [SilkMod("API Showcase", new[] { "Silk Team" }, "1.0.0", "0.6.1", "api-showcase", 1)]
    public class APIShowcaseMod : SilkMod
    {
        private bool _autoKillEnabled = false;
        private float _lastEnemyCheck = 0f;

        public override void Initialize()
        {
            Logger.LogInfo("=== Silk API Showcase Mod ===");
            Logger.LogInfo("Press F1-F10 to test different features!");
            Logger.LogInfo("See QUICK_REFERENCE.md for all keybinds");
        }

        public void Update()
        {
            // F1 - Kill all enemies
            if (Input.WasKeyPressedThisFrame(Key.F1))
            {
                int enemyCount = Enemies.GetEnemyCount();
                Enemies.KillAllEnemies();
                Logger.LogInfo($"[F1] Killed {enemyCount} enemies!");
            }

            // F2 - Create explosion at mouse position
            if (Input.WasKeyPressedThisFrame(Key.F2))
            {
                Vector3 mousePos = Input.GetMouseWorldPosition();
                Effects.CreateColoredExplosion(mousePos, Color.red, 250f, 12f);
                Logger.LogInfo("[F2] Explosion created at mouse!");
            }

            // F3 - Toggle auto-kill enemies
            if (Input.WasKeyPressedThisFrame(Key.F3))
            {
                _autoKillEnabled = !_autoKillEnabled;
                Logger.LogInfo($"[F3] Auto-kill: {(_autoKillEnabled ? "ON" : "OFF")}");
                
                if (_autoKillEnabled)
                {
                    Actions.ExecuteRepeating(() => 
                    {
                        if (_autoKillEnabled && Enemies.GetAliveEnemyCount() > 0)
                        {
                            Enemies.KillAllEnemies();
                        }
                    }, 0.5f, -1);
                }
            }

            // F4 - Slow motion for 3 seconds
            if (Input.WasKeyPressedThisFrame(Key.F4))
            {
                Logger.LogInfo("[F4] Slow motion activated!");
                Game.SetTimeScale(0.3f);
                
                Actions.ExecuteAfterDelay(() => 
                {
                    Game.SetTimeScale(1f);
                    Logger.LogInfo("[F4] Normal time restored");
                }, 3f);
            }

            // F5 - Freeze all enemies
            if (Input.WasKeyPressedThisFrame(Key.F5))
            {
                Enemies.FreezeAllEnemies();
                Logger.LogInfo("[F5] All enemies frozen!");
            }

            // F6 - Heal player (enable shield)
            if (Input.WasKeyPressedThisFrame(Key.F6))
            {
                if (Health.EnablePlayerShield())
                {
                    Logger.LogInfo("[F6] Player shield enabled!");
                }
                else
                {
                    Logger.LogInfo("[F6] Could not enable shield");
                }
            }

            // F7 - Pull all objects to player
            if (Input.WasKeyPressedThisFrame(Key.F7))
            {
                var player = Player.GetLocalPlayer();
                if (player != null)
                {
                    Physics.PullObjectsToPoint(player.transform.position, 800f, 30f);
                    Logger.LogInfo("[F7] Pulling objects to player!");
                }
            }

            // F8 - Spawn objects in circle around player
            if (Input.WasKeyPressedThisFrame(Key.F8))
            {
                var player = Player.GetLocalPlayer();
                if (player != null)
                {
                    // Try to find a weapon prefab to spawn
                    var weapons = UnityEngine.Objects.GetAllWeapons();
                    if (weapons.Length > 0)
                    {
                        GameObject weaponPrefab = weapons[0].gameObject;
                        Spawning.SpawnInCircle(weaponPrefab, player.transform.position, 5f, 8);
                        Logger.LogInfo("[F8] Spawned 8 objects in circle!");
                    }
                }
            }

            // F9 - Create shockwave at player
            if (Input.WasKeyPressedThisFrame(Key.F9))
            {
                var player = Player.GetLocalPlayer();
                if (player != null)
                {
                    Effects.CreateShockwave(player.transform.position, 25f, 2f, 500f);
                    Logger.LogInfo("[F9] Shockwave created!");
                }
            }

            // F10 - Teleport to random spawn point
            if (Input.WasKeyPressedThisFrame(Key.F10))
            {
                var player = Player.GetLocalPlayer();
                var spawnPoint = Spawning.GetRandomSpawnPoint();
                if (player != null && spawnPoint != null)
                {
                    Player.SetPlayerPosition(player, spawnPoint.transform.position);
                    Logger.LogInfo("[F10] Teleported to random spawn!");
                }
            }

            // Left Mouse - Continuous explosions
            if (Input.IsLeftMousePressed())
            {
                Vector3 mousePos = Input.GetMouseWorldPosition();
                if (Time.frameCount % 5 == 0) // Every 5 frames
                {
                    Effects.CreateExplosion(mousePos, 100f, 5f);
                }
            }

            // Right Mouse - Spawn explosion and freeze time briefly
            if (Input.IsRightMousePressed() && Time.frameCount % 10 == 0)
            {
                Vector3 mousePos = Input.GetMouseWorldPosition();
                Effects.CreateColoredExplosion(mousePos, Color.cyan, 150f, 8f);
                
                // Brief slowdown
                float originalScale = Game.GetTimeScale();
                Game.SetTimeScale(0.1f);
                Actions.ExecuteAfterDelay(() => Game.SetTimeScale(originalScale), 0.2f);
            }

            // Display enemy count every second
            if (Time.time - _lastEnemyCheck > 1f)
            {
                int alive = Enemies.GetAliveEnemyCount();
                int total = Enemies.GetEnemyCount();
                if (total > 0)
                {
                    Logger.LogInfo($"Enemies: {alive}/{total} alive");
                }
                _lastEnemyCheck = Time.time;
            }

            // Automatic healing when player exists
            if (Health.IsPlayerAlive())
            {
                // Example: Could implement low-health auto-heal here
            }

            // Show game state info
            if (Input.WasKeyPressedThisFrame(Key.I))
            {
                ShowGameInfo();
            }
        }

        private void ShowGameInfo()
        {
            Logger.LogInfo("=== Game Information ===");
            Logger.LogInfo($"Scene: {Game.GetCurrentSceneName()}");
            Logger.LogInfo($"Time Scale: {Game.GetTimeScale()}");
            Logger.LogInfo($"Is Server: {Player.IsServer()}");
            Logger.LogInfo($"Is Client: {Player.IsClient()}");
            Logger.LogInfo($"Is Host: {Player.IsHost()}");
            Logger.LogInfo($"Versus Mode: {Game.IsVersusModeActive()}");
            Logger.LogInfo($"Survival Mode: {Game.IsSurvivalModeActive()}");
            Logger.LogInfo($"Parkour Mode: {Game.IsParkourActive()}");
            Logger.LogInfo($"Player Count: {Game.GetPlayerCount()}");
            Logger.LogInfo($"In Waiting Room: {Game.IsInWaitingRoom()}");
            
            var player = Player.GetLocalPlayer();
            if (player != null)
            {
                Logger.LogInfo($"Player Position: {player.transform.position}");
                Logger.LogInfo($"Player Active: {Player.IsPlayerActive(player)}");
            }

            Logger.LogInfo($"Alive Enemies: {Enemies.GetAliveEnemyCount()}");
            Logger.LogInfo($"Total Enemies: {Enemies.GetEnemyCount()}");
            Logger.LogInfo($"All Weapons: {Objects.GetAllWeapons().Length}");
            Logger.LogInfo($"All Portals: {Objects.GetAllPortals().Length}");
            Logger.LogInfo($"Gamepad Connected: {Input.IsGamepadConnected()}");
        }

        public override void Unload()
        {
            Logger.LogInfo("API Showcase Mod unloaded");
            _autoKillEnabled = false;
            Actions.StopAllRepeating();
        }
    }
}
