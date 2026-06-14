# Panic Zone Toggler

An emergency response utility that instantly forces all player-controlled pawns, mechanoids, and animals into a designated "Panic" allowed area. No more pausing the game to manually reassign dozens of individual restriction zones while a massive raid breaches your walls.

## 🚨 Key Features

* **Native Hotkey Support:** Hit **'P'** (fully rebindable via the native RimWorld Options -> Controls menu) to instantly trigger or deactivate the alert.
* **The Panic Button:** Adds an emergency toggle right next to the game speed controls. One click locks down your colony; clicking it again restores everyone to their exact previous allowed zones.
* **Granular Control Toggles:** Choose exactly who responds to the panic alarm in the settings menu. Force any combination of Humans, Mechanoids, or Animals into safety while leaving others to their normal routines.
* **Bulletproof Fallback:** Looks for an allowed area named "Panic" by default (customizable). If the designated area doesn't exist on a map, it automatically falls back to the **Home Zone** to keep your colonists safe.

## 🛠️ Technical Details

* **Zero Performance Impact:** This mod contains no active tick loops. It consumes **0.000ms** of CPU time while idle, making it perfectly safe for massive, 500+ mod load orders.
* **Caravan Safe:** Cleanly preserves and restores the restriction states of pawns even if they leave the map or form caravans while the panic state is active.

_Open to feature recommendations :)_

## 🌐 Open Source & Contributions

* **GitHub:** [PanicZoneToggler Repository](https://github.com/JoyfulReaper/PanicZoneToggler)
* **License:** [BSD 2-Clause License](LICENSE)
