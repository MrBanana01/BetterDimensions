using BetterDimensionsEditorTools;
using Monke_Dimensions.API;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterDimensions.Content {
    public class BDCommand {
        public string Command = "";
        public List<char> GrabbedValueSpots = new List<char>();
    }

    public enum CommandType {
        Method,
        Event,
        Variable
    }

    public class BetterDimensionsManager : MonoBehaviour {
        public static BetterDimensionsManager? instance;
        public GameObject MapObjectsParent;

        void Awake() {
            if (instance is null)
                instance = this;
            else
                Destroy(this);

            MapObjectsParent = GameObject.Find("LoadedDimension");

            DimensionEvents.OnDimensionEnter += DimensionEntered;
            BDEvent += EventCalled;
        }

        void DimensionEntered(string DimensionName) {
            foreach (Transform child in MapObjectsParent.transform)
                AllObjects.Add(child.gameObject);

            ApplyCommands();
        }

        void EventCalled(GameObject obj, int ID) {

        }

        public List<BDCommand> Commands = new List<BDCommand>() {
            new BDCommand {
                Command = "debuglog/%",
                GrabbedValueSpots = new List<char> { '%'  }
                //Grabbed value: debuglog/message
            },

            new BDCommand {
                Command = "debuglogwarning/%",
                GrabbedValueSpots = new List<char> { '%'  }
                //Grabbed value: debuglogwarning/message
            },

            new BDCommand {
                Command = "debuglogerror/%",
                GrabbedValueSpots = new List<char> { '%'  }
                //Grabbed value: debuglogerror/message
            },

            new BDCommand {
                Command = "runmethod/%",
                GrabbedValueSpots = new List<char> { '%'  }
                //Grabbed value: runmethod/Gameobject with method
            },

            new BDCommand {
                Command = "if/%/*/&/^",
                GrabbedValueSpots = new List<char> { '%', '*', '&', '^'  }
                //Grabbed value: if/Gameobject with variable/expectedValue/EventID if true/EventID if false
            },

            new BDCommand {
                Command = "ifnot/%/*/&/^",
                GrabbedValueSpots = new List<char> { '%', '*', '&', '^'  }
                //Grabbed value: ifnot/Gameobject with variable/expectedValue/EventID if true/EventID if false
            },

            new BDCommand {
                Command = "changevar/%/*",
                GrabbedValueSpots = new List<char> { '%', '*' }
                //Grabbed value: changevar/Gameobject with variable/variable data to change to
            },

            new BDCommand {
                Command = "addtrigger/%/^",
                GrabbedValueSpots = new List<char> { '%', '^' }
                //Grabbed value: addtrigger/EventID/Trigger Type (left, right, body, both, all)
            },

            new BDCommand {
                Command = "eventcalled/%/^",
                GrabbedValueSpots = new List<char> { '%', '^' }
                //Grabbed value: eventcalled/EventID/Gameobject With Method
            },

            new BDCommand {
                Command = "setactive/*/%",
                GrabbedValueSpots = new List<char> { '*', '%' }
                //Grabbed value: setactive/bool/gameobject name
            },

            new BDCommand {
                Command = "sethitsound/%",
                GrabbedValueSpots = new List<char> { '%' }
                //Grabbed value: sethitsound/HitsoundID
            },

            new BDCommand {
                Command = "playaudio/%",
                GrabbedValueSpots = new List<char> { '%' }
                //Grabbed value: playaudio/Gameobject with audiosource
            },

            new BDCommand {
                Command = "starttimer/*/%",
                GrabbedValueSpots = new List<char> { '*', '%' }
                //Grabbed value: starttimer/EventID/Length in seconds
            },
        };

        public Action<GameObject, int>? BDEvent;

        List<GameObject> AllObjects = new List<GameObject>();

        void ApplyCommands() {
            foreach(GameObject obj in AllObjects) {
                if (!ObjectContainsCommands(obj))
                    continue;

                bool HasEventCMD = obj.GetComponent<BDEvent>() != null;
                bool HasMethodCMD = obj.GetComponent<BDMethod>() != null;
                bool HasVariableCMD = obj.GetComponent<BDVariable>() != null;

                int trueCount = (HasEventCMD ? 1 : 0) + (HasMethodCMD ? 1 : 0) + (HasVariableCMD ? 1 : 0);

                if (trueCount != 1) {
                    Debug.LogError($"Object {obj.name} can only have one BD component.");
                    Destroy(obj);
                    continue;
                }
                else {
                    if (obj.GetComponents<BDEvent>().Length > 1) {
                        Debug.LogError($"Object {obj.name} has more than one BDEvent component.");
                        Destroy(obj);
                        continue;
                    }
                    if (obj.GetComponents<BDMethod>().Length > 1) {
                        Debug.LogError($"Object {obj.name} has more than one BDMethod component.");
                        Destroy(obj);
                        continue;
                    }
                    if (obj.GetComponents<BDVariable>().Length > 1) {
                        Debug.LogError($"Object {obj.name} has more than one BDVariable component.");
                        Destroy(obj);
                        continue;
                    }
                }

                if (!HasMethodCMD)
                    return;

                BDMethod method = obj.GetComponent<BDMethod>();

                if (method.MethodType is MethodType.Awake)
                    RunCommands(method);
            }
        }

        public void RunCommands(BDMethod method) {
            string[] MethodCommands = method.Commands.Split('|');

            foreach(string cmd in MethodCommands) {
                string[] Commandparts = cmd.Split('/');
                foreach(BDCommand command in Commands) {
                    string[] CheckParts = command.Command.Split('/');

                    if (CheckParts[0] is "" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"\" but");
                            break;
                        }

                        GameObject obj = DimensionTools.FindObjectInDimension(Commandparts[1]);

                        if (obj is null || obj.GetComponent<BDMethod>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"\" but");
                            break;
                        }
                        break;
                    }

                    if (CheckParts[0] is "debuglog" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"debuglog\" but put no message to log");
                            break;
                        }

                        Debug.Log(Commandparts[1]);

                        break;
                    }

                    if (CheckParts[0] is "debuglogwarning" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"debuglogwarning\" but put no message to log");
                            break;
                        }

                        Debug.LogWarning(Commandparts[1]);

                        break;
                    }

                    if (CheckParts[0] is "debuglogerror" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"debuglogerror\" but put no message to log");
                            break;
                        }

                        Debug.LogError(Commandparts[1]);

                        break;
                    }

                    if (CheckParts[0] is "runmethod" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"runmethod\" but put no GameObject method to run");
                            break;
                        }

                        GameObject obj = DimensionTools.FindObjectInDimension(Commandparts[1]);

                        if(obj is null || obj.GetComponent<BDMethod>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"runmethod\" but put an invalid GameObject method to run");
                            break;
                        }

                        RunCommands(obj.GetComponent<BDMethod>());
                        break;
                    }

                    if (CheckParts[0] is "if" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2]) || string.IsNullOrWhiteSpace(Commandparts[3])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"if\" some values were empty");
                            break;
                        }

                        GameObject obj = DimensionTools.FindObjectInDimension(Commandparts[1]);

                        if (obj is null || obj.GetComponent<BDVariable>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"runmethod\" but put an invalid GameObject variable to check");
                            break;
                        }

                        if (Commandparts[1] == Commandparts[2])
                            RunEvent(obj, int.Parse(Commandparts[3]));
                        else
                            RunEvent(obj, int.Parse(Commandparts[4]));

                        break;
                    }

                    if (CheckParts[0] is "ifnot" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2]) || string.IsNullOrWhiteSpace(Commandparts[3])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"if\" some values were empty");
                            break;
                        }

                        GameObject obj = DimensionTools.FindObjectInDimension(Commandparts[1]);

                        if (obj is null || obj.GetComponent<BDVariable>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"runmethod\" but put an invalid GameObject variable to check");
                            break;
                        }

                        if (Commandparts[1] != Commandparts[2])
                            RunEvent(obj, int.Parse(Commandparts[3]));
                        else
                            RunEvent(obj, int.Parse(Commandparts[4]));

                        break;
                    }

                    if (CheckParts[0] is "changevar" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"changevar\" but put no GameObject variable to change");
                            break;
                        }

                        GameObject obj = DimensionTools.FindObjectInDimension(Commandparts[1]);

                        if (obj is null || obj.GetComponent<BDVariable>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"runmethod\" but put an invalid GameObject variable to change");
                            break;
                        }

                        obj.GetComponent<BDVariable>().VariableBuild.VariableData = Commandparts[2];
                        break;
                    }
                }
            }
        }

        public void RunEvent(GameObject obj, int EventID) =>
            BDEvent?.Invoke(obj, EventID);

        public bool ObjectContainsCommands(GameObject obj) {
            if(obj.GetComponent<BDEvent>() != null || obj.GetComponent<BDMethod>() != null || obj.GetComponent<BDVariable>() != null)
                return true;

            return false;
        }
    }
}