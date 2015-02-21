using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering
{
    class Shader
    {
        private int vertexShader, fragmentShader, shaderProgram;
        private string vertexShaderContent;
        private string fragmentShaderContent;
        public Shader(string vertex, string fragment)
        {
            vertexShaderContent = vertex;
            fragmentShaderContent = fragment;
        }

        public void Compile()
        {
            vertexShader = GL.CreateShader(ShaderType.FragmentShader);
            fragmentShader = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexShader, vertexShaderContent);
            GL.ShaderSource(fragmentShader, fragmentShaderContent);

            GL.CompileShader(vertexShader);
            GL.CompileShader(fragmentShader);

            shaderProgram = GL.CreateProgram();

            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            GL.LinkProgram(shaderProgram);

            if (GL.GetError() != ErrorCode.NoError)
                throw new Exception("Error compiling shader.");

            //Cleanup
            GL.DetachShader(shaderProgram, vertexShader);
            GL.DetachShader(shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

        }

        public void SetVar(string varName, Matrix4 value)
        {

        }

    }
}
