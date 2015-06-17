var settings = require('../settings.js')
var userdata = require('./user-data.js')

module.exports.showLoginPage = function (request, response) {

}

module.exports.apiLogin = function (request, response) {
    if (request.body.username === "alastair" && request.body.password === "mustang1")
        response.send(JSON.stringify({
            "username": request.body.username,
            "guid": userdata.getGuid(request.body.username),
            "token": request.token,
            "valid": settings.authTokenValidityLengthSeconds,
            "userData": userdata.getUserData(userdata.getGuid(request.body.username))
        }));
    else
        response.status(401).send(JSON.stringify({
            "error": "unauthorized",
            "message": "AuthorizationFailedUsernameOrPasswordIncorrect"
        }));
}