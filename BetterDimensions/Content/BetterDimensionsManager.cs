using System.Collections.Generic;
using UnityEngine;

namespace BetterDimensions.Content {
    public struct BDCommand {
        public List<string> Prefixes;
        public string Command;
        public bool GrabsValue;
    }

    /*
     * $ = manual method
     * @ = event
     * # = awake method
     * * = variable
     */

    public class BetterDimensionsManager : MonoBehaviour {
        public List<BDCommand> Commands = new List<BDCommand>() {
            new BDCommand {
                Prefixes = new List <string> { "*" },
                Command = "var",
                GrabsValue = true,
                //Grabbed value: var/data
            },

            new BDCommand {
                Prefixes = new List <string> { "$", "#" },
                Command = "if",
                GrabsValue = true,
                //Grabbed value: if/Gameobject with variable/expectedValue/Gameobject with method if true/Gameobject with method if false
            },

            new BDCommand {
                Prefixes = new List <string> { "$", "#" },
                Command = "changevar",
                GrabsValue = true,
                //Grabbed value: changevar/Gameobject with variable
            },

            new BDCommand {
                Prefixes = new List<string> { "#", "$" },
                Command = "addtrigger",
                GrabsValue = true,
                //Grabbed value: addtrigger/Trigger ID
            },

            new BDCommand {
                Prefixes = new List<string> { "@" },
                Command = "ontrigger",
                GrabsValue = true
                //Grabbed value: ontrigger/TriggerID/Gameobject With Method
            },

            new BDCommand {
                Prefixes = new List<string> { "#", "$" },
                Command = "setactive",
                GrabsValue = true,
                //Grabbed value: setactive=bool(gameobject name)
            },

            new BDCommand {
                Prefixes = new List<string> { "#", "$" },
                Command = "sethitsound",
                GrabsValue = true,
                //Grabbed value: sethitsound/HitsoundID
            },

            new BDCommand {
                Prefixes = new List <string> { "#", "$" },
                Command = "playaudio",
                GrabsValue = true,
                //Grabbed value: playaudio/Gameobject with audiosource
            },

            new BDCommand {
                Prefixes = new List <string> { "#", "$" },
                Command = "starttimer",
                GrabsValue = true,
                //Grabbed value: starttimer/Timer ID/Length in seconds
            },

            new BDCommand {
                Prefixes = new List <string> { "@" },
                Command = "ontimerend",
                GrabsValue = true,
                //Grabbed value: ontimerend/Timer ID/Gameobject with method
            },
        };
    }
}