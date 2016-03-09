
using System;
using NetBitz;
using OmniBean.PowerCrypt4.Utilities;

namespace NetBitz
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("NetBitz Lite - (c) 2016 0xFireball");
			var f = new AssemblyFactory();
			if (args.Length!=4)
			{
				Console.WriteLine("Invalid arguments!\nUsage: \n   NetBitz.Lite.exe <output file name> <input file name> <encryption key> <extracted file name>");
				return;
			}
			f.CreateStubModule(args[0], System.IO.File.ReadAllBytes(args[1]).GetString(), args[2],args[3]);
		}
	}
}