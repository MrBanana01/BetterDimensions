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
