using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capi.Entities;
using Capi.Hubs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Capi.Controllers
{
    [ApiController]
    [EnableCors("PermitirApiRequest")]
    public class ChatController : ControllerBase
    {
        private IHubContext<ChatHub> _hub;
        private DataContext context;
        public ChatController(IHubContext<ChatHub> hub, DataContext context)
        {
            _hub = hub;
            this.context = context;
        }
        [Route("mihub")]
        [HttpGet]
        public async Task<ActionResult<Producto>> Get()
        {


            var producto = await context.productos.ToListAsync();
            var timerManager = _hub.Clients.All.SendAsync("transferchartdata", producto);
            return Ok(new { Message = "Request Completedo" });
        }
    }
}