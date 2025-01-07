# Changelog

## [0.3.0] - 01/03/25

### Added

-   Harmony patches for common Unity functions (e.g. `GameObject.AddComponent`, `Resources.Load`, etc.).
-   `SilkUnity` class for accessing Unity-specific functions.
-   `SilkMod` attribute for marking mod classes.
-   `ModUtils` class for common mod-related utility functions.
-   `ISilkMod` interface for mods to implement.
-   `ModManager` class for managing mods.
-   `Weapon` API for modifying weapons.
-   `UI` API for modifying the game's UI.
-   `Player` API for modifying the player.

### Changed

-   Updated build scripts for both Linux and Windows to define versioning and improve build directory handling.
-   Improved installation instructions in README.md and here.
-   Mod format.

### Fixed

-   Resolved issues with mod compatibility when using outdated modding tools.

### Removed

-   Deprecated legacy mod code in preparation for refactoring.

## [0.2.0] - 12/26/24

### Added

-   Logging console to check mods.
-   Added mods menu.
-   Added Weapon API, UI API, Player API, and more.
-   Silk Atributte for mods
-   Extra Mod functions

### Changed

-   Updated build scripts for both Linux and Windows to define versioning and improve build directory handling.
-   Improved installation instructions in README.md and here.
-   Mod format.

### Fixed

-   Resolved issues with mod compatibility when using outdated modding tools.

### Removed

-   Deprecated legacy mod code in preparation for refactoring.

## [0.1.0] - 12/24/24

### Added

-   Initial release of Silk with basic mod loading and runtime patching capabilities(?).
