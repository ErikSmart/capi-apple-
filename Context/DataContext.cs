using System;
using Capi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Capi.Modelos;

namespace Capi
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Producto> productos { get; set; }
        public DbSet<Detalle> detalles { get; set; }
        public DbSet<Cliente> clientes { get; set; }

    }
}