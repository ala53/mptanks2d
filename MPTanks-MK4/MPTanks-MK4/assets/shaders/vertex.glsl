 #version 140
 
// the transform matrix
uniform mat4 model_matrix;
			
//the view matrix
uniform mat4 view_matrix;            
 
//the projection matrix
uniform mat4 projection_matrix;
 
 
// incoming vertex position
in vec2 vertex_position;
in vec2 vertex_texCoord;

//Just a passthrough vertex shader
void main(void)
{
	// transforming the incoming vertex position
	gl_Position = (projection_matrix * view_matrix * model_matrix * vec4(vertex_position, 0, 1)).xy;

	//and the texcoord
	gl_TexCoord[0] = gl_MultiTexCoord0;
}