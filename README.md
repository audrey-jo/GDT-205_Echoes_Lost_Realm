# Echoes of the Lost Realm

Echoes of the Lost Realm is a group project created for GDT-205 using Unity 6.3.

The game follows Aria as she explores dangerous environments while AI systems respond to hazards, obstacles, enemies, and player movement.

## Current Milestone

### Milestone 4: Decision-Making

For this milestone, decision-making systems were added to the enemies in the InnerTemple level.

The AI systems include:

- Finite State Machines (FSM)
- Behavior Trees
- Enemy wandering, chasing, and retreating
- Boss enemy summoning
- Two-phase boss encounter
- Adaptive enemy behavior
- State transitions based on player actions
- Boss projectile attacks
- Testing controls for Aria
- Boss health and phase information displayed in the Unity Console

## Finite State Machine

The regular enemies use a Finite State Machine to switch between different behaviors depending on their surroundings and interactions with Aria.

The Minions have three main states:

| State | Behavior |
| --- | --- |
| Wander | Moves around the assigned area |
| Chase | Follows Aria when she gets close |
| Retreat | Moves away when health gets low |

These states allow enemies to respond to Aria and changes in their environment.

## Boss Behavior Tree

A boss was added to the InnerTemple level that uses a Behavior Tree to control its actions and enemy summoning.

The Behavior Tree uses Selector, Sequence, Condition, and Action nodes to determine what the boss should do.

The boss includes:

- Detection range for Aria
- Normal enemy summoning
- Additional reinforcements when Aria gets closer
- Summoning cooldowns
- Maximum number of active summoned Minions
- Replacement of defeated Minions
- Adaptive summoning based on Aria's distance
- Two phases based on the boss's remaining health
- Projectile attacks during the second phase
- Console messages for boss health, phase changes, and defeat

Summoned Minions use the existing FSM to control their movement and behavior.

## Boss Phases

The boss has two phases, with the second phase activating when its health reaches 50%.

| Phase | Behavior |
| --- | --- |
| Phase 1 | The boss summons Minions to attack Aria. Summoning speed increases when Aria gets too close. |
| Phase 2 | At half health, the boss becomes more aggressive, summons Minions more frequently and in greater numbers, and begins firing projectiles at Aria. |

### Phase 1: Normal Behavior

During the first phase, the boss mainly relies on its summoned Minions to defend itself.

Aria's distance affects how frequently the boss summons reinforcements.

When Aria enters the danger zone surrounding the boss, the summoning cooldown decreases, causing enemies to spawn more frequently.

### Phase 2: Aggressive Behavior

Once the boss reaches half health, Phase 2 begins.

During this phase:

- The boss summons larger groups of Minions.
- The time between summons decreases.
- The boss begins firing projectiles at Aria.
- Getting too close to the boss further increases the Minion spawning rate.
- The encounter becomes more difficult as the boss becomes more aggressive.

The boss remains in Phase 2 until it is defeated.

## Adaptive Enemy Spawning

The boss adjusts its summoning behavior based on Aria's distance and its current phase.

### Normal Range

When Aria is within the boss's detection range but outside its danger zone, Minions spawn at the normal rate for the current phase.

### Danger Zone

When Aria gets too close to the boss:

- The summoning cooldown decreases.
- Minions spawn more frequently.
- The increased spawning rate continues while Aria remains inside the danger zone.
- Leaving the danger zone returns the summoning rate to normal for the current phase.

### Phase 2 Changes

During Phase 2, the boss summons enemies in greater numbers and more frequently than in Phase 1.

The danger zone still affects the summoning rate, making it more dangerous for Aria to remain close to the boss.

## Boss Health and Console Debugging

The boss's health and important events can be monitored through Unity's Console window during gameplay.

The Console displays:

- The boss's current health
- When the boss enters Phase 2 at 50% health
- When the boss is defeated

These messages make it easier to test the boss's behavior and confirm that the phase transition and defeat conditions are working correctly.

