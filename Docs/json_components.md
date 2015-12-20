JSON components documentation
=============================

Components and visual attributes for MP Tanks objects can be defined in a separate
JSON file. The first level of the JSON file is always an object (`{}`). Below is a 
list of the allowed attributes for the object.

Data types
 - string
   - a plain JSON string
 - vector 
   - A serialized representation of a 2D vector in the form of `{"x":0,"y":0}`
 - object 
   - A plain JSON object (`{}`)
 - object(of <type>)
   - A plain JSON object (`{}`), but with a required internal data representation
 - array 
   - A plain JSON array (`[]`)
 - array(of <type>) 
   - A plain JSON array (`[]`), but with a required internal data representation
 - number
   - A floating point number in the form of either `0.0` or `0`
 - integer
   - An number without a decimal point
 - color
   - A color serialized in the form of `{"r":0,"g":0,"b":0,"a":0}` where the `0`'s are integers.
 - time
   - A span of time, represented in full or fractional milliseconds, as a number. E.g. `19.72` is 19.72 milliseconds of time.

"Type" specifications for objects
 - Keyed
   - An object that has a "key" to it -- that is, it can be referenced either from code or from "[ref]" tags
   - These should be unique to the Components JSON file.
 - Triggerable
   - The object is triggered by some sort of event
   - `destroy`
     - The trigger is invoked when `GameObject.Kill()` is called.
   - `destroy_ended`
     - The trigger is invoked at the moment when the object is removed from the game, which may be delayed from when `GameObject.Kill()` is called.
   - `create` or **nothing**
     - The trigger is invoked when the object is first created. This is the default
   - `t=<time>`
     - It is triggered `<time>` milliseconds after the object is first created.
   - Custom
     - Any string can be used as a trigger. You can invoke the trigger from code via calling `GameObject.InvokeTrigger(string name)`

First level
===========
 - string/path `__image__body` (optional, compiler only/special)
   - A special attribute that tells the mod compiler to take an black and white masked image (black for filled parts, white or transparent for unfilled parts) and generate a physics body from it. This allows one to generate custom shaped bodies instead of just squares. For an example, see the tank blocks.
   - This requires that the mod be run through the mod compiler, as it compiles to a `body` variable.
 - vector `size` (**REQUIRED**)
   - This defines the "base size" for the object. If the map creates a copy at half or twice size, it doesn't matter, because the components will be appropriately scaled.
 - array(of string) `flags` (optional)
   - Special flags for the object. In terms of the runtime, these do almost nothing. However, flags are used to provide directives on interactions with other objects. For example, the `DamagedByAll` flag overrides the `DamagesMapObjects` property of projectiles and guarantees a map object will be impacted by all projectiles.
   - These can be completely custom and are up to the relevant `GameObject` class to provide an implementation.
 - time `lifespan` (optional)
   - The amount of time, in milliseconds, that the object "lives" for. After `lifespan` milliseconds have passed, the object will automatically call `Kill()` on itself and initiate the destruction process.
   - Specify `0` to disable automated destruction based on time
 - number `health` (optional)
   - The number of health points the object has. Once this reaches 0, if `invincible` is not set to true, the object will be destroyed.
   - If not specified or specified as 0, the object will have infinite health -- in effect, it will be invincible.
 - time `removeAfter` (optional)
   - Once an object is destroyed (via `Kill()` or other means), this specifies how long it should remain "in the game" before being removed.
   - Very useful for having a destruction animation (see BasicTank)
 - object(of Asset) `sheet` (**REQUIRED** if using textures)
   - The "base" sprite sheet for this object. If a sprite sheet is not specified for a `Component` object, it will use this one instead.
 - array(of ComponentGroup) `componentGroups` (optional)
   - Provides a way to group individual rendering components. Objects grouped here can be addressed in code as `ComponentGroups[X]` and all property setting (but not getting) will work as with individual components.
   - Useful for grouping a set of logically similar components. E.g. all parts of the turret on a tank must rotate at the same time. Rather than calling `Components["TurretCenter"].Rotation = 5`, followed by 3 more calls for other parts of the tank, one can call `ComponentGroups["Turret"].Rotation = 5`, and all members will be updated simultaneously.
 - array(of Component) `components` (optional, but pretty much required)
   - The components / parts of the `GameObject` that will be rendered. Any object that is rendered, other than particle emitters and lights, are components.
 - array(of Light) `lights` (optional)
   - **NOT IMPLEMENTED**
 - array(of Asset) `otherAssets` (optional)
   - A list of all the assets this mod uses. This is for _keyed_ access in the components file or for user access via the `GameObject.Assets[]` property
   - Assets will be auto resolved to their correct "on disk" file names.
 - array(of Emitter) `emitters` (optional)
   - The particle emitters this object uses. E.g. smoke emission, dust trails, pretty much any sort of particle.
 - array(of Sprite) `otherSprites` (optional)
   - A keyed list of sprites this object uses, other than the ones directly specified in `Component` objects.
 - array(of Animation) `animations` (optional)
   - The animations (which are not "part" of the object) that this object has. For example, an explosion is an animation.
 - array(of Sound) `sounds` (optional)
   - The sounds (as in: sound files) that this object can play. E.g. a tank can play an explosion sound.
Assets (`Keyed`)
======
An asset is a sprite sheet (but not a sprite contained therein), a sound file, or really any other type of file referenced by this object.

Sprites (`Keyed`)
=======


Component Groups (`Keyed`)
================
A logically grouped set of component objects. Defined as:
```
{
  "key": "MY-KEY",
  "components": [
    "COMPONENT-1-NAME",
    "COMPONENT-2-NAME"
  ]
}
```
 - string `key` (**REQUIRED**)
   - The (unique) name with which the component group is addressed.
 - array(of string) `components`
   - The names of the components that this component group addresses.

Components (`Keyed`)
==========
A rendered entity. Size, shape, scale, color, and art assets are all defined here.

```
{
  "drawLayer": 0,
  "mask": {
    "r": 255,
    "g": 255,
    "b": 255,
    "a": 255
  },
  "name": "base",
  "offset": {
    "x": 0.0,
    "y": 0.0
  },
  "rotation": 0.0,
  "rotationVelocity": 0.0,
  "rotationOrigin": {
    "x": 0.0,
    "y": 0.0
  },
  "scale": null,
  "size": {
    "x": 3.0,
    "y": 5.0
  },
  "visible": false,
  "image": null,
  "ignoresObjectMask": false
}
```

Lights (`Keyed`, `Triggerable`)
======
**NOT IMPLEMENTED**

Emitters (`Keyed`, `Triggerable`)
========

Animations (`Keyed`, `Triggerable`)
==========

Sounds (`Keyed`, `Triggerable`)
======