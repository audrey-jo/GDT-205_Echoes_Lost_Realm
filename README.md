# Echoes of the Lost Realm

Echoes of the Lost Realm is a group project created for GDT-205 in Unity.

The game follows Aria as she explores dangerous environments while AI systems help respond to hazards and movement through the level.

## Current Milestone

### Milestone 2: Terrain Analysis

For this milestone, terrain analysis was added to the second level of the game.

The terrain system includes:

- An Influence Map
- A Heat Map
- Spike pit danger zones
- Evil Spirit danger zones
- Blocked wall areas
- Dynamic terrain updates
- Player movement tracking
- Map display controls

These systems help support safer AI path decisions in the ruins level.

## Terrain Analysis

The Influence Map identifies dangerous and blocked areas in the level.

The Heat Map records where Aria moves and shows which areas have more player activity.

The terrain system currently recognizes:

| Area | Result |
| --- | --- |
| Normal Floor | Safe |
| Evil Spirit | Dangerous |
| Spike Pit | Blocked |
| Wall | Blocked |

The Evil Spirit danger zones move with the enemies, so the Influence Map updates as they move around the level.

## Map Controls

| Key | Action |
| --- | --- |
| 1 | Hide the current map |
| 2 | Toggle Influence Map |
| 3 | Toggle Heat Map |

The maps continue updating even when they are hidden.

## Main Scripts

### TerrainMaps.cs

Location:

```text
Assets/Project/Scripts/HeatMap/TerrainMaps.cs
