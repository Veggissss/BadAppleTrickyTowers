using BepInEx;
using HarmonyLib;

namespace TrickyMultiplayerPlus
{

    [BepInPlugin("badapple", "BadApple", "1.0")]
	public class TrickyTowersModPlugin : BaseUnityPlugin
	{
		void Awake()
		{
			var harmony = new Harmony("badapple");
			harmony.PatchAll();
		}
	}
}