using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct SOTypes
{
	public const byte TYPE_UNDEFINED = 0;

	public const byte TYPE_NULL = 1;

	public const byte TYPE_BOOL_FALSE = 2;

	public const byte TYPE_BOOL_TRUE = 3;

	public const byte TYPE_INT = 4;

	public const byte TYPE_DOUBLE = 5;

	public const byte TYPE_STRING = 6;
}
