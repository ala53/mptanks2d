
/**
 * Module dependencies.
 */

var express = require('express');
var routes = require('./routes');
var createAccount = require('./routes/create-account.js');
var login = require('./routes/login.js');
var tokenAuth = require('./routes/token-auth.js')
var http = require('http');
var path = require('path');
var settings = require('./settings.js');
var userdata = require('./routes/user-data.js')

var app = express();

// all environments
app.set('port', process.env.PORT || settings.port);
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');
app.use(express.favicon());
app.use(express.logger('dev'));
app.use(express.json({ limit: settings.maxUploadSize }));
app.use(express.urlencoded({ limit: settings.maxUploadSize }));
app.use(express.methodOverride());
app.use(app.router);
app.use(require('stylus').middleware(path.join(__dirname, 'public')));
app.use(express.static(path.join(__dirname, 'public')));

// development only
if ('development' == app.get('env')) {
    app.use(express.errorHandler());
}

app.get('/', routes.index);

app.post('/api/login', login.apiLogin);
app.get('/login', login.showLoginPage);

app.get('/api/get-private-user-data/:token', userdata.apiGetPrivateUserData);
//get /api/verify-token/auth_token
app.get('/api/verify-token/:token', tokenAuth.authenticateToken);
//get /api/refresh-token/auth_token
app.get('/api/refresh-token/:token', tokenAuth.refreshToken);
//get /api/get-server-token/auth_token
app.get('/api/get-server-token/:token', tokenAuth.getServerToken);
//get /api/verify-server-token/SERVER_TOKEN
app.get('/api/verify-server-token/:token', tokenAuth.authenticateServerToken);

app.post('/api/create-account', createAccount.apiCreateAccount);
app.get('/create-account', createAccount.showCreateAccountPage);

//get /api/user-info/ed801941-1e39-4f3d-8b99-c71d290690cc
app.get('/api/user-info/:id', userdata.apiGetUserData);

http.createServer(app).listen(app.get('port'), function () {
    console.log('Express server listening on port ' + app.get('port'));
});
