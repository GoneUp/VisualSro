using System;
using System.Text;

namespace Framework
{
    /// <summary>
    /// Represents a Silkroad packet that stores Length,Opcode and Data.
    /// </summary>
    public class cPacket : IDisposable
    {

        #region -----Field/Propertys-----


        /// <summary>
        /// The Opcode as HexWord
        /// </summary>
        public string OpcodeHEX
        {
            get { return pOpcode.ToString("X2"); }
            set { pOpcode = Convert.ToUInt16(value, 16); }
        }

        /// <summary>
        /// The Opcode as Ushort
        /// </summary>
        public ushort Opcode
        {
            get { return pOpcode; }
            set { pOpcode = value; }
        }
        private ushort pOpcode;

        /// <summary>
        /// Current pointer position
        /// </summary>
        public int Pointer
        {
            get { return pPointer; }
            set { pPointer = value; }
        }
        private int pPointer;

        /// <summary>
        /// Packet Length
        /// </summary>
        public int Length
        {
            get { return pLength; }
            set { pLength = value; }
        }
        private int pLength;

        /// <summary>
        /// BufferSize : Default 8192
        /// </summary>
        public int BufferSize
        {
            get { return pBufferSize; }
            set
            {
                pBufferSize = value;
                Array.Resize<byte>(ref pBuffer, value);
            }
        }
        private int pBufferSize = 8192; //Silkroad Default: 8192

        /// <summary>
        /// Data Buffer
        /// </summary>
        public byte[] Buffer
        {
            get { return pBuffer; }
            set { pBuffer = value; }
        }
        private byte[] pBuffer;

        #endregion

        #region -----Constructor-----

        /// <summary>
        /// Creates an instance of cPacket using only the received bytes from client.
        /// </summary>
        /// <param name="buffer">The Bytearray received from client.</param>
        public cPacket(byte[] buffer)
        {
            pLength = BitConverter.ToUInt16(buffer, 0);
            pOpcode = BitConverter.ToUInt16(buffer, 2);
            pBuffer = new Byte[pBufferSize];
            if (pLength > 0)
            {
                Array.ConstrainedCopy(buffer, 6, pBuffer, 0, pLength);
            }


        }

        /// <summary>
        /// Creates an empty instance of cPacket using only the Opcode.
        /// </summary>
        /// <param name="Opcode">Opcode as ushort.</param>
        public cPacket(ushort Opcode)
        {
            pOpcode = Opcode;
            pBuffer = new Byte[pBufferSize];
        }

        /// <summary>
        /// Creates an empty instance of cPacket using only the Opcode.
        /// </summary>
        /// <param name="Opcode">Opcode as string.</param>
        public cPacket(string Opcode)
        {
            pOpcode = Convert.ToUInt16(Opcode, 16);

            pBuffer = new Byte[pBufferSize];
        }

        /// <summary>
        /// Creates an intance of cPacket using the Opcode and Data(Buffer)
        /// </summary>
        /// <param name="Opcode">Opcode as ushort.</param>
        /// <param name="Buffer">Data(Buffer) as Bytearray.</param>
        public cPacket(ushort Opcode, Byte[] Buffer)
        {
            pOpcode = Opcode;
            pBuffer = Buffer;
        }

        /// <summary>
        /// Creates an intance of cPacket using the Opcode and Data(Buffer)
        /// </summary>
        /// <param name="Opcode">Opcode as ushort.</param>
        /// <param name="Buffer">Data(Buffer) as HexString.</param>
        public cPacket(ushort Opcode, string Buffer)
        {
            pOpcode = Opcode;
            pBuffer = new Byte[pBufferSize];
            AddHex(Buffer);
        }

        /// <summary>
        /// Creates an intance of cPacket using the Opcode and Data(Buffer)
        /// </summary>
        /// <param name="Opcode">Opcode as string.</param>
        /// <param name="Buffer">Data(Buffer) as Bytearray.</param>
        public cPacket(string Opcode, Byte[] Buffer)
        {
            pOpcode = Convert.ToUInt16(Opcode, 16);
            pBuffer = Buffer;
        }

