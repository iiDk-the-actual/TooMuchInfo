using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TooMuchInfo.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("SerializeReadShared", MethodType.Normal)]
    internal class OnDataReceived
    {
        private static void Postfix(VRRig __instance)
        {
            __instance.UpdateName();
        }
    }
}
