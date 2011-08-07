using System;
using System.Collections.Generic;
using System.IO;

namespace Framework.PK2
{
    public class cPK2Reader : IDisposable
    {
        /*
         * Orginal by keinplan1337
         * Translated to C# by Daxter
         */

        //Blowfish:
        //Orginal    {0x32, 0xCE, 0xDD, 0x7C, 0xBC, 0xA8}
        //eSRO 1.037 {0x60, 0x99, 0x88, 0x25, 0x79, 0xF8}

        #region PK2Loading Algorithmus

        private string m_file;
        private Blowfish m_blowfish;
        private Dictionary<String, sFile> m_content;

        private bool m_isLoaded = false;

        public cPK2Reader()
        {
            m_blowfish = new Blowfish(new byte[] { 0x32, 0xCE, 0xDD, 0x7C, 0xBC, 0xA8 }, 0, 6);
            m_content = new Dictionary<string, sFile>();
        }
        public cPK2Reader(byte[] BlowfishKey)
        {
            m_blowfish = new Blowfish(BlowfishKey, 0, BlowfishKey.Length);
            m_content = new Dictionary<string, sFile>();
        }

        public cPK2Reader(string PK2Path)
        {
            m_file = PK2Path;

            m_blowfish = new Blowfish(new byte[] { 0x32, 0xCE, 0xDD, 0x7C, 0xBC, 0xA8 }, 0, 6);
            m_content = new Dictionary<string, sFile>();
        }
        public cPK2Reader(string PK2Path, byte[] BlowfishKey)
        {
            m_file = PK2Path;

            m_blowfish = new Blowfish(BlowfishKey, 0, BlowfishKey.Length);
            m_content = new Dictionary<string, sFile>();
        }

        public bool Load()
        {
            if (String.IsNullOrEmpty(m_file) == false && System.IO.File.Exists(m_file))
            {
                try
                {
                    FileStream fs = new FileStream(m_file, FileMode.Open, FileAccess.Read);

                    BinaryReader r = new BinaryReader(fs);

                    r.BaseStream.Position = 0;
                    m_content.Clear();

                    string type = new string(r.ReadChars(30));
                    r.ReadUInt32();
                    r.ReadUInt32();
                    r.BaseStream.Position += 218; //00-00-00..

                    //First Entry
                    byte[] buffer = r.ReadBytes(128);
                    m_blowfish.DecryptRev(buffer, 0, buffer, 0, 131);

                    using (BinaryReader rr = new BinaryReader(new MemoryStream(buffer)))
                    {
                        rr.ReadByte(); //Type
                        string name = new string(rr.ReadChars(81)); //Name
                        rr.ReadBytes(24);
                        rr.ReadUInt32(); //PosLow
                        rr.ReadUInt32(); //PosHigh
                        rr.ReadUInt32(); //Size
                        rr.ReadUInt32(); //NextChain                        
                    }

                    while (ProcessFile(r.BaseStream.Position, ref r, ref buffer)) { }

                    r.Close();
                    fs.Close();

                    m_isLoaded = true;
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("PK2Loading Algorithmus Exception: " + ex.Message, ex);
                }
            }
            else
            {
                m_isLoaded = false;
                throw new Exception("The file : \"" + m_file + "\" does not exists.");
            }
        }
        public bool Load(string PK2Path)
        {
            m_file = PK2Path;
            return Load();
        }

        public bool IsLoaded
        {
            get
            {
                return m_isLoaded;
            }
        }

