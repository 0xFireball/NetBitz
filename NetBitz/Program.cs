
using System;
using System.IO;
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
			if (args.Length!=2)
			{
				Console.WriteLine("Invalid arguments!\nUsage: \n   NetBitz.Lite.exe <SFX output file name> <input file name>");
				return;
			}
			MemoryStream ms = f.CreateSFXModule(System.IO.File.ReadAllBytes(args[1]).GetString(), args[1]);
			using (var fs = File.Create(args[0]))
        	{
            	ms.WriteTo(fs);
        	}
		}
	}
}