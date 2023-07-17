const { Server } = require('socket.io');
const { io } = require('socket.io-client');

const server = new Server(3001);
const client = io.connect("http://localhost:3000/", {
    reconnection: true
});

client.on('connect', function(socket) {
    console.log('connected to server');

    client.on('listener::message', function(message) {
        handleListenerMessage(message);
    });
});

server.on('connection', function(socket) {
    console.log('[ws] Client connected');

    socket.on('persistence::message', function(message) {
        console.log('message received from persistence', message);
        handlePersistenceMessage(socket, message);
    });
});

// enviar mensaje a persistence
function handleListenerMessage(message) {
    server.emit('negocio::message', {
        source: 'NEGOCIO',
        method: message.method,
        action: message.action,
        entity: message.entity,
        body: message.body
    })
}

// enviar mensaje a listener
function handlePersistenceMessage(socket, message) {
    client.emit('negocio::message', {
        source: 'NEGOCIO',
        method: message.method,
        action: message.action,
        entity: message.entity,
        body: message.body
    });
}