using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using BepInEx.Configuration;

namespace CustomLevels
{

    [BepInPlugin("acr.rougetower.custommaps", "Custom Maps", "0.0.1")]
    public class MapPlugin : BaseUnityPlugin
    {
        // State
        static AllTiles SavedTiles;
        static CustomMaps CustomMaps;
        static Queue<string> currentMap;
        internal static BepInEx.Logging.ManualLogSource StaticLogger;
        static public ConfigEntry<string> configCustomMap;
        static public ConfigEntry<string> configSelectedMap;


        private void Awake()
        {
            StaticLogger = Logger;

            configCustomMap = Config.Bind("General", "Custom Map", "S, S, P", "Description of custom map, see readme for details");
            CustomMaps = new CustomMaps(configCustomMap.Value);

            var mapNames = CustomMaps.maps.Keys.ToArray();
            var AcceptableMapNames = new AcceptableValueList<string>(mapNames);
            var customMapsDescription = new ConfigDescription("What map you want to play, use \"Custom\" to play your custom map.", AcceptableMapNames);
            configSelectedMap = Config.Bind("General", "Selected Map", "Barely Legal", customMapsDescription);

            Harmony.CreateAndPatchAll(typeof(TileManagerPatch));
        }

        static public void setupMap()
        {
            var mapTiles = CustomMaps.maps[configSelectedMap.Value];
            currentMap = new Queue<string>(mapTiles);
        }

        [HarmonyPatch(typeof(TileManager))]
        public class TileManagerPatch
        {
            [HarmonyPatch("Awake")]
            [HarmonyPrefix]
            static void Awake(TileManager __instance)
            {
                setupMap();
                SaveTiles(__instance);
                ClearTiles(__instance);
            }

            [HarmonyPatch("SpawnNewTile")]
            [HarmonyPrefix]
            static void SpawnNewTilePatch(TileManager __instance)
            {

                string nextMapDirection = currentMap.Dequeue();
                var nextTiles = SavedTiles.tiles[nextMapDirection];
                __instance.deadEndTiles = nextTiles;
            }


            static void ClearTiles(TileManager __instance)
            {
                __instance.Ltiles = new GameObject[0];
                __instance.Rtiles = new GameObject[0];
                __instance.Ttiles = new GameObject[0];
                __instance.LTtiles = new GameObject[0];
                __instance.LRtiles = new GameObject[0];
                __instance.TRtiles = new GameObject[0];
                __instance.LTRtiles = new GameObject[0];
                __instance.deadEndTiles = new GameObject[0];
            }

            static void SaveTiles(TileManager __instance)
            {
                if (!SavedTiles.initialized)
                {
                    SavedTiles = new AllTiles(__instance);
                }
            }
        }


    }

    struct AllTiles
    {
        public AllTiles(TileManager tileManager)
        {
            tiles = new Dictionary<string, GameObject[]>();

            // No splits
            tiles.Add("S", tileManager.Ttiles);
            tiles.Add("L", tileManager.Ltiles);
            tiles.Add("R", tileManager.Rtiles);

            // Doube Splits
            tiles.Add("SL", tileManager.LTtiles);
            tiles.Add("SR", tileManager.TRtiles);
            tiles.Add("LR", tileManager.LRtiles);

            // Triple Split
            tiles.Add("SLR", tileManager.LTRtiles);

            // Portal
            tiles.Add("P", tileManager.deadEndTiles);

            initialized = true;
        }

        public Dictionary<string, GameObject[]> tiles;
        public bool initialized;

    }

    struct CustomMaps
    {

        public CustomMaps(string customMapCSV)
        {
            maps = new Dictionary<string, string[]>();

            maps.Add("Barely Legal", new string[] { "L", "S", "L", "S", "L", "L", "P" });
            maps.Add("Instant Portal", new string[] { "P", "P", "P" });
            maps.Add("3-way Instant Portal", new string[] { "SLR", "P", "P", "P" });
            maps.Add("5 Straights and 1 Portal", new string[] { "S", "S", "S", "S", "S", "P" });
            maps.Add("The Clock", new string[] { "L", "L", "S", "L", "S", "L", "S", "P" });
            maps.Add("1 turn 1 Portal", new string[] { "L", "P" });
            maps.Add("1 turn 3 Portals", new string[] { "L", "SLR", "P", "P", "P" });
            maps.Add("T", new string[] { "S", "S", "LR", "P", "P" });


            string sanitizedCustomMap = String.Concat(customMapCSV.Where(c => !Char.IsWhiteSpace(c)));
            maps.Add("Custom", sanitizedCustomMap.Split(','));
        }

        public Dictionary<string, string[]> maps;

    }



}
