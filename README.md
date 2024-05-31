A mod for the gorilla tag mod "MonkeDimensions" that adds a whole programming language that can be used in the editor.

--------------------------------------------------------------------------------------------------------------

# base things to know.
Only one BD script can be put on an object at a time.

the charater '|' is what differs it checks for to run commands. For example:
`command1|command2|command3`

it will run command1, then command2, and then command3.
only use this if you need to put multiple commands in the same object.

# how to commands work?!?! 
Commands are separated with the charater '/'. The first word is the starting command, like "if" or "ifnot" or "playaudio or "stopaudio". The values after that separated by slashes are the input values of the command. For example: `changevar/Cool Object name With Variable Comp/Data to change to` 

The "Cool object name with variable comp" should be replaced with a gameobject's name that has a BDVariable component. And the "Data to change to" is the value that the gameobject's BDVariable component will change to.

The MethodTypes are "Awake" and "Manual".
Manual = must be ran by another method
Awake = gets ran on map load & can be ran by another method

![method](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20150716.png?raw=true)

![variable](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20150706.png?raw=true)

----------------------------------------------------------------------------------------------------

# how do events work?!?!
Events have a few settings to them:
EventType:
```
Regular = just a normal event that can be triggered multiple times
OneTrigger = An event that can only be triggered once
```
When a method runs an event, it also sends an "EventID". And the BDEvent component will check if it's Event ID is the same as the one being called. If it is, there is a box where you put the gameobject's with method comps and they will be ran.

![starttimer](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20151939.png?raw=true)

![next](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20151950.png?raw=true)

![runmethod](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20152000.png?raw=true)

---------------------------------------------------------------------------------------------------------

# how do variables work?!?!
Variables hold data and can be gotten later with the "if" and "ifnot" commands.
here's an example on how you would use an "if" command:

![if](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20152804.png?raw=true)

![var](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20152814.png?raw=true)

![true](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20152823.png?raw=true)

![false](https://github.com/MrBanana01/BetterDimensions/blob/master/Documentation%20Images/Screenshot%202024-05-31%20152831.png?raw=true)

Variables also have a few types:
```
Regular = Regular and can be gotten and changed
OneChange = can only be changed once but can still be gotten mutliple times
GetOnly = Cannot be changed and can only be gotten
```

---------------------------------------------------------------------------------------------------------------

# All methods:
- debuglog/message
- debuglogwarning/message
- debuglogerror/message
- runmethod/Gameobject with method
- if/Gameobject with variable/expectedValue/EventID if true/EventID if false
- ifnot/Gameobject with variable/expectedValue/EventID if true/EventID if false
- changevar/Gameobject with variable/variable data to change to
- addtrigger/EventID/Trigger Type (left, right, both)
- setactive/gameobject name/bool
- sethitsound/HitsoundID
- playaudio/Gameobject with audiosource
- starttimer/EventID/Length in seconds
- stopaudio/Gameobject with audiosource
- randomint/min/max/Gameobject with variable/Event ID
- settext/newvalue/Gameobject with TMPro/variable
