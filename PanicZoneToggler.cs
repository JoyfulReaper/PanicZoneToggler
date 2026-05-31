/**
Copyright 2026 Kyle Givler

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS “AS IS” AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 **/

using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace PanicZoneToggler
{
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
            Area targetArea = map.areaManager.GetLabeled(PanicZoneMod.Settings.TargetArea);
            if (targetArea == null)
            {
                Messages.Message($"Unable to locate Area: {PanicZoneMod.Settings.TargetArea}", MessageTypeDefOf.CautionInput);
                targetArea = map.areaManager.GetLabeled("Home");
                if (targetArea == null)
                {
                    Verse.Log.Error("<color=red>[PanicZoneToggler] Unable to locate valid Panic Area!");
                    return;
                }
            }

            PreviousRestrictions.Clear();
            List<Pawn> allColonyPawns = map.mapPawns.PawnsInFaction(Faction.OfPlayer);

            bool panicHumans = PanicZoneMod.Settings.PanicHumans;
            bool panicMechs = PanicZoneMod.Settings.PanicMechs;
            bool panicAnimals = PanicZoneMod.Settings.PanicAnimals;

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
            List<Pawn> allColonyPawns = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
            foreach (Pawn pawn in allColonyPawns)
            {
                if (pawn.playerSettings != null)
                {
                    if (PreviousRestrictions.ContainsKey(pawn))
                    {
                        if (PreviousRestrictions.TryGetValue(pawn, out Area oldArea))
                        {
                            pawn.playerSettings.AreaRestrictionInPawnCurrentMap = oldArea;
                        }
                        else
                        {
                            pawn.playerSettings.AreaRestrictionInPawnCurrentMap = null;
                        }
                    }
                }
            }

            PreviousRestrictions.Clear();
            Messages.Message($"PanicZoneToggler: Pawns set to original Area", MessageTypeDefOf.CautionInput);
        }

        public override void MapComponentOnGUI()
        {
            if (!PanicZoneMod.Settings.ShowButton)
                return;

            base.MapComponentOnGUI();

            if (Find.CurrentMap != this.map || WorldRendererUtility.WorldRendered)
                return;

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
}
