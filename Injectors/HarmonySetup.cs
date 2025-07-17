using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace TrickyMultiplayerPlus
{

    [BepInPlugin("badapple", "BadApple", "1.0")]
	public class TrickyTowersModPlugin : BaseUnityPlugin
	{
        public static ConfigEntry<string> BricksetConfig;
        public static string[] Brickset => BricksetConfig.Value.Split(',').Select(s => s.Trim()).ToArray();

        void Awake()
		{
			var harmony = new Harmony("badapple");
			harmony.PatchAll();

            BricksetConfig = Config.Bind(
                "General",
                "Brickset",
                "BRICK_O",
                "Comma-separated list of brick IDs to use (e.g., BRICK_T,BRICK_L,BRICK_J,BRICK_O,BRICK_I,BRICK_S,BRICK_Z)"
            );
            Debug.Log($"Loaded brickset: {string.Join(", ", Brickset)}");
        }
	}
}