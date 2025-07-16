using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

namespace BadAppleTrickyTowersMod.Injectors
{
    [HarmonyPatch(typeof(ResourceManager))]
    [HarmonyPatch(MethodType.Normal)]
    [HarmonyPatch("GetRawByName")]
    [HarmonyPatch(new Type[] { typeof(string) })]
    public class ResourceManagerSubstitutionsPatch
    {
        static void Postfix(string resourceName, ref UnityEngine.Object __result)
        {
            switch (resourceName)
            {
                case "MUSIC_BAD_APPLE":
                    UnityEngine.Object replacement = ReplaceMusic("bad_apple.mp3");
                    if (replacement != null)
                    {
                        __result = replacement;
                        Debug.Log("Loaded asset bad_apple.mp3");
                    }
                    else
                    {
                        Debug.Log("Replacement audio is null!");
                    }
                    break;
                default:
                    break;
            }
        }

        private static UnityEngine.Object ReplaceMusic(string clipName)
        {
            AssetBundle bundle = GetAssetBundle();
            if (bundle == null)
            {
                Debug.LogError("Failed to load asset bundle.");
                return null;
            }

            // Load the audio prefab (To have audio included assetbundle it need to be a prefab)
            // https://support.unity.com/hc/en-us/articles/206484773-How-do-I-add-an-AudioClip-to-an-AssetBundle
            GameObject prefab = bundle.LoadAsset<GameObject>("assets/music/badapplesong.prefab");
            if (prefab == null)
            {
                Debug.LogError("Failed to load prefab from asset bundle.");
                return null;
            }

            // Instantiate and extract AudioClip
            GameObject instance = GameObject.Instantiate(prefab);
            AudioSource source = instance.GetComponent<AudioSource>();
            if (source == null || source.clip == null)
            {
                Debug.LogError("Prefab does not contain AudioSource with AudioClip.");
                return null;
            }
            
            // Return AudioClip as UnityEngine.Object
            return source.clip;
        }

        private void LogAllAssetBundleItems()
        {
            AssetBundle bundle = GetAssetBundle();
            if (bundle == null)
            {
                Debug.LogError("Failed to load asset bundle.");
                return;
            }
            foreach (string assetName in bundle.GetAllAssetNames())
            {
                Debug.Log("Asset in bundle: " + assetName);
            }
        }


        private static AssetBundle GetAssetBundle()
        {
            return AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Application.dataPath), "BepInEx/plugins/BadAppleMod/AssetBundle/badapple"));
        }
    }
}
