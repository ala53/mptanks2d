//
// Timespans
// Literally, a timespan class
//
//

var game = game || {}
game.core = game.core || {}


/*

    Begin section for timespans

*/

game.core.timespan = function (microseconds) {
    return Object.freeze({
        ticks: microseconds,
        get milliseconds() {
            return microseconds / 1000
        },
        get seconds() {
            return milliseconds / 1000
        },
        get minutes() {
            return seconds / 60
        },
        get hours() {
            return minutes / 60
        },
        get days() {
            return hours / 24
        }
    })
}

game.core.timespan.fromMilliseconds = function (milliseconds) {
    return game.core.timespan(milliseconds * 1000)
}

game.core.timespan.fromSeconds = function (seconds) {
    return game.core.timespan.fromMilliseconds(seconds * 1000)
}

game.core.timespan.fromMinutes = function (minutes) {
    return game.core.timespan.fromSeconds(minutes * 60)
}

game.core.timespan.fromHours = function (hours) {
    return game.core.timespan.fromMinutes(hours * 60)
}

game.core.timespan.fromDays = function (days) {
    return game.core.timespan.fromHours(days * 24)
}
