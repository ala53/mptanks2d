/*
 *
 * Global class for MPTanks
 * Here we inject all dependencies, load the game code,
 * precompile the modules and sandbox them
 *
*/
var fs = require('fs')
var vm = require('vm')

//And the global for whether we're a server or a client
var client = true, server = true

var gameDef = require('./globals/game.json')
var dependenciesDef = require('./globals/dependencies.json')
var sandboxedDef = require('./globals/plugins.json')

var global = Function('return this')()

var base = "game"

var gameFolder = "code"
var thirdPartyFolder = "thirdparty"
var sandboxFolder = "plugins"

var sandboxedModules = {}

function initializeGame () {
	console.log('Starting to load scripts.')
	loadScripts() //load all the scripts from disk
	console.log('Scripts have been loaded successfully.')
	console.log('Initializing game')

	//and defer before starting up the engine
	//so v8 has time to compile/parse everything
	setTimeout(function() {
		game.initialize(sandboxedModules)
	}, 500)
}

function loadScripts () {
	for (var id in dependenciesDef) {
		var dependency = dependenciesDef[id]
		console.log("[Dependency] Loading " + dependency.name + " (" +
			dependency.version + ") - " + dependency.license)

		for (var id in dependency.files)
			loadScript(thirdPartyFolder + "/" + dependency.files[id])
	}

	for (var id in gameDef) {
		var gameClass = gameDef[id]
		console.log("[GameClass] Loading " + gameClass.name + " (" +
			gameClass.version + ")")

		for (var id in gameClass.files)
			loadScript(gameFolder + "/" + gameClass.files[id])
	}

	for (var id in sandboxedDef) {
		var sandbox = sandboxedDef[id]
		console.log("[Module] Loading " + sandbox.name + " (" +
			sandbox.version + ") - " + sandbox.license)
		var sandboxedScope = handleSandboxing(sandbox)
		sandboxedModules[sandbox.name] = sandboxedScope
	}
}

function loadScript(file) {
	var includeInThisContext = function(path) {
		var head = document.getElementsByTagName('head')[0];
		var script = document.createElement('script');
		script.type = 'text/javascript';
		script.charset = 'utf-8';
		script.name = path;
		script.src = path;
		head.appendChild(script);
	}

	includeInThisContext(file)
}

function handleSandboxing(asset) {
	//We create a wrapper object so it is easy to instantiate
	//the module as an instance
	var module = {
		name: asset.name,
		asset: asset,
		createInstance: function (globalScope) {
			precompile()

			//create the sandbox
			var sb = new sandbox(undefined, globalScope)
			sb.addScript(code) //add the scripts
		},
		/**
		 * Compiles the module to avoid the overhead of loading and
		 * compiling it upon first use, which may be in-game
		*/
		compile: function () {
			if (module.code == undefined || module.code == null) {
				code = []
				for (var id in asset.files)
					code.push(sandbox.compile(fs.readFileSync(
						base + "/" + sandboxFolder + "/" + asset.files[id])))
				module.code = code
			}
		}
	}

	return module
}
