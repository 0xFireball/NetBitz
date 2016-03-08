
using System;
using System.Collections.Generic;
using System.Text;
using dnlib;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OmniBean.PowerCrypt4;

namespace NetBitz
{
	public class AssemblyFactory
	{
		string assemblyName = "ZBytzX";
		string namespaceName = "NBytzCore";
		public void CreateStubModule(string fileName, string message, string key)
		{
			/*
			ModuleDef mod = new ModuleDefUser(fileName)
			{
				Kind = ModuleKind.Console
			};
			*/
			/*
			ModuleDef libpcMod = ModuleDefMD.Load("LibPowerCrypt4.dll");
			libpcMod.Kind = ModuleKind.Console;
			AssemblyDef asm = new AssemblyDefUser(assemblyName, new Version(1, 2, 3, 4));
			
			asm.Modules.Add(libpcMod);
			*/
			AssemblyDef libpowercryptDll = AssemblyDef.Load("LibPowerCrypt4.dll"); //Load powercrypt
			ModuleDef libpcMod = libpowercryptDll.Modules[0];
			libpcMod.Kind = ModuleKind.Console; //convert to EXE
			Importer importer = new Importer(libpcMod);
			
			
			/*
			// Add a .NET resource
			byte[] resourceData = Encoding.UTF8.GetBytes("Hello, world!");
			string nbDataName = namespaceName+".nbdata";
			mod.Resources.Add(new EmbeddedResource(nbDataName, resourceData,
							ManifestResourceAttributes.Private));
			*/

			// Add the startup type. It derives from System.Object.
			TypeDef startUpType = new TypeDefUser(namespaceName, "Startup", libpcMod.CorLibTypes.Object.TypeDefOrRef);
			startUpType.Attributes = TypeAttributes.NotPublic | TypeAttributes.AutoLayout |
									TypeAttributes.Class | TypeAttributes.AnsiClass;
			// Add the type to the module
			libpcMod.Types.Add(startUpType);

			// Create the entry point method
			MethodDef entryPoint = new MethodDefUser("Main",
				MethodSig.CreateStatic(libpcMod.CorLibTypes.Int32, new SZArraySig(libpcMod.CorLibTypes.String)));
			entryPoint.Attributes = MethodAttributes.Private | MethodAttributes.Static |
							MethodAttributes.HideBySig | MethodAttributes.ReuseSlot;
			entryPoint.ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed;
			// Name the 1st argument (argument 0 is the return type)
			entryPoint.ParamDefs.Add(new ParamDefUser("args", 1));
			// Add the method to the startup type
			startUpType.Methods.Add(entryPoint);
			// Set module entry point
			libpcMod.EntryPoint = entryPoint;

			// Create a TypeRef to System.Console
			TypeRef consoleRef = new TypeRefUser(libpcMod, "System", "Console", libpcMod.CorLibTypes.AssemblyRef);
			// Create a method ref to 'System.Void System.Console::WriteLine(System.String)'
			MemberRef consoleWrite1 = new MemberRefUser(libpcMod, "WriteLine",
						MethodSig.CreateStatic(libpcMod.CorLibTypes.Void, libpcMod.CorLibTypes.String),
						consoleRef);
			
			AssemblyRef powerAESLibRef = libpowercryptDll.ToAssemblyRef();
			
			TypeRef powerAESRef = new TypeRefUser(libpcMod, "OmniBean.PowerCrypt4", "PowerAES",
                         powerAESLibRef);
			
			MemberRef decryptRef = new MemberRefUser(libpcMod, "Decrypt", 
                         MethodSig.CreateStatic(libpcMod.CorLibTypes.String, libpcMod.CorLibTypes.String, libpcMod.CorLibTypes.String)
                        ,powerAESRef);

			// Add a CIL method body to the entry point method
			CilBody epBody = new CilBody();
			entryPoint.Body = epBody;
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(PowerAES.Encrypt(message, key)));
			
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(key));
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(decryptRef));
			
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleWrite1));
			epBody.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());
			epBody.Instructions.Add(OpCodes.Ret.ToInstruction()); //Return/End

			// Save the assembly to a file on disk
			libpcMod.Write(fileName);
		}
	}
}