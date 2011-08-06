using System;
using System.Text;

namespace SilkroadSniffer
{
    /// <summary>
    /// Represents a Silkroad packet that stores Length,Opcode and Data.
    /// </summary>
    public class phPacket : IDisposable
    {

        #region -----Field/Propertys-----


        /// <summary>
        /// The Opcode as HexWord
        /// </summary>
        public string OpcodeHEX
        {
            get { return m_Opcode.ToString("X2"); }
            set { m_Opcode = Convert.ToUInt16(value, 16); }
        }

        /// <summary>
        /// The Opcode as Ushort
        /// </summary>
        public ushort Opcode
        {
            get { return m_Opcode; }
            set { m_Opcode = value; }
        }
        private ushort m_Opcode;

        private byte m_SecurityCount;
        public byte SecurityCount
        {
            get { return m_SecurityCount; }
            set { m_SecurityCount = value; }
        }

        private byte m_SecurityCrc;
        public byte SecurityCrc
        {
            get { return m_SecurityCrc; }
            set { m_SecurityCrc = value; }
        }

        /// <summary>
        /// Current pointer position
        /// </summary>
        public int Pointer
        {
            get { return m_Pointer; }
            set { m_Pointer = value; }
        }
        private int m_Pointer;

        /// <summary>
        /// Packet Length
        /// </summary>
        public int Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }
        private int m_Length;

        /// <summary>
        /// BufferSize : Default 8192
        /// </summary>
        public int BufferSize
        {
            get { return m_BufferSize; }
            set
            {
                m_BufferSize = value;
                Array.Resize<byte>(ref m_Buffer, value);
            }
        }
        private int m_BufferSize = 8192; //Silkroad Default: 8192

        /// <summary>
        /// Data Buffer
        /// </summary>
        public byte[] Buffer
        {
            get { return m_Buffer; }
            set { m_Buffer = value; }
        }
        private byte[] m_Buffer;

        #endregion

        #region -----Constructor-----

        /// <summary>
        /// Creates an instance of cPacket using only the received bytes from client.
        /// </summary>
        /// <param name="buffer">The Bytearray received from client.</param>
        public phPacket(byte[] buffer)
        {
            m_Length = BitConverter.ToUInt16(buffer, 0);
            m_Opcode = BitConverter.ToUInt16(buffer, 2);
            m_SecurityCount = buffer[4];
            m_SecurityCrc = buffer[5];
            m_Buffer = new Byte[m_BufferSize];
            if (m_Length > 0)
            {
                Array.ConstrainedCopy(buffer, 6, m_Buffer, 0, m_Length);
            }


        }

        /// <summary>
        /// Creates an empty instance of cPacket using only the Opcode.
        /// </summary>
        /// <param name="Opcode">Opcode as ushort.</param>
        public phPacket(ushort Opcode)
        {
            m_Opcode = Opcode;
            m_Buffer = new Byte[m_BufferSize];
        }

        /// <summary>
        /// Creates an empty instance of cPacket using only the Opcode.
        /// </summary>
        /// <param name="Opcode">Opcode as string.</param>
        public phPacket(string Opcode)
        {
            m_Opcode = Convert.ToUInt16(Opcode, 16);

            m_Buffer = new Byte[m_BufferSize];
        }

        /// <summary>
        /// Creates an intance of cPacket using the Opcode and Data(Buffer)
        /// </summary>
        /// <param name="Opcode">Opcode as ushort.</param>
        /// <param name="Buffer">Data(Buffer) as Bytearray.</param>
        public phPacket(ushort Opcode, Byte[] Buffer)
        {
            m_Opcode = Opcode;
            m_Buffer = Buffer;
        }

        /// <summary>
        /// Creates an intance of cPacket using the Opcode and Data(Buffer)
        /// </summary>
        /// <param name="Opcode">Opcode as ushort.</param>
        /// <param name="Buffer">Data(Buffer) as HexString.</param>
        public phPacket(ushort Opcode, string Buffer)
        {
            m_Opcode = Opcode;
            m_Buffer = new Byte[m_BufferSize];
            AddHex(Buffer);
        }

