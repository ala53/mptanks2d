//
// Unified Input Wrapper
// Description: this is the unified wrapper that handles input from multiple 
//     sources and aggregates the data into a single inputState
//
var input = input || {}

input.initialize = function() {
	for (var device in input.handlers) {
		if (!device.initialized) {
			device.initialize()
			device.initialized = true
		}
	}
}

input.getInputState = function() {
	var states = []
	//get all the states
	for (var device in input.handlers) {
		if (!device.initialized) {
			device.initialize()
			device.initialized = true
		}
		states.push(device.getInputState())
	}	
	//then merge them to get the best option
	var finalState = new input.inputState()
	
	for (var state in states)
		finalState.merge(state)
	
	return finalState	
}

input.getUiInputState = function() {
	var states = []
	//get all the states
	for (var device in input.handlers) {
		if (!device.initialized) {
			device.initialize()
			device.initialized = true
		}
		states.push(device.getUiInputState())
	}	
	
	//then merge to get the best option
	var finalState = new input.uiInputState()
	
	for (var state in states)
		finalState.merge(state)
	
	return finalState
}

input.close = function() {
	for (var device in input.handlers) {
		if (device.initialized) {
			device.initialized = false
			device.close()
		}
	}
}