        private string m_level = "\\";
        private bool ProcessFile(long startpos, ref BinaryReader r, ref byte[] buffer)
        {
            sPK2Entry entry = new sPK2Entry();
            r.BaseStream.Position = startpos;

            buffer = r.ReadBytes(128);
            if (buffer.GetUpperBound(0) != 0x80 - 1)
            {
                return false;
            }
            m_blowfish.DecryptRev(buffer, 0, buffer, 0, 131);

            using (BinaryReader rr = new BinaryReader(new MemoryStream(buffer)))
            {
                entry.Type = rr.ReadByte();
                for (int i = 1; i < 81; i++)
                {
                    byte b = rr.ReadByte();
                    if (b >= 0x20 && b <= 0x7E)
                    {
                        entry.Name += Convert.ToChar(b);
                    }
                    else
                    {
                        rr.BaseStream.Position = 82;
                        break; //Exit For
                    }
                }

                rr.BaseStream.Position += 24;
                entry.PosLow = rr.ReadUInt32();
                entry.PosHigh = rr.ReadUInt32();
                entry.size = rr.ReadUInt32();
                entry.nextChain = rr.ReadUInt32();
            }

            switch (entry.Type)
            {
                case 1: //Directory
                    if (entry.Name != "." && entry.Name != "..")
                    {
                        //Some name fixes
                        m_level = m_level + entry.Name + "\\";
                        m_level = m_level.ToLower();

                        //Store Directory Position
                        long currentpos = r.BaseStream.Position;

                        //Go to first Directory entry
                        r.BaseStream.Position = entry.PosLow;

                        //Read subfolders & files
                        while (ProcessFile(r.BaseStream.Position, ref r, ref buffer)) { }

                        r.BaseStream.Position = currentpos; //Back to Directory

                        //Restore Directorys root path
                        m_level = m_level.Remove(Len(m_level) - Len(entry.Name) - 1, Len(entry.Name) + 1);

                    }
                    break;

                case 2: //File
                    if (entry.size != 0 && String.IsNullOrEmpty(entry.Name) == false)
                    {
                        sFile file = new sFile();
                        file.Pos = entry.PosLow;
                        file.size = entry.size;
                        if (m_content.ContainsKey(m_level + entry.Name))
                        {
                            Console.WriteLine("Warning: File: " + m_level + entry.Name + " already exist.");
                            //return false;
                        }
                        else
                        {
                            m_content.Add(m_level + entry.Name.ToLower(), file);
                        }
                    }
                    break;

                default:
                    return false;
            }

            if (entry.nextChain > 0)
            {
                r.BaseStream.Position = entry.nextChain;
            }
            if (r.BaseStream.Position == entry.PosLow)
            {
                return false;
            }
            return true;
        }
        private int Len(string expression)
        {
            if (expression == null)
            {
                return 0;
            }
            else
            {
                return expression.Length;
            }
        }

        public Dictionary<string, sFile> Content
        {
            get
            {
                return m_content;
            }
        }
        public int ContentCount
        {
            get
            {
                return m_content.Count;
            }
        }

        public struct sFile
        {
            public uint Pos;
            public uint size;
        }
        private struct sPK2Entry
        {
            public byte Type;
            public string Name;
            public uint PosLow;
            public uint PosHigh;
            public uint size;
            public uint nextChain;
        }

        public void Unload()
        {
            m_isLoaded = false;
            m_content.Clear();
        }

        #endregion

        //Custom functions to get Files,Images etc...

        public byte[] GetFile(sFile file)
        {
            if (m_isLoaded)
            {
                byte[] buffer;

                using (FileStream fs = new FileStream(m_file, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader r = new BinaryReader(fs))
                    {
                        r.BaseStream.Position = file.Pos;
                        buffer = r.ReadBytes(Convert.ToInt32(file.size));
                    }
                }
                return buffer;
            }
            else
            {
                throw new Exception("Can not access content because PK2 is not loaded.");
            }
        }
        public byte[] GetFile(string file)
        {
            if (m_isLoaded)
            {
                byte[] buffer;

                file = file.ToLower();

                if (m_content.ContainsKey(file) == false)
                {
                    return null;
                }

                using (FileStream fs = new FileStream(m_file, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader r = new BinaryReader(fs))
                    {
                        r.BaseStream.Position = m_content[file].Pos;
                        buffer = r.ReadBytes(Convert.ToInt32(m_content[file].size));
                    }
                }
                return buffer;
            }
            else
            {
                throw new Exception("Can not access content because PK2 is not loaded.");
            }
        }

        public void PrintContent(string filepath)
        {
            if (m_isLoaded)
            {
                StreamWriter sw = new StreamWriter(filepath, false);
                foreach (string entry in m_content.Keys)
                {
                    sw.WriteLine(entry);
                }
                sw.Close();
            }
            else
            {
                throw new Exception("Can not access content because PK2 is not loaded.");
            }
        }
        public void PrintContent()
        {
            FileInfo fi = new FileInfo(m_file);
            PrintContent(Environment.CurrentDirectory + "\\" + fi.Name.Replace(".pk2", ".txt"));
            fi = null;
        }

        public byte[] this[string key]
        {
            get
            {
                if (m_isLoaded)
                {
                    if (m_content.ContainsKey(key))
                    {
                        return GetFile(m_content[key]);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new Exception("Can not access content because PK2 is not loaded.");
                }
            }

        }

        #region IDisposable Member

        public void Dispose()
        {

            m_content.Clear();
            m_content = null;
            m_blowfish = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

