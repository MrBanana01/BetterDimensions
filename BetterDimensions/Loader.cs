using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using BetterDimensions.Content;

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
                Debug.Log("Loading BetterDimensions content...");
                gameObject.AddComponent<BetterDimensionsManager>();

                if(gameObject.GetComponent<BetterDimensionsManager>() == null)
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