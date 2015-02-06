//
// Color class
// Wanna guess? It's colors, no more, no less
//
//

var game = game || {}
game.core = game.core || {}

game.core.color = function (r, g, b, a, __noFloatHeuristics) {
    //deal with undefined values by setting them to
    //0 and if all are undefined, return opaque black
    if (r == undefined) r = 0; if (g == undefined) g = 0
    if (b == undefined) g = 0; if (a == undefined) a = 1

    //then, if they *all* appear to be in normal
    //float form, we take the assumption that
    //they actually are in float form unless
    //the safety flag is set
    if (r >= 0 && r < 1 && g >= 0 && g < 1 &&
        b >= 0 && b < 1 && a >= 0 && a < 1 &&
        !__noFloatHeuristics) {
            r *= 255
            g *= 255
            b *= 255
            a *= 255
        }
    //Then, normalize them to 8 bit form to be safe
    //r
    if (r > 255) r = 255; if (r <= 0) r = 0
    //g
    if (g > 255) g = 255; if (g <= 0) g = 0
    //b
    if (b > 255) b = 255; if (b <= 0) b = 0
    //a
    if (a > 255) a = 255; if (a <= 0) a = 0

    //and bitshift to an 32 bit actual color
    this.backing = r | (g << 8) | (b << 16) | (a << 24);
    //here's to hoping that V8 optimizes the property adds
    //to avoid the overhead of doing hidden class lookups
    //every time
    this.r = r
    this.g = g
    this.b = b
    this.a = a

    //make it immutable
    return Object.freeze(this);
}

game.core.color.prototype.asHex = function() {
    return "0x" + this.backing.toString(16)
}

game.core.color.prototype.toString = function() {
    return "0x" + this.backing.toString(16)
}

//
// Here, we have a list of named colors
//
game.core.color.red = new game.core.color(1, 0, 0, 1)
game.core.color.green = new game.core.color(0, 1, 0, 1)
game.core.color.blue = new game.core.color(0, 0, 1, 1)
game.core.color.white = new game.core.color(1, 1, 1, 1)
game.core.color.black = new game.core.color(0, 0, 0, 1)
game.core.color.transparent = new game.core.color(0, 0, 0, 0)

game.core.color.yellow = new game.core.color(1, 1, 0, 1)
game.core.color.orange = new game.core.color(1, 0.65, 0, 1)
game.core.color.purple = new game.core.color(0.5, 0, 0.5, 1)
game.core.color.gray = new game.core.color(0.5, 0.5, 0.5, 1)

//and a function to look up html colors

/**
 * Finds a color based on its css color value
*/
game.core.color.findColor = function (color) {
    var tmp = $('<div style="' + color + '"/>')
    //code stolen from http://stackoverflow.com/questions/1740700
    var hexDigits = new Array
        ("0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f");

    //Function to convert hex format to a rgb color
    var rgb2hex =function (rgb) {
        rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
    }

    var hex = function (x) {
        return isNaN(x) ? "00" : hexDigits[(x - x % 16) / 16]
            + hexDigits[x % 16];
    }

    return new game.core.color(rgb2hex(tmp.css('color')))
}
