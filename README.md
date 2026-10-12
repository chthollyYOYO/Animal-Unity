# Animal Food Truck (动物餐车)

A team-developed demo of a 2D restaurant management game, with story sequences and boss encounters featuring simple rhythm-based quick-time events (QTEs).

This repository presents the Unity portion of **动物餐车** (translated here as *Animal Food Truck*), a studio project demo I contributed to during my internship at **Luozhou Studio (螺舟工作室)**. The studio has permitted me to share this project publicly for my portfolio. This is a team project; the contributions described below are my responsibilities, not a claim of sole authorship of the game or its assets.

- **Contributor:** Kerui Bao
- **Role:** Unity Gameplay / Client Development Intern
- **Internship:** January - June 2026

## Gameplay

- Run a restaurant: receive customers, manage queues and tables, prepare and serve orders, and complete service interactions.
- Use shops, inventory, and contextual interactions to support the restaurant loop.
- Follow dialogue and story sequences with branching choices.
- Take part in boss encounters that introduce simple rhythm-game QTEs, including timed taps, holds, and charge-bar interactions.

## My Contributions

### Restaurant Simulation and Interaction

Implemented restaurant gameplay using C# and Unity's component architecture. Developed `StallSystem` to coordinate customer spawning, queueing, seating, service, and departure, with dictionaries maintaining customer and table relationships. Built a reusable interaction framework for selection, highlighting, long-press actions, and state transitions.

**Explore:** [Restaurant systems](Assets/Scripts/GameLogic/Stall) · [Interaction framework](Assets/Scripts/GameLogic/InteractableObject)

### Story and ADV Dialogue

Developed Excel-driven dialogue structures for character lines, branching choices, and story transitions. Used UniTask for asynchronous sequence control, typewriter text, event waits, and UI synchronization. Connected Timeline sequences to gameplay through custom events.

**Explore:** [Dialogue systems](Assets/Scripts/GameLogic/Dialogue) · [Dialogue UI](Assets/Scripts/UI/Dialogue) · [Configuration tools](Assets/Editor/Tools/ExcelTools.cs)

### Rhythm QTEs for Boss Encounters

Designed and implemented a beatmap-driven rhythm framework supporting taps, holds, and charge bars. Created Timeline markers and custom playable clips so designers could author rhythm events in the editor. Implemented timing windows, input evaluation, hit feedback, and pooled checker objects.

**Explore:** [Beatmap system](Assets/Scripts/Beatmap/BeatmapSystem.cs) · [Timeline authoring](Assets/Scripts/Beatmap/Timeline) · [Note checkers](Assets/Scripts/Beatmap/Checker)

### UI Framework and Tools

Built FairyGUI interfaces for inventory, shops, payments, tutorials, and dialogue choices. Encapsulated UI creation, display, and hiding through lifecycle interfaces, and supported keyboard and mouse input with synchronized UI and gameplay states.

**Explore:** [UI framework](Assets/Scripts/Framework/UI) · [Game interfaces](Assets/Scripts/UI) · [Input system](Assets/Scripts/Framework/InputSystem)

### URP Interaction Outlines

Implemented a screen-space outline rendering system with a custom URP `ScriptableRendererFeature`. Used an additional camera and render texture for interaction masks, and addressed scene occlusion using occluder proxies and sorting groups.

**Explore:** [Outline rendering](Assets/Scripts/GameLogic/SobelTexture)

### Architecture and Resource Management

Worked on modular manager systems for scenes, resources, audio, and input. Used object pooling to reduce repeated instantiation and destruction, supporting reusable gameplay components and clearer system boundaries.

**Explore:** [Framework](Assets/Scripts/Framework) · [Object pools](Assets/Scripts/Framework/Pool)

## Technology

| Area | Tools |
| --- | --- |
| Engine | Unity 2021.3.27f1c2 |
| Language | C# |
| Rendering | Universal Render Pipeline (URP) 12.1.12 |
| Sequencing | Unity Timeline, custom markers and playable clips |
| UI | FairyGUI |
| Asynchronous flows | UniTask |
| Data workflow | Excel and Luban-generated configuration |
| Other project dependencies | Spine runtimes, xNode, Sirenix/Odin |

## Opening the Project

1. Clone this repository and add its root folder to Unity Hub.
2. Use **Unity 2021.3.27f1c2**, the exact version recorded in `ProjectSettings/ProjectVersion.txt`.
3. Restore **Sirenix/Odin** from your own appropriately licensed installation. Its plugin files are excluded from this public repository; code referencing Sirenix will require them before compilation can succeed.
4. Let Unity restore packages and import assets. The package lock file contains references to `packages.unity.cn`, which must be reachable or configured for your environment.
5. Open `Assets/Scenes/Init.unity`, the first enabled scene in Build Settings, then enter Play mode once dependencies and compilation are resolved.

These are setup instructions derived from the project configuration. A clean-clone import and playable build have not yet been verified.

## Repository Scope

This repository contains the Unity project's `Assets`, `Packages`, and `ProjectSettings`, including asset metadata and generated configuration/UI code. Unity caches, IDE-generated project files, local user settings, and the original SVN metadata are excluded.

The original workspace also contains separate FairyGUI authoring files, art source files, Excel configuration sheets, and generation tools. Those sibling projects are outside this repository. In particular, the Excel export menu depends on the original `Config` and `Tools` folders; regenerating configuration requires restoring that authoring workspace. Generated Unity-side files are included for inspection and use.

## Credits and Permissions

Published with the studio's permission as an internship portfolio showcase. Game design, artwork, audio, and other team contributions remain credited to their respective creators and rights holders.

Public availability is not a blanket license to reuse or redistribute studio content. No additional open-source license is granted by this repository. Third-party components remain subject to their own licenses and notices; the studio's permission does not replace those terms. Existing third-party notices are preserved, including the Spine runtime notices. Obtain any required third-party licenses before using the corresponding components.
