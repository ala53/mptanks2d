//
// Game core module
// The central code for the game. This manages
// lifetime and update loops
//

var game = game || {}

game.running = false
game.startTime = performance.now()

game.lastFrame = performance.now()

game.initialize = function (sandboxedModules) {
	game.running = true
	requestAnimationFrame(game.updateLoop) //start somewhere

	game.renderer = new rendering.renderer()

	game.renderer.initialize(document.getElementById("gameArea"))

	game.startTime = performance.now()
	game.lastFrame = performance.now()
}

/**
 * The update loop for the game.
 *
 * @private
 * @static
*/
game.updateLoop = function () {
	//make sure we're running before we schedule the next frame
	if (game.running)
		requestAnimationFrame(game.updateLoop)
	else
		return;

	var time = {
		total: new game.core.timespan(performance.now() - game.startTime),
		delta: new game.core.timespan(performance.now() - game.lastFrame),
	}
	//And call the update function
	game.update(time)

	//And the render function
	var time = {
		total: new game.core.timespan(performance.now() - game.startTime),
		delta: new game.core.timespan(performance.now() - game.lastFrame),
	}
	game.draw(time)

	game.lastFrame = performance.now()

}

game.update = function (time) {
	//update code here

	for (var id in game.updateFunctions)
		game.updateFunctions[id](time)
}

game.draw = function (time) {
	//draw code here

	for (var id in game.drawFunctions)
		game.drawFunctions[id](time)

	game.renderer.render()
}

game.stop = function () {
	game.running = false
}

/**
 * The collection of functions to be called during updates.
 * @private
*/
game.updateFunctions = []

/**
 * Adds a function to be called during the update loop
 * (signature: function(time) where time is
 * {total: game.core.timespan, delta: game.core.timespan }
 *
 * @param {Function} func The function to call during the update loop
*/
game.registerUpdateFunction = function (func) {
	game.updateFunctions.push(func)
}

/**
 * The collection of functions to be called during updates.
 * @private
*/
game.drawFunctions = []

/**
 * Adds a function to be called during the draw loop
 * (signature: function(time) where time is
 * {total: game.core.timespan, delta: game.core.timespan }
 *
 * @param {Function} func The function to call during the draw loop
*/
game.registerDrawFunction = function (func) {
	game.drawFunctions.push(func)
}
