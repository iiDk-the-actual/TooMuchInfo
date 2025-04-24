using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TooMuchInfo.Patches
{
    [HarmonyPatch(typeof(VRRig), "UpdateName", new Type[] { typeof(bool) })]
    public class NamePatch
    {
        public static void Postfix(VRRig __instance, bool isNamePermissionEnabled)
        {
            if (__instance != GorillaTagger.Instance.offlineVRRig)
                Plugin.UpdateName(__instance);
        }
    }
}
