using UnityEngine;

namespace BetterDimensions.Content {
    public enum TriggerType {
        RightHand,
        LeftHand,
        BothHands
    }

    internal class Trigger : MonoBehaviour {
        public TriggerType Type;

        public int ID;

        #pragma warning disable IDE0051

        void Awake() =>
            gameObject.layer = 18;

        void OnTriggerEnter(Collider other) {
            switch (Type) {
                case TriggerType.RightHand:
                    if(other.name is "")
                        BetterDimensionsManager.instance?.RunEvent(gameObject, ID);
                    break;
                case TriggerType.LeftHand:
                    if (other.name is "")
                        BetterDimensionsManager.instance?.RunEvent(gameObject, ID);
                    break;
                case TriggerType.BothHands:
                    if (other.name is "")
                        BetterDimensionsManager.instance?.RunEvent(gameObject, ID);
                    break;
            }
        }
    }
}