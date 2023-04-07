using System;
using com.ultrabit.bitheroes.fromflash;
using Sfs2X.Util;

namespace com.ultrabit.bitheroes.parsing.model.utility;

public class PNGEnc
{
	private static Array crcTable;

	private static bool crcTableComputed;

	public static ByteArray encode(BitmapData img, uint type = 0u)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private static void writeRaw(BitmapData img, ByteArray IDAT)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private static void writeSub(BitmapData img, ByteArray IDAT)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private static void writeChunk(ByteArray png, uint type, ByteArray data)
	{
		throw new Exception("Error --> CONTROL.");
	}
}
