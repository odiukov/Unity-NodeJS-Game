var port = process.env.PORT || 3000;
var io = require('socket.io')(port);
var shortId = require('shortid');

var players = [];

console.log("server started on port " + port);

io.on('connection', function (socket) {
    
    var thisPlayerId = shortId.generate();
    var player = {
      id:thisPlayerId,
      x:0,
      y:0    
    };
    players[thisPlayerId] = player;
    
    console.log("client connected, id = ", thisPlayerId);
   
    socket.broadcast.emit('spawn', {id:thisPlayerId});
    socket.broadcast.emit('requestPosition');
    
    for(var playerId in players){
        if(playerId == thisPlayerId)
            continue;
        socket.emit('spawn', players[playerId]);
    };
    
    
    socket.on('move', function (data) {
        data.id = thisPlayerId;
        console.log('client moved', JSON.stringify(data));
        
        player.x = data.x;
        player.y = data.y;
        
        socket.broadcast.emit('move', data);
    });
    
    socket.on('updatePosition', function (data) {
        data.id = thisPlayerId;
        socket.broadcast.emit('updatePosition', data);
    });
    
    socket.on('disconnect', function () {
        console.log('client disconected');
        delete players[thisPlayerId];
        socket.broadcast.emit('disconnected', {id:thisPlayerId});
    });
});