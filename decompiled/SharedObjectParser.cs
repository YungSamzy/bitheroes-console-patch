using System;
using System.Collections.Generic;
using System.IO;
using com.ultrabit.bitheroes.model.utility;

public class SharedObjectParser
{
	public static SharedObject Parse(string filename, SharedObject so = null)
	{
		if (so == null)
		{
			so = new SharedObject();
		}
		if (!File.Exists(filename))
		{
			Console.WriteLine("SharedObject " + filename + " doesn't exist.");
			return so;
		}
		SOReader sOReader = new SOReader(filename);
		List<string> list = new List<string>();
		SOHeader sOHeader = default(SOHeader);
		sOHeader.padding1 = sOReader.Read16();
		sOHeader.file_size = sOReader.Read32();
		sOReader.file_size = (int)(sOHeader.file_size + 6);
		sOHeader.padding2 = sOReader.Read32();
		sOHeader.padding3 = sOReader.Read16();
		sOHeader.padding4 = sOReader.Read32();
		Console.WriteLine("Data size: " + sOHeader.file_size);
		ushort length = sOReader.Read16();
		string text = sOReader.ReadString(length);
		Console.WriteLine("SO name: " + text);
		Console.WriteLine("SO type: " + sOReader.Read32());
		while (sOReader.pos < sOReader.file_size)
		{
			SOValue item = default(SOValue);
			uint num = (uint)sOReader.ReadCompressedInt();
			bool flag = (num & 1) != 0;
			num >>= 1;
			if (flag)
			{
				item.key = sOReader.ReadString((int)num);
				list.Add(item.key);
			}
			else if (num < list.Count)
			{
				item.key = list[(int)num];
			}
			else
			{
				D.Log($"SharedObjectParser out of Range {num}");
			}
			Console.WriteLine(item.key + " (inline: " + flag + ")");
			item.type = sOReader.Read8();
			if (item.type == 1)
			{
				Console.WriteLine("\tNULL");
			}
			else if (item.type == 2)
			{
				item.bool_val = false;
				Console.WriteLine("\tFalse");
			}
			else if (item.type == 3)
			{
				item.bool_val = true;
				Console.WriteLine("\tTrue");
			}
			else if (item.type == 4)
			{
				item.int_val = sOReader.ReadCompressedInt();
				Console.WriteLine("\t" + item.int_val);
			}
			else if (item.type == 5)
			{
				item.double_val = sOReader.ReadDouble();
				Console.WriteLine("\t" + item.double_val);
			}
			else if (item.type == 6)
			{
				int num2 = sOReader.ReadCompressedInt();
				bool num3 = (num2 & 1) > 0;
				num2 >>= 1;
				if (!num3)
				{
					Console.WriteLine("\tReference to string: " + num2);
					if (num2 < list.Count)
					{
						item.string_val = list[num2];
						Console.WriteLine("\t" + item.string_val);
					}
				}
				else
				{
					item.string_val = sOReader.ReadString(num2);
					list.Add(item.string_val);
					Console.WriteLine("\t" + item.string_val + " (" + num2 + ")");
				}
			}
			else
			{
				Console.WriteLine("Type not implemented yet: " + item.type);
				while (sOReader.pos < sOReader.file_size)
				{
					if (sOReader.Read8() == 0)
					{
						sOReader.pos--;
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(item.key) && !so.values.Contains(item))
			{
				so.values.Add(item);
			}
			else
			{
				D.Log("so_value key is empty (" + item.key + ") or so_value is already on dictionary");
			}
			if (sOReader.pos < sOReader.file_size)
			{
				sOReader.Read8();
			}
		}
		return so;
	}
}
