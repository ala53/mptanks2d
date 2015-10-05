var userdata = require('./user-data.js');
var settings = require('../settings.js')

module.exports.authenticateToken = function (request, response) {
    if (request.param("token") === "auth_token" || request.param("token") === "auth_token_2")
        response.json({
            "username": "alastair",
            "guid": userdata.getGuid("alastair"),
            "authenticated": true,
            "valid": settings.authTokenValidityLengthSeconds
        });
    else
        response.status(401).json({
            "error": "invalid_token",
            "authenticated": true,
            "message": "TokenInvalidReenterCredentials"
        });
}

module.exports.refreshToken = function (request, response) {
    if (request.param("token") === "auth_token")
        response.json({
            "username": "alastair",
            "authenticated": true,
            "guid": userdata.getGuid("alastair"),
            "token": "auth_token_2",
            "valid": settings.authTokenValidityLengthSeconds
        });
    else if (request.param("token") === "auth_token_2")
        response.json({
            "username": "alastair",
            "authenticated": true,
            "guid": userdata.getGuid("alastair"),
            "token": "auth_token",
            "valid": settings.authTokenValidityLengthSeconds
        });
    else
        response.status(401).json({
            "error": "invalid_token",
            "authenticated": false,
            "message": "TokenInvalidReenterCredentials"
        });
    
}

module.exports.authenticateServerToken = function (request, response) {
    if (request.param("token") === "SERVER_TOKEN") {
        response.json({
            "username": "alastair",
            "guid": userdata.getGuid("alastair"),
            "authenticated": true,
            "userData": userdata.getUserData(userdata.getGuid("alastair"))
        });
    }
    else
        response.status(401).json({
            "error": "invalid_server_token",
            "authenticated": false,
            "message": "ServerTokenInvalidKickClient"
        });
}

module.exports.getServerToken = function (request, response) {
    if (request.param("token") === "auth_token" || request.param("token") === "auth_token_2")
        response.json({
            "token": "SERVER_TOKEN",
            "username": request.body.username,
            "guid": userdata.getGuid(request.body.username),
            "authenticated": true,
            "valid": settings.serverAuthTokenValidityLengthSeconds
        });
    else
        response.status(401).json({
            "error": "invalid_token",
            "authenticated": false,
            "message": "TokenInvalidReenterCredentials"
        });
}