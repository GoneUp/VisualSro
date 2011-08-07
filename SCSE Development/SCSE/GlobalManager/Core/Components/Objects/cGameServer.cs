using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalManager.Core.Components
{
    public class cGameServer : cServer
    {
        //General Server Data
        //Daxter: Warum sollte der GlobalManager das wissen wollen ?
        public uint MobCount = 0;
        public uint ItemCount = 0;
        public uint NpcCount = 0;

        //Statistic Data (Todo??)
        //Übertragung ist aufwendig ?
        public uint AllItemsCount = 0;
        public uint AllPlayersCount = 0;
        public uint AllSkillsCount = 0;

        //Settings
        //Daxter: Ist x255 nicht schnell genug??
        public ushort XPRate = 1;
        public ushort SPRate = 1;
        public ushort GoldRate = 1;
        public ushort DropRate = 1;
        public ushort SpawnRate = 1;
        public ushort HwanRate = 1; //(Berserk Obrain Rate)
    }
}
