//
// Sandboxing module
// Sandboxes untrusted code in it's own little context.
// Not the most secure thing in the world but if the game
// is kept updated, it'll be pretty hard to break as we
// wrap everything in strict mode and block "require" calls
//

vm = vm || require('vm')

/**
 * 
 *
 * @param {String/sandbox.compile} code The code to use for the sandbox. Either
 *  from sandbox.compile or a string
 *
 * @param {Object} context The context to run the code in, an arbitrary
 *  object.
 * 
 * @param {String} name For debugging purposes, the name of the script
 *  to show in the debugger when errors occur (optional)
 *
 * @constructor
 *
*/
sandbox = function(code, context, name) {
	if (context == undefined || context == null)
		this.global = {}
	else
		this.global = context
		
	if (typeof code == 'string') {
		this.script = sandbox.compile(code, name)
		this.context = vm.createContext(context)
		script.runInContext(context)
		
	} else if (code == undefined || code == null) {
		//Create an empty context
		this.context = vm.createContext(this.global)
	} else {
		this.context = vm.createContext(this.global)
		script.runInContext(context)
	}
}

/**
 * Adds a script to the sandbox. 
 * @param {String/sandbox.compile} code The code to add to the sandbox.
 * @param {String} name (optional) The name to display in the debugger.
*/
sandbox.prototype.addScript = function(code, name) {
	if (typeof code == 'string') {
		var tmp = sandbox.compile(code, name)
		tmp.runInContext(this.context)		
	} else {
		code.runInContext(this.context)
	}
}

/**
 * Injects a value into the sandbox but runs it on the vulnerable host context.
 * (properties defined by the sandbox are theoretically ran in the sandbox instead)
 * Allows for shared state.
 *
 * @param {String} name The name of the value as seen by the sandboxed code.
 *
 * @param {*} value The value to expose to the sandbox
*/
sandbox.prototype.setHostValue = function(name, value) {
	this.context[name] = value
}

/**
 * Allows you to safely inject a value into the sandbox by JSON transfer.
 * Much slower than setHostValue, doesn't allow for shared state, and 
 * does not support some data types, but it is (almost) guaranteed safe.
 *
 * @param {String} name The name of the value as seen by the sandboxed code.
 *
 * @param {Object} value The value to expose to the sandbox. Must contain only JSON types.
*/
sandbox.prototype.injectSandboxValue = function(name, value) {
	//we have to deep clone to avoid letting them run host level code
	vm.runInContext("var " + name + "=JSON.parse(" + JSON.stringify(value) + ");", this.context)
}

/**
 * Calls a method in the sandbox, directly passing arguments.
 *
 * @param {String} name The name of the function to call.
 *
 * @param {...} args The arguments to call the function with.
 *
 * @return {*} The return value of the function.
*/
sandbox.prototype.execute = function(name, args) {
	return this.context[name](Array.prototype.slice.call(arguments, 1))
}

/**
 * Calls a method safely from the sandbox by JSON cloning the arguments.
 *
 * @param {String} name The name of the function to call.
 *
 * @param {...} args The arguments to call the function with.
 *  Must only contain JSON types.
 *
 * @return {*} The return value of the function, only JSON types allowed.
*/
sandbox.prototype.executeSafe = function(name, args) {
	vm.runInContext("var ___ss3mficd___ = JSON.stringify(" + name + 
		".apply(JSON.parse(" + 
		JSON.stringify(Array.prototype.slice.call(arguments, 1)) +
		")))", this.context)
	
	var result = this.context["___ss3mficd___"]
	
	vm.runInContext("___ss3mficd___ = undefined;", this.context)
	
	return JSON.parse(result)
}

/**
 * Compiles code for use in many sandbox instances.
 * This precompiliation should speed up the creation of sandboxes drastically.
 *
 * @param {String} code The javascript source to compile.
 * @param {String} name (optional} The name of the code to display in the debugger.
*/
sandbox.compile = function(code, name) {
	if (typeof code != 'string')
		throw new Error('Code must be compiled from a string of source code!')
	
	if (name == undefined || name == null)
		return new vm.Script("'use strict';" + code, { displayErrors: true })
	else
		return new vm.Script("'use strict';" + code, { filename: name, displayErrors: true })
}
