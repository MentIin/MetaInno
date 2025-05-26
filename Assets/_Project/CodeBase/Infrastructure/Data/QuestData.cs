using System.Collections.Generic;

namespace CodeBase.Infrastructure.Data
{
    public class QuestData
    {
        public int Id;
        public int Record;
        public List<int> CurrentPlayers = new List<int>();
    }
}