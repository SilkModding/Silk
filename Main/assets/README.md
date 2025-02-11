[![Logo](./assets/banner.png)](https://silkmodding.com)

[![GitHub stars](https://img.shields.io/github/stars/SilkModding/Silk?style=flat)](https://github.com/SilkModding/Silk/stargazers)
[![GitHub issues](https://img.shields.io/github/issues/SilkModding/Silk)](https://github.com/SilkModding/Silk/issues)
![Discord](https://img.shields.io/discord/1314422173082845204?label=Join%20the%20discord!&link=https%3A%2F%2Fdiscord.gg%2FGGv92crcx3)
[![GitHub All Releases](https://img.shields.io/github/downloads/SilkModding/Silk/total.svg)](https://github.com/SilkModding/Silk/releases)
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/SilkModding/Silk)](https://github.com/SilkModding/Silk/releases/latest)

# Silk Mod Loader

A modding tool for Spiderheck. Silk is a mod loader for Spiderheck that allows you to easily install and manage mods for the game. It's designed to be easy to use, and it's designed to be as compatible as possible with other modding tools.

## Why Silk?

In order to load mods, you need a mod loader. This one was made beacause Modweaver is a great tool for
modding Spiderheck, but it's not without its issues. Modweaver is a bit of a black box, and it can be difficult
to get it to work. Additionally, Modweaver is outdated, and it can lead to mods that are not very
compatible with each other. Silk is a more lightweight alternative to Modweaver. It's designed to be easy to use,
and it's designed to be as compatible as possible with other mods.

## How it works

Silk works by patching the Spiderheck executable at runtime. It does this by using the [Harmony](https://github.com/pardeike/Harmony)
library, which is a library for patching .NET assemblies at runtime.

## Features

Silk currently has the following features:

-   It can load mods from the `Mods` directory.
-   It can patch the Spiderheck executable at runtime.
-   It can steal the console back from Unity.

## Installation

To install Silk, follow these steps:

1. Download the latest release from the repository.
2. Extract the contents of the release to a directory of your choice.
3. Start Spiderheck.
4. Place your mods in the `Silk/Mods` directory to load them.
