#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace Server.Models;

public class TodoContext : DbContext 
{     
    public TodoContext(DbContextOptions options) : base(options) { }   

   public DbSet<TodoItem> TodoItems {get;set;}
   
}