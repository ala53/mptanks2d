var input = input || {}

/**
 * Creates an immutable input state instance.
 * 
 * @param {Number} movementSpeed The normalized speed, between -1 and +1, that the tank will move at.
 * @param {Number} rotationSpeed The rotation of the tank, between 0 and 360 degrees.
 * @param {Number} turretRotationSpeed The rotation of the barrel relative to the tank, between 0 and 360 degrees.
 * @param {Number} turretPower The power on the tank, between 0 and 1. Only applies to certain weapons and 1 
 *		is screen edge, while 0 is screen center.
 *
 * @param {bool} primaryFireActive Whether the primary fire button is pressed.
 * @param {bool} secondaryFireActive Whether the secondary fire button is pressed.
 * @param {bool} tertiaryFireActive Whether the tertiary fire button is pressed.
 *
 * @constructor
*/
input.inputState = function(movementSpeed, rotationSpeed, turretRotationSpeed, turretPower,
	primaryFireActive, secondaryFireActive, tertiaryFireActive) {
	this.movementSpeed = movementSpeed
	this.rotationSpeed = rotationSpeed
	this.turretRotationSpeed = turretRotationSpeed	
	this.turretPower = turretPower
	this.primaryFireActive = primaryFireActive
	this.secondaryFireActive = secondaryFireActive
	this.tertiaryFireActive = tertiaryFireActive
	Object.freeze(this)
}

/**
 * Merges 2 input states.
 *
 * @param {inputState} b The second state to merge
*/
input.inputState.prototype.merge = function(b) {
	return input.inputState.merge(this, b)
}

input.inputState.merge = function(a, b) {
	var output = {}
	
	if (b.movementSpeed > 0.05 || b.movementSpeed < -0.05)
		output.movementSpeed = b.movementSpeed
	else
		output.movementSpeed = a.movementSpeed
		
	if (b.rotationSpeed > 0.05 || b.rotationSpeed < -0.05)
		output.rotationSpeed = b.rotationSpeed
	else
		output.rotationSpeed = a.rotationSpeed
		
	if (b.turretRotationSpeed > 0.05 || b.turretRotationSpeed < -0.05)
		output.turretRotationSpeed = b.turretRotationSpeed
	else
		output.turretRotationSpeed = a.turretRotationSpeed
		
	if (b.turretPower > 0.05 || b.turretPower < -0.05)
		output.turretPower = b.turretPower
	else
		output.turretPower = a.turretPower
	
	output.primaryFireActive = a.primaryFireActive || b.primaryFireActive
	output.secondaryFireActive = a.secondaryFireActive || b.secondaryFireActive
	output.tertiaryFireActive = a.tertiaryFireActive || b.tertiaryFireActive
	
	//and create an immutable return object
	return new input.inputState(
		output.movementSpeed, output.rotationSpeed,
		output.turretRotationSpeed, output.turretPower,
		output.primaryFireActive, output.secondaryFireActive,
		output.tertiaryFireActive)
}
/**
 * A representation of input for the UI, rather than the game
 * 
 * @constructor
 * @param {bool} backPressed Is the back key pressed
 * @param {bool} selectPressed Is the enter/select key pressed
 * @param {bool} leftPressed Is the left key pressed
 * @param {bool} rightPressed Is the right key pressed
 * @param {bool} upPressed Is the up key pressed
 * @param {bool} downPressed Is the down key pressed
*/
input.uiInputState = function (backPressed, selectPressed, leftPressed,
	rightPressed, upPressed, downPressed) {
	this.backPressed = backPressed
	this.selectPressed = selectPressed
	this.rightPressed = rightPressed
	this.upPressed = upPressed
	this.downPressed = downPressed
	
	Object.freeze(this)
}

/**
 * Merges 2 input states
 * @param {uiInputState} b The second object to merge
*/
input.uiInputState.prototype.merge = function(b) {
	return input.uiInputState.merge(this, b)
}

input.uiInputState.merge = function(a, b) {
	var output = {}
	
	output.backPressed = a.backPressed || b.backPressed
	output.selectPressed = a.selectPressed || b.selectPressed
	output.leftPressed = a.leftPressed || b.leftPressed
	output.rightPressed = a.rightPressed || b.rightPressed
	output.upPressed = a.upPressed || b.upPressed
	output.downPressed = a.downPressed || b.downPressed
	
	//and return a new immutable object
	return new input.uiInputState(
		output.backPressed, output.selectPressed, 
		output.leftPressed, output.rightPressed,
		output.upPressed, output.downPressed
	)
}