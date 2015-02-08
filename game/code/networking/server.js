var networking = networking || {}
var networking.peer = networking.peer || require('../../node-lib/peer.js')
var networking.PeerServer =
    networking.PeerServer || require('../../node-lib/peerserver.js').PeerServer

networking.server = function (port) {
    this.base = new networking.base(address)
    /*
        Just a note here: the reason we're creating a local peer
        server is so we can host the server offline on a lan
        network without having to connect to a global server.
        Plus, we have much less load on the central servers
    */

    //For web servers, we want to be safe and let people use ssl
    //so we provide the option in-game
    var ssl = game.settings.networking.useSsl ?
        {
            key: fs.readFileSync(game.settings.networking.sslKey),
            cert: fs.readFileSync(game.settings.networking.sslCert)
        } : undefined

    this.peerServer = networking.PeerServer({
        path: game.settings.globals.peerjsPath,
        port: port,
        ssl: ssl
    })
}