        /// <summary>
        /// Creates an intance of cPacket using the Opcode and Data(Buffer)
        /// </summary>
        /// <param name="Opcode">Opcode as string.</param>
        /// <param name="Buffer">Data(Buffer) as Bytearray.</param>
        public phPacket(string Opcode, Byte[] Buffer)
        {
            m_Opcode = Convert.ToUInt16(Opcode, 16);
            m_Buffer = Buffer;
        }

        /// <summary>
        /// Creates an intance of cPacket using the Opcode and Data(Buffer)
        /// </summary>
        /// <param name="Opcode">Opcode as string.</param>
        /// <param name="Buffer">Data(Buffer) as HexString.</param>
        public phPacket(string Opcode, string Buffer)
        {
            m_Opcode = Convert.ToUInt16(Opcode, 16);
            m_Buffer = new Byte[m_BufferSize];
            AddHex(Buffer);
        }


        #endregion

        #region -----Read-----

        /// <summary>
        /// Reads a boolean value from buffer and increments the Pointer by 1.
        /// </summary>
        /// <returns>Returns the read byte as boolean.</returns>
        public bool ReadBool()
        {
            //Increase Pointer
            m_Pointer++;

            return Convert.ToBoolean(m_Buffer[m_Pointer - 1]);
        }

        /// <summary>
        /// Reads a char value from buffer and increments the Pointer by 1.
        /// </summary>
        /// <returns>Returns the read byte as char.</returns>
        public char ReadChar()
        {
            //Increase Pointer
            m_Pointer++;

            return Convert.ToChar(m_Buffer[m_Pointer - 1]);
        }

        /// <summary>
        /// Reads a byte value from buffer and increments the Pointer by 1.
        /// </summary>
        /// <returns>the read byte.</returns>
        public byte ReadByte()
        {
            //Increase Pointer
            m_Pointer++;

            return m_Buffer[m_Pointer - 1];
        }

        /// <summary>
        /// Reads a bytearray value from buffer and increments the Pointer by count.
        /// </summary>
        /// <param name="count">Number of bytes to read.</param>
        /// <returns>the read bytearray.</returns>
        public byte[] ReadByte(int count)
        {
            //Increase Pointer
            m_Pointer += count;

            //Copy to tempBuffer
            byte[] tempBuffer = new byte[count];
            Array.ConstrainedCopy(m_Buffer, m_Pointer - count, tempBuffer, 0, count);

            return tempBuffer;
        }

        /// <summary>
        /// Reads an ushort from buffer and increments the Pointer by 2.
        /// </summary>
        /// <returns>the read ushort.</returns>
        public ushort ReadUShort()
        {
            //Increase Pointer
            m_Pointer += sizeof(ushort);

            return BitConverter.ToUInt16(m_Buffer, m_Pointer - sizeof(ushort));
        }

        /// <summary>
        /// Reads a short from buffer and increments the Pointer by 2.
        /// </summary>
        /// <returns>the read short.</returns>
        public short ReadShort()
        {
            //Increase Pointer
            m_Pointer += sizeof(ushort);

            return BitConverter.ToInt16(m_Buffer, m_Pointer - sizeof(ushort));
        }

        /// <summary>
        /// Reads an uinteger from buffer and increments the Pointer by 4.
        /// </summary>
        /// <returns>the read uinteger.</returns>
        public uint ReadUInteger()
        {
            //Increase Pointer
            m_Pointer += sizeof(uint);

            return BitConverter.ToUInt32(m_Buffer, m_Pointer - sizeof(int));
        }

        /// <summary>
        /// Reads an integer from buffer and increments the Pointer by 4.
        /// </summary>
        /// <returns>the read integer.</returns>
        public int ReadInteger()
        {
            //Increase Pointer
            m_Pointer += sizeof(uint);

            return BitConverter.ToInt32(m_Buffer, m_Pointer - sizeof(uint));
        }

