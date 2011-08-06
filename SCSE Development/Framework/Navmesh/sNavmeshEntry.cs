using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Navmesh
{
    public struct sNavmeshEntry
    {
        public uint ID;
        public float x;
        public float y;
        public float z;
        public short uk2;
        public float Angle;
        public short uk4;
        public short uk5;
        public short uk6;
        public short grid;
        public ushort extraCount;
        public sNavmeshEntryExtra[] extraArray;
    }
}
