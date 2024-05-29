using System.Collections.Generic;
using UnityEngine;

namespace BetterDimensions.Content {
    public struct BDCommand {
        public List<string> Prefixes;
        public string Command;
        public string CommandBuild;
        public bool GrabsValue;
    }

    /*
     * $ = manual method
     * @ = event
     * # = awake method
     * * = variable
     * % = Custom Data
     */

    public class BetterDimensionsManager : MonoBehaviour {
        /// <summary>
        /// All commands that can be used
        /// </summary>
        public static List<BDCommand> Commands = new List<BDCommand>() {
            new BDCommand {
                Prefixes = new List <string> { "*" },
                Command = "var",
                CommandBuild = "{0}/*",
                GrabsValue = true,
                //Grabbed value: var/data
            },

            new BDCommand {
                Prefixes = new List <string> { "$", "#" },
                Command = "if",
                CommandBuild = "{0}/%/*/%/%",
                GrabsValue = true,
                //Grabbed value: if/Gameobject with variable/expectedValue/Gameobject with method if true/Gameobject with method if false
            },

            new BDCommand {
                Prefixes = new List <string> { "$", "#" },
                Command = "changevar",
                CommandBuild = "{0}/%",
                GrabsValue = true,
                //Grabbed value: changevar/Gameobject with variable
            },

            new BDCommand {
                Prefixes = new List<string> { "#", "$" },
                Command = "addtrigger",
                CommandBuild = "{0}/*/%",
                GrabsValue = true,
                //Grabbed value: addtrigger/Trigger ID/Trigger Type (left, right, body, both, all)
            },

            new BDCommand {
                Prefixes = new List<string> { "@" },
                Command = "ontrigger",
                CommandBuild = "{0}/*/%",
                GrabsValue = true
                //Grabbed value: ontrigger/TriggerID/Gameobject With Method
            },

            new BDCommand {
                Prefixes = new List<string> { "#", "$" },
                Command = "setactive",
                CommandBuild = "{0}/*/%",
                GrabsValue = true,
                //Grabbed value: setactive/bool/gameobject name
            },

            new BDCommand {
                Prefixes = new List<string> { "#", "$" },
                Command = "sethitsound",
                CommandBuild = "{0}/*",
                GrabsValue = true,
                //Grabbed value: sethitsound/HitsoundID
            },

            new BDCommand {
                Prefixes = new List <string> { "#", "$" },
                Command = "playaudio",
                CommandBuild = "{0}/%",
                GrabsValue = true,
                //Grabbed value: playaudio/Gameobject with audiosource
            },

            new BDCommand {
                Prefixes = new List <string> { "#", "$" },
                Command = "starttimer",
                CommandBuild = "{0}/*/*",
                GrabsValue = true,
                //Grabbed value: starttimer/Timer ID/Length in seconds
            },

            new BDCommand {
                Prefixes = new List <string> { "@" },
                Command = "ontimerend",
                CommandBuild = "{0}/*/%",
                GrabsValue = true,
                //Grabbed value: ontimerend/Timer ID/Gameobject with method
            },
        };

        //This is temp until I get the reference from the mod
        public List<GameObject> AllObjects = new List<GameObject>();

        /// <summary>
        /// Checks every gameobject for commands and applies them
        /// </summary>
        public void CheckForCommands() {
            foreach (GameObject obj in AllObjects) {

                var matchedCommand = GetMatchedCommand(obj.name);

                if (matchedCommand == null)
                    return;

                switch ((obj.name, matchedCommand)) {
                    case (string ObjName, BDCommand command):

                        break;
                }
            }
        }

        public void RunCommand(GameObject objWithCommand, BDCommand command) {

        }

        BDCommand? GetMatchedCommand(string objName) {
            foreach (var command in Commands) {
                if (objName.Contains(command.Command)) {
                    return command;
                }
            }

            return null;
        }
    }
}