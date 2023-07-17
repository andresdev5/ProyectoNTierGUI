const net = require('net');
const { Server } = require('socket.io');
const { parseSocketResponse, buildSocketMessage } = require('./utils');

const socketServer = net.createServer(function(socket) {
	socket.pipe(socket);
});
let webSocket = null;
let netSocket = null;

socketServer.on('error', function(err) {
	console.log('Server error:', err.message);
});

socketServer.on('close', function() {
	console.log('Server closed');
});

// listen messages
socketServer.on('connection', function(socket) {
	netSocket = socket;

	socket.on('data', function(data) {
		try {
			const message = parseSocketResponse(data.toString());
			handleSocketMessage(socket, message);
		} catch (exception) {
			console.log(exception);
		}
	});

	socket.on('error', function(err) {
		if (err.code !== 'ECONNRESET') {
			console.log('Socket error:', err);
		}
	});
});

socketServer.listen(5555, '127.0.0.1', () => {
	console.log('Server started');

	const ws = new Server(3000);
	
	ws.on('connection', function(socket) {
		webSocket = socket;

		console.log('[ws] Client connected');

		socket.on('negocio::message', function(message) {
			console.log('message received from negocio', message);
			const response = buildSocketMessage('LISTENER', 'OUTPUT', message.entity, message.action, message.body);
			netSocket.write(response);
		});
	});
});

/**
 * 
 * @param {net.Socket} socket 
 * @param {{model: any, type: string, source: string}} data 
 */
function handleSocketMessage(socket, message) {
	webSocket.emit('listener::message', message);
}