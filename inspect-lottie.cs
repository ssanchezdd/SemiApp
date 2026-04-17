using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

var loadContext = new AssemblyLoadContext("inspect", isCollectible: true);
var asm = loadContext.LoadFromAssemblyPath(@"D:\\SemiApp\\pkg\\mpowerkit.lottie\\1.2.0\\lib\\net9.0\\MPowerKit.Lottie.dll");
var type = asm.GetType("MPowerKit.Lottie.LottieView", throwOnError: true)!;
Console.WriteLine("TYPE=" + type.FullName);
foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name))
{
    Console.WriteLine($"PROP {p.Name} : {p.PropertyType.FullName}");
}
foreach (var e in type.GetEvents(BindingFlags.Public | BindingFlags.Instance).OrderBy(e => e.Name))
{
    Console.WriteLine($"EVENT {e.Name} : {e.EventHandlerType?.FullName}");
}