        /// <summary>
        /// Reads an ulong from buffer and increments the Pointer by 8.
        /// </summary>
        /// <returns>the read ulong.</returns>
        public ulong ReadULong()
        {
            //Increase Pointer
            m_Pointer += sizeof(ulong);

            return BitConverter.ToUInt64(m_Buffer, m_Pointer - sizeof(ulong));
        }

        /// <summary>
        /// Reads a long from buffer and increments the Pointer by 8.
        /// </summary>
        /// <returns>the read long.</returns>
        public long ReadLong()
        {
            //Increase Pointer
            m_Pointer += sizeof(ulong);

            return BitConverter.ToInt64(m_Buffer, m_Pointer - sizeof(long));
        }

        /// <summary>
        /// Reads a float from buffer and increments the Pointer by 4.
        /// </summary>
        /// <returns>the read float.</returns>
        public float ReadFloat()
        {
            //Increase Pointer
            m_Pointer += sizeof(float);

            return BitConverter.ToSingle(m_Buffer, m_Pointer - sizeof(float));
        }

        /// <summary>
        /// Reads the stringlength + ansi-string from buffer and increments the Pointer by 4 + stringlength.
        /// </summary>
        /// <returns>the read ANSI-String.</returns>
        public string ReadString()
        {
            int length = ReadUShort();

            //Increase Pointer
            m_Pointer += length;

            return Encoding.Default.GetString(m_Buffer, m_Pointer - length, length);
        }

        /// <summary>
        /// Reads an ansi-string from buffer using the given length and increments the Pointer by length.
        /// </summary>
        /// <returns>the read ANSI-String.</returns>
        public string ReadString(int length)
        {

            //Increase Pointer
            m_Pointer += length;

            return Encoding.Default.GetString(m_Buffer, m_Pointer - length, length);
        }

        /// <summary>
        /// Reads the stringlength + unicode string from buffer and increments the Pointer by 4 + stringlength.
        /// </summary>
        /// <returns>the read Unicode-String.</returns>
        public string ReadUnicodeString()
        {
            int length = ReadUShort();

            //Increase Pointer
            m_Pointer += length;

            return Encoding.Unicode.GetString(m_Buffer, m_Pointer - length, length);
        }

        /// <summary>
        /// Reads an Unicode-string from buffer using the given length and increments the Pointer by length.
        /// </summary>
        /// <returns>the read Unicode-String.</returns>
        public string ReadUnicodeString(int length)
        {
            length *= 2;

            //Increase Pointer
            m_Pointer += length;

            return Encoding.Unicode.GetString(m_Buffer, m_Pointer - length, length);
        }

        #endregion

        #region -----Add-----

        /// <summary>
        /// Adds a boolean to the buffer and increments the length by 1.
        /// </summary>
        /// <param name="value">Boolean to add.</param>
        public void AddBool(bool value)
        {
            m_Length++;
            m_Buffer[m_Length - 1] = Convert.ToByte(value);
        }
        public void AddBool(object value)
        {
            m_Length++;
            m_Buffer[m_Length - 1] = Convert.ToByte(value);
        }

        /// <summary>
        /// Adds a char to the buffer and increments the length by 1.
        /// </summary>
        /// <param name="value">Char to add.</param>
        public void AddChar(char value)
        {
            m_Buffer[m_Length] = Convert.ToByte(value);
            m_Length++;
        }
        public void AddChar(object value)
        {
            m_Buffer[m_Length] = Convert.ToByte(value);
            m_Length++;
        }

        /// <summary>
        /// Adds a byte to the buffer and increments the length by 1.
        /// </summary>
        /// <param name="value">Byte to add.</param>
        public void AddByte(byte value)
        {
            m_Buffer[m_Length] = value;
            m_Length++;
        }
        public void AddByte(object value)
        {
            m_Buffer[m_Length] = Convert.ToByte(value);
            m_Length++;
        }

