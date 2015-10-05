var settings = require('../settings.js')
var userdata = require('./user-data.js')

module.exports.showLoginPage = function (request, response) {

}

module.exports.apiLogin = function (request, response) {
    if (request.body.username === "alastair" && request.body.password === "mustang1")
        response.json({
            "username": request.body.username,
            "guid": userdata.getGuid(request.body.username),
            "authenticated": true,
            "token": "auth_token"
        });
    else
        response.status(401).json({
            "error": "unauthorized",
            "authenticated": false,
            "message": "AuthorizationFailedUsernameOrPasswordIncorrect"
        });
}