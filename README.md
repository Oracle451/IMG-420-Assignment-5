# IMG-420 Assignment 5

## Overview
This project demonstrates a combination of **Custom Shaders on GPU Particles**, **Rigid Body Physics With Joints**, and **Raycast Laser Player Detection**. It includes a custom shader for animated distortion effects, a laser detector that triggers alarms on player detection, and a dynamic physics chain reacting to player forces.

---

## Part 1: Custom Canvas Item Shader with Particles (3 points) (`custom_particle.gdshader`)

### How It Works
The **custom particle shader** applies a color gradient effect and wave distortion over time animation for GPU particles.  
It takes a **time uniform** passed from the C# `ParticleController` script and uses it to procedurally vary the particles scale with a sine wave effect to simulate a wave.

Behavior includes:
- Oscillating UV distortion based on sine waves
- Smooth noise or ripple motion over time

### Physics Properties Chosen (in `ParticleController.cs`)
- **Initial Velocity (20–40 px/s):** Adds variation to particle motion.
- **Spread (50°):** Ensures the emission is not uniform, creating a natural spray.
- **Direction (0, -1):** Particles emit upward to look like vapor.
- **Scale (0.5–1.0):** Adds size variation for visual depth.
- **Gravity (0, 0):** Disabled to keep effects floating upward.

These settings produce lightweight, smooth movement that pairs well with shader-driven distortion.

---

## Part 2: Advanced Rigid Body Physics with Joints (3.5 points) (`PhysicsChain.cs`)

### How It Works
The **PhysicsChain** creates a rope-like sequence of `RigidBody2D` segments connected via `PinJoint2D`s.

**Setup process:**
1. A **static anchor** acts as the top attachment point to keep the entire chain from falling down.
2. Each chain link (RigidBody2D) is instantiated below the previous one.
3. **PinJoint2D** nodes connect each segment to the next, allowing flexible motion.
4. Adjacent links ignore collisions to prevent physics explosions.
5. The chain starts **frozen** until one physics frame passes, ensuring stable initialization.

This results in a dynamic, physics-reactive chain that can swing, react to the player, or carry weight naturally.

---

## Part 3: Raycasting Laser with Player Detection (3.5 points) (`LaserDetector.cs`)

### How It Works
The **laser detector** emits a ray using `RayCast2D` that continuously checks for collisions along its length.  
A `Line2D` node visually represents the beam, dynamically shortening if an object is hit.

**Core behavior:**
1. The `RayCast2D` is aimed forward (default: 500px).
2. Each frame (`_PhysicsProcess`), it updates and checks for collisions.
3. When a collider is detected:
   - The `Line2D` end position adjusts to the hit point.
   - If the collider matches the `Player` node, an **alarm** is triggered.
4. The alarm flashes the laser color to red for 1.5 seconds before resetting.

**Key Functions:**
- `SetupRaycast()` – Creates and enables the raycast for continuous detection.
- `SetupVisuals()` – Draws the visible laser beam.
- `TriggerAlarm()` – Activates when the player is detected, logs the event, and changes the beam color.

This system provides a simple but effective **security or trap detection mechanic** in 2D.

---

## Player System (`Player.cs`)

### Movement & Interaction
The **player** uses a `CharacterBody2D` with basic arrow-key input.  
Movement is handled via `MoveAndSlide()` for smooth collision-based motion.

When colliding with `RigidBody2D` objects (like chain links), the player applies an **impulse force** based on the collision normal.  
This lets the player physically **push or swing the chain**, creating interactive movement.

---

## System Integration

- The **LaserDetector** references the **Player** through its exported `PlayerPath` property.
- The **PhysicsChain** reacts to forces, allowing the player to affect the environment.
- The **ParticleController**’s shader provides ambient or reactive visual effects for interactive zones.

Together, these components create a small physics sandbox where:
- Visual shaders add atmosphere.
- Raycasting detects and responds to player motion.
- Physics objects interact dynamically with player movement.

---

## Files Overview

| File | Description |
|------|--------------|
| `custom_particle.gdshader` | GPU shader for animated particle distortion |
| `ParticleController.cs` | C# script managing shader parameters and emission behavior |
| `LaserDetector.cs` | Raycast-based laser detection and alarm logic |
| `PhysicsChain.cs` | Procedural rope/chain generation with PinJoint2D physics |
| `Player.cs` | Player movement and physics interaction logic |

---

**Engine:** Godot 4.4
**Language:** C# (Mono)
