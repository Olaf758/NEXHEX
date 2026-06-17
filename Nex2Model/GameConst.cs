using Avalonia;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using Avalonia.Platform;

namespace Nexomon2Model
{
    public static class ItemsConsts
    {
        private static Dictionary<string, int>? itemsdic;
        private static Dictionary<int, string>? itemsdicinverted;
        private static Dictionary<string, List<Item.ItemBase>>? itemspercategory;
        private static Dictionary<string, int>? coresdic;
        private static Dictionary<int, Item.ItemBase>? coresbuiltdic;
        public static List<string> ItemsList => ItemsDic.Keys.ToList();
        public static List<int> ItemsIdList => ItemsDic.Values.ToList();
        public static List<string> CoresList => CoresDic.Keys.ToList();
        public static List<int> CoresIdList => CoresDic.Values.ToList();
        public static ObservableCollection<Item.ItemBase>? coreslistbuilt;

        public static Dictionary<string, int> ItemsDic
        {
            get
            {
                if (itemsdic == null)
                {
                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/items.json");  // ← nom du projet exact !
                    using var stream = AssetLoader.Open(jsonUri);
                    string json = (new StreamReader(stream).ReadToEndAsync()).Result;
                    JsonDocument ItemsIdJson = JsonDocument.Parse(json);
                    itemsdic = new Dictionary<string, int>();
                    foreach (JsonProperty property in ItemsIdJson.RootElement.EnumerateObject())
                    {
                        string serstring = property.Value.GetProperty("serialization").ToString();
                        int ser = int.Parse(serstring);
                        itemsdic.Add(property.Name, ser);
                    }
                }
                return itemsdic;
            }
        }
        public static Dictionary<string, int> CoresDic
        {
            get
            {
                if(coresdic == null)
                {
                    coresdic = new Dictionary<string, int>();
                    foreach(Item.ItemBase item in ItemsPerCategory["core"])
                    {
                        coresdic.Add(item.Name, item.Id);
                    }
                }
                return coresdic;
            }
        }
        public static Dictionary<int, Item.ItemBase> CoresBuiltDic
        {
            get
            {
                if (coresbuiltdic == null)
                {
                    coresbuiltdic = new Dictionary<int, Item.ItemBase>();
                    foreach (Item.ItemBase item in ItemsPerCategory["core"])
                    {
                        coresbuiltdic.Add(item.Id, new Item.ItemBase(item.Id));
                    }
                }
                return coresbuiltdic;
            }
        }
        public static Dictionary<string, List<Item.ItemBase>> ItemsPerCategory
        {
            get
            {
                if (itemspercategory == null)
                {
                    itemspercategory = new Dictionary<string, List<Item.ItemBase>>();
                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/items.json");  // ← nom du projet exact !
                    using var stream = AssetLoader.Open(jsonUri);
                    string json = (new StreamReader(stream).ReadToEndAsync()).Result;
                    JsonDocument doc = JsonDocument.Parse(json);
                    string category;
                    foreach (JsonProperty property in doc.RootElement.EnumerateObject())
                    {
                        category = property.Value.GetProperty("category").ToString();
                        if (!itemspercategory.ContainsKey(category))
                        {
                            itemspercategory.Add(category, new List<Item.ItemBase>());
                        }
                        itemspercategory[category].Add(new Item.ItemBase(property.Name));
                    }
                    itemspercategory["core"].Add(new Item.ItemBase(-1));
                }
                return itemspercategory;
            }
        }
        public static Dictionary<int, string> ItemsDicInverted
        {
            get
            {
                if (itemsdicinverted == null)
                {
                    itemsdicinverted = ItemsDic.ToDictionary(k => k.Value, k => k.Key);
                }
                return itemsdicinverted;
            }
        }
        public static ObservableCollection<Item.ItemBase> CoresListBuilt
        {
            get
            {
                if(coreslistbuilt == null)
                {
                    coreslistbuilt = new ObservableCollection<Item.ItemBase>();
                    coreslistbuilt.Add(Item.ItemBase.None);
                    foreach(int i in CoresIdList)
                    {
                        coreslistbuilt.Add(new Item.ItemBase(i));
                    }
                }
                return coreslistbuilt;
            }
        }
        public static int GetItemId(string name)
        {
            return ItemsDic[name];
        }
        public static Item.ItemBase GetCore(int Id)
        {
            return (CoresBuiltDic[Id]);
        }
    }
    public static class MonstersConsts
    {
        private static Dictionary<string, int>? monstersdic;
        private static Dictionary<int, string>? monstersdicinverted;
        public static List<string> MonstersNameList = MonstersDicInverted.Values.ToList();
        public static List<int> MonsterIdList = MonstersDic.Values.ToList();
        private static Dictionary<int, Dictionary<string, object>>? monstersdicfull;

