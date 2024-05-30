using UnityEngine;

namespace BetterDimensions.Content {
    public enum TriggerType {
        RightHand,
        LeftHand,
        BothHands,
        Body,
        All
    }

    internal class Trigger : MonoBehaviour {
        public TriggerType Type;

        public int ID;

        #pragma warning disable IDE0051

        void OnTriggerEnter(Collider other) {
            switch (Type) {
                case TriggerType.RightHand:
                    if(other.name is "")
                        BetterDimensionsManager.instance.RunEvent(gameObject, ID);
                    break;
                case TriggerType.LeftHand:
                    if (other.name is "")
                        BetterDimensionsManager.instance.RunEvent(gameObject, ID);
                    break;
                case TriggerType.BothHands:
                    if (other.name is "")
                        BetterDimensionsManager.instance.RunEvent(gameObject, ID);
                    break;
                case TriggerType.Body:
                    if (other.name is "")
                        BetterDimensionsManager.instance.RunEvent(gameObject, ID);
                    break;
                case TriggerType.All:
                    if (other.name is "")
                        BetterDimensionsManager.instance.RunEvent(gameObject, ID);
                    break;
            }
        }
    }
}