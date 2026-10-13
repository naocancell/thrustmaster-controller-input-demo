# Task Plan: Thrustmaster HOTAS A-10C Gamepad Input & Haptics Inspector Demo

## 1. Goal
Provide a zero-dependency, plug-and-play visual demo to test Thrustmaster (A-10C HOTAS) gamepad inputs (axes, buttons, POV hat) and force feedback / vibration capabilities, designed for immediate browser testing and ready to be integrated into the user's software.

## 2. Technical Stack & Architecture
- Visual Demo: Single-file HTML5/CSS3/JavaScript utilizing the Web Gamepad API.
- Aerospace / A-10C Tactical Cockpit HUD Dark UI design with high-contrast readouts.
- Comprehensive Gamepad API integration:
  - Multi-gamepad hotplug detection and switching.
  - Multi-axis visual radar (Pitch/Roll X-Y crosshair, Throttle/Rudder sliders).
  - Button state grid (high-speed visual highlight, trigger counters).
  - POV / Hat switch 8-direction visualization.
  - Haptic / Vibration test suite (`vibrationActuator` dual-rumble / trigger-rumble tests).
  - Real-time input stream logger.
- Software Integration Guide:
  - Web / Electron integration steps.
  - Native C# (.NET) / C++ DirectInput8 architectural equivalents for long-term software integration.

## 3. Execution Steps
- [x] Step 1: Clarify tech stack, model (A-10C), and requirements (Visual + Haptics/Vibration).
- [x] Step 2: Implement `index.html` with complete HUD visual interface, Gamepad API listener, and vibration testing.
- [x] Step 3: Implement C# (.NET / Windows) native integration reference `ThrustmasterNativeExample.cs` for future backend/desktop embedding.
- [x] Step 4: Verify file creation and local accessibility.
- [x] Step 5: Deliver detailed user documentation and usage instructions in Chinese.
