/*
 */
using System;
using System.Reflection;
using System.Collections.Generic;
using OmniBean.PowerCrypt4.Utilities;
using OmniBean.PowerCrypt4;

namespace NBytzCube
{
	public class NBCube
	{
		public static void LaunchAssembly(byte[] assemblyContents)
		{
			if (assemblyContents.Length == 0)
			{
				Console.WriteLine("Decryption Failure.");
				return;
			}
			Assembly asm = Assembly.Load(assemblyContents);
			MethodInfo main = asm.EntryPoint;
			main.Invoke(null, new Object[] { new string[0] });
		}
		public static void ExtractAndLaunchAssembly(string compressedAssemblyContents)
		{
			byte[] assemblyContents = StringCompressor.DecompressString(compressedAssemblyContents).GetBytes();
			Assembly asm = Assembly.Load(assemblyContents);
			MethodInfo main = asm.EntryPoint;
			main.Invoke(null, null);
		}		
	}
}