        public static Dictionary<string, int> MonstersDic
        {
            get
            {
                if (monstersdic == null)
                {
                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/monsters.json");  // ← nom du projet exact !
                    using var stream = AssetLoader.Open(jsonUri);
                    string json = (new StreamReader(stream).ReadToEndAsync()).Result;
                    JsonDocument IdMonsterJson = JsonDocument.Parse(json);
                    monstersdic = new Dictionary<string, int>();
                    int i = 1;
                    foreach (JsonProperty element in IdMonsterJson.RootElement.EnumerateObject())
                    {
                        monstersdic.Add(element.Name, i);
                        i++;
                    }
                }
                return monstersdic;
            }
        }
        public static Dictionary<int, Dictionary<string, object>> MonstersDicFull
        {
            get
            {
                if (monstersdicfull == null)
                {
                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/monsters.json");  // ← nom du projet exact !
                    using var stream = AssetLoader.Open(jsonUri);
                    string json = (new StreamReader(stream).ReadToEndAsync()).Result;
                    JsonDocument IdMonsterJson = JsonDocument.Parse(json);
                    monstersdicfull = new Dictionary<int, Dictionary<string, object>>();
                    int i = 1;
                    string[] keyvalues = ["element", "rarity", "hp", "sta", "atk", "def", "spd"];
                    foreach (JsonProperty Element in IdMonsterJson.RootElement.EnumerateObject())
                    {
                        Dictionary<string, object> Value = new Dictionary<string, object>();
                        foreach (JsonProperty element in Element.Value.EnumerateObject())
                        {
                            if (keyvalues.Contains(element.Name))
                            {
                                int value;
                                if (int.TryParse(element.Value.ToString(), out value))
                                    Value.Add(element.Name, value);
                                else Value.Add(element.Name, element.Value.ToString());
                            }
                        }
                        if (Value != null)
                            monstersdicfull.Add(i, Value);
                        i++;
                    }
                }
                return monstersdicfull;
            }
        }
        public static Dictionary<int, string> MonstersDicInverted
        {
            get
            {
                if (monstersdicinverted == null)
                {
                    monstersdicinverted = MonstersDic.ToDictionary(k => k.Value, k => k.Key);
                    monstersdicinverted = monstersdicinverted.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                }
                return monstersdicinverted;
            }
        }

