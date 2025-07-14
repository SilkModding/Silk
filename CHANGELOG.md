# Changelog

## [0.6.0] - 07/14/25

### Added

- Introduced a full configuration system with structured YAML-based config files.
- Added configuration options:

  - `debug.enableDebugLogging`: enables internal debug logging.
  - `debug.testLogging`: toggles example/test log output.
  - `updater.checkForUpdates`: controls whether Silk checks for updates on launch.
  - `updater.latestVersionUrl`: remote URL to fetch the latest version string.
  - `updater.downloadUrl`: URL template for downloading updates.
  - `loader.modFilePattern`: glob pattern for matching mod DLL files.
  - `patcher.enable`: enables or disables the Harmony patching system.
  - `patcher.disableLeaderboardWithOnlineMods`: disables leaderboard functionality when online mods are loaded.
  - `logger.redirectUnityLogs`: redirects Unity logs through the Silk logger.

* Added support for per-mod configuration files.

### Changed

- Refactored the `Logger` system for cleaner output, better structure, and config-driven control.
- Moved `SilkModData` to its own dedicated location to separate data structures from runtime logic.

### Fixed

- Resolved compatibility issues between Harmony and HarmonyX by unifying internal patching logic.
- Fixed mod entrypoint resolution issues that could cause mod loading to fail silently.
- Fixed an issue with the mod loader that could cause mods to be re-fired after they were already loaded, leading to duplicate mod loading.

### Removed

- Removed the legacy `SilkModAttribute` entrypoint specification in favor of a cleaner, explicit loading approach.

## [0.5.0] - 03/13/25

### Added

- Added `SilkModAttribute` for marking mod classes.
- Added `ModManager` class for managing mods.
- Added extensive documentation.
- Added a leaderboard patch to prevent cheating while modding.
- Added Nuget support
- Added networking attribute
- Added leaderboard cheating prevention

### Changed

- Modified the mod format.
- Updated the ModsUI screen.
- Implemented numerous null checks.
- Renamed `SilkUpdateRestarter` to `Updater`.
- Changed the configuration to follow a YAML structure.
- Enhanced the doorstop for improved user experience.
- Moved the updater to a new directory.
- Documented the code more

### Fixed

- Resolved compatibility issues with outdated modding tools.
- Addressed an issue where Harmony would not load sometimes.
- Fixed Windows build to support the updater.
- Fixed build script to update the test build before building the test mod

### Removed

- Removed obsolete updater code.
- Removed outdated elements.

## [0.4.0] - 01/30/25

### Added

- `SilkUpdateRestarter` executable for restarting the game after an update.
- Added `ModUtils` class for common mod-related utility functions.
- Added `Weapon` API for modifying weapons.
- Added `UI` API for modifying the game's UI.
- Added `Player` API for modifying the player.

### Changed

- Updated build scripts for both Linux and Windows to define versioning and improve build directory handling.
- Improved installation instructions in README.md and here.
- Mod format.

### Fixed

- Resolved issues with mod compatibility when using outdated modding tools.

### Removed

- Deprecated legacy mod code in preparation for refactoring.

## [0.3.0] - 01/03/25

### Added

- Harmony patches for common Unity functions (e.g. `GameObject.AddComponent`, `Resources.Load`, etc.).
- `SilkUnity` class for accessing Unity-specific functions.
- `SilkMod` attribute for marking mod classes.
- `ModUtils` class for common mod-related utility functions.
- `ISilkMod` interface for mods to implement.
- `ModManager` class for managing mods.
- `Weapon` API for modifying weapons.
- `UI` API for modifying the game's UI.
- `Player` API for modifying the player.

### Changed

- Updated build scripts for both Linux and Windows to define versioning and improve build directory handling.
- Improved installation instructions in README.md and here.
- Mod format.

### Fixed

- Resolved issues with mod compatibility when using outdated modding tools.

### Removed

- Deprecated legacy mod code in preparation for refactoring.

## [0.2.0] - 12/26/24

### Added

- Logging console to check mods.
- Added mods menu.
- Added Weapon API, UI API, Player API, and more.
- Silk Atributte for mods
- Extra Mod functions

### Changed

- Updated build scripts for both Linux and Windows to define versioning and improve build directory handling.
- Improved installation instructions in README.md and here.
- Mod format.

### Fixed

- Resolved issues with mod compatibility when using outdated modding tools.

### Removed

- Deprecated legacy mod code in preparation for refactoring.

## [0.1.0] - 12/24/24

### Added

- Initial release of Silk with basic mod loading and runtime patching capabilities(?).
