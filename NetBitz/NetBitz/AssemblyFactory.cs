
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
			ModuleDef mod = new ModuleDefUser(fileName)
			{
				Kind = ModuleKind.Console
			};
			ModuleDefMD libpcMod = ModuleDefMD.Load("LibPowerCrypt4.dll");
			AssemblyDef asm = new AssemblyDefUser(assemblyName, new Version(1, 2, 3, 4));
			asm.Modules.Add(mod);
			
			
			/*
			// Add a .NET resource
			byte[] resourceData = Encoding.UTF8.GetBytes("Hello, world!");
			string nbDataName = namespaceName+".nbdata";
			mod.Resources.Add(new EmbeddedResource(nbDataName, resourceData,
							ManifestResourceAttributes.Private));
			*/

			// Add the startup type. It derives from System.Object.
			TypeDef startUpType = new TypeDefUser(namespaceName, "Startup", mod.CorLibTypes.Object.TypeDefOrRef);
			startUpType.Attributes = TypeAttributes.NotPublic | TypeAttributes.AutoLayout |
									TypeAttributes.Class | TypeAttributes.AnsiClass;
			// Add the type to the module
			mod.Types.Add(startUpType);

			// Create the entry point method
			MethodDef entryPoint = new MethodDefUser("Main",
				MethodSig.CreateStatic(mod.CorLibTypes.Int32, new SZArraySig(mod.CorLibTypes.String)));
			entryPoint.Attributes = MethodAttributes.Private | MethodAttributes.Static |
							MethodAttributes.HideBySig | MethodAttributes.ReuseSlot;
			entryPoint.ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed;
			// Name the 1st argument (argument 0 is the return type)
			entryPoint.ParamDefs.Add(new ParamDefUser("args", 1));
			// Add the method to the startup type
			startUpType.Methods.Add(entryPoint);
			// Set module entry point
			mod.EntryPoint = entryPoint;

			// Create a TypeRef to System.Console
			TypeRef consoleRef = new TypeRefUser(mod, "System", "Console", mod.CorLibTypes.AssemblyRef);
			// Create a method ref to 'System.Void System.Console::WriteLine(System.String)'
			MemberRef consoleWrite1 = new MemberRefUser(mod, "WriteLine",
						MethodSig.CreateStatic(mod.CorLibTypes.Void, mod.CorLibTypes.String),
						consoleRef);

			// Add a CIL method body to the entry point method
			CilBody epBody = new CilBody();
			entryPoint.Body = epBody;
			epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(PowerAES.Encrypt(message, key)));
			epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleWrite1));
			epBody.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());
			epBody.Instructions.Add(OpCodes.Ret.ToInstruction());

			// Save the assembly to a file on disk
			mod.Write(fileName);
		}
	}
}