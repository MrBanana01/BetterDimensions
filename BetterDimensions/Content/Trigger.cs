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
        public BDCommand CommandRef;

        public TriggerType Type;

        void OnTriggerEnter(Collider other) {
            switch (Type) {
                case TriggerType.RightHand:
                    if(other.name is "")
                        BetterDimensionsManager.RunCommand(gameObject, CommandRef);
                    break;
                case TriggerType.LeftHand:
                    if (other.name is "")
                        BetterDimensionsManager.RunCommand(gameObject, CommandRef);
                    break;
                case TriggerType.BothHands:
                    if (other.name is "")
                        BetterDimensionsManager.RunCommand(gameObject, CommandRef);
                    break;
                case TriggerType.Body:
                    if (other.name is "")
                        BetterDimensionsManager.RunCommand(gameObject, CommandRef);
                    break;
                case TriggerType.All:
                    if (other.name is "")
                        BetterDimensionsManager.RunCommand(gameObject, CommandRef);
                    break;
            }
        }
    }
}