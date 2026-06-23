# [PZT-CORE: SYSTEMS ARCHITECTURE MANUAL]

**PROTOCOL ID:** PZT-EVAC-01  
**CODENAME:** Panic Zone Toggler  
**SYSTEM STATUS:** OPERATIONAL  
**MAINTAINER:** K. GIVLER (ADMIN)  

---

## 1.0 SYSTEM OVERVIEW

The **Panic Zone Toggler** is an emergency containment and rapid threat-evacuation utility designed for the *RimWorld* colonist-management framework. During high-threat events (such as hostile breaches or structural incursions), manual re-allocation of individual zone assignments introduces severe latency and operational overhead.

This utility intercepts the zone restriction architecture, executing a global, instantaneous override that routes all player-controlled assets into a designated lockdown zone.

## 2.0 FUNCTIONAL CAPABILITIES

The system features an automated lockdown pipeline operated via two primary input vectors:

* **Hardware Interrupt (Hotkey Support):** Operates natively via the `'P'` keybind registry. The trigger signal can be completely remapped within the baseline configuration menu (`Options -> Controls`) for rapid deployment.
* **Console Interface Override:** Injects an emergency toggle switch directly adjacent to the simulation speed controls. Activating the interface forces immediate relocation; deactivating the switch safely rolls back all targeted assets to their exact pre-panic zone configurations.
* **Redundancy Fallback Routine:** The framework scans local map arrays for an allowed area matching the `"Panic"` string key. If the targeted area definition cannot be verified, the routine automatically routes assets to the baseline **Home Zone** allocation index to preserve unit integrity.

## 3.0 SUBSYSTEM CONFIGURATION MATRIX

Administrators can isolate or combine target classes within the utility configuration panel to control which assets respond to the emergency signal:

| Target Vector Class | Operational Assignment | Routine Behavior |
| --- | --- | --- |
| **Humanoid Assets** | Colonists | Immediate containment routing |
| **Mechanized Assets** | Player-controlled mechanoids | Immediate containment routing |
| **Biological Assets** | Domesticated colony animals | Immediate containment routing |

* *Note: Any combination of the above vectors can be toggled independent of one another, allowing functional units to remain on schedule while non-combatants flee.*

## 4.0 TECHNICAL SPECIFICATIONS

The system architecture is engineered to adhere to strict high-efficiency coding standards:

* **Polled Memory Footprint:** The framework contains no active tick loops or asynchronous polling threads. While in an idle state, the mod consumes exactly **0.000ms** of CPU execution time, rendering it fully compatible with massive 500+ modification load profiles.
* **Volatile Reference Preservation (Caravan Stability):** The utility features isolated tracking state retention. Assets that exit the primary simulation map boundaries or merge into caravan vectors while the lockdown signal is active will cleanly preserve their assigned pre-panic zones upon re-instantiation.

## 5.0 SYSTEM ACCESS NODES

* **[Source Code Repository (GitHub)](https://github.com/JoyfulReaper/PanicZoneToggler)**
* **[Distribution Rights (BSD 2-Clause License)](https://www.google.com/search?q=LICENSE)**

---

## 6.0 LEGAL & COMPLIANCE

**COPYRIGHT NOTICE:** © 2026 Kyle Givler.

**DISTRIBUTION:** This software utility is distributed under open-source compliance standards. The administrator welcomes structural recommendations or features optimized for high-threat containment execution paths.

---

**[PZT-CORE: SYSTEMS ARCHITECTURE MANUAL]**
