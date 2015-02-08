

var filesystem = filesystem || {}

var fs = require('fs')

filesystem.homeDir = (function () {
    //gets the user's home directory
    var path = process.env[
        (process.platform == 'win32') ?
        'USERPROFILE' : 'HOME']
    //if our game folder exists, return that
    if (fs.existsSync(path + '/games/mptanks')) {
        return new filesystem.folder(path)
    }
    else {
        //if it doesn't, create the folder
        if (!fs.existsSync(path + '/games'))
            fs.mkdirSync(path + '/games')
        fs.mkdirSync(path + '/games/mptanks')
        return new filesystem.folder(path)
    }
})()

filesystem.installDir = (function() {
    return new filesystem.folder(
        window.location.pathname.substring
        (1, window.location.pathname.lastIndexOf('/')
        + 1))
})()


filesystem.folder = function (fullPath) {
    this.path = fullPath
    Object.defineProperty(this, "files", {get: function () {
        var fl = fs.readdir(this.path)

        var files = {}
        for (var i = 0; i < fl.length; i++) {
            if (name === "." || name === "..")
                break;
            var name = fl[i]
            if (!fs.lstatSync(this.path + "/" + name).isDirectory())
                files[name] = new filesystem.file(this.path + '/' + name)
        }

        return files
    }})
    Object.defineProperty(this, "folders", {get: function () {
        var fl = fs.readdir(this.path)

        var folders = {}
        for (var i = 0; i < fl.length; i++) {
            if (name === "." || name === "..")
                break;
            var name = fl[i]
            if (fs.lstatSync(this.path + "/" + name).isDirectory())
                folders[name] = new filesystem.folder(this.path + '/' + name)
        }

        return folders
    }})
}

/**
 * Tries to create a subfolder in the current folder
 *
 * @param {string} folder The name of the folder to write
 *
 * @returns a reference to the folder
*/
filesystem.folder.prototype.createSubfolder = function (name) {

}

filesystem.folder.prototype.deleteSubfolder = function (name) {

}

filesystem.folder.prototype.createFile = function (name) {

}

filesystem.file = function (fullPath) {
    this.path = fullPath
}

filesystem.file.prototype.delete = function () {

}

filesystem.file.prototype.open = function () {

}

filesystem.file.prototype.readString = function () {
    return fs.readFileSync(this.path, "utf8")
}

filesystem.file.prototype.readLines = function () {
    return this.readString().split('\n')
}

filesystem.file.prototype.readBinary = function () {

}

filesystem.file.prototype.write = function (data) {

}
