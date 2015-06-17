
/**
 * Module dependencies.
 */

var express = require('express');
var routes = require('./routes');
var user = require('./routes/user');
var createAccount = require('./routes/create-account.js');
var login = require('./routes/login.js');
var tokenAuth = require('./routes/token-auth.js')
var http = require('http');
var path = require('path');
var settings = require('./settings.js');

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
app.get('/users', user.list);

app.post('/api/login', login.apiLogin);
app.get('/login', login.showLoginPage);

app.post('/api/verify-token', tokenAuth.authenticateToken);
app.post('/api/refresh-token', tokenAuth.refreshToken);
app.post('/api/get-server-token', tokenAuth.getServerToken);
app.post('/api/verify-server-token', tokenAuth.authenticateServerToken);

app.post('/api/create-account', createAccount.apiCreateAccount);
app.post('/create-account', createAccount.showCreateAccountPage);

http.createServer(app).listen(app.get('port'), function () {
    console.log('Express server listening on port ' + app.get('port'));
});
