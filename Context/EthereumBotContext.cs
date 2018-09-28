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
        public DbSet<StateModel> States {get;set;}
        public DbSet<CommandsModel> Commands {get;set;}
        public DbSet<TextModel> Texts {get;set;}

        public EthereumBotContext(DbContextOptions<EthereumBotContext> options)
            :base(options)
        {
            if (Database.EnsureCreated())
            {
                var state = new StateModel(){
                    State = "Start"
                };
                States.Add(state);
                States.Add(new StateModel(){
                    State = "Settings"
                });
                States.Add(new StateModel(){
                    State = "Currency"
                });
                States.Add(new StateModel(){
                    State = "Exchange"
                });
                var Text = new TextModel(){
                    Text = "Крипто бот\rБиржа = {0}, ETH = {1} USD\r (дата обновления: {2})"
                };
                Texts.Add(Text);
                Commands.AddRange(
                    new CommandsModel(){
                        Command = "GetCurrentRate",
                        State = state,
                        Text = Text
                    }, 
                    new CommandsModel(){
                        Command = "Settings",
                        State = state,
                        Text = Text
                    });
                SaveChanges();
            }
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
                .WithMany(b => b.Users);
        }
    }
}