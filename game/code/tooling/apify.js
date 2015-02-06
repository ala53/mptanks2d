//
// Api helper
// Helper method for securing an object as an API accessible by 
// sandboxed, untrusted code. For now, it just runs Object.freeze()
//

var apify = function (object) {
	return Object.freeze(object)
}