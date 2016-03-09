using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OmniBean.PowerCrypt4;
using OmniBean.PowerCrypt4.Utilities;
using System.Linq;
using NetBitz;

namespace NetBitz.SFXModuleGen
{
	static class MultiSFXModuleBuilder
	{
		public static string assemblyName = "ZBytzX";
		public static string namespaceName = "NBytzCore";
		
		public static MemoryStream CreateSFXModuleEx(Dictionary<ModuleDefMD, string> inputModules)
		{
			var cube = new NBytzCube.NBCube(); //Dummy to import assembl		
			AssemblyDef cubeDll = AssemblyDef.Load("NBytzCube.dll"); //Load NBCube
			ModuleDef nbCubeMod = cubeDll.Modules[0];
			nbCubeMod.Kind = ModuleKind.Console; //convert to EXE
			Importer importer = new Importer(nbCubeMod);
			
			IEnumerable<ModuleDefMD> __mainModule = inputModules.Keys.Where(mod=>mod.Kind==ModuleKind.Console||mod.Kind==ModuleKind.Windows);
			if (__mainModule.Count()!=1)
			{
				throw new InvalidAssemblySetException();
			}
			ModuleDefMD mainModule = __mainModule.ElementAt(0);
			nbCubeMod.Kind = mainModule.Kind;
			string moduleContents = "";
			moduleContents+=Convert.ToBase64String(File.ReadAllBytes(inputModules[mainModule])); //add exe module first
			inputModules.Remove(mainModule);
			foreach (string fileName in inputModules.Values)
			{
				moduleContents+="_"+Convert.ToBase64String(File.ReadAllBytes(fileName)); //add module to mess
			}
			moduleContents = CompressString(moduleContents); //compress
			GC.Collect();
			GC.WaitForPendingFinalizers(); //Clean up the massive memory usage
			
			#region Create EntryPoint
			// Add the startup type. It derives from System.Object.
			TypeDef startUpType = new TypeDefUser(namespaceName, "Startup", nbCubeMod.CorLibTypes.Object.TypeDefOrRef);
			startUpType.Attributes = TypeAttributes.NotPublic | TypeAttributes.AutoLayout |
									TypeAttributes.Class | TypeAttributes.AnsiClass;
			// Add the type to the module
			nbCubeMod.Types.Add(startUpType);

			// Create the entry point method
			MethodDef entryPoint = new MethodDefUser("Main",
				MethodSig.CreateStatic(nbCubeMod.CorLibTypes.Int32, new SZArraySig(nbCubeMod.CorLibTypes.String)));
			entryPoint.Attributes = MethodAttributes.Private | MethodAttributes.Static |
							MethodAttributes.HideBySig | MethodAttributes.ReuseSlot;
			entryPoint.ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed;
			// Name the 1st argument (argument 0 is the return type)
			entryPoint.ParamDefs.Add(new ParamDefUser("args", 1));
			// Add the method to the startup type
			startUpType.Methods.Add(entryPoint);
			// Set module entry point
			nbCubeMod.EntryPoint = entryPoint;
			#endregion
			
			#region TypeRefs
			// Create a TypeRef to System.Console
			TypeRef consoleRef = new TypeRefUser(nbCubeMod, "System", "Console", nbCubeMod.CorLibTypes.AssemblyRef);
			// Create a method ref to 'System.Void System.Console::WriteLine(System.String)'
			MemberRef consoleWrite1 = new MemberRefUser(nbCubeMod, "WriteLine",
						MethodSig.CreateStatic(nbCubeMod.CorLibTypes.Void, nbCubeMod.CorLibTypes.String),
						consoleRef);
			
			MemberRef consoleReadLine1 = new MemberRefUser(nbCubeMod, "ReadLine",
						MethodSig.CreateStatic(nbCubeMod.CorLibTypes.String),
						consoleRef);
			
			AssemblyRef powerAESLibRef = cubeDll.ToAssemblyRef();
			
			TypeRef powerAESRef = new TypeRefUser(nbCubeMod, "OmniBean.PowerCrypt4", "PowerAES",
                         powerAESLibRef);
			
			ITypeDefOrRef byteArrayRef = importer.Import(typeof(System.Byte[]));
			
			MemberRef decryptRef = new MemberRefUser(nbCubeMod, "Decrypt", 
                         MethodSig.CreateStatic(nbCubeMod.CorLibTypes.String, nbCubeMod.CorLibTypes.String, nbCubeMod.CorLibTypes.String)
                        ,powerAESRef);
			
			TypeRef byteConverterRef = new TypeRefUser(nbCubeMod, "OmniBean.PowerCrypt4.Utilities", "ByteConverter",
                         powerAESLibRef);
						
			MemberRef getBytesRef = new MemberRefUser(nbCubeMod, "GetBytes", 
                         MethodSig.CreateStatic(byteArrayRef.ToTypeSig(), nbCubeMod.CorLibTypes.String)
                        ,byteConverterRef);
			
			TypeRef nbCubeRef = new TypeRefUser(nbCubeMod, "NBytzCube", "NBCube",
                         powerAESLibRef);
						
			MemberRef extractAndLaunchAsmRef = new MemberRefUser(nbCubeMod, "ExtractUnpackAndLaunchAssembly", 
                         MethodSig.CreateStatic(nbCubeMod.CorLibTypes.Void, nbCubeMod.CorLibTypes.String)
                        ,nbCubeRef);
			
			TypeRef fileRef = new TypeRefUser(nbCubeMod, "System.IO", "File",
                         nbCubeMod.CorLibTypes.AssemblyRef);
			
			MemberRef writeBytesRef = new MemberRefUser(nbCubeMod, "WriteAllBytes", 
                        MethodSig.CreateStatic(nbCubeMod.CorLibTypes.Void, nbCubeMod.CorLibTypes.String, byteArrayRef.ToTypeSig()),
                        fileRef);
			#endregion
			
			// Add a CIL method body to the entry point method
			CilBody epBody = new CilBody();
			entryPoint.Body = epBody;
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction("NetBytz Encrypted SFX - (c) 2016 0xFireball"));
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleWrite1));
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(moduleContents)); //push encrypted text
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(extractAndLaunchAsmRef));//Helper Method Launch assembly
			epBody.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());//push 0
			epBody.Instructions.Add(OpCodes.Ret.ToInstruction()); //Return/End			
			//write to stream
			var ms = new MemoryStream();
			nbCubeMod.Write(ms);
			return ms;
		}
		
		static string CompressString(string theString)
		{
			return NBytzCube.StringCompressor.CompressString(theString);
		}
	}
}
