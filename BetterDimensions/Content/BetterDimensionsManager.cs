using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterDimensions.Content {
    public struct BDCommand {
        public List<char> Prefixes;
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
        public static List<BDCommand> Commands = new List<BDCommand>() {
            new BDCommand {
                Prefixes = new List <char> { '*' },
                Command = "var",
                CommandBuild = "{0}/*",
                GrabsValue = true,
                //Grabbed value: var/data
            },

            new BDCommand {
                Prefixes = new List <char> { '$', '#' },
                Command = "if",
                CommandBuild = "{0}/%/*/%/%",
                GrabsValue = true,
                //Grabbed value: if/Gameobject with variable/expectedValue/Gameobject with method if true/Gameobject with method if false
            },

            new BDCommand {
                Prefixes = new List <char> { '$', '#' },
                Command = "changevar",
                CommandBuild = "{0}/%",
                GrabsValue = true,
                //Grabbed value: changevar/Gameobject with variable
            },

            new BDCommand {
                Prefixes = new List<char> { '#', '$' },
                Command = "addtrigger",
                CommandBuild = "{0}/*/%",
                GrabsValue = true,
                //Grabbed value: addtrigger/EventID/Trigger Type (left, right, body, both, all)
            },

            new BDCommand {
                Prefixes = new List<char> { '@' },
                Command = "eventcalled",
                CommandBuild = "{0}/*/%",
                GrabsValue = true
                //Grabbed value: eventcalled/EventID/Gameobject With Method
            },

            new BDCommand {
                Prefixes = new List<char> { '#', '$' },
                Command = "setactive",
                CommandBuild = "{0}/*/%",
                GrabsValue = true,
                //Grabbed value: setactive/bool/gameobject name
            },

            new BDCommand {
                Prefixes = new List<char> { '#', '$' },
                Command = "sethitsound",
                CommandBuild = "{0}/*",
                GrabsValue = true,
                //Grabbed value: sethitsound/HitsoundID
            },

            new BDCommand {
                Prefixes = new List <char> { '#', '$' },
                Command = "playaudio",
                CommandBuild = "{0}/%",
                GrabsValue = true,
                //Grabbed value: playaudio/Gameobject with audiosource
            },

            new BDCommand {
                Prefixes = new List <char> { '#', '$' },
                Command = "starttimer",
                CommandBuild = "{0}/*/*",
                GrabsValue = true,
                //Grabbed value: starttimer/EventID/Length in seconds
            },
        };

        public static Action<int>? BDEvent;

        //This is temp until I get the reference from the mod
        public List<GameObject> AllObjects = new List<GameObject>();

        public void CheckForCommands() {
            foreach (GameObject obj in AllObjects) {

                var matchedCommand = GetMatchedCommand(obj.name);

                if (matchedCommand is null)
                    continue;

                char UsedPrefix = '?';

                foreach(var prefix in matchedCommand.Value.Prefixes) {
                    if (obj.name.Contains(prefix)) {
                        UsedPrefix = prefix;
                        break;
                    }
                }

                if (UsedPrefix is '?') {
                    Debug.LogError($"Command detected on object \"{obj.name}\" but the wrong prefix was used. Skipping object...");
                    continue;
                }

                switch (matchedCommand.Value.Command) {
                    case "addtrigger":
                        if (UsedPrefix is '#')
                            RunCommand(obj, matchedCommand.Value);
                        break;
                }
            }
        }

        public static void RunCommand(GameObject obj, BDCommand command) {
            switch (command.Command) {
                case "addtrigger":
                    if(obj.GetComponent<Trigger>() != null) {
                        Debug.LogWarning($"The object \"{obj.name}\" already has a trigger while a trigger was attempting to be added");
                        break;
                    }

                    Trigger comp = obj.AddComponent<Trigger>();
                    comp.CommandRef = command;
                    string[] parts = obj.name.Split('/');

                    #pragma warning disable IDE0059

                    if (int.TryParse(parts[1], out int value)) {
                        Debug.LogError($"The trigger on the object \"{obj.name}\" was being created with an event ID that is not a number. Please make it an int/number");
                        break;
                    }

                    comp.ID = int.Parse(parts[1]);

                    switch (parts[2]) {
                        case "left":
                            comp.Type = TriggerType.LeftHand;
                            break;
                        case "right":
                            comp.Type = TriggerType.RightHand;
                            break;
                        case "both":
                            comp.Type = TriggerType.BothHands;
                            break;
                        case "body":
                            comp.Type = TriggerType.Body;
                            break;
                        case "all":
                            comp.Type = TriggerType.All;
                            break;
                    }
                    break;
            }
        }

        public static void RunEvent(int EventID) =>
            BDEvent?.Invoke(EventID);

        BDCommand? GetMatchedCommand(string objName) {
            foreach (var command in Commands)
                foreach (var prefix in command.Prefixes)
                    if (objName.Contains($"{prefix}{command.Command}"))
                        return command;

            return null;
        }
    }
}