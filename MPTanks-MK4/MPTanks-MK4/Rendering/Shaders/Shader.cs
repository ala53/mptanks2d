using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering.Shaders
{
    abstract class Shader
    {
        protected int shaderProgram;
        public Shader(string source)
        {

        }

        public abstract void Compile();

        public abstract void BindActive(int pipeline);


        public void SetParameter(string name, float value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform1(location, 1, ref value);
        }

        public void SetParameter(string name, float[] value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform1(location, value.Length, value);
        }

        public void SetParameter(string name, double value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform1(location, 1, ref value);
        }

        public void SetParameter(string name, double[] value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform1(location, value.Length, value);
        }

        public void SetParameter(string name, int value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform1(location, 1, ref value);
        }

        public void SetParameter(string name, int[] value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform1(location, value.Length, value);
        }

        public void SetParameter(string name, uint value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform1(location, 1, ref value);
        }

        public void SetParameter(string name, uint[] value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform1(location, value.Length, value);
        }

        public void SetParameter(string name, OpenTK.Matrix4 value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.UniformMatrix4(location, false, ref value);
        }

        public void SetParameter(string name, OpenTK.Vector4 value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform4(location, value);
        }

        public void SetParameter(string name, OpenTK.Vector3 value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform3(location, value);
        }

        public void SetParameter(string name, OpenTK.Vector2 value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform2(location, value);
        }

        public void SetParameter(string name, OpenTK.Graphics.Color4 value)
        {
            var location = GL.GetUniformLocation(shaderProgram, name);
            GL.Uniform4(location, value);
        }


        public abstract void Destroy();
    }
}
