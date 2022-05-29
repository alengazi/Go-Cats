using System.Collections.Generic;
using System;
using System.Text;


public class ByteBuffer : IDisposable 
{
    private List<byte> Buff;
    private byte[] readBuff;
    private int readpos;
    private bool buffUpdated = false;

    public ByteBuffer()
    {
        Buff = new List<byte>();
        readpos = 0;
    }

    public int GetReadPos()
    {
        return readpos;
    }

    public byte[] ToArray()
    {
        return Buff.ToArray();
    }

    public int Count()
    {
        return Buff.Count;
    }

    public int Length()
    {
        return Count() - readpos;
    }

    public void Clear()
    {
        Buff.Clear();
        readpos = 0;
    }

    public void WriteBytes(byte[] input)
    {
        Buff.AddRange(Input);
        buffUpdated = true;
    }

    public void WriteShort(short input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }

    public void WriteInteger(int input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }

    public void WriteFloat(float input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }

    public void WriteLong(long input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffUpdated = true;
    }

    public void WriteString(string input)
    {
        Buff.AddRange(BitConverter.GetBytes(input.Length));
        Buff.AddRange(Encoding.ASCII.GetBytes(input));
        buffUpdated = true;
    }

    public int ReadInteger(bool peek = true)
    {
        if(Buff.Count > readpos)
        {
            if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            int ret = BitConverter.ToInt32(readBuff, readpos);

            if(peek && Buff.Count > readpos)
            {
                readpos += 4; // shift to next byte
            }
            return ret; 
        }
        else
        {
            throw new Exception("Byte buffer passed limit");
        }
    }

    public byte[] ReadBytes(int length, bool peek = true)
    {
        if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            byte[] ret = Buff.GetRange(readBuff, length).ToArray();

            if(peek)
            {
                readpos += Length; // shift to next info
            }
    }

    public string ReadString(bool peek = true)
    {
        int len = ReadInteger(true);
        if(buffUpdated)
        {
            readBuff = Buff.ToArray();
            buffUpdated = false;
        }

        string ret = Encoding.ASCII.GetString(readBuff, readpos, len);

        if(peek && Buff.Count > readpos)
        {
           if(ret.Length > 0 )
           {
               readpos += len;
           }
        }
        return ret;
    }

    public short ReadShort(bool peek = true)
    {
        if(buffUpdated)
            {
                readBuff = Buff.ToArray();
                buffUpdated = false;
            }

            short ret = BitConverter.ToInt16(readBuff, readpos);

            if(peek && Buff.Count > readpos)
            {
                readpos += 4; // shift to next byte
            }
            return ret;
    }

    public float ReadFloat(bool peek = true)
    {
        if(buffUpdated)
        {
            readBuff = Buff.ToArray();
            buffUpdated = false;
        }

        float ret = BitConverter.ToSingle(readBuff, readpos);

        if(peek && Buff.Count > readpos)
        {
            readpos += 4;
        }
        return ret;
    }

    public long ReadLong(bool peek = true)
    {
    if(buffUpdated)
        {
            readBuff = Buff.ToArray();
            buffUpdated = false;
        }

        long ret = BitConverter.ToInt64(readBuff, readpos);

        if(peek && Buff.Count > readpos)
        {
            readpos += 8;
        }   
        return ret;
    }

    private bool disposedValue = false; // Detect reduntant calls
    protected virtual void Dispose(bool disposing)
    {
        if(!disposedValue)
        {
            if(disposing)
            {
                Buff.Clear();
            }
            readpos = 0;
        }
         disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(instace);
    }
}
