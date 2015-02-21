//A quick shader that lets you do a color mask in opengl (w/ shaders)

//the color mask to do the blending
uniform vec4 maskColor;
uniform sampler2D texture;

void main (void) 
{
	gl_FragColor = maskColor * texture2D(texture, gl_TexCoord[0].st);
}