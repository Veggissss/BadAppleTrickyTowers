# Bad Apple but it's played in Tricky Towers

This project renders the [Bad Apple!!](https://youtu.be/FtutLA63Cp8?si=vIyMtJBY3EmeY4-x) video inside [Tricky Towers](https://store.steampowered.com/app/437920/Tricky_Towers/) using Tetrominoes (Tetris Bricks).  
Each frame of the video is represented as a grid of black-and-white pixels, and Tetris bricks are shown or removed to match the pixel data over time.

The animation runs at 30 frames per second and is synced with the original audio track.

## Credits

- Video frames were originally extracted by [TheSavageTeddy/bad-apple-tetrio](https://github.com/TheSavageTeddy/bad-apple-tetrio), who created a similar project in another game, **TETR.IO**.  
  The frame extraction logic were reused for this mod.

- This project was built using [foxmadnes/tricky-multiplayer-plus](https://github.com/foxmadnes/tricky-multiplayer-plus) as a base for modding **Tricky Towers**.

See [LICENSES/](LICENSES/) for individual MIT license details.

## Demo Video

[![Demo video](https://img.youtube.com/vi/BvXCpEuPhxQ/maxresdefault.jpg)](https://youtu.be/BvXCpEuPhxQ?feature=shared&t=46)

## Installation
To install, unzip a [mod release](https://github.com/Veggissss/BadAppleTrickyTowers/releases) into your tricky towers install location so the BepInEx folder is alongside the TrickyTowers.exe file.

## Configuration
You can change the bricks used in `Tricky Towers/BepInEx/config/badapple.cfg`

## Compilation
To compile the mod yourself, use Visual Studio and link the game dll files as well as the dlls in `BepInEx_x86_5.4.15.0`.

Assets are compiled from the pngs in the assets folder in an empty unity project with an AssetBundle named `badapple`. Built with [Unity 5.5.1 Patch 3](https://unity.com/releases/editor/patch-releases/5.5.1p3).
