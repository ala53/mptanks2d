//
// Multithreading helpers for nodejs
// Normally, this would work easily but web workers
// will not compile on iojs / nwjs 0.12. Until then
// this is just an empty wrapper for future multithreading
// For reference: https://github.com/audreyt/node-webworker-threads
//

//TODO IMPLEMENT THIS

var threading = threading || {}

var threadpoolSize = require('os').cpus().length - 1
    //1 thread less than the cpu count because render thread

threading.serialize = function(obj, ndeep) {
    //better serialization so we can send functions too
    //blatant code theft from http://stackoverflow.com/questions/5612787#27005746
    switch(typeof obj){
        case "string": return '"'+obj+'"';
        case "function": return obj.toString();
        case "object":
            var indent = Array(ndeep||1).join('\t'), isArray = Array.isArray(obj);
            return ('{['[+isArray] + Object.keys(obj).map(function(key){
                return '\n\t' + indent +(isArray?'': key + ': ' )+
                objToString(obj[key], (ndeep||1)+1);
            }).join(',') + '\n' + indent + '}]'[+isArray])
            .replace(/[\s\t\n]+(?=(?:[^\'"]*[\'"][^\'"]*[\'"])*[^\'"]*$)/g,'');
            default: return obj.toString();
        }
}

threading.deserialize = function(serialized) {
	return eval(serialized)
}

/**
 * An individual thread to run.
 *
 * @param {Function} callFunction The function to call on another thread
 * @param {Object} param The parameter object to pass to the task.
 * @param {Function} callback The callback to run upon thread termination,
 *  passing the task's result. Signature: function callback(thread, error, result)
*/
threading.thread = function(callFunction, param, callback) {
    this.func = callFunction
    this.params = param
    this.callback = callback

    this.blob = new Blob(callFunction.toString(),
        {type: 'application/javascript'})
    this.blobUrl = URL.createObjectUrl(this.blob)

    this.worker = new Worker(this.blobUrl)

    try {
        var result = null
        this.state = "running"
        //multithreading goes here / TODO
        result = callFunction(param)
        //and flag completion
        this.state = "completed"
        callback (this, null, result)
    } catch (ex) {
        this.state = "faulted"
        callback(this, ex, null)
    }
}

threading.thread.prototype.running() {
    return this.state == "running"
}

threading.thread.prototype.faulted() {
    return this.state == "faulted"
}

threading.thread.prototype.completed() {
    return this.state == "completed"
}

/**
 * Posts a value to the thread if it is
 * running at this time
*/
threading.thread.prototype.post(value) {

}

/**
 * Registers a function to be called when data
 * is received from the other thread
*/
threading.thread.prototype.onReceive(callback) {
    this.receiveMessageCallback = callback
}

/**
 * Waits for a task to complete execution.
*/
threading.thread.prototype.wait() {
    //do nothing
}

/**
 * Forcibly stops a task from running.
*/
threading.thread.prototype.stop() {
    //do nothing
}


/**
 * A task that is ran on the threadpool
 *
 * @param {Function} callFunction The function to call on the threadpool.
 * @param {Object} param The scope to pass to the task.
 * @param {Function} callback The callback to run upon task completion,
 *  passing the task's result. Signature: function callback(thread, error, result)
*/
threading.task = function(callFunction, param, callback) {
    this.func = callFunction
    this.params = param
    this.callback = callback
    try {
        var result = null
        this.state = "running"
        //multithreading goes here / TODO
        result = callFunction(param)
        //and flag completion
        this.state = "completed"
        callback (this, null, result)
    } catch (ex) {
        this.state = "faulted"
        callback(this, ex, null)
    }
}

threading.task.prototype.isRunning() {
    return this.state == "running"
}

threading.task.prototype.faulted() {
    return this.state == "faulted"
}

threading.task.prototype.completed() {
    return this.state == "completed"
}

/**
 * Waits for a task to complete execution.
*/
threading.task.prototype.wait() {

}

/**
 * Forcibly stops a task from running.
*/
threading.task.prototype.stop() {

}

/**
 * Blocks execution until the threadpool queue is empty.
 * Unsurprisingly, calling this from a task will break everything.
*/
threading.task.waitForQueue() {

}
