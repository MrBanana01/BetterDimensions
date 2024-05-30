using UnityEngine;

namespace BetterDimensions.Content.EditorTools {
    public enum MethodType {
        Manual,
        Awake
    }

    public class Method : MonoBehaviour {
        [Tooltip("A manual method must be ran by another command while an Awake method runs when the map loads")]
        public MethodType type;
    }
}