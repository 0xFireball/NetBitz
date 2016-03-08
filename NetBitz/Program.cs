
using System;
using NetBitz;

namespace NetBitz
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("NetBitz - (c) 2016 0xFireball");
			var f = new AssemblyFactory();
			f.CreateStubModule("nothing.exe", "Hello, World!", "hi");
		}
	}
}