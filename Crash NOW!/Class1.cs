using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;
using HugsLib;
using Harmony;

namespace CrashNOW
{

    public class main : ModBase
    {
        public override string ModIdentifier
        {
            get
            {
                return "CrashNOW";
            }
        }

        public static bool BaseCanDoNext()
        {
            return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
        }

        public static bool CanDoNext()
        {
            if (!BaseCanDoNext())
            {
                return false;
            }
            int selectedTile = Find.WorldInterface.SelectedTile;
            if (selectedTile < 0)
            {
                Messages.Message("MustSelectLandingSite".Translate(), MessageSound.RejectInput);
                return false;
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (!TileFinder.IsValidTileForNewSettlement(selectedTile, stringBuilder))
            {
                Messages.Message(stringBuilder.ToString(), MessageSound.RejectInput);
                return false;
            }
            Tile tile = Find.WorldGrid[selectedTile];
            return TutorSystem.AllowAction("ChooseBiome-" + tile.biome.defName + "-" + tile.hilliness.ToString());
        }

        // orginal method: public override void DoWindowContents(Rect inRect)
        [HarmonyPatch(typeof(Page_SelectLandingSite), "DoCustomBottomButtons")]
        public static class Page_SelectLandingSite_DoCustomBottomButtons_Patch
        {
            [HarmonyPostfix]
            public static void DrawCrashNowButtons(Page_SelectLandingSite __instance)
            {
                var buttonSize = new Vector2(150f, 80f);

                Widgets.DrawWindowBackground(new Rect(40, 70, 330, 100));

                if (Widgets.ButtonText(new Rect(50, 80, buttonSize.x, buttonSize.y), "Random Pawns\nCurrent Location", true, false, true) && CanDoNext())
                {
                    // tells the game what starting tile was selected
                    Find.GameInitData.startingTile = Find.WorldInterface.SelectedTile;

                    // goes to next page (pawn generation screen)

                    Page next = __instance.next;

                    if (__instance.next != null)
                    {
                        Find.WindowStack.Add(next);

                    }
                    if (__instance.nextAct != null)
                    {
                        next.nextAct();
                    }
                    TutorSystem.Notify_Event("PageClosed");
                    TutorSystem.Notify_Event("GoToNextPage");

                    __instance.Close(true);

                    // randomize the pawns (in case they went in and played with them and then went back to the location select screen)
                    // ...or not, just going to the next page again for now, might fix later

                    if (next.next != null)
                    {
                        Find.WindowStack.Add(next.next);
                    }
                    if (next.nextAct != null)
                    {
                        next.nextAct();
                    }
                    TutorSystem.Notify_Event("PageClosed");
                    TutorSystem.Notify_Event("GoToNextPage");
                    next.Close(true);
                }
                if (Widgets.ButtonText(new Rect(210, 80, buttonSize.x, buttonSize.y), "Random Pawns\nRandom Location", true, false, true))
                {
                    // gets random location
                    Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
                    Find.GameInitData.startingTile = Find.WorldInterface.SelectedTile;

                    // goes to next page (pawn generation screen)

                    Page next = __instance.next;

                    if (__instance.next != null)
                    {
                        Find.WindowStack.Add(next);

                    }
                    if (__instance.nextAct != null)
                    {
                        next.nextAct();
                    }
                    TutorSystem.Notify_Event("PageClosed");
                    TutorSystem.Notify_Event("GoToNextPage");

                    __instance.Close(true);

                    // randomize the pawns (in case they went in and played with them and then went back to the location select screen)
                    // ...or not, just going to the next page again for now, might fix later

                    if (next.next != null)
                    {
                        Find.WindowStack.Add(next.next);
                    }
                    if (next.nextAct != null)
                    {
                        next.nextAct();
                    }
                    TutorSystem.Notify_Event("PageClosed");
                    TutorSystem.Notify_Event("GoToNextPage");
                    next.Close(true);


                }


            }
        }
    }
}
