using System;
using System.IO;
using System.Text;

internal class SOReader
{
	public byte[] file_data;

	public int file_size;

	public int pos;

	public SOReader(string filename)
	{
		file_data = File.ReadAllBytes(filename);
		file_size = 0;
		pos = 0;
	}

	public byte Read8()
	{
		return file_data[pos++];
	}

	public ushort Read16()
	{
		return (ushort)((file_data[pos++] << 8) | file_data[pos++]);
	}

	public uint Read32()
	{
		uint num = 0u;
		for (int i = 0; i < 4; i++)
		{
			num = (num << 8) | file_data[pos++];
		}
		return num;
	}

	public int ReadCompressedInt()
	{
		int num = 0;
		byte b = 0;
		bool flag = true;
		int num2 = 0;
		for (int i = 0; i < 3; i++)
		{
			b = Read8();
			flag = (b & 0x80) == 0;
			num <<= 7;
			num |= b & 0x7F;
			num2 += 7;
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			b = Read8();
			num <<= 8;
			num |= b;
			num2 += 8;
		}
		if (num >> num2 - 1 == 1 && num2 == 29)
		{
			num = (int)(-(~(num | (uint)(-1 << num2)) + 1));
		}
		return num;
	}

	public double ReadDouble()
	{
		byte[] array = new byte[8];
		for (int i = 0; i < 8; i++)
		{
			array[i] = file_data[pos + 7 - i];
		}
		pos += 8;
		return BitConverter.ToDouble(array, 0);
	}

	public string ReadString(int length)
	{
		string @string = Encoding.UTF8.GetString(file_data, pos, length);
		pos += length;
		return @string;
	}
}
