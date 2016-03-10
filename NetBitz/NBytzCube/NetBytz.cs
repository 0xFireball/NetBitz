/*
 */
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using OmniBean.PowerCrypt4.Utilities;
using OmniBean.PowerCrypt4;

namespace NBytzCube
{
    public class NBCube
    {
        static List<Assembly> loadedAssemblies;
        public static void LaunchAssembly(byte[] assemblyContents)
        {
            if (assemblyContents.Length == 0)
            {
                Console.WriteLine("Decryption Failure.");
                return;
            }
            Assembly asm = Assembly.Load(assemblyContents);
            MethodInfo main = asm.EntryPoint;
            var defaultParameters = main.GetParameters().Select(p => GetDefaultValue(p.ParameterType)).ToArray();
            main.Invoke(null, defaultParameters);
        }
        public static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        public static void ExtractAndLaunchAssembly(string compressedAssemblyContents)
        {
            byte[] assemblyContents = StringCompressor.DecompressString(compressedAssemblyContents).GetBytes();
            Assembly asm = Assembly.Load(assemblyContents);
            MethodInfo main = asm.EntryPoint;
            var defaultParameters = main.GetParameters().Select(p => GetDefaultValue(p.ParameterType)).ToArray();
            main.Invoke(null, defaultParameters);
        }
        public static void ExtractUnpackAndLaunchAssembly(string compressedAssemblyContents)
        {
            loadedAssemblies = new List<Assembly>();
#if DEBUG
            Console.WriteLine("Decompressing...");
#endif
            string decompressedAssemblyContents = StringCompressor.DecompressString(compressedAssemblyContents);
            GC.Collect();
            GC.WaitForPendingFinalizers(); //Clean up the massive memory usage
            string[] squashedAssemblies = decompressedAssemblyContents.Split('_').Reverse().ToArray();
            //AppDomain packerDomain = AppDomain.CreateDomain("NBPackerLaunch");
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += PackerDomain_ReflectionOnlyAssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
#if DEBUG
            Console.WriteLine("Unpacking...");
#endif
            foreach (var sqAsm in squashedAssemblies.Take(squashedAssemblies.Length - 2)) //All except the last
            {
                loadedAssemblies.Add(Assembly.ReflectionOnlyLoad(Convert.FromBase64String(sqAsm)));
            }
#if DEBUG
            Console.WriteLine("Initializing...");
#endif
            var executableAsm = squashedAssemblies[squashedAssemblies.Length - 1];
            Assembly asm = Assembly.Load(Convert.FromBase64String(executableAsm));
            /*
            foreach (Assembly asm in packerDomain.GetAssemblies().Reverse()) //Find the EXE and run it!
            {
                if (asm.EntryPoint != null)
                {
                    MethodInfo main = asm.EntryPoint;
                    var defaultParameters = main.GetParameters().Select(p => GetDefaultValue(p.ParameterType)).ToArray();
                    main.Invoke(null, defaultParameters);
                }
            }
            
            Assembly asm = Assembly.Load(Convert.FromBase64String(executableAsm)); //The last one, the executable Assembly
            //currentDomain.Load(Convert.FromBase64String(executableAsm));
            */
            MethodInfo main = asm.EntryPoint;
            var defaultParameters = main.GetParameters().Select(p => GetDefaultValue(p.ParameterType)).ToArray();
            main.Invoke(null, defaultParameters);

        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return loadedAssemblies.Where(asm => asm.FullName == args.RequestingAssembly.FullName).ElementAt(0); //load the assembly from the list
        }

        private static Assembly PackerDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return System.Reflection.Assembly.ReflectionOnlyLoad(args.Name);
        }
    }
}