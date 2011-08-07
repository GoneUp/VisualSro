using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Navmesh
{
    public class NavmeshEngine
    {
        //24758 Tief Town        
        private Dictionary<int, cSector> m_sectors;
        
        public NavmeshEngine(int[] regionCodes, ref PK2.cPK2Reader MediaPK2, ref PK2.cPK2Reader DataPK2)
        {

            //Preload all regions
            foreach (var region in regionCodes)
            {
                byte x = BitConverter.GetBytes(region)[1];
                byte y = BitConverter.GetBytes(region)[0];
                m_sectors.Add(region, new cSector(ref MediaPK2, ref DataPK2, x, y));
            }

            foreach (var sector in m_sectors.Values)
            {
                sector.LoadNavmesh(ref DataPK2);
            }

        }

    }
}
