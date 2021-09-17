# MP Tanks 2D
A multiplayer 2D game I am working on. A 2D Tanks game with mod support, full networked multiplayer with basic latency compensation, and a custom game engine using Monogame to drive renderer. Also known as a playground for ideas I've been working on (see the game renderer in `/MPTanks-MK5/Client/Backend/Renderer` and the shaders in `/MPTanks-MK5/Client/Backend/assets/mgcontent/` for an example).

### Building

Building is Windows only. To build, install Monogame 3.4, Visual Studio 2015, run `prebuild.cmd` after check out (needs to be run only once). Then, just build in Visual Studio (requires VS 2015+ for C# 6 features). Although it has not been tested, it should be easy to port to Linux. Most Windows specific APIs were avoided and rendering is OpenGL based.

### Mod support
The game has built in mod support. It's controlled by the code in the `Modding` subproject.

### TODOS

The UI is incomplete, especially the UI for mods, game completion, and auto detection of servers.

The game is significantly lacking on content and art.
 
### License
 
 The game code and assets are released under the MIT License. So, you can freely modify the game code, build your own games off of it, and more. This includes almost ALL code: renderer, engine, networking, and more.
 
 Any dependencies (under `/3rd_Party` as well as the two song assets) are under their own licenses. Lidgren.Network is MIT, I believe. EmptyKeys is also MIT. MonoGame/MGCB is Ms-PL, etc. FXAA follows NVidia's license. The two songs are under Machinima's old "royalty free" license.
 
#### TLDR License
 All the code is free to use for pretty much...whatever you want (although, please don't build nuclear weaponry using my game engine - it would be much appreciated). Third party stuff is controlled by the third parties.
