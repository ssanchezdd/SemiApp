using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

var runtimeDir = RuntimeEnvironment.GetRuntimeDirectory();
var coreAssemblies = Directory.GetFiles(runtimeDir, "*.dll");
var packageAsm = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages", "mpowerkit.lottie", "1.2.0", "lib", "net9.0", "MPowerKit.Lottie.dll");
var resolver = new PathAssemblyResolver(coreAssemblies.Concat(new[]{ packageAsm }));
using var mlc = new MetadataLoadContext(resolver);
var asm = mlc.LoadFromAssemblyPath(packageAsm);
var type = asm.GetType("MPowerKit.Lottie.LottieView", throwOnError: true);
Console.WriteLine("PROPERTIES");
foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).OrderBy(p => p.Name))
{
    Console.WriteLine($"{p.Name}: {p.PropertyType.FullName}");
}
Console.WriteLine("EVENTS");
foreach (var e in type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).OrderBy(e => e.Name))
{
    Console.WriteLine($"{e.Name}: {e.EventHandlerType?.FullName}");
}
