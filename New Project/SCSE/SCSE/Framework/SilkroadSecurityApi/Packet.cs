using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Framework.SilkroadSecurityApi
{
    public class Packet
    {
        private ushort m_opcode;
        private PacketWriter m_writer;
        private PacketReader m_reader;
        private bool m_encrypted;
        private bool m_massive;
        private bool m_locked;
        byte[] m_reader_bytes;
        object m_lock;

        public ushort Opcode
        {
            get { return m_opcode; }
        }
        public bool Encrypted
        {
            get { return m_encrypted; }
        }
        public bool Massive
        {
            get { return m_massive; }
        }

        #region ctor

        public Packet(Packet rhs)
        {
            lock (rhs.m_lock)
            {
                m_lock = new object();

                m_opcode = rhs.m_opcode;
                m_encrypted = rhs.m_encrypted;
                m_massive = rhs.m_massive;

                m_locked = rhs.m_locked;
                if (!m_locked)
                {
                    m_writer = new PacketWriter();
                    m_reader = null;
                    m_reader_bytes = null;
                    m_writer.Write(rhs.m_writer.GetBytes());
                }
                else
                {
                    m_writer = null;
                    m_reader_bytes = rhs.m_reader_bytes;
                    m_reader = new PacketReader(m_reader_bytes);
                }
            }
        }
        public Packet(ushort opcode)
        {
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = false;
            m_massive = false;
            m_writer = new PacketWriter();
            m_reader = null;
            m_reader_bytes = null;
        }
        public Packet(ushort opcode, bool encrypted)
        {
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = encrypted;
            m_massive = false;
            m_writer = new PacketWriter();
            m_reader = null;
            m_reader_bytes = null;
        }
        public Packet(ushort opcode, bool encrypted, bool massive)
        {
            if (encrypted && massive)
            {
                throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
            }
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = encrypted;
            m_massive = massive;
            m_writer = new PacketWriter();
            m_reader = null;
            m_reader_bytes = null;
        }
        public Packet(ushort opcode, bool encrypted, bool massive, byte[] bytes)
        {
            if (encrypted && massive)
            {
                throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
            }
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = encrypted;
            m_massive = massive;
            m_writer = new PacketWriter();
            m_writer.Write(bytes);
            m_reader = null;
            m_reader_bytes = null;
        }
        public Packet(ushort opcode, bool encrypted, bool massive, byte[] bytes, int offset, int length)
        {
            if (encrypted && massive)
            {
                throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
            }
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = encrypted;
            m_massive = massive;
            m_writer = new PacketWriter();
            m_writer.Write(bytes, offset, length);
            m_reader = null;
            m_reader_bytes = null;
        }

        #endregion

        public byte[] GetBytes()
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    return m_reader_bytes;
                }
                return m_writer.GetBytes();
            }
        }

        public override string ToString()
        {
            lock (m_lock)
            {
                if (Length == 0)
                {
                    return "Empty";
                }

                string sData = BitConverter.ToString(this.GetBytes()).Trim('-');
                if (Position > 0)
                {
                    sData = sData.Insert(Position * 2, "|");
                }
                return sData;
            }
        }

        public void Lock()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    m_reader_bytes = m_writer.GetBytes();
                    m_reader = new PacketReader(m_reader_bytes);
                    m_writer.Close();
                    m_writer = null;
                    m_locked = true;
                }
            }
        }

        public long SeekRead(long offset, SeekOrigin orgin)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot SeekRead on an unlocked Packet.");
                }
                return m_reader.BaseStream.Seek(offset, orgin);
            }
        }

        public int RemainingRead
        {
            get
            {
                lock (m_lock)
                {
                    if (!m_locked)
                    {
                        throw new Exception("Cannot get RemainingRead on an unlocked Packet.");
                    }
                    return (int)(m_reader.BaseStream.Length - m_reader.BaseStream.Position);
                }
            }
        }

        public int Position
        {
            get
            {
                lock (m_lock)
                {
                    if (!m_locked)
                    {
                        throw new Exception("Cannot get Position on an unlocked Packet.");
                    }
                    return (int)(m_reader.BaseStream.Position);
                }
            }
        }

        public int Length
        {
            get
            {
                lock (m_lock)
                {
                    if (!m_locked)
                    {
                        throw new Exception("Cannot get Length on an unlocked Packet.");
                    }
                    return (int)(m_reader.BaseStream.Length);
                }
            }
        }


        #region Read

        public byte ReadByte()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadByte();
            }
        }
        public sbyte ReadSByte()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadSByte();
            }
        }
        public ushort ReadUShort()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadUInt16();
            }
        }
        public short ReadShort()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadInt16();
            }
        }
        public uint ReadUInteger()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadUInt32();
            }
        }
        public int ReadInteger()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadInt32();
            }
        }
        public ulong ReadULong()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadUInt64();
            }
        }
        public long ReadLong()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadInt64();
            }
        }
        public float ReadFloat()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadSingle();
            }
        }
        public double ReadDouble()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadDouble();
            }
        }
        public string ReadString()
        {
            return ReadString(1252);
        }
        public string ReadString(int codepage)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }

                UInt16 length = m_reader.ReadUInt16();
                byte[] bytes = m_reader.ReadBytes(length);

                return Encoding.GetEncoding(codepage).GetString(bytes);
            }
        }
        public string ReadUString()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }

                UInt16 length = m_reader.ReadUInt16();
                byte[] bytes = m_reader.ReadBytes(length * 2);

                return Encoding.Unicode.GetString(bytes);
            }
        }

        public byte[] ReadByteArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                byte[] values = new byte[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadByte();
                }
                return values;
            }
        }
        public sbyte[] ReadSByteArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                sbyte[] values = new sbyte[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadSByte();
                }
                return values;
            }
        }
        public ushort[] ReadUShortArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                UInt16[] values = new UInt16[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadUInt16();
                }
                return values;
            }
        }
        public short[] ReadShortArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                Int16[] values = new Int16[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadInt16();
                }
                return values;
            }
        }
        public uint[] ReadUIntegerArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                UInt32[] values = new UInt32[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadUInt32();
                }
                return values;
            }
        }
        public int[] ReadIntegerArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                Int32[] values = new Int32[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadInt32();
                }
                return values;
            }
        }
        public ulong[] ReadULongArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                UInt64[] values = new UInt64[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadUInt64();
                }
                return values;
            }
        }
        public long[] ReadLongArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                Int64[] values = new Int64[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadInt64();
                }
                return values;
            }
        }
        public float[] ReadFloatArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                Single[] values = new Single[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadSingle();
                }
                return values;
            }
        }
        public double[] ReadDoubleArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                Double[] values = new Double[count];
                for (int x = 0; x < count; ++x)
                {
                    values[x] = m_reader.ReadDouble();
                }
                return values;
            }
        }
        public string[] ReadStringArray(int count)
        {
            return ReadStringArray(1252);
        }
        public string[] ReadStringArray(int codepage, int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                String[] values = new String[count];
                for (int x = 0; x < count; ++x)
                {
                    UInt16 length = m_reader.ReadUInt16();
                    byte[] bytes = m_reader.ReadBytes(length);
                    values[x] = Encoding.UTF7.GetString(bytes);
                }
                return values;
            }
        }
        public string[] ReadUStringArray(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                String[] values = new String[count];
                for (int x = 0; x < count; ++x)
                {
                    UInt16 length = m_reader.ReadUInt16();
                    byte[] bytes = m_reader.ReadBytes(length * 2);
                    values[x] = Encoding.Unicode.GetString(bytes);
                }
                return values;
            }
        }

        #endregion

        public long SeekWrite(long offset, SeekOrigin orgin)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot SeekWrite on a locked Packet.");
                }
                return m_writer.BaseStream.Seek(offset, orgin);
            }
        }

        #region Write

        public void WriteByte(byte value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteSByte(sbyte value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteUShort(ushort value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteShort(short value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteUInteger(uint value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteInteger(int value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteULong(ulong value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteLong(long value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteFloat(float value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteDouble(double value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteString(string value)
        {
            WriteString(value, 1252);
        }
        public void WriteString(string value, int code_page)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }

                byte[] codepage_bytes = Encoding.GetEncoding(code_page).GetBytes(value);
                string utf7_value = Encoding.UTF7.GetString(codepage_bytes);
                byte[] bytes = Encoding.Default.GetBytes(utf7_value);

                m_writer.Write((ushort)bytes.Length);
                m_writer.Write(bytes);
            }
        }
        public void WriteUString(string value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }

                byte[] bytes = Encoding.Unicode.GetBytes(value);

                m_writer.Write((ushort)value.ToString().Length);
                m_writer.Write(bytes);
            }
        }

        public void WriteByte(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write((byte)(Convert.ToUInt64(value) & 0xFF));
            }
        }
        public void WriteSByte(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write((sbyte)(Convert.ToInt64(value) & 0xFF));
            }
        }
        public void WriteUShort(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write((ushort)(Convert.ToUInt64(value) & 0xFFFF));
            }
        }
        public void WriteShort(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write((ushort)(Convert.ToInt64(value) & 0xFFFF));
            }
        }
        public void WriteUInteger(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write((uint)(Convert.ToUInt64(value) & 0xFFFFFFFF));
            }
        }
        public void WriteInteger(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write((int)(Convert.ToInt64(value) & 0xFFFFFFFF));
            }
        }
        public void WriteULong(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(Convert.ToUInt64(value));
            }
        }
        public void WriteLong(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(Convert.ToInt64(value));
            }
        }
        public void WriteFloat(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(Convert.ToSingle(value));
            }
        }
        public void WriteDouble(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(Convert.ToDouble(value));
            }
        }
        public void WriteString(object value)
        {
            WriteString(value, 1252);
        }
        public void WriteString(object value, int code_page)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }

                byte[] codepage_bytes = Encoding.GetEncoding(code_page).GetBytes(value.ToString());
                string utf7_value = Encoding.UTF7.GetString(codepage_bytes);
                byte[] bytes = Encoding.Default.GetBytes(utf7_value);

                m_writer.Write((ushort)bytes.Length);
                m_writer.Write(bytes);
            }
        }
        public void WriteUString(object value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }

                byte[] bytes = Encoding.Unicode.GetBytes(value.ToString());

                m_writer.Write((ushort)value.ToString().Length);
                m_writer.Write(bytes);
            }
        }

        #endregion

        #region WriteArray

        public void WriteByteArray(byte[] values)
        {
            if (m_locked)
            {
                throw new Exception("Cannot Write to a locked Packet.");
            }
            m_writer.Write(values);
        }
        public void WriteByteArray(byte[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteSByteArray(sbyte[] values)
        {
            WriteSByteArray(values, 0, values.Length);
        }
        public void WriteSByteArray(sbyte[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteSByte(values[x]);
                }
            }
        }
        public void WriteUShortArray(ushort[] values)
        {
            WriteUShortArray(values, 0, values.Length);
        }
        public void WriteUShortArray(ushort[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteShortArray(short[] values)
        {
            WriteShortArray(values, 0, values.Length);
        }
        public void WriteShortArray(short[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteUIntegerArray(uint[] values)
        {
            WriteUIntegerArray(values, 0, values.Length);
        }
        public void WriteUIntegerArray(uint[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteIntegerArray(int[] values)
        {
            WriteIntegerArray(values, 0, values.Length);
        }
        public void WriteIntegerArray(int[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteULongArray(ulong[] values)
        {
            WriteULongArray(values, 0, values.Length);
        }
        public void WriteULongArray(ulong[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteLongArray(long[] values)
        {
            WriteLongArray(values, 0, values.Length);
        }
        public void WriteLongArray(long[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteFloatArray(float[] values)
        {
            WriteFloatArray(values, 0, values.Length);
        }
        public void WriteFloatArray(float[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteDoubleArray(double[] values)
        {
            WriteDoubleArray(values, 0, values.Length);
        }
        public void WriteDoubleArray(double[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    m_writer.Write(values[x]);
                }
            }
        }
        public void WriteStringArray(string[] values, int codepage)
        {
            WriteStringArray(values, 0, values.Length, codepage);
        }
        public void WriteStringArray(string[] values, int index, int count, int codepage)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteString(values[x], codepage);
                }
            }
        }
        public void WriteStringArray(string[] values)
        {
            WriteStringArray(values, 0, values.Length, 1252);
        }
        public void WriteStringArray(string[] values, int index, int count)
        {
            WriteStringArray(values, index, count, 1252);
        }
        public void WriteUStringArray(string[] values)
        {
            WriteUStringArray(values, 0, values.Length);
        }
        public void WriteUStringArray(string[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteUString(values[x]);
                }
            }
        }

        public void WriteByteArray(object[] values)
        {
            WriteByteArray(values, 0, values.Length);
        }
        public void WriteByteArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteByte(values[x]);
                }
            }
        }
        public void WriteSByteArray(object[] values)
        {
            WriteSByteArray(values, 0, values.Length);
        }
        public void WriteSByteArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteSByte(values[x]);
                }
            }
        }
        public void WriteUShortArray(object[] values)
        {
            WriteUShortArray(values, 0, values.Length);
        }
        public void WriteUShortArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteUShort(values[x]);
                }
            }
        }
        public void WriteShortArray(object[] values)
        {
            WriteShortArray(values, 0, values.Length);
        }
        public void WriteShortArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteShort(values[x]);
                }
            }
        }
        public void WriteUIntegerArray(object[] values)
        {
            WriteUIntegerArray(values, 0, values.Length);
        }
        public void WriteUIntegerArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteUInteger(values[x]);
                }
            }
        }
        public void WriteIntegerArray(object[] values)
        {
            WriteIntegerArray(values, 0, values.Length);
        }
        public void WriteIntegerArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteInteger(values[x]);
                }
            }
        }
        public void WriteULongArray(object[] values)
        {
            WriteULongArray(values, 0, values.Length);
        }
        public void WriteULongArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteULong(values[x]);
                }
            }
        }
        public void WriteLongArray(object[] values)
        {
            WriteLongArray(values, 0, values.Length);
        }
        public void WriteLongArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteLong(values[x]);
                }
            }
        }
        public void WriteFloatArray(object[] values)
        {
            WriteFloatArray(values, 0, values.Length);
        }
        public void WriteFloatArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteFloat(values[x]);
                }
            }
        }
        public void WriteDoubleArray(object[] values)
        {
            WriteDoubleArray(values, 0, values.Length);
        }
        public void WriteDoubleArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteDouble(values[x]);
                }
            }
        }
        public void WriteStringArray(object[] values, int codepage)
        {
            WriteStringArray(values, 0, values.Length, codepage);
        }
        public void WriteStringArray(object[] values, int index, int count, int codepage)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteString(values[x].ToString(), codepage);
                }
            }
        }
        public void WriteStringArray(object[] values)
        {
            WriteStringArray(values, 0, values.Length, 1252);
        }
        public void WriteStringArray(object[] values, int index, int count)
        {
            WriteStringArray(values, index, count, 1252);
        }
        public void WriteUStringArray(object[] values)
        {
            WriteUStringArray(values, 0, values.Length);
        }
        public void WriteUStringArray(object[] values, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                for (int x = index; x < index + count; ++x)
                {
                    WriteUString(values[x].ToString());
                }
            }
        }

        #endregion
    }
}
