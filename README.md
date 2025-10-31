# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials entry package via Unity's Package Manager, then install modules from the Tools menu.

- Add the entry package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

---

# Frame Time Profiling

> Quick overview: Lightweight, in-game frame time overlay with a bar graph and live stats (FPS, frame ms, render ms, memory, GC spikes). Works in Editor and Player. Toggle with keys; configurable size and screen pivot.

A minimal runtime profiler overlay that renders a compact frame-time graph and textual stats directly to the screen. It samples frame time, render time, memory usage, and GC activity, and draws a pixel‑perfect bar chart with reference lines at your display’s refresh rate. Designed to be unobtrusive and easy to drop into any scene.

![screenshot](Documentation/Screenshot.png)

## Features
- Live on-screen stats
  - FPS, total frame time (ms), render time (ms)
  - Memory usage (MB) and recent GC collection sizes
- Compact frame-time graph
  - Per‑frame bars: total frame time (green), render time (blue)
  - GC spikes highlighted (magenta overlay)
  - GC allocation marker bars (purple) below the axis
  - Reference lines: target FPS, half, quarter, and 1 FPS
- Smart scaling and placement
  - Graph size is a percentage of the screen (width × height)
  - Bottom‑left pivot as a percentage of the screen for positioning
  - Pixel‑aligned GL rendering for crisp lines
- Simple controls
  - Toggle key (default: F3). To re‑enable when hidden, hold the modifier (default: Left Shift) + F3
- Runtime‑ready and SRP/Built‑in compatible
  - Draws after camera rendering via `RenderPipelineManager.endCameraRendering`
  - Minimal overhead; graph draws only when visible
- Optional custom font for the text overlay

## Requirements
- Unity 6000.0+ (Runtime + Editor)
- Works with Built‑in and SRP pipelines (uses `RenderPipelineManager.endCameraRendering`)

## Usage

Add the profiler overlay to any scene.

1) Create an empty GameObject (e.g., `FrameTimeProfiler`)
2) Add the following components to the same GameObject:
   - `FrameTimeManager`
   - `FrameTimeGraphRenderer`
   - `FrameTimeGUI`
   - `FrameTimeMonitor` (required by the others)
3) Optionally assign a font to `FrameTimeGUI._customFont` for the text
4) Press Play; the overlay appears in the bottom‑left by default

Tip: The manager auto‑wires missing component references in `Awake()` if they’re on the same GameObject.

### Controls
- Toggle overlay: F3
- Re‑enable if hidden: hold Left Shift and press F3
- You can change keys in the `FrameTimeManager` inspector (Toggle Key and Modifier Key)

### Customization
- In `FrameTimeManager`:
  - Display Settings: enables/disables the overlay
  - Graph Size (%): width × height as a percentage of the screen
  - Bottom‑Left Pivot (%): anchoring point from the bottom‑left corner of the screen
  - Target Refresh Rate: auto‑filled from `Screen.currentResolution.refreshRateRatio`
- In `FrameTimeGUI`:
  - Custom Font: assign your own font for the overlay

## Notes and Limitations
- Rendering
  - Uses immediate‑mode GL with the `Hidden/Internal-Colored` shader for the graph; it draws after Game camera rendering
  - The graph width is the number of history samples shown; history length is managed by `FrameTimeMonitor`
- Input behavior
  - To prevent accidental enabling, re‑enabling requires holding the modifier when hidden (Shift + F3 by default)
- Screen and scaling
  - Positions/sizes derive from the current `Screen.width/height`; multi‑display or dynamic resolution may affect layout
- Stats accuracy
  - Memory and GC figures reflect managed memory; values may vary by platform and Mono/IL2CPP runtime behavior
- Performance
  - Overhead is low and only incurred while visible; still recommended to disable in shipping builds

## Files in This Package
- `Runtime/FrameTimeManager.cs` – Settings, input, and render pipeline hook; orchestrates sampling and rendering
- `Runtime/FrameTimeGraphRenderer.cs` – Pixel‑aligned GL graph (frame time, render time, reference lines, GC markers)
- `Runtime/FrameTimeGUI.cs` – Text overlay (FPS, ms, memory, GC labels) with optional custom font
- `Runtime/FrameTimeMonitor.cs` – Sampling and history (frame data, memory, GC collections)
- `package.json` – Package manifest metadata

## Tags
unity, runtime, profiling, performance, fps, frame-time, render-time, overlay, gl, ongui, memory, gc, hud
