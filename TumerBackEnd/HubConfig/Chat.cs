﻿using Microsoft.AspNetCore.SignalR;
using TumerBackEnd.HubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace TumerBackEnd.HubConfig
{
    //4Tutorial
    public partial class MyHub
    {
        public async Task getOnlineUsers()
        {
            Guid currUserId = (Guid)ctx.Connections.Where(c => c.SignalrId == Context.ConnectionId).Select(c => c.PersonId).SingleOrDefault();
            List<User> onlineUsers = ctx.Connections
                .Where(c => c.PersonId != currUserId)
                .Select(c =>
                    new User((Guid)c.PersonId, ctx.Person.Where(p => p.Id == c.PersonId).Select(p => p.Name).SingleOrDefault(), c.SignalrId)
                ).ToList();
            await Clients.Caller.SendAsync("getOnlineUsersResponse", onlineUsers);
        }


        public async Task sendMsg(string connId, string msg)
        {
            await Clients.Client(connId).SendAsync("sendMsgResponse", Context.ConnectionId, msg);
        }
    }
}