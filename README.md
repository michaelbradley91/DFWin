# Dwarf Fortress for Windows (DFWin)
Firstly, please understand this game **is not Dwarf Fortress**. All credit for the underlying mechanics should go to [Bay12Games](http://www.bay12games.com/dwarves/).<br>
Please lend them your support!

Dwarf Fortress is an **amazing game**, but I feel its user interface could be improved to make the game more accessible, so more people can have fun with it.

There are many existing tools out there for Dwarf Fortress, many of which are detailed in its [Wiki](http://dwarffortresswiki.org/index.php/DF2014:Utilities). 

[DFHack](http://dwarffortresswiki.org/index.php/Utility:DFHack) in particular is probably the most advanced, analysing the game's memory allowing for some direct manipulation of the game. Please check it out as it might be a better fit for you.

**This project takes a radically different approach**.

## How it works

DFWin will constantly screenscrape Dwarf Fortress while it runs in the background. By using a special tileset, DFWin can understand exactly what you would normally see on Dwarf Fortress's screen. As you play the game, it really sends key presses to Dwarf Fortress.

By understanding the tiles that make up Dwarf Fortress's UI, the game can choose to copy the UI exactly or, more importantly, it can provide its own UI and translate your interaction into key presses for the game. This allows it to rewire keys in the game with mouse clicks and so on to make it easier to play.

## How to use

1. [Install Dwarf Fortress](http://www.bay12games.com/dwarves/)
2. Replace data/init/init.txt with [this one](init.txt).
3. Add [this tileset](DFWin/DFWin.Core/Resources/ComputerTileSetMicro.bmp) to data/art.
4. Clone this repository, build the solution (I use VS 2017) and run the application. You can download [this older example](DFWinTest.zip) of the game to see if it will work.

The init.txt needs to point to the correct tileset for the application to work. I've skipped the opening movie for development, but it will probably work with that enabled too!

## Other bits

If you like the project and would like me to spend more time on it, please let me know!
