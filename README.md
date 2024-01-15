# Rock Paper Scissors Boids
## Overview
This project is an visualization of how [boids](https://en.wikipedia.org/wiki/Boids) work. This project uses entities of rock, paper, and scissors. Each entity will try to tag the entity that they are strong against in order to transform them. For example, if scicssors tags a rock, the rock will transform into scissors. Entities will try to avoid other entities that they are weak against to stay alive longer.
## Forces
Here are all the forces each entity has in order to make them feel alive:
- Seek: Move towards the target
- Flee: Run away from the target (not the same as avoidance)
- Wander: Aimlessly move around
- Stay in bounds: Stay on the screen
- Avoidance: Stay clear of target (not the same and flee)
- Flocking
    -  Separation: Stay away from flock
    -  Allignment: Move in the date direction as a flock
    -  Cohesion: Move towards the center of a flock
- Future time (in seconds): Not a force, but a parameter that will help some forces (Wander, Stay in bounds, Avoidance) work properly since they rely on knowing their future position at a certain point of time in order to work.

## Player Abilities
- Spawn entities (cap of [COUNT])
- Toggle which forces are on/off for each entity
- Change the weight of each force, changing the priotry of each entity
- Add an obstacle that all entities will avoid
    - Click on the obstacle to remove it
- Add a power up that will spawn 5 entities of the type that grabbed it
