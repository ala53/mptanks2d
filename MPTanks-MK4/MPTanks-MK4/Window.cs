using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace MPTanks_MK4
{
    class Window : GameWindow
    {
        protected override void OnLoad(EventArgs e)
        {
            FMOD.System system;
            FMOD.Factory.System_Create(out system);

            system.init(5, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
            base.OnLoad(e);
            GL.ClearColor(Color4.Red);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
