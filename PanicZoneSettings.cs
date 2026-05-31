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

using UnityEngine;
using Verse;

namespace PanicZoneToggler
{
    public class PanicZoneSettings : ModSettings
    {
        public string TargetArea = "Panic";
        public bool ShowButton = true;
        public bool PanicHumans = true;
        public bool PanicMechs = true;
        public bool PanicAnimals = true;

        public string Version
        {
            get
            {
                return "0.0.3";
            }
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("Panic Zone Toggler Settings", 24f);
            listingStandard.Gap(12f);

            listingStandard.Label("Panic Area Name: ");
            TargetArea = listingStandard.TextEntry(TargetArea);
            listingStandard.Gap(4f);
            listingStandard.Label("<color=gray>Panic Zone Toggler will search for a zone matching this exact name. If not found, it falls back to the 'Home' zone.</color>");
            listingStandard.Gap(12f);

            listingStandard.CheckboxLabeled("Assign Humanlike", ref PanicHumans);
            listingStandard.Gap(12f);
            listingStandard.CheckboxLabeled("Assign Mechs", ref PanicMechs);
            listingStandard.Gap(12f);
            listingStandard.CheckboxLabeled("Assign Animals", ref PanicAnimals);
            listingStandard.Gap(12f);

            listingStandard.CheckboxLabeled("Show Panic button", ref ShowButton);

            listingStandard.End();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref TargetArea, "targetZoneName", "Panic");
            Scribe_Values.Look(ref ShowButton, "showButton", true);
            Scribe_Values.Look(ref PanicHumans, "panicHumans", true);
            Scribe_Values.Look(ref PanicAnimals, "panicAnimals", true);
            Scribe_Values.Look(ref PanicMechs, "panicMechs", true);
        }
    }
}
