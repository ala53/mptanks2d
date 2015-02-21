using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Resources.Resource
{
    /// <summary>
    /// Basically just an abstraction so we never touch the rendering framework outside of the renderer.
    /// A.K.A. We can port the engine easier.
    /// </summary>
    class Texture
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int GLTextureId { get; set; }
        public Texture(int textureId)
        {
            GLTextureId = textureId;
        }

        public void Destroy()
        {
            GL.DeleteTexture(GLTextureId);
        }
    }
}
