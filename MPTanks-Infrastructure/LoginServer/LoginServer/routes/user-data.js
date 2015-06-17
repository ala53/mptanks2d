module.exports.getUserData = function (guid) {
    if (guid === "ed801941-1e39-4f3d-8b99-c71d290690cc")
        return {
            "isPremium": true,
            "hostCredits": 100000,
            "tanksWithCustomTextures": [
                {
                    "name": "Basic Tank Type 2",
                    "reflection": "BasicTankMPVersion2",
                    "imageUrl": "http://tanktextures.usercontent.zsbgames.me/ed801941-1e39-4f3d-8b99-c71d290690cc/BasicTankMPVersion2.png"
                }
            ],
            "tanksWithCustomColors": [
                {
                    "name": "Basic Tank Type 2",
                    "reflection": "BasicTankMPVersion2",
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
            ]
        };
}

module.exports.getGuid = function (username) {
    if (username === "alastair") return "ed801941-1e39-4f3d-8b99-c71d290690cc";
}