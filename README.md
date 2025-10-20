# MIV3 - CosmosOS Text Editor Update Project

 - MIV functions are similar to VIM.

It is best to use this in a CLI-like environment where you have commands to start MIV3.\n
In order to use MIV3 like this you need to put StartMIV(); in your command switch.
After StartMIV(); is run, it will walk you through the rest of the steps needed.
 
Possible action inside editor:
 - i (Enter INSERT mode)
 - ESC button (Exit INSERT mode, or go back to editor from help screen)
 - :x (Save and Exit), returns String of text
 - :q (Quit without saving), returns null

 - :help (Display start/help page)
