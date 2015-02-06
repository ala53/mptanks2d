//
// Renderer component
// A wrapper for pixijs to have a standard rendering api
//
//


var rendering = rendering || {}

/**
 * Creates a new renderer object, currently uses pixijs
 *
 * @class
*/
rendering.renderer = function () {

}

rendering.renderer.prototype.initialize = function (canvas) {
	//bind resize
	$(window).bind('resize', function() {
		var nW = $(window).width(), nH = $(window).height()
		$(canvas).width(nW)
		$(canvas).height(nH)
		this.pixi.resize(nW, nH)
	})

	this.pixi = new PIXI.autoDetectRenderer($(canvas).width(),
		$(canvas).height(), { view: canvas })
	this.pixiStage = new PIXI.Stage(0x000000)

	this.dirty = false
}

rendering.renderer.prototype.drawComponent = function (component, worldOffset) {
	this.dirty = true
}

rendering.renderer.prototype.drawComponents = function (components, worldOffset) {
	this.dirty = true

	for (id in components)
		this.drawComponent(components[id], worldOffset)
}

rendering.renderer.prototype.render = function () {
	this.pixi.render(this.pixiStage)
}


/**
 * Tries to free the resources used by the renderer
*/
rendering.renderer.prototype.dispose = function () {

}
