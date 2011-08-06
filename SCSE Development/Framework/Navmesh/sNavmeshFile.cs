using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Navmesh
{
    public struct sNavmeshFile
    {
        public string company;
        public string format;
        public string version;

        public short entryCount;
        public sNavmeshEntry[] entries;

        public int zone1Count;
        public int zone1Extra;
        public sNavmeshZone1[] zone1s;

        public int zone2Count;
        public sNavmeshZone2[] zone2s;

        public int zone3Count;
        public sNavmeshZone3[] zone3s;

        public sTextureMapEntry[,] texturemap; // = new sTextureMapEntry[96, 96];
        public float[] heightmap; // = new float[9409];
        public string last3;
        public float[] last4; // = new float[36];
    }
}
