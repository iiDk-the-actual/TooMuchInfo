using BepInEx;
using Photon.Realtime;
using PlayFab.ClientModels;
using PlayFab;
using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using System.Text.RegularExpressions;
using GorillaNetworking;
using Photon.Pun;
using System.IO;
using System.Linq;

namespace TooMuchInfo

{

    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]


    public class Plugin : BaseUnityPlugin

    {
        static HashSet<string> detectedCheaters = new HashSet<string>();

        static Dictionary<string, string> specialPlayers = new Dictionary<string, string>
{
    { "9DBC90CF7449EF64", "StyledSnail" },
    { "6FE5FF4D5DF68843", "Pine" },
    { "52529F0635BE0CDF", "PapaSmurf" },
    { "10D31D3BDCCE5B1F", "Deezey" },
    { "BAC5807405123060", "britishmonke" },
    { "A6FFC7318E1301AF", "jmancurl" },
    { "3B9FD2EEF24ACB3", "VMT" },
    { "04005517920EBO", "K9?" },
    { "33FFA45DBFD33B01", "will" },
    { "D6971CA01F82A975", "Elliot" },
    { "636D8846E76C9B5A", "Clown" },
    { "65CB0CCF1AED2BF", "Ethyb" },
    { "48437FE432DE48BE", "BBVR" },
    { "61AD990FF3A423B7", "Boda 1" },
    { "AAB44BFD0BA34829", "Boda 2" },
    { "6713DA80D2E9BFB5", "AHauntedArmy" },
    { "B4A3FF01312B55B1", "Pluto" },
    { "E354E818871BD1D8", "developer9998" },
    { "FBE3EE50747CB892", "Lunakitty/Gizmo" },
    { "339E0D392565DC39", "kishark" },
    { "F08CE3118F9E793E", "TurboAlligator" },
    { "D6E20BE9655C798", "TTTPIG 1" },
    { "71AA09D13C0F408D", "TTTPIG 2" },
    { "1D6E20BE9655C798", "TTTPIG 3" },
    { "22A7BCEFFD7A0BBA", "TTTPIG 4" },
    { "C3878B068886F6C3", "ZZEN" },
    { "6F79BE7CB34642AC", "CodyO'Quinn" },
    { "5AA1231973BE8A62", "Apollo" },
    { "7F31BEEC604AE189", "ElectronicWall 1" },
    { "42C809327652ECDD", "ElectronicWall 2" },
    { "ECDE8A2FF8510934", "Antoca" },
    { "80279945E7D3B57D", "Jolyne" },
    { "7E44E8337DF02CC1", "Nunya" },
    { "DE601BC40DB68CE0", "Graic" },
    { "F5B5C64914C13B83", "HatGirl" },
    { "660814E013F31EFA", "HOLLOWZZGT" },
    { "2E408ED946D55D51", "Haunted" },
    { "D345FE394607F946", "Bzzz the 18th" },
    { "498D4C2F23853B37", "POGTROLL" },
    { "BC9764E1EADF8BE0", "Circuit" },
    { "D0CB396539676DD8", "FrogIlla" },
    { "A1A99D33645E4A94", "STEAMVRAVTS / YEAT" },
    { "CA8FDFF42B7A1836", "Brokenstone" },
    { "CBCCBBB6C28A94CF", "PTMstar" },
    { "6DC06EEFFE9DBD39", "Lucio" },
    { "4ACA3C76B334B17F", "Wihz" },
    { "41988726285E534E", "Colussus" },
    { "571776944B6162F1", "CubCub" },
    { "FB5FCEBC4A0E0387", "PepsiDee" },
    { "645222265FB972B", "Chaotic Asriel" },
    { "BC99FA914F506AB8", "Lemming 1" },
    { "3A16560CA65A51DE", "Lemming 2" },
    { "59F3FE769DE93AB9", "Lemming 3" },
    { "EE9FB127CF7DBBD5", "NOTMARK" },
    { "54DCB69545BE0800", "Biffbish" },
    { "A04005517920EB0", "K9" },
    { "3CB4F61C87A5AF24", "Octoburr/Evelyn" },
    { "4994748F8B361E31", "Octoburr/Evelyn" },
    { "5CCCAA8A225A468B", "furina" },
    { "ABD60175B46E45C5", "Saltwater" },
    { "964C4A68F65A804C", "YottaBite" },
    { "8FECBBC89D69575E", "KyleTheScientist" },
    { "4D5EB238C8253D04", "Person" },
    { "B4E45E48C5CE0656", "ZBR" },
    { "A100E9E6C4D91E75", "MYCRAFTS" },
    { "7952F9E08FEF8E83", "MYCRAFTS" },
    { "10E12F25533C13F2", "KIRPI4" }
};
        void Start()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
            {
                {
                    PluginInfo.HashKey, PluginInfo.Version
                }
            });
        }

        static string CheckCosmetics(VRRig rig)
        {
            string specialties = "";

            Dictionary<string, string[]> specialCosmetics = new Dictionary<string, string[]> {
                { "LBAAD.", new string[] { "ADMIN", "FF0000" } },
                { "LBAAK.", new string[] { "STICK", "867556" } },
                { "LBADE.", new string[] { "FINGER PAINTER", "00FF00" } },
                { "LBAGS.", new string[] { "ILLUSTRATOR", "C76417" } },
                { "LMAPY.", new string[] { "FIRE STICK", "FF8000" } } };
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
        {"GFaces", new string[] {"FACES" , "707070"  } },
        {"ObsidianMC", new string[] {"OBSIDIAN" , "DC143C" } },
        {"hgrehngio889584739_hugb", new string[] {"RESURGENCE" , "707070" } },
        {"GTrials", new string[] {"TRIALS","707070" } },
        { "github.com/ZlothY29IQ/GorillaMediaDisplay", new string[] { "GMD", "B103FC" } },
        { "github.com/ZlothY29IQ/TooMuchInfo", new string[] { "TOOMUCHINFO", "B103FC" } },
        { "github.com/ZlothY29IQ/RoomUtils-IW", new string[] { "ROOMUTILS-IW", "B103FC" } },
        { "github.com/ZlothY29IQ/MonkeClick", new string[] { "MONKECLICK", "B103FC" } },
        { "GPronouns", new string[] { "PRONOUNS", "707070" } },
        { "CSVersion", new string[] {"CustomSkin", "707070"} },
        { "ЗлотhРРР", new string[] {"ZlothRRR", "B103FC" } },
        { "github.com/ZlothY29IQ/Zloth-RecRoomRig", new string[] {"ZLOTH-RRR", "B103FC" } },
        { "ShirtProperties", new string[] { "SHIRTS", "707070" } },
        { "WhoIsThatMonke", new string[] { "WhoIsThatMonke", "707070" } },
        { "GS", new string[] { "OLD SHIRTS", "707070" } },
        { "genesis", new string[] { "GENESIS", "DC143C" } },
        { "HP_Left", new string[] { "HOLDABLEPAD", "B103FC" } },
        { "GrateVersion", new string[] { "GRATE", "707070" } },
        { "void", new string[] { "VOID", "DC143C" } },
        { "BananaOS", new string[] { "BANANAOS", "FFFF00" } },
        { "GC", new string[] { "GORILLACRAFT", "43B581" } },
        { "CarName", new string[] { "VEHICLES", "43B581" } },
        { "6XpyykmrCthKhFeUfkYGxv7xnXpoe2", new string[] { "CCMV2", "DC143C" } },
        { "cronos", new string[] { "CRONOS", "DC143C" } },
        { "ORBIT", new string[] { "ORBIT", "DC143C" } },
        { "Violet On Top", new string[] { "VIOLET", "DC143C" } },
        { "MonkePhone", new string[] { "MONKEPHONE", "7AA11F" } },
        { "GorillaWatch", new string[] { "GORILLAWATCH", "707070" } },
        { "InfoWatch", new string[] { "INFOWATCH", "707070" } },
        { "BananaPhone", new string[] { "BANANAPHONE", "FFFC45" } },
        { "Vivid", new string[] { "VIVID", "DC143C" } },
        { "CustomMaterial", new string[] { "CUSTOMCOSMETICS", "FF0000" } },
        { "cheese is gouda", new string[] { "WHOSICHEATING", "707070" } },
        { "I like cheese", new string[] { "RECROOMRIG", "FE8232" } } };

            foreach (KeyValuePair<string, string[]> specialMod in specialModsList)
            {
                if (creator.GetPlayerRef().CustomProperties.ContainsKey(specialMod.Key))
                    specialMods += (specialMods == "" ? "" : ", ") + "<color=#" + specialMod.Value[1] + ">" + specialMod.Value[0] + "</color>";
            }

            CosmeticsController.CosmeticSet cosmeticSet = rig.cosmeticSet;
            foreach (CosmeticsController.CosmeticItem cosmetic in cosmeticSet.items)
            {
                if (!cosmetic.isNullItem && !rig.concatStringOfCosmeticsAllowed.Contains(cosmetic.itemName))
                {
                    specialMods += (specialMods == "" ? "" : ", ") + "<color=red>COSMETX</color>";
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

        static string GetSpecialPlayerName(VRRig rig)
        {
            string userId = rig.Creator.UserId;
            if (specialPlayers.ContainsKey(userId))
            {
                return $"<color=blue>{specialPlayers[userId]}</color>";
            }
            return null;
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

            return "QUEST";
        }

        static void LogUnknownSpecialCosmetic(string userId, string nickname, List<string> cosmeticIds)
        {
            string path = Path.Combine(Paths.BepInExRootPath, "SpecialCosmeticsUnknownUserID.txt");
            string idLinePrefix = $"UserId={userId}, Nickname={nickname}";

            if (File.Exists(path))
            {
                string[] existingLines = File.ReadAllLines(path);

                foreach (string existingLine in existingLines)
                {
                    if (existingLine.Contains(idLinePrefix))
                        return;
                }
            }

            string line = $"{DateTime.Now}: {idLinePrefix}, Cosmetics=[{string.Join(", ", cosmeticIds)}]";
            File.AppendAllText(path, line + Environment.NewLine);
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

        static int CountTotalCosmetics(VRRig rig)
        {
            if (string.IsNullOrEmpty(rig.concatStringOfCosmeticsAllowed))
                return 0;

            return rig.concatStringOfCosmeticsAllowed.Count(c => c == '.');
        }


        static string FormatColor(Color color)
        {
            return "<color=red>" + Math.Round(color.r * 9).ToString() +
                   "</color> <color=green>" + Math.Round(color.g * 9).ToString() +
                   "</color> <color=blue>" + Math.Round(color.b * 9).ToString() + "</color>";
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
                        //"ID " + creator.UserId
                    };

                    string specialName = GetSpecialPlayerName(rig);
                    if (specialName != null) lines.Add($"<color=purple>{specialName}</color>");

                    string cosmetics = CheckCosmetics(rig);
                    if (cosmetics != null)
                    {
                        lines.Add(cosmetics);

                        List<string> foundSpecialCosmetics = new List<string>();
                        string[] specialPrefixes = new string[] { "LBADE.", "LBAAK.", "LBAAD.", "LBAGS.", "LMAPY." };

                        foreach (string prefix in specialPrefixes)
                        {
                            foreach (string cosmeticId in rig.concatStringOfCosmeticsAllowed.Split(','))
                            {
                                if (cosmeticId.StartsWith(prefix))
                                {
                                    foundSpecialCosmetics.Add(cosmeticId);
                                }
                            }
                        }

                        if (foundSpecialCosmetics.Count > 0 && !specialPlayers.ContainsKey(rig.Creator.UserId))
                        {
                            LogUnknownSpecialCosmetic(rig.Creator.UserId, rig.Creator.NickName, foundSpecialCosmetics);
                        }
                    }

                    
                    

                    string mods = CheckMods(rig);
                    if (mods != null) lines.Add(mods);

                    string creation = CreationDate(rig);
                    if (creation != null) lines.Add(creation);

                    string color = FormatColor(rig.playerColor);
                    if (color != null) lines.Add(color);

                    string turnSettings = GetTurnSettings(rig);
                    if (turnSettings != null) lines.Add(turnSettings);

                    string platform = GetPlatform(rig);
                    if (platform != null) lines.Add(platform);

                    int totalCosmetics = CountTotalCosmetics(rig);
                    if (totalCosmetics > 0)
                    {
                        lines.Add("Cosmetics: " + totalCosmetics.ToString());
                    }



                    targetText = string.Join("\n", lines);
                }

                Regex noRichText = new Regex("<.*?>");
                rig.playerText1.text = targetText;
                rig.playerText2.text = noRichText.Replace(targetText, "");
            }
            catch { }
        }
    }
}