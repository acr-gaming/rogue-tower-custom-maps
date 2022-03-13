# WARNING

These are all designed for single lane. Triple/Dual Lane will not generally work.

# Custom Maps

## [Youtube How To Video](https://www.youtube.com/watch?v=BtKTXujwAjA)

Adds custom maps to the game. Currently only really supports single lane/no splits effectively. 
All portals after a split works fine too.

## Map Descriptions
```
    maps.Add("Barely Legal", new string[] { "L", "S", "L", "S", "L", "L", "P" });
    // The below one actually does work with 2/3 way
    maps.Add("Instant Portal", new string[] { "P", "P", "P" });
    maps.Add("3-way Instant Portal", new string[] { "SLR", "P", "P", "P" });
    maps.Add("5 Straights and 1 Portal", new string[] { "S", "S", "S", "S", "S", "P" });
    maps.Add("The Clock", new string[] { "L", "L", "S", "L", "S", "L", "S", "P" });
    maps.Add("1 turn 1 Portal", new string[] { "L", "P" });
    maps.Add("1 turn 3 Portals", new string[] { "L", "SLR", "P", "P", "P" });
    maps.Add("T", new string[] { "S", "S", "LR", "P", "P" });
```

# Make Your Own Map

You want to set the selected map to custom in the config
```
Selected Map = Custom
```
Then fill in the custom map field with a "description" of the map
```
Custom Map = S, S, P
```

## Caveats

Splits generally do not work. If you are very very careful with the order of expansion *CAN* make
any arbitrary map work. The tiles will appear in the order you expand in so if you really really
want to get a specific map with splits you can do it by carefully planning the order in which
you expand while playing the game.
If there is is enough interest I'll look at adding proper support for splits.

### Caveats' Caveats

One caveat of the above caveat is that if you only do a single split then all portals after that works.

## Tile Types/Names
```
P - Portal
```
### No Splits
```
S - Straight
L - Left
R - Right
```
### Double Splits
```
SL - Straight Left
SR - Straight Right
LR - Left Right
```
### Triple Splits
```
SLR - Straight Left Right
```

## Examples

### The Clock
```
Custom Map = L, L, S, L, S, L, S, P
```

### Three Way Split Then Portals:
```
Custom Map = SLR, P, P, P
```




