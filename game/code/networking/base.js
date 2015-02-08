var networking = networking || {}
var networking.peer = networking.peer || require('../../node-lib/peer.js')

/**
    Psst. Don't tell anyone but this is basically just a peer class
    We more or less wrap Peerjs and provide some serialization help.
*/
networking.base = function () {
    this.channels = []
}

networking.base.typeRevivers = []

/**
 * Allows you to register a type reviver so that when a blob
 * with a unique integer identifier is sent over the wire, it
 * can be rebuilt to it's object at this end of the connection
 *
 * @param {Function} reviver The function to call to revive
 * the object (signature: function (string: data))
 *
 * @param {Number} id The id to assign to the type for networked
 * data transmission.
*/
networking.base.registerTypeReviver = function (reviver, id) {

}

networking.base.typeCompressors = []

/**
 * Allows you to register a function that compresses an object
 * whose prototype is equal to the type value into a buffer,
 * so it can be faster sent over the network.
*/
networking.base.registerTypeCompressor = function (type, id) {

}

/**
 * Send data to a target over a specific data channel (between 0 and 256)
*/
networking.base.prototype.send = function (data, channel, target) {

}

/**
 * The function to call when data is received over the network
*/
networking.base.prototype.onReceive = function (data, channel, sender) {

}

networking.base.prototype.configureChannel = function (channel, options) {
    this.channels[channel] = this.channels[channel] || {
        reliable: options.reliable,
        ordered: options.ordered
    }
}