        /// <summary>
        /// Creates an intance of cPacket using the Opcode and Data(Buffer)
        /// </summary>
        /// <param name="Opcode">Opcode as string.</param>
        /// <param name="Buffer">Data(Buffer) as HexString.</param>
        public cPacket(string Opcode, string Buffer)
        {
            pOpcode = Convert.ToUInt16(Opcode, 16);
            pBuffer = new Byte[pBufferSize];
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
            pPointer++;

            return Convert.ToBoolean(pBuffer[pPointer - 1]);
        }

        /// <summary>
        /// Reads a char value from buffer and increments the Pointer by 1.
        /// </summary>
        /// <returns>Returns the read byte as char.</returns>
        public char ReadChar()
        {
            //Increase Pointer
            pPointer++;

            return Convert.ToChar(pBuffer[pPointer - 1]);
        }

        /// <summary>
        /// Reads a byte value from buffer and increments the Pointer by 1.
        /// </summary>
        /// <returns>the read byte.</returns>
        public byte ReadByte()
        {
            //Increase Pointer
            pPointer++;

            return pBuffer[pPointer - 1];
        }

        /// <summary>
        /// Reads a bytearray value from buffer and increments the Pointer by count.
        /// </summary>
        /// <param name="count">Number of bytes to read.</param>
        /// <returns>the read bytearray.</returns>
        public byte[] ReadByte(int count)
        {
            //Increase Pointer
            pPointer += count;

            //Copy to tempBuffer
            byte[] tempBuffer = new byte[count];
            Array.ConstrainedCopy(pBuffer, pPointer - count, tempBuffer, 0, count);

            return tempBuffer;
        }

        /// <summary>
        /// Reads an ushort from buffer and increments the Pointer by 2.
        /// </summary>
        /// <returns>the read ushort.</returns>
        public ushort ReadUShort()
        {
            //Increase Pointer
            pPointer += sizeof(ushort);

            return BitConverter.ToUInt16(pBuffer, pPointer - sizeof(ushort));
        }

        /// <summary>
        /// Reads a short from buffer and increments the Pointer by 2.
        /// </summary>
        /// <returns>the read short.</returns>
        public short ReadShort()
        {
            //Increase Pointer
            pPointer += sizeof(ushort);

            return BitConverter.ToInt16(pBuffer, pPointer - sizeof(ushort));
        }

        /// <summary>
        /// Reads an uinteger from buffer and increments the Pointer by 4.
        /// </summary>
        /// <returns>the read uinteger.</returns>
        public uint ReadUInteger()
        {
            //Increase Pointer
            pPointer += sizeof(uint);

            return BitConverter.ToUInt32(pBuffer, pPointer - sizeof(int));
        }

        /// <summary>
        /// Reads an integer from buffer and increments the Pointer by 4.
        /// </summary>
        /// <returns>the read integer.</returns>
        public int ReadInteger()
        {
            //Increase Pointer
            pPointer += sizeof(uint);

            return BitConverter.ToInt32(pBuffer, pPointer - sizeof(uint));
        }

        /// <summary>
        /// Reads an ulong from buffer and increments the Pointer by 8.
        /// </summary>
        /// <returns>the read ulong.</returns>
        public ulong ReadULong()
        {
            //Increase Pointer
            pPointer += sizeof(ulong);

            return BitConverter.ToUInt64(pBuffer, pPointer - sizeof(ulong));
        }

        /// <summary>
        /// Reads a long from buffer and increments the Pointer by 8.
        /// </summary>
        /// <returns>the read long.</returns>
        public long ReadLong()
        {
            //Increase Pointer
            pPointer += sizeof(ulong);

            return BitConverter.ToInt64(pBuffer, pPointer - sizeof(long));
        }

        /// <summary>
        /// Reads a float from buffer and increments the Pointer by 4.
        /// </summary>
        /// <returns>the read float.</returns>
        public float ReadFloat()
        {
            //Increase Pointer
            pPointer += sizeof(float);

            return BitConverter.ToSingle(pBuffer, pPointer - sizeof(float));
        }

