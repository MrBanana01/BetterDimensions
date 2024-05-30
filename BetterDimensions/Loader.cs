using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using BetterDimensions.Content;
using System.IO;
using System;

namespace BetterDimensions {

    [BepInPlugin(GUID, NAME, VERSION)]
    public class Loader : BaseUnityPlugin {
        const string
            GUID = "MrBanana.GorillaTag.BetterDimensions",
            NAME = "BetterDimensions",
            VERSION = "1.0.0";

        #pragma warning disable IDE0051

        void Start() {
            Patch();

            Utilla.Events.GameInitialized += (sender, args) => {
                string[] typeNames = { "BDEvent", "BDMethod", "BDVariable" };

                string pluginPath = $"{Paths.PluginPath}\\BetterDimensions\\BetterDimensionEditor.dll";

                byte[] dllBytes = System.IO.File.ReadAllBytes(pluginPath);
                Assembly loadedAssembly = Assembly.Load(dllBytes);

                Debug.Log("Loading BetterDimensions content...");
                gameObject.AddComponent<BetterDimensionsManager>();

                if(gameObject.GetComponent<BetterDimensionsManager>() is null || loadedAssembly is null)
                    Debug.LogError("BetterDimensions failed to load for unknown reasons");
                else
                    Debug.Log("Successfully loaded BetterDimensions");
            };
        }

        void Patch() {
            instance = new Harmony(GUID);
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }

        static Harmony? instance;
    }
}