
var io = require('socket.io')(process.env.PORT || 8081);
var playercount = 0;
var cse = 0 ;
var Score1;
 var Name1;
 var Score2 ;
 var Name2;
console.log('server started');

var express = require('express');
var app = express();
var mysql = require('mysql');

var connection = mysql.createConnection({
    host:'ec2-13-126-252-100.ap-south-1.compute.amazonaws.com',
   //host:'localhost',
    user:'sengokusoul',
    password:'0851408244Ss',
    database:'game1'
   
    });
    var putname;
    var putpass;
    var putscore;
    
    connection.connect(function (err)
    {
        if(err){
            console.log('Error Connecting',err.stack);
            return;
        }
        console.log('Connected as id',connection.threadId);
    })
    app.get ('/user/add/user',function(req,res)
    {
        var Name = req.query.Name;
        var Pasword = req.query.Pasword;

        var user =[[Name,Pasword]];
        queryAddUser(user,function(err,result)
        {
            res.end(result);
        });
      //  res.end(Name+Pasword);
      //  http://localhost:8081/user/add/user?Name=Lolo&Pasword=5231
    });
app.get ('/users',function(req,res)
{
//res.end('hello');
queryAllUser(function(err,result)
{
    res.end(result);
});
});

app.get ('/Topusers',function(req,res)
{
//res.end('hello');
queryTopTenUser(function(err,result)
{
    res.end(result);
});
});
app.get ('/user/:Name',function(req,res)
{
//res.end('hello');
var Name = req.params.Name;
putname = req.params.Name;
console.log(Name);
queryUser(function(err,result)
{
    res.end(result);
});
});

app.get ('/userpass/:Name/:Pasword',function(req,res)
{
//res.end('hello');
var Name = req.params.Name;
var Pasword = req.params.Pasword;
putname = req.params.Name;
putpass = req.params.Pasword;


//var user =[[Name,Pasword]];
console.log(putname);
queryCheckUser(function(err,result)
{
    res.end(result);
});
});
app.get ('/userscore/:Name/:Score',function(req,res)
{

var Name = req.params.Name;
var Score = req.params.Score;
putname = req.params.Name;
putscore = req.params.Score;


//var user =[[Name,Pasword]];
console.log(putname);
querySaveScore(function(err,result)
{
    res.end(result);
});
});
/*
app.get ('/user/Jay',function(req,res)
{
//res.end('hello');
var Name = req.params.Name;
putname = 'Jay';
console.log(Name);
queryUser(function(err,result)
{
    res.end(result);
});
});
app.get ('/user/Jack',function(req,res)
{
//res.end('hello');
var Name = req.params.Name;
putname = 'Jack';
console.log(Name);
queryUser(function(err,result)
{
    res.end(result);
});
});
app.get ('/user/Jj',function(req,res)
{
//res.end('hello');
var Name = req.params.Name;
putname = 'Jj';
console.log(Name);
queryUser(function(err,result)
{
    res.end(result);
});
});
*/
var server = app.listen (8081,function()
{
    console.log('Server: Running');
});

function queryAllUser (Callback)
{
    var json = '';
    connection.query('SELECT * FROM game1',function(err ,rows,fields)
{
    if(err)throw err;
    json = JSON.stringify(rows);
    Callback(null,json);
    
});
}

function queryUser (Callback)
{
    var json = '';
    connection.query("SELECT * FROM game1 WHERE Name ='"+putname+"';",function(err ,rows,fields)
{
    if(err)throw err;
    json = JSON.stringify(rows);
    Callback(null,json);
    
});
}
function queryAddUser (user,Callback)
{
    var sql = 'INSERT INTO game1 (Name,Pasword) values ?';
    connection.query(sql,[user],
        function(err){
            var result = '[{"success":"true"}]'
            
    if(err)
    {
        var result = '[{"success":"false"}]'
        throw err;
    }
    Callback(null,null);
    
});
}
function queryCheckUser (Callback)
{
    var json = '';
    connection.query("SELECT Name,Score, COUNT(Id) as Check_ID FROM game1 WHERE Name = '"+putname+"' and Pasword = '"+putpass+"';",function(err ,rows,fields)
{
    if(err)throw err;
    json = JSON.stringify(rows);
    Callback(null,json);
    
});
}

function queryTopTenUser (Callback)
{
    var json = '';
    connection.query('SELECT Name,Score FROM game1 ORDER BY length(Score) DESC , Score DESC LIMIT 10;',function(err ,rows,fields)
{
    if(err)throw err;
    json = JSON.stringify(rows);
    Callback(null,json);
    
});
}

function querySaveScore (Callback)
{
    var json = '';
    connection.query("UPDATE game1 SET Score ='"+ putscore+"' WHERE Name = '"+putname+"';",function(err ,rows,fields)
{
    if(err)throw err;
    json = JSON.stringify(rows);
    Callback(null,json);
    
});
}

io.on('connection',function(socket)
{
    console.log('Clinet Connected');

    socket.broadcast.emit('spawn');
    playercount++
    for(i = 0;i<playercount;i++)
    {
        socket.emit('spawn');
        console.log('sending spawned to new player');
    }

    socket.on('disconnect',function()
    {
        playercount--;
         cse --;
        console.log('client disconneted');
        console.log(cse);
    });

    socket.on('checkin',function(data)
    {
       
        console.log('client Score',data.Score);
        console.log('client Name',data.Name);
        
       // var Cs1   = data.Cs;
        cse+=1;
        if(cse == 1)
        {

             Score1 = data.Score;
             Name1 = data.Name;
            
            
             socket.broadcast.emit('Enemy',{Name:Name1,Score:Score1,Cs:cse});
            socket.emit('Enemy',{Name:Name1,Score:Score1,Cs:cse});
           
        }else
        if(cse == 2)
        {

             Score2 = data.Score;
             Name2 = data.Name;

            
            socket.broadcast.emit('Enemy',{Name:Name2,Score:Score2,Name1:Name1,Score1:Score1,Cs:cse});
            socket.emit('Enemy',{Name:Name2,Score:Score2,Name1:Name1,Score1:Score1,Cs:cse});
        }
      
        console.log(cse);
       
   
    });
    socket.on('AddP1sc',function()
    {
        socket.broadcast.emit('P1',{P1score:100});
        socket.emit('P1',{P1score:100});
    });
    socket.on('AddP2sc',function()
    {
        socket.broadcast.emit('P2',{P2score:100});
        socket.emit('P2',{P2score:100});
    });
    socket.on('StartTime',function()
    {
        console.log("StartGame");
        socket.broadcast.emit('gamestart',{ct:1});
        socket.emit('gamestart',{ct:1});
    });

});