        /// <summary>
        /// Reads the stringlength + ansi-string from buffer and increments the Pointer by 4 + stringlength.
        /// </summary>
        /// <returns>the read ANSI-String.</returns>
        public string ReadString()
        {
            int length = ReadUShort();

            //Increase Pointer
            pPointer += length;

            return Encoding.Default.GetString(pBuffer, pPointer - length, length);
        }

        /// <summary>
        /// Reads an ansi-string from buffer using the given length and increments the Pointer by length.
        /// </summary>
        /// <returns>the read ANSI-String.</returns>
        public string ReadString(int length)
        {

            //Increase Pointer
            pPointer += length;

            return Encoding.Default.GetString(pBuffer, pPointer - length, length);
        }

        /// <summary>
        /// Reads the stringlength + unicode string from buffer and increments the Pointer by 4 + stringlength.
        /// </summary>
        /// <returns>the read Unicode-String.</returns>
        public string ReadUnicodeString()
        {
            int length = ReadUShort();

            //Increase Pointer
            pPointer += length;

            return Encoding.Unicode.GetString(pBuffer, pPointer - length, length);
        }

        /// <summary>
        /// Reads an Unicode-string from buffer using the given length and increments the Pointer by length.
        /// </summary>
        /// <returns>the read Unicode-String.</returns>
        public string ReadUnicodeString(int length)
        {
            length *= 2;

            //Increase Pointer
            pPointer += length;

            return Encoding.Unicode.GetString(pBuffer, pPointer - length, length);
        }

        #endregion

        #region -----Add-----

        /// <summary>
        /// Adds a boolean to the buffer and increments the length by 1.
        /// </summary>
        /// <param name="value">Boolean to add.</param>
        public void AddBool(bool value)
        {
            pLength++;
            pBuffer[pLength - 1] = Convert.ToByte(value);
        }
        public void AddBool(object value)
        {
            pLength++;
            pBuffer[pLength - 1] = Convert.ToByte(value);
        }

        /// <summary>
        /// Adds a char to the buffer and increments the length by 1.
        /// </summary>
        /// <param name="value">Char to add.</param>
        public void AddChar(char value)
        {
            pBuffer[pLength] = Convert.ToByte(value);
            pLength++;
        }
        public void AddChar(object value)
        {
            pBuffer[pLength] = Convert.ToByte(value);
            pLength++;
        }

        /// <summary>
        /// Adds a byte to the buffer and increments the length by 1.
        /// </summary>
        /// <param name="value">Byte to add.</param>
        public void AddByte(byte value)
        {
            pBuffer[pLength] = value;
            pLength++;
        }
        public void AddByte(object value)
        {
            pBuffer[pLength] = Convert.ToByte(value);
            pLength++;
        }

        /// <summary>
        /// Adds a Bytearray to the buffer and increments the length by Bytearray length.
        /// </summary>
        /// <param name="value">Bytearray to add.</param>
        public void AddByte(byte[] value)
        {
            //? What is faster CopyTo or ConstrainedCopy ?
            //! Test done, result ConstrainedCopy
            Array.ConstrainedCopy(value, 0, pBuffer, pLength, value.Length);
            //value.CopyTo(pBuffer, pLength);
            pLength += value.Length;
        }

