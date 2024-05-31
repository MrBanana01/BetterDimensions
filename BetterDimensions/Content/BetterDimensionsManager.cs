using BetterDimensionsEditorTools;
using Monke_Dimensions.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
            AllObjects = GetAllChildren(MapObjectsParent.transform);

            ApplyCommands();
        }

        void EventCalled(GameObject obj, int ID) {
            Debug.Log("Event gotten");
            Debug.Log(AllObjects);
            Debug.Log(AllObjects.Count);
            foreach (GameObject BDobj in AllObjects) {
                Debug.Log("looped gameobject");
                if (BDobj.GetComponent<BDEvent>() != null) {
                    Debug.Log("object has BDEvent");
                    BDEvent Event = BDobj.GetComponent<BDEvent>();
                    Debug.Log(Event);
                    if (Event.EventID == ID) {
                        Debug.Log("EventID is correct");
                        string[] MethodCommands = Event.ObjectCommands.Split('|');
                        foreach (string cmd in MethodCommands) {
                            Debug.Log("Cycle through method commands");
                            GameObject objInMap = DimensionTools.FindObjectInDimension(cmd);
                            Debug.Log("found object");
                            if (objInMap.GetComponent<BDMethod>() is null) {
                                Debug.LogError($"Object {obj.name} recived an event and tried to execute a method but the method was not found");
                                break;
                            }

                            Debug.Log("has method cmd");

                            if (!Event.EventRan) {
                                Debug.Log("run cmd");
                                RunCommands(objInMap.GetComponent<BDMethod>());
                                if (Event.type is BetterDimensionsEditorTools.EventType.OneTrigger)
                                    Event.EventRan = true;
                            }
                        }
                    }
                }
            }
        }

        public List<GameObject> GetAllChildren(Transform parent) {
            List<GameObject> children = new List<GameObject>();
            GetChildrenLoop(parent, children);
            return children;
        }

        void GetChildrenLoop(Transform parent, List<GameObject> children) {
            foreach (Transform child in parent) {
                children.Add(child.gameObject);
                GetChildrenLoop(child, children);
            }
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
                Command = "setactive/*/%",
                GrabbedValueSpots = new List<char> { '*', '%' }
                //Grabbed value: setactive/gameobject name/bool
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

            new BDCommand {
                Command = "stopaudio/%",
                GrabbedValueSpots = new List<char> { '%' }
                //Grabbed value: stopaudio/Gameobject with audiosource
            },

            new BDCommand {
                Command = "randomint/%/*/^/$",
                GrabbedValueSpots = new List<char> { '%', '*', '^', '$' }
                //Grabbed value: randomint/min/max/Gameobject with variable/Event ID
            },

            new BDCommand {
                Command = "settext/%/*/&",
                GrabbedValueSpots = new List<char> { '%', '*', '&' }
                //Grabbed value: settext/newvalue/Gameobject with TMPro/variable
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

        public GameObject FindObjectInDimension(string ObjectName) {
            foreach(GameObject obj in AllObjects) {
                if(obj.name == ObjectName)
                    return obj;
            }

            return null;
        }

        public async void RunCommands(BDMethod method) {
            string[] MethodCommands = method.Commands.Split('|');

            foreach(string cmd in MethodCommands) {
                string[] Commandparts = cmd.Split('/');
                foreach(BDCommand command in Commands) {
                    string[] CheckParts = command.Command.Split('/');

                    if (CheckParts[0] is "" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"\" but some values were empty");
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

                        GameObject obj = FindObjectInDimension(Commandparts[1]);

                        if(obj is null || obj.GetComponent<BDMethod>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"runmethod\" but put an invalid GameObject method to run");
                            break;
                        }

                        RunCommands(obj.GetComponent<BDMethod>());
                        break;
                    }

                    if (CheckParts[0] is "if" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2]) || string.IsNullOrWhiteSpace(Commandparts[3]) || string.IsNullOrWhiteSpace(Commandparts[4])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"if\" some values were empty");
                            break;
                        }

                        GameObject obj = FindObjectInDimension(Commandparts[1]);

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
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2]) || string.IsNullOrWhiteSpace(Commandparts[3]) || string.IsNullOrWhiteSpace(Commandparts[4])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"if\" some values were empty");
                            break;
                        }

                        GameObject obj = FindObjectInDimension(Commandparts[1]);

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

                        GameObject obj = FindObjectInDimension(Commandparts[1]);
                        BDVariable BDvar = obj.GetComponent<BDVariable>();

                        if (obj is null || BDvar is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"runmethod\" but put an invalid GameObject variable to change");
                            break;
                        }

                        if (!BDvar.VariableChanged) {
                            BDvar.VariableData = Commandparts[2];

                            if(BDvar.Type == VariableType.OneChange)
                                BDvar.VariableChanged = true;
                        }
                        break;
                    }

                    if (CheckParts[0] is "addtrigger" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"addtrigger\" but some values were empty");
                            break;
                        }

                        Trigger trigger = method.gameObject.AddComponent<Trigger>();
                        trigger.ID = int.Parse(Commandparts[1]);

                        switch (Commandparts[2]) {
                            case "right":
                                trigger.Type = TriggerType.RightHand;
                                break;
                            case "left":
                                trigger.Type = TriggerType.LeftHand;
                                break;
                            case "both":
                                trigger.Type = TriggerType.BothHands;
                                break;
                        }
                        break;
                    }

                    if (CheckParts[0] is "setactive" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"setactive\" but some values were empty");
                            break;
                        }

                        GameObject obj = FindObjectInDimension(Commandparts[1]);

                        if (obj is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"setactive\" on an object but that object didn't exist");
                            break;
                        }

                        if (Commandparts[2] is "true" || Commandparts[2] is "t")
                            obj.SetActive(true);
                        else if (Commandparts[2] is "false" || Commandparts[2] is "f")
                            obj.SetActive(false);
                        break;
                    }

                    if (CheckParts[0] is "sethitsound" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"sethitsound\" but some values were empty");
                            break;
                        }

                        if(method.gameObject.GetComponent<GorillaSurfaceOverride>() is null)
                            method.gameObject.AddComponent<GorillaSurfaceOverride>();

                        method.gameObject.GetComponent<GorillaSurfaceOverride>().overrideIndex = int.Parse(Commandparts[1]);
                        break;
                    }

                    if (CheckParts[0] is "playaudio" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"playaudio\" but some values were empty");
                            break;
                        }

                        GameObject obj = FindObjectInDimension(Commandparts[1]);

                        if (obj is null || obj.GetComponent<AudioSource>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"playaudio\" but the targeted GameObject didn't have an audio source or didn't exist");
                            break;
                        }

                        obj.GetComponent<AudioSource>().Play();
                        break;
                    }

                    if (CheckParts[0] is "stopaudio" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"stopaudio\" but some values were empty");
                            break;
                        }

                        GameObject obj = FindObjectInDimension(Commandparts[1]);

                        if (obj is null || obj.GetComponent<AudioSource>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"stopaudio\" but the targeted GameObject didn't have an audio source or didn't exist");
                            break;
                        }

                        obj.GetComponent<AudioSource>().Stop();
                        break;
                    }

                    if (CheckParts[0] is "starttimer" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"starttimer\" but some values were empty");
                            break;
                        }

                        int EventID = int.Parse(Commandparts[1]);
                        int TimeInSeconds = int.Parse(Commandparts[2]);

                        await Task.Delay(TimeInSeconds * 1000);

                        RunEvent(method.gameObject, EventID);
                        break;
                    }

                    if (CheckParts[0] is "randomint" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2]) || string.IsNullOrWhiteSpace(Commandparts[3]) || string.IsNullOrWhiteSpace(Commandparts[4])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"randomint\" but some values were empty");
                            break;
                        }

                        int randomValue = Random.Range(int.Parse(Commandparts[1]), int.Parse(Commandparts[2]));
                        GameObject obj = FindObjectInDimension(Commandparts[3]);

                        if(obj is null || obj.GetComponent<BDVariable>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"randomint\" and changing a variable on a gameobject but it didn't exist or didn't have a variable comp");
                            break;
                        }

                        obj.GetComponent<BDVariable>().VariableData = randomValue.ToString();
                        RunEvent(method.gameObject, int.Parse(Commandparts[4]));
                        break;
                    }

                    if (CheckParts[0] is "settext" && Commandparts[0] == CheckParts[0]) {
                        if (string.IsNullOrWhiteSpace(Commandparts[1]) || string.IsNullOrWhiteSpace(Commandparts[2])) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"settext\" but some values were empty");
                            break;
                        }

                        GameObject obj = DimensionTools.FindObjectInDimension(Commandparts[2]);
                        GameObject obj3 = null;

                        if (!string.IsNullOrWhiteSpace(Commandparts[3]))
                           obj3 = DimensionTools.FindObjectInDimension(Commandparts[3]);

                        if (obj is null || obj.GetComponent<TextMeshPro>() is null) {
                            Debug.LogError($"Object {method.gameObject.name} tried running \"settext\" but no objects were found with the TextMeshPro comp");
                            break;
                        }

                        if (!string.IsNullOrWhiteSpace(Commandparts[3]))
                            obj.GetComponent<TextMeshPro>().text = obj3.GetComponent<BDVariable>().VariableData;
                        else
                            obj.GetComponent<TextMeshPro>().text = Commandparts[1];
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