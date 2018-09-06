using System;
using Microsoft.EntityFrameworkCore;

namespace Ether_bot.Models
{
    public class EthereumBotContext:DbContext
    {
        public DbSet<UserModel> Users {get;set;}
        public DbSet<CurrencyModel> Currencies {get;set;}
        public DbSet<ExchangeModel> Exchanges {get;set;}
        public DbSet<StateModel> States {get;set;}
        public DbSet<PairModel> Pairs {get;set;}
        public DbSet<RateExchangeModel> RateExchanges {get;set;}

        public EthereumBotContext(DbContextOptions<EthereumBotContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<UserModel>()
                .HasOne(a => a.Currency)
                .WithMany(b => b.Users);
            modelBuilder.Entity<UserModel>()
                .HasOne(a => a.Exchange)
                .WithMany(b => b.Users);
            modelBuilder.Entity<UserModel>()
                .HasOne(a => a.State)
                .WithOne(b => b.User);
            modelBuilder.Entity<RateExchangeModel>()
                .HasKey(k => new {k.IdExchangeModel, k.IdPairModel});
        }
    }
}