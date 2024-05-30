using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterDimensions.Content {
    public class BDCommand {
        public List<char>? Prefixes;
        public string? Command;
        public string? CommandBuild;
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
        public static BetterDimensionsManager? instance;

        void Awake() {
            if (instance is null)
                instance = this;
            else
                Destroy(this);

            ApplyCommands();
            WaitForEvent();
        }

        void WaitForEvent() =>
            BDEvent += (obj, ID) => {

            };

        [HideInInspector]
        public List<BDCommand> Commands = new List<BDCommand>() {
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

        [HideInInspector]
        public Action<GameObject, int>? BDEvent;

        void ApplyCommands() {

        }

        public void RunCommand(GameObject obj, BDCommand command, object[]? data = null) {

        }

        public void RunEvent(GameObject obj, int EventID) =>
            BDEvent?.Invoke(obj, EventID);
    }
}