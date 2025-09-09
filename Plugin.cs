using BepInEx;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TooMuchInfo
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Start()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        static string CheckCosmetics(VRRig rig)
        {
            string specialties = "";

            Dictionary<string, string[]> specialCosmetics = new Dictionary<string, string[]> { 
                { "LBAAD.", new string[] { "ADMINISTRATOR", "FF0000" } }, 
                { "LBAAK.", new string[] { "FOREST GUIDE", "867556" } }, 
                { "LBADE.", new string[] { "FINGER PAINTER", "00FF00" } }, 
                { "LBAGS.", new string[] { "ILLUSTRATOR", "C76417" } },
                { "LMAPY.", new string[] { "FOREST GUIDE MOD STICK", "FF8000" } },
                { "LBANI.", new string[] { "AA CREATOR BADGE", "291447" } } };
            foreach (KeyValuePair<string, string[]> specialCosmetic in specialCosmetics)
            {
                if (rig.concatStringOfCosmeticsAllowed.Contains(specialCosmetic.Key))
                    specialties += (specialties == "" ? "" : ", ") + "<color=#" + specialCosmetic.Value[1] + ">" + specialCosmetic.Value[0] + "</color>";
            }

            return specialties == "" ? null : specialties;
        }

        static string CheckMods(VRRig rig)
        {
            string specialMods = "";
            NetPlayer creator = rig.Creator;

            Dictionary<string, string[]> specialModsList = new Dictionary<string, string[]> {
                { "genesis", new string[] { "GENESIS", "07019C" } },
                { "HP_Left", new string[] { "HOLDABLEPAD", "332316" } },
                { "GrateVersion", new string[] { "GRATE", "707070" } },
                { "void", new string[] { "VOID", "FFFFFF" } },
                { "BANANAOS", new string[] { "BANANAOS", "FFFF00" } },
                { "GC", new string[] { "GORILLACRAFT", "43B581" } },
                { "CarName", new string[] { "GORILLAVEHICLES", "43B581" } },
                { "6p72ly3j85pau2g9mda6ib8px", new string[] { "CCMV2", "BF00FC" } },
                { "FPS-Nametags for Zlothy", new string[] { "FPSTAGS", "B103FC" } },
                { "cronos", new string[] { "CRONOS", "0000FF" } },
                { "ORBIT", new string[] { "ORBIT", "FFFFFF" } },
                { "Violet On Top", new string[] { "VIOLET", "DF6BFF" } },
                { "MP25", new string[] { "MONKEPHONE", "707070" } },
                { "GorillaWatch", new string[] { "GORILLAWATCH", "707070" } },
                { "InfoWatch", new string[] { "GORILLAINFOWATCH", "707070" } },
                { "BananaPhone", new string[] { "BANANAPHONE", "FFFC45" } },
                { "Vivid", new string[] { "VIVID", "F000BC" } },
                { "RGBA", new string[] { "CUSTOMCOSMETICS", "FF0000" } },
                { "cheese is gouda", new string[] { "WHOSICHEATING", "707070" } },
                { "shirtversion", new string[] { "GORILLASHIRTS", "707070" } },
                { "gpronouns", new string[] { "GORILLAPRONOUNS", "707070" } },
                { "gfaces", new string[] { "GORILLAFACES", "707070" } },
                { "monkephone", new string[] { "MONKEPHONE", "707070" } },
                { "pmversion", new string[] { "PLAYERMODELS", "707070" } },
                { "gtrials", new string[] { "GORILLATRIALS", "707070" } },
                { "msp", new string[] { "MONKESMARTPHONE", "707070" } },
                { "gorillastats", new string[] { "GORILLASTATS", "707070" } },
                { "using gorilladrift", new string[] { "GORILLADRIFT", "707070" } },
                { "monkehavocversion", new string[] { "MONKEHAVOC", "707070" } },
                { "tictactoe", new string[] { "TICTACTOE", "a89232" } },
                { "ccolor", new string[] { "INDEX", "0febff" } },
                { "imposter", new string[] { "GORILLAAMONGUS", "ff0000" } },
                { "spectapeversion", new string[] { "SPECTAPE", "707070" } },
                { "cats", new string[] { "CATS", "707070" } },
                { "made by biotest05 :3", new string[] { "DOGS", "707070" } },
                { "fys cool magic mod", new string[] { "FYSMAGICMOD", "707070" } },
                { "colour", new string[] { "CUSTOMCOSMETICS", "707070" } },
                { "chainedtogether", new string[] { "CHAINED TOGETHER", "707070" } },
                { "goofywalkversion", new string[] { "GOOFYWALK", "707070" } },
                { "void_menu_open", new string[] { "VOID", "303030" } },
                { "violetpaiduser", new string[] { "VIOLETPAID", "DF6BFF" } },
                { "violetfree", new string[] { "VIOLETFREE", "DF6BFF" } },
                { "obsidianmc", new string[] { "OBSIDIAN.LOL", "303030" } },
                { "dark", new string[] { "SHIBAGT DARK", "303030" } },
                { "hidden menu", new string[] { "HIDDEN", "707070" } },
                { "oblivionuser", new string[] { "OBLIVION", "5055d3" } },
                { "hgrehngio889584739_hugb\n", new string[] { "RESURGENCE", "470050" } },
                { "eyerock reborn", new string[] { "EYEROCK", "707070" } },
                { "asteroidlite", new string[] { "ASTEROID LITE", "707070" } },
                { "elux", new string[] { "LUNAR", "707070" } },
                { "cokecosmetics", new string[] { "COKE COSMETX", "00ff00" } },
                { "GFaces", new string[] { "gFACES", "707070" } },
                { "github.com/maroon-shadow/SimpleBoards", new string[] { "SIMPLEBOARDS", "707070" } },
                { "ObsidianMC", new string[] { "OBSIDIAN", "DC143C" } },
                { "hgrehngio889584739_hugb", new string[] { "RESURGENCE", "707070" } },
                { "GTrials", new string[] { "gTRIALS", "707070" } },
                { "github.com/ZlothY29IQ/GorillaMediaDisplay", new string[] { "GMD", "B103FC" } },
                { "github.com/ZlothY29IQ/TooMuchInfo", new string[] { "TOOMUCHINFO", "B103FC" } },
                { "github.com/ZlothY29IQ/RoomUtils-IW", new string[] { "ROOMUTILS-IW", "B103FC" } },
                { "github.com/ZlothY29IQ/MonkeClick", new string[] { "MONKECLICK", "B103FC" } },
                { "github.com/ZlothY29IQ/MonkeClick-CI", new string[] { "MONKECLICK-CI", "B103FC" } },
                { "github.com/ZlothY29IQ/MonkeRealism", new string[] { "MONKEREALISM", "B103FC" } },
                { "MediaPad", new string[] { "MEDIAPAD", "B103FC" } },
                { "GorillaCinema", new string[] { "gCINEMA", "B103FC" } },
                { "ChainedTogetherActive", new string[] { "CHAINEDTOGETHER", "B103FC" } },
                { "GPronouns", new string[] { "gPRONOUNS", "707070" } },
                { "CSVersion", new string[] { "CustomSkin", "707070" } },
                { "github.com/ZlothY29IQ/Zloth-RecRoomRig", new string[] { "ZLOTH-RRR", "B103FC" } },
                { "ShirtProperties", new string[] { "SHIRTS-OLD", "707070" } },
                { "GorillaShirts", new string[] { "SHIRTS", "707070" } },
                { "GS", new string[] { "OLD SHIRTS", "707070" } },
                { "6XpyykmrCthKhFeUfkYGxv7xnXpoe2", new string[] { "CCMV2", "DC143C" } },
                { "Body Tracking", new string[] { "BODYTRACK-OLD", "7AA11F" } },
                { "Body Estimation", new string[] { "HANBodyEst", "7AA11F" } },
                { "Gorilla Track", new string[] { "BODYTRACK", "7AA11F" } },
                { "CustomMaterial", new string[] { "CUSTOMCOSMETICS", "707070" } },
                { "I like cheese", new string[] { "RECROOMRIG", "FE8232" } },
                { "silliness", new string[] { "SILLINESS", "FFBAFF" } },
            };

            Dictionary<string, object> customProps = new Dictionary<string, object>();
            foreach (DictionaryEntry dictionaryEntry in creator.GetPlayerRef().CustomProperties)
                customProps[dictionaryEntry.Key.ToString().ToLower()] = dictionaryEntry.Value;

            foreach (KeyValuePair<string, string[]> specialMod in specialModsList)
            {
                if (customProps.ContainsKey(specialMod.Key.ToLower()))
                    specialMods += (specialMods == "" ? "" : ", ") + "<color=#" + specialMod.Value[1].ToUpper() + ">" + specialMod.Value[0].ToUpper() + "</color>";
            }

            CosmeticsController.CosmeticSet cosmeticSet = rig.cosmeticSet;
            foreach (CosmeticsController.CosmeticItem cosmetic in cosmeticSet.items)
            {
                if (!cosmetic.isNullItem && !rig.concatStringOfCosmeticsAllowed.Contains(cosmetic.itemName))
                {
                    specialMods += (specialMods == "" ? "" : ", ") + "<color=green>COSMETX</color>";
                    break;
                }
            }

            return specialMods == "" ? null : specialMods;
        }

        static Dictionary<string, string> datePool = new Dictionary<string, string> { };
        static string CreationDate(VRRig rig)
        {
            string UserId = rig.Creator.UserId;

            if (datePool.ContainsKey(UserId))
                return datePool[UserId];
            else
            {
                datePool.Add(UserId, "LOADING");
                PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = UserId }, delegate (GetAccountInfoResult result)
                {
                    string date = result.AccountInfo.Created.ToString("MMM dd, yyyy HH:mm").ToUpper();
                    datePool[UserId] = date;
                    rig.UpdateName();
                }, delegate { datePool[UserId] = "ERROR"; rig.UpdateName(); }, null, null);
                return "LOADING";
            }
        }

        static string GetFPS(VRRig rig)
        {
            Traverse fps = Traverse.Create(rig).Field("fps");

            if (fps != null)
                return "FPS " + fps.GetValue().ToString();

            return null;
        }

        static string GetTaggedPlayer(VRRig rig)
        {
            int taggedById = (int)Traverse.Create(rig).Field("taggedById").GetValue();
            NetPlayer tagger = PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(taggedById, false);

            if (tagger != null)
                return "TAGGED BY " + tagger.NickName;

            return null;
        }

        static string GetPlatform(VRRig rig)
        {
            string concatStringOfCosmeticsAllowed = rig.concatStringOfCosmeticsAllowed;

            if (concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                return "STEAM";
            else if (concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") || rig.Creator.GetPlayerRef().CustomProperties.Count >= 2)
                return "PC";

            return "STANDALONE";
        }

        static string GetTurnSettings(VRRig rig)
        {
            Traverse turnType = Traverse.Create(rig).Field("turnType");
            Traverse turnFactor = Traverse.Create(rig).Field("turnFactor");

            if (turnType != null && turnFactor != null)
            {
                string turnTypeValue = (string)turnType.GetValue();
                return turnTypeValue == "NONE" ? "NONE" : turnTypeValue + " " + turnFactor.GetValue();
            }

            return null;
        }

        static string FormatColor(Color color)
        {
            return "COLOR <color=red>" + Math.Round(color.r * 255).ToString() + 
                   "</color> <color=green>" + Math.Round(color.g * 255).ToString() + 
                   "</color> <color=blue>" + Math.Round(color.b * 255).ToString() + "</color>";
        }

        public static void UpdateName(VRRig rig)
        {
            try
            {
                string targetText = "Name";
                NetPlayer creator = rig.Creator;

                if (creator != null)
                {
                    List<string> lines = new List<string>
                    {
                        "",
                        "",
                        "",
                        creator.NickName,
                        "ID " + creator.UserId
                    };

                    string creation = CreationDate(rig);
                    if (creation != null) lines.Add(creation);

                    string color = FormatColor(rig.playerColor);
                    if (color != null) lines.Add(color);

                    string platform = GetPlatform(rig);
                    if (platform != null) lines.Add(platform);

                    string cosmetics = CheckCosmetics(rig);
                    if (cosmetics != null) lines.Add(cosmetics);

                    string mods = CheckMods(rig);
                    if (mods != null) lines.Add("MODS " + mods);

                    string tagged = GetTaggedPlayer(rig);
                    if (tagged != null) lines.Add(tagged);

                    string fps = GetFPS(rig);
                    if (fps != null) lines.Add(fps);

                    string turnSettings = GetTurnSettings(rig);
                    if (turnSettings != null) lines.Add(turnSettings);

                    targetText = string.Join("\n", lines);
                }

                Regex noRichText = new Regex("<.*?>");
                rig.playerText1.text = targetText;
            } catch { }
        }
    }
}
