using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering.Shaders
{
    class PixelShader : Shader
    {
        private string source;

        public PixelShader(string pShader)
            : base(pShader)
        {
            source = pShader;
        }

        public override void Compile()
        {
            shaderProgram = GL.CreateShaderProgram(ShaderType.FragmentShader, 1, new string[] { source });

            if (GL.GetError() != ErrorCode.NoError)
                throw new Exception("Error compiling shader.");

        }

        public override void BindActive(int pipeline)
        {
            GL.UseProgramStages(pipeline, ProgramStageMask.FragmentShaderBit, shaderProgram);
        }

        public override void Destroy()
        {
            GL.DeleteProgram(shaderProgram);
        }
    }
}
