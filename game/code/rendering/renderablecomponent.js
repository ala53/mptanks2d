//
// Renderable Component Class
// A component that has a texture/spritesheet accessible
// along with relative transforms so it can be rendered
//

var rendering = rendering || {}

/**
 * A renderable object that has animation capabilities
 *
 * @param {rendering.sprites.spritesheet} spritesheet The spritesheet to
 * associate with this component.
 *
 * @class
*/
rendering.renderableComponent = function (spritesheet) {
    Object.defineProperty("colorMask",
        {
        get: function () {

        },
        set: function (value) {
            //check if the value is a color 
        }})
}
