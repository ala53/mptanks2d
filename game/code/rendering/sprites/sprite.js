//
// Sprite
// A single sprite in a texture atlas
//
//


var rendering = rendering || {}
rendering.sprites = rendering.sprites || {}

/**
 * Creates a sprite when given the sprite sheet and the sprite name.
 *
 *
*/
rendering.sprites.sprite = function (name, renderer, spritesheet, rectangle) {

}

rendering.sprites.sprite.fromSpriteObject = function(sprite, renderer, spritesheet) {
    return new rendering.sprites.sprite(
        sprite.name, renderer, spritesheet, sprite.frame)
}
