
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OmniBean.PowerCrypt4;
using NetBitz.SFXModuleGen;
using System.Linq;

namespace NetBitz
{
	public class InvalidAssemblySetException : Exception
	{
		public InvalidAssemblySetException() : base() {}
		public InvalidAssemblySetException(string message) : base(message) {}		
	}
	public class AssemblyFactory
	{
		string assemblyName = "ZBytzX";
		string namespaceName = "NBytzCore";
		
		[Obsolete("OldEncryptSFX is deprecated, use one of the newer methods.", true)]
		public void OldEncryptSFX(string fileName, string message, string key)
		{
			var cube = new NBytzCube.NBCube(); //Dummy to import assembly
			
			AssemblyDef cubeDll = AssemblyDef.Load("NBytzCube.dll"); //Load powercrypt
			ModuleDef nbCubeMod = cubeDll.Modules[0];
			nbCubeMod.Kind = ModuleKind.Console; //convert to EXE
			//AssemblyDef dnlibDll = AssemblyDef.Load("dnlib.dll");
			//ModuleDef dnlibModule = dnlibDll.Modules[0];
			Importer importer = new Importer(nbCubeMod);
			

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
						
			MemberRef launchAsmRef = new MemberRefUser(nbCubeMod, "LaunchAssembly", 
                         MethodSig.CreateStatic(nbCubeMod.CorLibTypes.Void, byteArrayRef.ToTypeSig())
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
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction("NetBytz Encrypted SFX - (c) 2016 0xFireball\nEnter key: "));
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleWrite1));
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(PowerAES.Encrypt(message, key))); //push encrypted text
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleReadLine1)); //push key from user
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(decryptRef)); //decrypt
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(getBytesRef)); //getbytes
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(launchAsmRef));//Launch assembly from bytes (managed code)
			epBody.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());//push 0
			epBody.Instructions.Add(OpCodes.Ret.ToInstruction()); //Return/End			
			// Save the assembly to a file on disk
			nbCubeMod.Write(fileName);
		}
		
		public MemoryStream CreateSFXModule(string moduleContents, string originalFileName=null)
		{
			var cube = new NBytzCube.NBCube(); //Dummy to import assembly			
			AssemblyDef cubeDll = AssemblyDef.Load("NBytzCube.dll"); //Load powercrypt
			ModuleDef nbCubeMod = cubeDll.Modules[0];
			nbCubeMod.Kind = ModuleKind.Console; //convert to EXE
			//AssemblyDef dnlibDll = AssemblyDef.Load("dnlib.dll");
			//ModuleDef dnlibModule = dnlibDll.Modules[0];
			Importer importer = new Importer(nbCubeMod);
			
			if (originalFileName != null)
			{
				AssemblyDef originalAsm = AssemblyDef.Load(originalFileName);
				ModuleDef originalMod = originalAsm.Modules[0];
				nbCubeMod.Kind = originalMod.Kind;
			}			

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
						
			MemberRef extractAndLaunchAsmRef = new MemberRefUser(nbCubeMod, "ExtractAndLaunchAssembly", 
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
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(CompressString(moduleContents))); //push encrypted text
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(extractAndLaunchAsmRef));//Helper Method Launch assembly
			epBody.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());//push 0
			epBody.Instructions.Add(OpCodes.Ret.ToInstruction()); //Return/End			
			//write to stream
			var ms = new MemoryStream();
			nbCubeMod.Write(ms);
			return ms;
		}
		
		public MemoryStream CreateSFXModuleEx(List<string> assemblyNames)
		{
			List<ModuleDefMD> modules = assemblyNames.Select(asmName => ModuleDefMD.Load(asmName)).ToList();
			return MultiSFXModuleBuilder.CreateSFXModuleEx(modules.Zip(assemblyNames, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v));
		}
		
		static string CompressString(string theString)
		{
			return NBytzCube.StringCompressor.CompressString(theString);
		}
	}
}