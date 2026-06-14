/**
BSD 2-Clause License

Copyright (c) 2026, Kyle Givler

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 **/

using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Event = UnityEngine.Event;
using GUI = UnityEngine.GUI;

namespace PanicZoneToggler;

public class MapComponent_PanicToggle : MapComponent
{
    public bool IsPanicActive = false;
    private Dictionary<Pawn, Area> PreviousRestrictions = new Dictionary<Pawn, Area>();

    public MapComponent_PanicToggle(Map map) : base(map) { }

    public void TogglePanicZone()
    {
        IsPanicActive = !IsPanicActive;

        if (IsPanicActive)
        {
            EnablePanicZone();
        }
        else
        {
            DisablePanicZone();
        }
    }

    private void EnablePanicZone()
    {
        Area targetArea = map.areaManager.GetLabeled(PanicZoneTogglerMod.Settings.TargetArea);
        if (targetArea == null)
        {
            Messages.Message($"Unable to locate Area: {PanicZoneTogglerMod.Settings.TargetArea}", MessageTypeDefOf.CautionInput);
            targetArea = map.areaManager.GetLabeled("Home");
            if (targetArea == null)
            {
                Verse.Log.Error("<color=red>[PanicZoneToggler] Unable to locate valid Panic Area!");
                return;
            }
        }

        PreviousRestrictions.Clear();
        List<Pawn> allColonyPawns = map.mapPawns.PawnsInFaction(Faction.OfPlayer);

        bool panicHumans = PanicZoneTogglerMod.Settings.PanicHumans;
        bool panicMechs = PanicZoneTogglerMod.Settings.PanicMechs;
        bool panicAnimals = PanicZoneTogglerMod.Settings.PanicAnimals;

        foreach (Pawn pawn in allColonyPawns)
        {
            if ((pawn.RaceProps.Humanlike && panicHumans) ||
                (pawn.IsColonyMech && panicMechs) ||
                (pawn.RaceProps.Animal && panicAnimals))
            {
                if (pawn.playerSettings != null)
                {
                    PreviousRestrictions[pawn] = pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
                    pawn.playerSettings.AreaRestrictionInPawnCurrentMap = targetArea;
                }
            }
        }
        Messages.Message($"PanicZoneToggler: Pawns forced to Area: {targetArea.Label}", MessageTypeDefOf.CautionInput);
    }

    private void DisablePanicZone()
    {
        foreach (var kvp in PreviousRestrictions)
        {
            Pawn pawn = kvp.Key;
            Area oldArea = kvp.Value;

            if (pawn != null && pawn.playerSettings != null)
            {
                pawn.playerSettings.AreaRestrictionInPawnCurrentMap = oldArea;
            }
        }

        PreviousRestrictions.Clear();
        Messages.Message($"PanicZoneToggler: Pawns set to original Area", MessageTypeDefOf.CautionInput);
    }

    public override void MapComponentOnGUI()
    {
        if (Find.CurrentMap != this.map || WorldRendererUtility.WorldRendered)
            return;

        if (PanicZoneKeyBindingDefOf.PanicZone_Toggle.KeyDownEvent)
        {
            // Don't trigger if the user is typing in a search bar or text input box
            if (GUI.GetNameOfFocusedControl().NullOrEmpty())
            {
                TogglePanicZone();
                Event.current.Use();
            }
        }

        if (!PanicZoneTogglerMod.Settings.ShowButton)
            return;

        base.MapComponentOnGUI();

        Rect buttonRect = new Rect(UI.screenWidth - 175, UI.screenHeight - 125, 45, 35);
        string buttonText = IsPanicActive ? "<color=green>Safe</color>" : "<color=red>Panic</color>";
        if (Widgets.ButtonText(buttonRect, buttonText, true, true, true))
        {
            TogglePanicZone();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref IsPanicActive, "IsPanicActive", false);

        if (Scribe.mode == LoadSaveMode.Saving)
        {
            PreviousRestrictions.RemoveAll((KeyValuePair<Pawn, Area> kvp) => kvp.Key == null || kvp.Key.Destroyed);
        }

        Scribe_Collections.Look<Pawn, Area>(ref PreviousRestrictions, "PreviousRestrictions", LookMode.Reference, LookMode.Reference);

        if (Scribe.mode == LoadSaveMode.PostLoadInit && PreviousRestrictions == null)
        {
            PreviousRestrictions = new Dictionary<Pawn, Area>();
        }
    }
}
