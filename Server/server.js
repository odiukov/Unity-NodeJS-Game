var port = process.env.PORT || 3000;
var io = require('socket.io')(port);

var playerCount = 0;
console.log("server started on port " + port);

io.on('connection', function (socket) {
    console.log("client connected");
    socket.broadcast.emit('spawn');
    playerCount++;
    
    for(var i = 0; i < playerCount; i++){
        socket.emit('spawn')
    }
    socket.on('move', function (data) {
        console.log('client moved');
    });
    
    socket.on('disconnect', function () {
        console.log('client disconected');
        playerCount--;
    })
});