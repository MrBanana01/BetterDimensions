using System.Collections.Generic;
using UnityEngine;

namespace BetterDimensions.Content {
    public struct BDCommand {
        public List<string> Prefixs;
        public string Command;
        public bool GrabsValue;
    }

    /*
     * $ = manual method
     * @ = event
     * # = awake method
     */

    public class BetterDimensionsManager : MonoBehaviour {
        public List<BDCommand> Commands = new List<BDCommand>() {
            new BDCommand {
                Prefixs = new List<string> { "#", "$" },
                Command = "addtrigger",
                GrabsValue = true,
                //Grabbed value: addtrigger/Trigger ID
            },

            new BDCommand {
                Prefixs = new List<string> { "@" },
                GrabsValue = true
                //Grabbed value: ontrigger/TriggerID/Gameobject With Method
            },

            new BDCommand {
                Prefixs = new List<string> { "#", "$" },
                Command = "setactive",
                GrabsValue = true,
                //Grabbed value: setactive=bool(gameobject name)
            },

            new BDCommand {
                Prefixs = new List<string> { "#", "$" },
                Command = "sethitsound",
                GrabsValue = true,
                //Grabbed value: sethitsound/HitsoundID
            },

            new BDCommand {
                Prefixs = new List < string > { "#", "$" },
                Command = "playaudio",
                GrabsValue = true,
                //Grabbed value: playaudio/HitsoundID
            },

            new BDCommand {
                Prefixs = new List < string > { "#", "$" },
                Command = "starttimer",
                GrabsValue = true,
                //Grabbed value: starttimer/Timer ID/Length in seconds
            },

            new BDCommand {
                Prefixs = new List < string > { "@" },
                Command = "@ontimerend",
                GrabsValue = true,
                //Grabbed value: ontimerend/Timer ID/Gameobject with method
            }
        };
    }
}