using System;
using System.Collections.Generic;
using System.Text;

namespace CNGPI
{
    public class MsgDataStream : IDisposable
    {
        System.IO.MemoryStream stream;
        public MsgDataStream(byte[] datatoread,int len)
        {
            stream = new System.IO.MemoryStream(datatoread,0,len);
        }
        public MsgDataStream(System.IO.MemoryStream streamtowrite)
        {
            stream = streamtowrite;
        }

        public byte[] ReadToEnd()
        {
            return ReadByteArray((int)stream.Length - (int)stream.Position - 4);
        }

        public int ReadInt16()
        {
            byte[] bt = ReadByteArray(2);
            return (bt[0] << 8) | bt[1];
        }

        public uint ReadInt32()
        {
            byte[] bt = ReadByteArray(4);
            return (uint)((bt[0] << 24) | (bt[1] << 16) | (bt[2] << 8) | (bt[3]));
        }

        public Byte ReadByte()
        {
            byte[] bt = ReadByteArray(1);
            return bt[0];
        }

        public Byte[] ReadByteArray(int len)
        {
            byte[] buffer = new byte[len];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public string ReadString(int len)
        {            
            return Utility.ByteToStr(ReadByteArray(len));
        }

        public string ReadHex(int len)
        {
            return Utility.ByteToHex(ReadByteArray(len));
        }
        public void WriteHex(string hex)
        {
            WriteByteArray(Utility.HexToByte(hex));
        }

        public void WriteString(string content,int len)
        {
            WriteByteArray(Utility.StrToByte(content,len));
        }

        public void  WriteByteArray(Byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public void WriteInt16(int val)
        {
            byte[] bt = new byte[] {
                  (byte)((val>>8)&0x000000ff),
                   (byte)((val)&0x000000ff)
            };
            WriteByteArray(bt);
        }

        public void WriteInt32(uint val)
        {
            byte[] bt = new byte[] {
                (byte)((val>>24)&0x000000ff),
                 (byte)((val>>16)&0x000000ff),
                  (byte)((val>>8)&0x000000ff),
                   (byte)((val)&0x000000ff)
            };
            WriteByteArray(bt);
        }

        public void WriteByte(byte val)
        {
            byte[] bt = new byte[] { val };
            WriteByteArray(bt);
        }

        public void Dispose()
        {
            stream.Close();
            stream.Dispose();
        }
    }
}
