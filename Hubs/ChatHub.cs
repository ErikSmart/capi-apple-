using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capi.Hubs
{
    public class ChatHub : Hub
    {
        private DataContext context;
        public ChatHub(DataContext context)
        {
            this.context = context;
        }
        public async Task<ActionResult<IEnumerable<Entities.Producto>>> SendMessage()
        {
            var producto = await context.productos.ToListAsync();
            await Clients.All.SendAsync("BroadcastMessage", producto.Count);
            return producto;
        }
    }
}