### Viewing Console Messages

1. Open the InnerTemple scene in Unity.
2. Navigate to `Window > General > Console`.
3. Press Play.
4. Damage the boss using Aria's projectiles.
5. Watch the Console for health updates.
6. Continue damaging the boss to trigger Phase 2.
7. Defeat the boss to view the defeat message.

## Testing Features

Additional controls were added to make testing the boss encounter easier.

| Key | Action |
| --- | --- |
| I | Toggle Aria's invincibility on or off |
| K | Toggle between normal projectiles and instant-kill projectiles |

### Invincibility Mode

Pressing I toggles Aria's invincibility.

This allows Aria to survive enemy attacks while testing the boss's behavior, summoning system, and projectile attacks.

### Instant-Kill Projectiles

Pressing K switches Aria between normal projectiles and instant-kill projectiles.

This makes it easier to test:

- Enemy deaths
- Boss health changes
- Boss phase transitions
- Boss defeat behavior
- Minion replacement after enemies are defeated

These controls are intended for testing the AI systems.

---

## Previous Milestones

### Milestone 3: Pathfinding

For this milestone, A* pathfinding was added to the temple level.

The pathfinding system includes:

- Node-based navigation
- A* pathfinding
- Static obstacle detection
- Enemy avoidance
- Dynamic path updates
- Pathfinding visualization
- Adjustable navigation settings

These systems allow AI agents to navigate through the temple while accounting for walls and other obstacles.

The pathfinding system can recalculate routes when obstacles change, allowing the AI to adjust its movement instead of following a blocked path.

### Main Pathfinding Scripts

| Script | Purpose |
| --- | --- |
| NodeGraph | Manages the navigation nodes |
| AStarPathfinder | Calculates efficient paths through the environment |
| PathDemo | Handles navigation testing and path updates |

The navigation settings include adjustable movement speed, obstacle detection, enemy avoidance, and path recalculation intervals.

---

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

## Main Terrain Scripts

### TerrainMaps.cs

Location:

```text
Assets/Project/Scripts/HeatMap/TerrainMaps.cs
```

This script manages the terrain analysis grid, Influence Map, Heat Map, and map visualization.

### TerrainZone.cs

This script identifies terrain zones and provides information about dangerous or blocked areas.

These zones can represent:

- Spike pits
- Evil Spirits
- Walls
- Other environmental hazards

---

## Running the Project

### Requirements

- Unity Hub
- Unity 6.3
- Windows PC

### Setup Instructions

1. Download or clone the GitHub repository.
2. Open Unity Hub.
3. Select Add Project from Disk.
4. Select the downloaded Unity project folder.
5. Open the project using Unity 6.3.
6. Open the desired scene from the project's Scenes folder.
7. Press Play to test the game.

### Main Scenes

| Scene | Description |
| --- | --- |
| Forest | Forest environment and early AI behaviors |
| Ruins | Terrain analysis, Heat Map, and Influence Map |
| Temple | A* pathfinding and obstacle navigation |
| InnerTemple | Enemy FSM, boss Behavior Tree, two-phase boss encounter, and testing features |

### Testing the Boss Encounter

1. Open the InnerTemple scene.
2. Press Play.
3. Move Aria toward the boss.
4. Observe the boss summoning Minions.
5. Move closer to the boss to increase the summoning rate.
6. Attack the boss until it reaches 50% health.
7. Observe the transition to Phase 2.
8. Watch the boss summon more Minions and fire projectiles.
9. Use I to toggle invincibility if needed.
10. Use K to toggle instant-kill projectiles for testing.
11. Defeat the boss.
12. Check the Unity Console for boss health, phase changes, and defeat messages.

---

## Project Information

| | |
| --- | --- |
| Course | GDT-205 - AI for Game Developers |
| Engine | Unity 6.3 |
| Platform | PC |
| Project Type | Group Project |
| Project Name | Echoes of the Lost Realm |
