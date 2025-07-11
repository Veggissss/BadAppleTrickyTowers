using HarmonyLib;
using System;

namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(InitResourcesCommand))]
	[HarmonyPatch("Execute")]
	class RegisterNewResourcesPatch
	{
		static bool Prefix(InitResourcesCommand __instance)
		{
			// Images will later be replaced.
			//Singleton<ResourceManager>.instance.RegisterResource("MODE_SELECT_MODE_ITEM_HEROIC",  "UI/State/ModeSelect/ModeItem01", false); // Needed?

            return true;
		}
	}
}
