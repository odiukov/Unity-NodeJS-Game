var port = process.env.PORT || 3000;
var io = require('socket.io')(port);
var shortId = require('shortid');

var players = [];

var playerSpeed = 3;

console.log("server started on port " + port);

io.on('connection', function (socket) {
    
    var thisPlayerId = shortId.generate();
    var player = {
        id:thisPlayerId,
        destination:{
        x:0,
        y:0    
        },
        lastPosition:{
            x:0,
            y:0
        },
        lastMoveTime : 0
    };
    players[thisPlayerId] = player;
    
    console.log("client connected, id = ", thisPlayerId);
   
   socket.emit('register', {id:thisPlayerId});
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
        
        player.destination.x = data.d.x;
        player.destination.y = data.d.y;
        
        var elapsedTime = Date.now() - player.lastMoveTime;
        
        var travelDistanceLimit = elapsedTime * playerSpeed / 1000;
        
        var requestedDistanceTraveled = lineDistance(player.lastPosition, data.c);
        
        player.lastMoveTime = Date.now();
        
        player.lastPosition = data.c;
        
        delete data.c;
        
        data.x = data.d.x;
        data.y = data.d.y;
        
        delete data.d;
        
        socket.broadcast.emit('move', data);
    });
    
     socket.on('follow', function (data) {
        data.id = thisPlayerId;
        console.log("follow request: ", data);
        socket.broadcast.emit('follow', data);
    });
    
    socket.on('updatePosition', function (data) {
        console.log("update position: ", data);
        data.id = thisPlayerId;
        socket.broadcast.emit('updatePosition', data);
    });
    
    socket.on('attack', function (data) {
        console.log("attack request: ", data);
        data.id = thisPlayerId;
        io.emit('attack', data);
    });
    
    socket.on('disconnect', function () {
        console.log('client disconected');
        delete players[thisPlayerId];
        socket.broadcast.emit('disconnected', {id:thisPlayerId});
    });
});

function lineDistance(vectorA, vectorB) {
    var xs = 0;
    var ys = 0;
    
    xs = vectorB.x - vectorA.x;
    xs = xs * xs;
    
    ys = vectorB.y - vectorA.y;
    ys = ys * ys;
    
    return Math.sqrt(xs + ys);
}