using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using HarmonyLib;

// ReSharper disable once CheckNamespace
public static class MarseyEntry
{
    public static void Entry()
    {
        Harmony.DEBUG = true;
        MarseyLogger.Info("Entry for patching started.");
        if (!TryGetAssembly("Content.Client"))
            return;

        var subversionAssembly = Assembly.GetExecutingAssembly();
        SubverterPatch.Harm.PatchAll(subversionAssembly);
    }

    private static bool TryGetAssembly(string assembly)
    {
        for (var loops = 0; loops < 50; loops++)
        {
            if (FindAssembly(assembly) != null)
                return true;

            Thread.Sleep(200);
        }
        return false;
    }
    private static Assembly? FindAssembly(string assemblyName)
    {
        var asmList = AppDomain.CurrentDomain.GetAssemblies();
        return asmList.FirstOrDefault(asm => asm.FullName?.Contains(assemblyName) == true);
    }
}