
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
		public void CreateStubModule(string fileName, string message, string key, string sfxOutputFileName)
		{
			/*			
			ModuleDef zBytzXOutput = new ModuleDefUser()
			{
				Kind = ModuleKind.Console,
			};
			AssemblyDef zBytzExe = new AssemblyDefUser(assemblyName, new Version(0,0,0,0));
			zBytzExe.Modules.Add();
			*/
			AssemblyDef libpowercryptDll = AssemblyDef.Load("LibPowerCrypt4.dll"); //Load powercrypt
			ModuleDef libpcMod = libpowercryptDll.Modules[0];
			libpcMod.Kind = ModuleKind.Console; //convert to EXE
			//AssemblyDef dnlibDll = AssemblyDef.Load("dnlib.dll");
			//ModuleDef dnlibModule = dnlibDll.Modules[0];
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

			#region TypeRefs
			// Create a TypeRef to System.Console
			TypeRef consoleRef = new TypeRefUser(libpcMod, "System", "Console", libpcMod.CorLibTypes.AssemblyRef);
			// Create a method ref to 'System.Void System.Console::WriteLine(System.String)'
			MemberRef consoleWrite1 = new MemberRefUser(libpcMod, "WriteLine",
						MethodSig.CreateStatic(libpcMod.CorLibTypes.Void, libpcMod.CorLibTypes.String),
						consoleRef);
			
			MemberRef consoleReadLine1 = new MemberRefUser(libpcMod, "ReadLine",
						MethodSig.CreateStatic(libpcMod.CorLibTypes.String),
						consoleRef);
			
			AssemblyRef powerAESLibRef = libpowercryptDll.ToAssemblyRef();
			
			TypeRef powerAESRef = new TypeRefUser(libpcMod, "OmniBean.PowerCrypt4", "PowerAES",
                         powerAESLibRef);
			
			ITypeDefOrRef byteArrayRef = importer.Import(typeof(System.Byte[]));
			
			MemberRef decryptRef = new MemberRefUser(libpcMod, "Decrypt", 
                         MethodSig.CreateStatic(libpcMod.CorLibTypes.String, libpcMod.CorLibTypes.String, libpcMod.CorLibTypes.String)
                        ,powerAESRef);
			
			TypeRef byteConverterRef = new TypeRefUser(libpcMod, "OmniBean.PowerCrypt4.Utilities", "ByteConverter",
                         powerAESLibRef);
						
			MemberRef getBytesRef = new MemberRefUser(libpcMod, "GetBytes", 
                         MethodSig.CreateStatic(byteArrayRef.ToTypeSig(), libpcMod.CorLibTypes.String)
                        ,byteConverterRef);
			
			
			TypeRef fileRef = new TypeRefUser(libpcMod, "System.IO", "File",
                         libpcMod.CorLibTypes.AssemblyRef);
			
			MemberRef writeBytesRef = new MemberRefUser(libpcMod, "WriteAllBytes", 
                        MethodSig.CreateStatic(libpcMod.CorLibTypes.Void, libpcMod.CorLibTypes.String, byteArrayRef.ToTypeSig()),
                        fileRef);
			#endregion
			
			// Add a CIL method body to the entry point method
			CilBody epBody = new CilBody();
			entryPoint.Body = epBody;
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction("NetBytz Encrypted SFX - (c) 2016 0xFireball\nEnter key: "));
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleWrite1));
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(sfxOutputFileName));
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(PowerAES.Encrypt(message, key))); //push encrypted text
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleReadLine1)); //push key from user
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(decryptRef)); //decrypt
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(getBytesRef)); //getbytes
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(writeBytesRef)); //writeAllBytes
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction("Contents Dumped to: "+sfxOutputFileName));
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleWrite1)); //console.writeline()
			epBody.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());//push 0
			epBody.Instructions.Add(OpCodes.Ret.ToInstruction()); //Return/End			
			// Save the assembly to a file on disk
			libpcMod.Write(fileName);
		}
	}
}