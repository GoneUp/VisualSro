using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Framework.Navmesh
{
    public class cSector
    {
        private byte m_X;
        private byte m_Y;

        public Bitmap m_image;
        private Graphics g;

        public cSector(PK2.cPK2Reader MediaPK2, PK2.cPK2Reader DataPK2, byte XSec, byte YSec)
        {
            m_X = XSec;
            m_Y = YSec;

            m_image = new Bitmap(256, 256);
            g = Graphics.FromImage(m_image);
            g.Clear(Color.Black);

            //Check Media
            if (MediaPK2.IsLoaded() == false)
            {
                g.DrawString("[cSector::ctor] Media.pk2 is not loaded.", new Font("Arial", 8), Brushes.Red, 0.0f, 0.0f);
            }
            else
            {
                byte[] buffer = MediaPK2["\\minimap\\" + m_X + "x" + m_Y + ".ddj"];
                if (buffer != null)
                {
                    m_image = PK2.DDSLoader.LoadDDJ(buffer);
                }
                else
                {
                    g.DrawString("[cSector::ctor] buffer is null.", new Font("Arial", 8), Brushes.White, 0.0f, 0.0f);
                }

            }

            //Check data
            if (DataPK2.IsLoaded() == false)
            {
                throw new Exception("[cSector::ctor] Data.pk2 is not loaded");
            }


        }

    }

}
