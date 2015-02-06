//
// Animation
// A list of sprites that are shown sequentially
//
//


var rendering = rendering || {}
rendering.sprites = rendering.sprites || {}

/**
 * Creates an instance of a rendering animation.
 * @class
 *
 * @param {Number} sprites The array of sprites to use
 *
 * @param {rendering.renderer} renderer The rendering context to create
 * the animation on.
 *
 * @param {Number} loopCount The number of times to loop the animation
 * when it is played
 *
*/
rendering.sprites.animation = function (sprites, renderer, loopCount) {
    //So, the best way to deal with this is suprisingly to go native.
    //We drop down and generate a pixi movieclip from the sprites
}
