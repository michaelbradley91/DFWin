# Dwarf Fortress for Windows (DFWin)
Firstly, please understand this game **is not Dwarf Fortress**. All credit for the underlying mechanics should go to [Bay12Games](http://www.bay12games.com/dwarves/).<br>
Please lend them your support!

Dwarf Fortress is an **amazing game**, but I feel its user interface could be improved to make the game more accessible, so more people can have fun with it.

There are many existing tools out there for Dwarf Fortress, many of which are detailed in its [Wiki](http://dwarffortresswiki.org/index.php/DF2014:Utilities). 

[DFHack](http://dwarffortresswiki.org/index.php/Utility:DFHack) in particular is probably the most advanced, analysing the game's memory allowing for some direct manipulation of the game. Please check it out as it might be a better fit for you.

**This project takes a radically different approach**.

## How it works

DFWin will constantly screenscrape Dwarf Fortress while it runs in the background. By using a special tileset, DFWin can understand exactly what you would normally see on Dwarf Fortress's screen! It really sends key presses to Dwarf Fortress as you play the game.

DFWin maintains a tile grid representation of what is visible in Dwarf Fortress. It then interprets these tiles, and provides its own UI based on them. Your interactions with the game are then translated into key presses for Dwarf Fortress.

If at some point DFWin becomes confused, it will revert back to showing Dwarf Fortress as is, so even if new pages appear in the game or you find pages that are not supported, you can still use DFWin if you really want to.

## How to run

1. [Install Dwarf Fortress](http://www.bay12games.com/dwarves/)
2. Replace data/init/init.txt with [this one](init.txt).
3. Add [this tileset](DFWin/DFWin.Core/Resources/ComputerTileSetMicro.bmp) to data/art.
4. Clone this repository, build the solution (I use VS 2017) and run the application. You can download [this older example](DFWinTest.zip) of the game to see if it will work.

The init.txt needs to point to the correct tileset for the application to work. I've skipped the opening movie for development, but it will probably work with that enabled too!

## Other bits

If you like the project and would like me to spend more time on it, please let me know!
