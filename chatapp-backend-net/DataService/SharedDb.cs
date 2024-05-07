using FormulaOne.ChatService.Models;
using System.Collections.Concurrent;

namespace FormulaOne.ChatService.DataService
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connections = new();

        public ConcurrentDictionary<string, UserConnection> connections => _connections;
    }
}
