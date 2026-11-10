using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ArcherStandIns
{
    // For players who do NOT have Vikings Archer: lets them see the bows and arrows of players who do.
    // A bow or arrow from that mod has a name this game doesn't know, so it shows nothing. Here every such
    // name gets a stand-in built from a game bow or arrow of a similar tier. Nothing from Vikings Archer
    // is copied: only its item names are used. With Vikings Archer installed, this mod does nothing.
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "felih.ArcherStandIns";
        public const string NAME = "Archer Stand-ins";
        public const string VERSION = "0.1.0";

        internal static ManualLogSource Log;
        internal static ConfigEntry<bool> Enabled, DetailedLog;
        internal static ConfigEntry<string> Bows, Arrows;

        private void Awake()
        {
            Log = Logger;
            Enabled = Config.Bind("1 - General", "Enabled", true,
                "Show stand-in bows and arrows for players who use Vikings Archer. Does nothing when you have Vikings Archer yourself.");
            Bows = Config.Bind("2 - Stand-ins", "Bows",
                "RDA_bow_meadows=Bow, RDA_bow_bforest=BowFineWood, RDA_bow_swamp=BowHuntsman, RDA_bow_mountain=BowDraugrFang, " +
                "RDA_bow_plains=BowDraugrFang, RDA_bow_mistlands=BowSpineSnap, RDA_bow_ashlands=BowAshlands",
                "Which game bow stands in for each Vikings Archer bow (its Barrage version uses the same one). If a game bow is missing, the crude Bow is used.");
            Arrows = Config.Bind("2 - Stand-ins", "Arrows",
                "bforest=ArrowBronze, swamp=ArrowIron, mountain=ArrowSilver, plains=ArrowNeedle, mistlands=ArrowCarapace, ashlands=ArrowCharred",
                "Which game arrow stands in for each biome's Vikings Archer arrows in flight (blunt, pierce and slash alike).");
            DetailedLog = Config.Bind("9 - Debug", "DetailedLog", false, "Writes details to the BepInEx log.");
            new Harmony(GUID).PatchAll(typeof(Plugin).Assembly);
            Log.LogInfo(NAME + " " + VERSION + " loaded");
        }

        internal static void Debug(string s) { if (DetailedLog.Value) Log.LogInfo(s); }
    }

    [HarmonyPatch]
    internal static class StandIns
    {
        private static readonly FieldInfo ItemByHash = AccessTools.Field(typeof(ObjectDB), "m_itemByHash");
        private static readonly FieldInfo NamedPrefabs = AccessTools.Field(typeof(ZNetScene), "m_namedPrefabs");
        private static readonly string[] Kinds = { "blunt", "pierce", "slash" };

        private static GameObject _holder;
        private static bool? _hasVikingsArcher;

        // Vikings Archer present? Then the real items exist and nothing here may be added.
        private static bool Active()
        {
            if (!Plugin.Enabled.Value) return false;
            if (_hasVikingsArcher == null)
            {
                bool found = false;
                foreach (string id in Chainloader.PluginInfos.Keys)
                    if (id.IndexOf("Vikings_Archer", System.StringComparison.OrdinalIgnoreCase) >= 0) { found = true; break; }
                _hasVikingsArcher = found;
                Plugin.Log.LogInfo(found ? "Vikings Archer is installed: stand-ins are off." : "Vikings Archer is not installed: stand-ins are on.");
            }
            return !_hasVikingsArcher.Value;
        }

        // Clones sit under an inactive object, so their scripts never wake up as templates.
        private static GameObject Clone(GameObject source, string name)
        {
            if (_holder == null)
            {
                _holder = new GameObject("ArcherStandIns_Templates");
                _holder.SetActive(false);
                Object.DontDestroyOnLoad(_holder);
            }
            Transform old = _holder.transform.Find(name);
            if (old != null) return old.gameObject;
            GameObject copy = Object.Instantiate(source, _holder.transform, false);
            copy.name = name;
            return copy;
        }

        private static List<KeyValuePair<string, string>> Pairs(string text)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (string part in (text ?? "").Split(','))
            {
                string[] kv = part.Split('=');
                if (kv.Length == 2 && kv[0].Trim().Length > 0 && kv[1].Trim().Length > 0)
                    list.Add(new KeyValuePair<string, string>(kv[0].Trim(), kv[1].Trim()));
            }
            return list;
        }

        // ------------------------------------------------------------ bows: what others hold in their hands

        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        [HarmonyPostfix]
        private static void ObjectDBAwake(ObjectDB __instance) { AddBows(__instance); }

        [HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
        [HarmonyPostfix]
        private static void ObjectDBCopy(ObjectDB __instance) { AddBows(__instance); }

        private static void AddBows(ObjectDB db)
        {
            if (db == null || db.m_items == null || db.m_items.Count == 0 || !Active()) return;
            Dictionary<int, GameObject> byHash = ItemByHash != null ? ItemByHash.GetValue(db) as Dictionary<int, GameObject> : null;
            int added = 0;
            foreach (KeyValuePair<string, string> pair in Pairs(Plugin.Bows.Value))
            {
                GameObject source = db.GetItemPrefab(pair.Value) ?? db.GetItemPrefab("Bow");
                if (source == null) continue;
                foreach (string name in new[] { pair.Key, pair.Key + "_barrage" })
                {
                    int hash = name.GetStableHashCode();
                    if (db.GetItemPrefab(hash) != null) continue;
                    GameObject stand = Clone(source, name);
                    db.m_items.Add(stand);
                    if (byHash != null) byHash[hash] = stand;
                    added++;
                    Plugin.Debug("bow " + name + " shows as " + source.name);
                }
            }
            if (added > 0) Plugin.Log.LogInfo("Added " + added + " stand-in bows.");
        }

        // ------------------------------------------------------------ arrows: what flies through the air

        [HarmonyPatch(typeof(ZNetScene), "Awake")]
        [HarmonyPostfix]
        private static void ZNetSceneAwake(ZNetScene __instance)
        {
            if (!Active()) return;
            Dictionary<int, GameObject> named = NamedPrefabs != null ? NamedPrefabs.GetValue(__instance) as Dictionary<int, GameObject> : null;
            if (named == null) { Plugin.Log.LogWarning("Could not reach the prefab list; arrows stay hidden."); return; }
            int added = 0;
            foreach (KeyValuePair<string, string> pair in Pairs(Plugin.Arrows.Value))
            {
                GameObject projectile = ArrowProjectile(__instance, pair.Value);
                if (projectile == null) { Plugin.Log.LogWarning("No game arrow called " + pair.Value + "; " + pair.Key + " arrows stay hidden."); continue; }
                foreach (string kind in Kinds)
                {
                    string name = "bow_projectile_" + pair.Key + "_" + kind;
                    int hash = name.GetStableHashCode();
                    if (named.ContainsKey(hash)) continue;
                    GameObject stand = Clone(projectile, name);
                    __instance.m_prefabs.Add(stand);
                    named[hash] = stand;
                    added++;
                    Plugin.Debug("arrow " + name + " shows as " + projectile.name);
                }
            }
            if (added > 0) Plugin.Log.LogInfo("Added " + added + " stand-in arrows.");
        }

        private static GameObject ArrowProjectile(ZNetScene scene, string arrowItem)
        {
            GameObject item = scene.GetPrefab(arrowItem);
            ItemDrop drop = item != null ? item.GetComponent<ItemDrop>() : null;
            if (drop == null) return null;
            return drop.m_itemData.m_shared.m_attack.m_attackProjectile;
        }
    }
}
