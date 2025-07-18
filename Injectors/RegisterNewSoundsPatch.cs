﻿using HarmonyLib;

namespace TrickyMultiplayerPlus
{
    [HarmonyPatch(typeof(InitAudioResourcesCommand))]
	[HarmonyPatch("Execute")]
	class RegisterNewSoundsPatch
	{
		static bool Prefix(InitResourcesCommand __instance)
		{
			Singleton<ResourceManager>.instance.RegisterResource("MUSIC_BAD_APPLE", "Audio/Music/000_Track_01_Race_stinger", false);
			return true;
		}
	}
}
