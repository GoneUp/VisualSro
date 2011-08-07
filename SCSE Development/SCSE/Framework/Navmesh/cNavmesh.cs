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
        private Graphics m_g;

        private bool m_readyToLoad;
        private bool m_readyToDraw;

        public cSector(ref PK2.cPK2Reader MediaPK2, ref PK2.cPK2Reader DataPK2, byte XSec, byte YSec)
        {
            m_X = XSec;
            m_Y = YSec;

            m_g = null;

            #region Check Media

            if (MediaPK2 != null)
            {
                if (MediaPK2.IsLoaded)
                {
                    byte[] buffer = MediaPK2["\\minimap\\" + m_X + "x" + m_Y + ".ddj"];
                    if (buffer != null)
                    {
                        m_image = PK2.DDSLoader.LoadDDJ(buffer);
                        m_g = Graphics.FromImage(m_image);
                        m_readyToDraw = true;
                    }
                }
            }

            #endregion

            #region Check Data

            if (DataPK2 != null)
            {
                if (DataPK2.IsLoaded)
                {
                    m_readyToLoad = true;
                    m_readyToDraw = true; //But without image.
                }
                else
                {
                    throw new Exception("[cSector::ctor] Data.pk2 is not loaded");
                }
            }
            else
            {
                throw new Exception("[cSector::ctor] Data.pk2 is not loaded");
            }

            #endregion
        }

        public void LoadNavmesh(ref PK2.cPK2Reader DataPK2)
        {
            var buffer = DataPK2["\navmesh\nv_" + m_Y.ToString("X2") + m_X.ToString("X2") + ".nvm"];
            if (buffer != null)
            {
                MemoryStream ms = new MemoryStream(buffer);
                BinaryReader br = new BinaryReader(ms);

            }
            else
            {
                throw new Exception("Navmesh file was not found X:" + m_X + " Y:" + m_Y + " (" + m_Y.ToString("X2") + m_X.ToString("X2") + ")");
            }

        }

    }

}
