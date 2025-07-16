using HarmonyLib;

namespace BadAppleTrickyTowersMod.Injectors
{
    [HarmonyPatch(typeof(LanguageManager))]
	[HarmonyPatch("LoadLanguage")]
	class RegisterNewStringsPatch
	{
		static void Postfix(LanguageManager __instance)
		{
            __instance.RegisterLanguage("STATE_TITLE_WORLD_NAME_BAD_APPLE", "Bad Apple", false, true);
            return;
		}
	}
}
