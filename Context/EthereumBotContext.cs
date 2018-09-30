using System;
using Microsoft.EntityFrameworkCore;
using Ether_bot.Models;

namespace Ether_bot.Context
{
    public class EthereumBotContext:DbContext
    {
        public DbSet<UserModel> Users {get;set;}
        public DbSet<CurrencyModel> Currencies {get;set;}
        public DbSet<ExchangeModel> Exchanges {get;set;}
        public DbSet<CommandsModel> Commands {get;set;}
        public DbSet<TextModel> Texts {get;set;}

        public DbSet<TreeCommands> TreeCommands {get;set;}

        public EthereumBotContext(DbContextOptions<EthereumBotContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<UserModel>()
                .HasOne(a => a.Currency)
                .WithMany(b => b.Users);
            modelBuilder.Entity<UserModel>()
                .HasOne(a => a.Exchange)
                .WithMany(b => b.Users);
        }
    }
}