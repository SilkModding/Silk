[![Logo](./banner.png)](https://github.com/SilkModding/Silk/)

[![GitHub stars](https://img.shields.io/github/stars/SilkModding/Silk?style=flat)](https://github.com/SilkModding/Silk/stargazers)
[![GitHub issues](https://img.shields.io/github/issues/SilkModding/Silk)](https://github.com/SilkModding/Silk/issues)
![Discord](https://img.shields.io/discord/1314422173082845204?label=Join%20the%20discord!&link=https%3A%2F%2Fdiscord.gg%2FGGv92crcx3)
[![GitHub All Releases](https://img.shields.io/github/downloads/SilkModding/Silk/total.svg)](https://github.com/SilkModding/Silk/releases)
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/SilkModding/Silk)](https://github.com/SilkModding/Silk/releases/latest)

# Silk Mod Loader

Silk is a mod loader for Spiderheck that allows you to easily install and manage mods for the game.

## How it works

Silk works by patching the Spiderheck executable at runtime. It does this by using the [Harmony](https://github.com/pardeike/Harmony)
library, which is a library for patching .NET assemblies at runtime.

## Features

Silk currently has the following features:

- **Mod loading**: Silk can load mods.
- **Runtime patching**: Silk can patch the Spiderheck executable at runtime.
- **Mod dependencies**: Silk can detect and load mods that depend on other mods.

## Installation

To install Silk, follow these steps:

1. **Find the game directory**: To find the game directory, right click on SpiderHeck in your Steam library and select "Properties". Then, click on the "Local Files" tab and click on the "Browse Local Files" button. This will open the game directory in your file explorer.
2. **Download Silk**: Go to the [Silk releases page](https://github.com/SilkModding/Silk/releases) and download the latest version of Silk.
3. **Extract Silk**: Extract the downloaded zip archive to a folder on your computer.
4. **Move Silk files to the game directory**: Move the following files from the extracted folder to the same directory as the `SpiderHeckApp.exe` in the spiderheck directory.
5. **Run the game**: Run the game as you normally would.
