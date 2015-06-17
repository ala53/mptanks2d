var userdata = require('./user-data.js');

module.exports.authenticateToken = function (request, response) {
    if (request.body.username === "alastair" && (request.body.token === "auth_token" || request.body.token === "auth_token_2"))
        response.send(JSON.stringify({
            "username": request.body.username,
            "guid": userdata.getGuid(request.body.username),
            "token": request.token,
            "valid": settings.authTokenValidityLengthSeconds,
            "userData": userdata.getUserData(userdata.getGuid(request.body.username))
        }));
    else
        response.status(401).send(JSON.stringify({
            "error": "invalid_token",
            "message": "LoginExpiredReenterCredentials"
        }));
}

module.exports.refreshToken = function (request, response) {
    if (request.body.username === "alastair" && request.body.token === "auth_token")
        response.send(JSON.stringify({
            "username": request.body.username,
            "guid": userdata.getGuid(request.body.username),
            "token": "auth_token_2",
            "valid": settings.authTokenValidityLengthSeconds,
            "userData": userdata.getUserData(userdata.getGuid(request.body.username))
        }));
    else if (request.body.username === "alastair" && request.body.token === "auth_token_2")
        response.send(JSON.stringify({
            "username": request.body.username,
            "guid": userdata.getGuid(request.body.username),
            "token": "auth_token",
            "valid": settings.authTokenValidityLengthSeconds,
            "userData": userdata.getUserData(userdata.getGuid(request.body.username))
        }));
    else
        response.status(401).send(JSON.stringify({
            "error": "invalid_token",
            "message": "LoginExpiredReenterCredentials"
        }));
    
}

module.exports.authenticateServerToken = function (request, response) {
    if (request.body.username === "alastair" && request.body.serverToken === "SERVER_TOKEN") {
        response.send(JSON.stringify({
            "username": request.body.username,
            "guid": userdata.getGuid(request.body.username),
            "userData": userdata.getUserData(userdata.getGuid(request.body.username))
        }));
    }
    else
        response.status(401).send(JSON.stringify({
            "error": "invalid_server_token",
            "message": "ServerTokenInvalidKickClient"
        }));
}

module.exports.getServerToken = function (request, response) {
    if (request.body.username === "alastair" && (request.body.token === "auth_token" || request.body.token === "auth_token_2"))
        response.send(JSON.stringify({
            "token": "SERVER_TOKEN",
            "valid": settings.serverAuthTokenValidityLengthSeconds
        }));
    else
        response.status(401).send(JSON.stringify({
            "error": "invalid_token",
            "message": "LoginExpiredReenterCredentials"
        }));
}