        /// <summary>
        /// Adds an ushort to the buffer and increments the length by 2.
        /// </summary>
        /// <param name="value">Ushort to add.</param>
        public void AddUShort(ushort value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, pBuffer, pLength, 2);
            pLength += 2;
        }
        public void AddUShort(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToUInt16(value)), 0, pBuffer, pLength, 2);
            pLength += 2;
        }

        /// <summary>
        /// Adds a short to the buffer and increments the length by 2.
        /// </summary>
        /// <param name="value">Short to add.</param>
        public void AddShort(short value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, pBuffer, pLength, 2);
            pLength += 2;
        }
        public void AddShort(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToInt32(value)), 0, pBuffer, pLength, 2);
            pLength += 2;
        }

        /// <summary>
        /// Adds an uinteger to the buffer and increments the length by 4.
        /// </summary>
        /// <param name="value">Uinteger to add.</param>
        public void AddUInteger(uint value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, pBuffer, pLength, 4);
            pLength += 4;
        }
        public void AddUInteger(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToUInt32(value)), 0, pBuffer, pLength, 4);
            pLength += 4;
        }

        /// <summary>
        /// Adds an integer to the buffer and increments the length by 4.
        /// </summary>
        /// <param name="value">Integer to add.</param>
        public void AddInteger(int value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, pBuffer, pLength, 4);
            pLength += 4;
        }
        public void AddInteger(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToInt32(value)), 0, pBuffer, pLength, 4);
            pLength += 4;
        }

        /// <summary>
        /// Adds an ulong to the buffer and increments the length by 8.
        /// </summary>
        /// <param name="value">Ulong to add.</param>
        public void AddULong(ulong value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, pBuffer, pLength, 8);
            pLength += 8;
        }
        public void AddULong(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToUInt64(value)), 0, pBuffer, pLength, 8);
            pLength += 8;
        }

        /// <summary>
        /// Adds an long to the buffer and increments the length by 8.
        /// </summary>
        /// <param name="value">Long to add.</param>
        public void AddLong(long value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, pBuffer, pLength, 8);
            pLength += 8;
        }
        public void AddLong(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToInt64(value)), 0, pBuffer, pLength, 8);
            pLength += 8;
        }

        /// <summary>
        /// Adds a float to the buffer and increments the length by 4.
        /// </summary>
        /// <param name="value">Float to add.</param>
        public void AddFloat(float value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(value), 0, pBuffer, pLength, 4);
            pLength += 4;
        }
        public void AddFloat(object value)
        {
            Array.ConstrainedCopy(BitConverter.GetBytes(Convert.ToSingle(value)), 0, pBuffer, pLength, 4);
            pLength += 4;
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
            Array.ConstrainedCopy(tempBuffer, 0, pBuffer, pLength, size);

            //Incease Length
            pLength += size;
        }
        public void AddString(object value)
        {
            ushort size = Convert.ToUInt16(((string)value).Length);

            //Add StringLength
            AddUShort(size);

            //Get Bytes & Copy to pBuffer
            byte[] tempBuffer = Encoding.Default.GetBytes((string)value);
            Array.ConstrainedCopy(tempBuffer, 0, pBuffer, pLength, size);

            //Incease Length
            pLength += size;
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
            Array.ConstrainedCopy(tempBuffer, 0, pBuffer, pLength, size * 2);

            //Incease Length
            pLength += (size * 2);
        }
        public void AddUString(object value)
        {
            ushort size = Convert.ToUInt16(((string)value).Length);

            //Add StringLength
            AddUShort(size);

            //Get Bytes & Copy to pBuffer
            byte[] tempBuffer = Encoding.Unicode.GetBytes((string)value);
            Array.ConstrainedCopy(tempBuffer, 0, pBuffer, pLength, size * 2);

            //Incease Length
            pLength += (size * 2);
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
            pPointer = 0;
        }

        /// <summary>
        /// Resets the Length to 0.
        /// </summary>
        public void ResetLength()
        {
            pLength = 0;
        }

        private void ResetBuffer()
        {
            pBuffer = new byte[pBufferSize];
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
                byte[] packet = new byte[pLength + 6];
                BitConverter.GetBytes(pLength).CopyTo(packet, 0);
                BitConverter.GetBytes(pOpcode).CopyTo(packet, 2);
                Array.Resize<byte>(ref pBuffer, pLength);
                pBuffer.CopyTo(packet, 6);
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
            if (pLength != 0)
            {
                string sData = BitConverter.ToString(pBuffer, 0, pLength).Replace("-", "");
                if (pPointer > 0)
                {
                    sData = sData.Insert(pPointer * 2, "|");
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
            Array.Clear(pBuffer, 0, pBuffer.Length);
            pBuffer = null;
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