        public static int GetMonsterId(string name)
        {
            return MonstersDic[name];
        }
    }
    public static class SkillsConsts
    {
        private static Dictionary<string, int>? skillsdic;
        private static Dictionary<int, string>? skillsdicinverted;
        private static Dictionary<int, Dictionary<string, object>>? skillsdicfull;
        public static List<string> SkillsList = SkillsDic.Keys.ToList();
        public static List<int> SkillsIdList = SkillsDic.Values.ToList();
        public static ObservableCollection<Skill>? skillslistbuilt;
        public static Dictionary<string, int> SkillsDic
        {
            get
            {
                if (skillsdic == null)
                {
                    skillsdic = new Dictionary<string, int>();
                    int actualId = 1;

                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/names.txt");  // ← nom du projet exact !
                    using var stream = AssetLoader.Open(jsonUri);
                    using var reader = new StreamReader(stream);
                    string[] names = reader.ReadToEnd().Split(new[] { "\r\n","\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string name in names)
                    {
                        skillsdic.Add(name, actualId);
                        actualId++;
                    }
                }
                return skillsdic;
            }
        }
        public static Dictionary<int, string> SkillsDicInverted
        {
            get
            {
                if (skillsdicinverted == null)
                {
                    skillsdicinverted = SkillsDic.ToDictionary(k => k.Value, k => k.Key);
                }
                return skillsdicinverted;
            }
        }
        public static Dictionary<int, Dictionary<string, object>> SkillsDicFull
        {
            get
            {
                if (skillsdicfull == null)
                {
                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/skills.json");
                    using var stream = AssetLoader.Open(jsonUri);
                    using var reader = new StreamReader(stream);
                    JsonDocument IdSkillJson = JsonDocument.Parse(reader.ReadToEnd());
                    skillsdicfull = new Dictionary<int, Dictionary<string, object>>();
                    int i = 1;
                    string[] keyvalues = ["element", "cost", "speed", "damage_target_power"];
                    foreach (JsonProperty Element in IdSkillJson.RootElement.EnumerateObject())
                    {
                        i = int.Parse(Element.Name);
                        Dictionary<string, object> Value = new Dictionary<string, object>();
                        foreach (JsonProperty element in Element.Value.EnumerateObject())
                        {
                            if (keyvalues.Contains(element.Name))
                            {
                                if (!element.Value.ToString().IsWhiteSpace())
                                {
                                    int value;
                                    if (int.TryParse(element.Value.ToString(), out value))
                                        Value.Add(element.Name, value);
                                    else Value.Add(element.Name, element.Value.ToString());
                                }
                            }
                        }
                        if (Value != null)
                            skillsdicfull.Add(i, Value);
                    }
                }
                return skillsdicfull;
            }
        }
        public static ObservableCollection<Skill> SkillsListBuilt
        {
            get
            {
                if(skillslistbuilt == null)
                {
                    skillslistbuilt = new ObservableCollection<Skill>();
                    skillslistbuilt.Add(Skill.None);
                    foreach(int skillid in SkillsIdList)
                    {
                        skillslistbuilt.Add(new Skill(skillid));
                    }
                }
                return skillslistbuilt;
            }
        }
        public static int GetSkillId(string name)
        {
            return SkillsDic[name];
        }
        public static Skill GetSkill(int id)
        {
            int number = id;
            if(id==-1) number = 0;
            return SkillsListBuilt[number];
        }
    }
    
    public static class OtherConsts
    {
        private static Bitmap? btmp;
        private static List<string>? playerbodyList;
        private static List<string>? petbodyList;
        private static List<int>? mapList;
        public static List<string> PlayerBodyList
        {
            get
            {
                if (playerbodyList == null)
                {
                    playerbodyList = new List<string>();
                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/avatars.json");
                    using var stream = AssetLoader.Open(jsonUri);
                    using var reader = new StreamReader(stream);
                    JsonDocument doc = JsonDocument.Parse(reader.ReadToEnd());
                    foreach (JsonElement element in doc.RootElement.EnumerateArray())
                    {
                        playerbodyList.Add(element.ToString());
                    }
                }
                return playerbodyList;
            }
        }
        public static List<string> PetBodyList
        {
            get
            {
                if (petbodyList == null)
                {
                    petbodyList = new List<string>();
                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/pets.json");
                    using var stream = AssetLoader.Open(jsonUri);
                    using var reader = new StreamReader(stream);
                    JsonDocument doc = JsonDocument.Parse(reader.ReadToEnd());
                    foreach (JsonElement element in doc.RootElement.EnumerateArray())
                    {
                        petbodyList.Add(element.ToString());
                    }
                }
                return petbodyList;
            }
        }
        public static List<int> MapList
        {
            get
            {
                if (mapList == null)
                {
                    mapList = new List<int>();
                    var jsonUri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/maps.json");
                    using var stream = AssetLoader.Open(jsonUri);
                    using var reader = new StreamReader(stream);
                    JsonDocument doc = JsonDocument.Parse(reader.ReadToEnd());
                    foreach (JsonProperty element in doc.RootElement.EnumerateObject())
                    {
                        mapList.Add(int.Parse(element.Name));
                    }
                }
                return mapList;
            }
        }
        public static string GetElementIcon(string element)
        {
            return ($"images/icon-types/{element}.png");
        }
        public static Bitmap DefaultBitmap
        {
            get
            {
                if (btmp == null)
                {
                    btmp = new WriteableBitmap(new PixelSize(1, 1), new Vector(96, 96));
                }
                return btmp;
            }
        }
    }
}

