var settings = require('../settings.js')

module.exports.getUserData = function (guid) {
    if (guid === "ed801941-1e39-4f3d-8b99-c71d290690cc")
        return {
            "isPremium": true,
            "tanksWithCustomTextures": [
                {
                    "name": "Basic Tank Type 2",
                    "mod": "MP Tanks Core",
                    "key": "BasicTankMPVersion2",
                    "imageUrl": "http://tanktextures.usercontent.zsbgames.me/ed801941-1e39-4f3d-8b99-c71d290690cc/BasicTankMPVersion2.png"
                }
            ],
            "tanksWithCustomColors": [
                {
                    "name": "Basic Tank Type 2",
                    "mod": "MP Tanks Core",
                    "key": "BasicTankMPVersion2", 
                    "data": module.exports.getCustomColorData(guid, "BasicTankMPVersion2")
                }
            ]
        };
}

module.exports.apiGetUserData = function (request, response) {
    if (request.param("id") === module.exports.getGuid("alastair"))
        response.json({
            "username": "alastair",
            "guid": request.param("id"),
            "data": module.exports.getUserData(request.param("id"))
        })
    else response.status(404).json({
        "error": "user_not_found",
        "message": "UserNotFoundInvalidId"
    })
}

module.exports.getCustomColorData = function (guid, tankId) {
    if (guid === module.exports.getGuid("alastair") && tankId == "BasicTankMPVersion2")
        return {
            "colors": [
                {
                    "component": "turret_door",
                    "mask": { "r": 255, "g": 0, "b": 225, "a": 255 }
                },
                {
                    "component": "turret_base",
                    "mask": { "r": 127, "g": 255, "b": 0, "a": 255 }
                },
                {
                    "component": "grillmask",
                    "mask": { "r": 200, "g": 200, "b": 225, "a": 255 }
                }
            ]
        }
}

module.exports.getGuid = function (username) {
    if (username === "alastair") return "ed801941-1e39-4f3d-8b99-c71d290690cc";
}

module.exports.apiGetPrivateUserData = function (request, response) {
    if (request.param("token") === "auth_token" || request.param("token") === "auth_token_2")
        response.json({
            "username": "alastair",
            "guid": module.exports.getGuid("alastair"),
            "token": request.token,
            "hostCredits": 100000,
            "valid": settings.authTokenValidityLengthSeconds,
        });
    else response.status(404).json({
        "error": "token_invalid",
        "message": "TokenInvalidReenterCredentials"
    })

}