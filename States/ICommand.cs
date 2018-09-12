using System.Threading.Tasks;

namespace Ether_bot.States
{
    public interface ICommand
    {
        Task Execute();
    }
}