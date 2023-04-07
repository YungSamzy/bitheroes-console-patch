using System;
using com.ultrabit.bitheroes.fromflash;
using Sfs2X.Util;

namespace com.ultrabit.bitheroes.parsing.model.utility;

public class PNGDecoder
{
	private const uint IHDR = 1229472850u;

	private const uint PLTE = 1347179589u;

	private const uint IDAT = 1229209940u;

	private const uint IEND = 1229278788u;

	private uint imgWidth;

	private uint imgHeight;

	private uint bitDepth;

	private uint colourType;

	private uint compressionMethod;

	private uint filterMethod;

	private uint interlaceMethod;

	private Array chunks;

	private ByteArray input;

	private ByteArray output;

	public BitmapData decode(ByteArray ba)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private void processIHDR(uint index)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private void processIDAT(uint index)
	{
		throw new Exception("Error --> CONTROL.");
	}

	private void getChunks()
	{
		throw new Exception("Error --> CONTROL.");
	}

	private bool readSignature()
	{
		throw new Exception("Error --> CONTROL.");
	}

	private string fixType(uint num)
	{
		throw new Exception("Error --> CONTROL.");
	}
}
