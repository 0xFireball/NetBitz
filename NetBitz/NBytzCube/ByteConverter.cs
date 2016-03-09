/*
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace OmniBean.PowerCrypt4.Utilities
{
	public static class ByteConverter
	{
		public static byte[] GetBytes(this string str)
		{
			Encoding iso = Encoding.GetEncoding("ISO-8859-1");
			return iso.GetBytes(str);
		}
		
		public static string GetString(this byte[] bytes)
		{
			Encoding iso = Encoding.GetEncoding("ISO-8859-1");
			return iso.GetString(bytes);
		}
		
		public static byte[] RawGetBytes(this string str)
		{
		    byte[] bytes = new byte[str.Length * sizeof(char)];
		    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		    return bytes;
		}
		
		public static string RawGetString(this byte[] bytes)
		{
		    char[] chars = new char[bytes.Length / sizeof(char)];
		    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		    return new string(chars);
		}
	}
}