        /// <summary>
        /// Adds a Bytearray to the buffer and increments the length by Bytearray length.
        /// </summary>
        /// <param name="value">Bytearray to add.</param>
        public void AddByte(byte[] value)
        {
            //? What is faster CopyTo or ConstrainedCopy ?
            //! Test done, result ConstrainedCopy
            Array.ConstrainedCopy(value, 0, m_Buffer, m_Length, value.Length);
            //value.CopyTo(pBuffer, pLength);
            m_Length += value.Length;
        }

        /// <summary>
        /// Adds an ushort to the buffer and increments the length by 2.
        /// </summary>
        /// <param name="value">Ushort to add.</param>
        public void AddUShort(ushort value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, m_Buffer, m_Length, 2);
            m_Length += 2;
        }
        public void AddUShort(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToUInt16(value)), 0, m_Buffer, m_Length, 2);
            m_Length += 2;
        }

        /// <summary>
        /// Adds a short to the buffer and increments the length by 2.
        /// </summary>
        /// <param name="value">Short to add.</param>
        public void AddShort(short value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, m_Buffer, m_Length, 2);
            m_Length += 2;
        }
        public void AddShort(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToInt32(value)), 0, m_Buffer, m_Length, 2);
            m_Length += 2;
        }

        /// <summary>
        /// Adds an uinteger to the buffer and increments the length by 4.
        /// </summary>
        /// <param name="value">Uinteger to add.</param>
        public void AddUInteger(uint value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, m_Buffer, m_Length, 4);
            m_Length += 4;
        }
        public void AddUInteger(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToUInt32(value)), 0, m_Buffer, m_Length, 4);
            m_Length += 4;
        }

        /// <summary>
        /// Adds an integer to the buffer and increments the length by 4.
        /// </summary>
        /// <param name="value">Integer to add.</param>
        public void AddInteger(int value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, m_Buffer, m_Length, 4);
            m_Length += 4;
        }
        public void AddInteger(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToInt32(value)), 0, m_Buffer, m_Length, 4);
            m_Length += 4;
        }

        /// <summary>
        /// Adds an ulong to the buffer and increments the length by 8.
        /// </summary>
        /// <param name="value">Ulong to add.</param>
        public void AddULong(ulong value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, m_Buffer, m_Length, 8);
            m_Length += 8;
        }
        public void AddULong(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToUInt64(value)), 0, m_Buffer, m_Length, 8);
            m_Length += 8;
        }

        /// <summary>
        /// Adds an long to the buffer and increments the length by 8.
        /// </summary>
        /// <param name="value">Long to add.</param>
        public void AddLong(long value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, m_Buffer, m_Length, 8);
            m_Length += 8;
        }
        public void AddLong(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToInt64(value)), 0, m_Buffer, m_Length, 8);
            m_Length += 8;
        }

        /// <summary>
        /// Adds a float to the buffer and increments the length by 4.
        /// </summary>
        /// <param name="value">Float to add.</param>
        public void AddFloat(float value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, m_Buffer, m_Length, 4);
            m_Length += 4;
        }
        public void AddFloat(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToSingle(value)), 0, m_Buffer, m_Length, 4);
            m_Length += 4;
        }

        /// <summary>
        /// Adds an ANSI-String to the buffer and increments the length by stringlength.
        /// </summary>
        /// <param name="value">ANSI-String to add.</param>
        public void AddString(string value)
        {
            ushort size = Convert.ToUInt16(value.Length);

            //Add StringLength
            AddUShort(size);

            //Get Bytes & Copy to pBuffer
            byte[] tempBuffer = Encoding.Default.GetBytes(value);
            Array.ConstrainedCopy(tempBuffer, 0, m_Buffer, m_Length, size);

            //Incease Length
            m_Length += size;
        }
        public void AddString(object value)
        {
            ushort size = Convert.ToUInt16(((string)value).Length);

            //Add StringLength
            AddUShort(size);

            //Get Bytes & Copy to pBuffer
            byte[] tempBuffer = Encoding.Default.GetBytes((string)value);
            Array.ConstrainedCopy(tempBuffer, 0, m_Buffer, m_Length, size);

            //Incease Length
            m_Length += size;
        }

        /// <summary>
        /// Adds an Unicode-String to the buffer and increments the length by stringlength.
        /// </summary>
        /// <param name="value">Unicode-String to add.</param>
        public void AddUString(string value)
        {
            ushort size = Convert.ToUInt16(value.Length);

            //Add StringLength
            AddUShort(size);

            //Get Bytes & Copy to pBuffer
            byte[] tempBuffer = Encoding.Unicode.GetBytes(value);
            Array.ConstrainedCopy(tempBuffer, 0, m_Buffer, m_Length, size * 2);

            //Incease Length
            m_Length += (size * 2);
        }
        public void AddUString(object value)
        {
            ushort size = Convert.ToUInt16(((string)value).Length);

            //Add StringLength
            AddUShort(size);

            //Get Bytes & Copy to pBuffer
            byte[] tempBuffer = Encoding.Unicode.GetBytes((string)value);
            Array.ConstrainedCopy(tempBuffer, 0, m_Buffer, m_Length, size * 2);

            //Incease Length
            m_Length += (size * 2);
        }

        public void AddDate(DateTime value)
        {
            AddUShort(value.Year);
            AddUShort(value.Month);
            AddUShort(value.Day);
            AddUShort(value.Hour);
            AddUShort(value.Minute);
            AddUShort(value.Second);
            AddUInteger(value.Millisecond);
        }
        public void AddDate(object value)
        {
            DateTime date = (DateTime)value;
            AddUShort(date.Year);
            AddUShort(date.Month);
            AddUShort(date.Day);
            AddUShort(date.Hour);
            AddUShort(date.Minute);
            AddUShort(date.Second);
            AddUInteger(date.Millisecond);
        }

        /// <summary>
        /// Adds a HexString to the buffer and increments the length by HexStringlength / 2.
        /// </summary>
        /// <param name="Hex">HexString to add.</param>
        public void AddHex(string Hex)
        {
            Hex = Hex.Replace(" ", "");
            Hex = Hex.Replace("-", "");
            AddByte(ToByteArray(Hex));
        }

        #endregion

        /// <summary>
        /// Resets the Pointer to 0.
        /// </summary>
        public void ResetPointer()
        {
            m_Pointer = 0;
        }

        /// <summary>
        /// Resets the Length to 0.
        /// </summary>
        public void ResetLength()
        {
            m_Length = 0;
        }

        private void ResetBuffer()
        {
            m_Buffer = new byte[m_BufferSize];
        }

        /// <summary>
        /// Clears the Buffer and resets the Pointer and Length.
        /// </summary>
        public void Reset()
        {
            ResetBuffer();
            ResetPointer();
            ResetLength();
        }

        /// <summary>
        /// Converts the cPacket instance to Bytearray.
        /// </summary>
        /// <returns>the cPacket instance as Bytearray.</returns>
        public byte[] ToByteArray()
        {
            try
            {
                byte[] packet = new byte[m_Length + 6];
                BitConverter.GetBytes(m_Length).CopyTo(packet, 0);
                BitConverter.GetBytes(m_Opcode).CopyTo(packet, 2);
                Array.Resize<byte>(ref m_Buffer, m_Length);
                m_Buffer.CopyTo(packet, 6);
                //pBuffer = new byte[pBufferSize];
                return packet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Converts the Buffer to String.
        /// </summary>
        /// <returns>the buffer as String.</returns>
        public override string ToString()
        {
            if (m_Length != 0)
            {
                string sData = BitConverter.ToString(m_Buffer, 0, m_Length).Replace("-", "");
                if (m_Pointer > 0)
                {
                    sData = sData.Insert(m_Pointer * 2, "|");
                }
                return sData;
            }
            else
            {
                return "Empty";
            }
        }

        public void Dispose()
        {
            Array.Clear(m_Buffer, 0, m_Buffer.Length);
            m_Buffer = null;
        }

        private byte[] ToByteArray(string HexString)
        {
            Byte[] bytes = new Byte[HexString.Length / 2];
            for (int i = 0; i < HexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}