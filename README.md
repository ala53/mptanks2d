# MP Tanks 2D
A multiplayer 2D game I am working on. Also known as a playground for ideas I've been working on (see the game renderer in `/MPTanks-MK5/Client/Backend/Renderer` and the shaders in `/MPTanks-MK5/Client/Backend/assets/mgcontent/` for an example).

### Building

Building is Windows only. To build, run `prebuild.cmd` after check out (needs to be run only once). Then, just build in Visual Studio (requires VS 2015 for C# 6 features).

### Mod support
The game has built in mod support. It's controlled by the code in the `Modding` subproject.

### TODOS

Welp, just about everything is a TODO.
 - The UI code is A MESS
 - Needs more content
 
### License
 
 The __game__ code is released under the MIT License. So, you can freely modify the game code, build your own games off of it, and more. This includes almost ALL code: renderer, engine, networking, and more.

Assets (as in textures and sounds) are under `Creative Commons Attribution Share Alike Non-Commercial` (CC-BY-NC-SA). 
 
 Any Dependent code (under `/3rd_Party`) is under its own license. Lidgren.Network is MIT, I believe. EmptyKeys is also MIT. MonoGame/MGCB is Ms-PL, etc. FXAA follows NVidia's license. The two songs are under Machinima's old "royalty free" license.
 
#### TLDR License
 All the code is free to use. Third party stuff is controlled by the third parties. Assets can't be used commercially and you need to attribute the original sources.
