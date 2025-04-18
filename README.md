[![Logo](./assets/banner.png)](https://silkmodding.com)

[![GitHub stars](https://img.shields.io/github/stars/SilkModding/Silk?style=flat)](https://github.com/SilkModding/Silk/stargazers)
[![GitHub issues](https://img.shields.io/github/issues/SilkModding/Silk)](https://github.com/SilkModding/Silk/issues)
![Discord](https://img.shields.io/discord/1314422173082845204?label=Join%20the%20discord!&link=https%3A%2F%2Fdiscord.gg%2FGGv92crcx3)
[![GitHub All Releases](https://img.shields.io/github/downloads/SilkModding/Silk/total.svg)](https://github.com/SilkModding/Silk/releases)
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/SilkModding/Silk)](https://github.com/SilkModding/Silk/releases/latest)

# Silk Mod Loader

Silk is a mod loader for Spiderheck that allows you to easily install and manage mods for the game. It's designed to be easy to use, and it's designed to be as compatible as possible with other modding tools.

## Why Silk?

In order to load mods, you need a mod loader. This one was made beacause Modweaver is a great tool for
modding Spiderheck, but it's not without its issues. Modweaver is a bit of a black box, and it can be difficult
to get it to work. Additionally, Modweaver is "outdated", and it can lead to mods that are not very
compatible with each other. Silk is a more lightweight alternative to Modweaver. It's designed to be easy to use,
and it's designed to be as compatible as possible with other mods.

## How it works

Silk works by patching the Spiderheck executable at runtime. It does this by using the [Harmony](https://github.com/pardeike/Harmony)
library, which is a library for patching .NET assemblies at runtime.

## Features

Silk currently has the following features:

- **Mod loading**: Silk can load mods.
- **Runtime patching**: Silk can patch the Spiderheck executable at runtime.
- **Harmony patches**: Silk uses the [Harmony](https://github.com/pardeike/Harmony) library to patch .NET assemblies at runtime.
- **Mod dependencies**: Silk can detect and load mods that depend on other mods.
- **Mod versioning**: Silk can detect and load mods with different versions.

## Installation

To install Silk, follow these steps:

1. **Find the game directory**: To find the game directory, right click on SpiderHeck in your Steam library and select "Properties". Then, click on the "Local Files" tab and click on the "Browse Local Files" button. This will open the game directory in your file explorer.
2. **Download Silk**: Go to the [Silk releases page](https://github.com/SilkModding/Silk/releases) and download the latest version of Silk.
3. **Extract Silk**: Extract the downloaded zip archive to a folder on your computer.
4. **Move Silk files to the game directory**: Move the following files from the extracted folder to the same directory as the `SpiderHeckApp.exe` in the spiderheck directory.
5. **Run the game**: Run the game as you normally would.
