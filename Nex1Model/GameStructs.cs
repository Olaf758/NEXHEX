using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nexomon1Model
{
    public class SaveData
    {
        public string filename { get; set; } = "nexomon-save.dat";
        private JsonNode Jsonnodesave { get; set; }
        public string playerName { get; set; }
        public string playerBodyName { get; set; }
        public string petBodyName{ get; set; }
        public int playerX { get; set; }
        public int playerY { get; set; }
        public int mapID { get; set; }
        public int lastRespawnX { get; set; }
        public int lastRespawnY { get; set; }
        public int lastRespawnMapID { get; set; }
        public JsonNode seenNexomon { get; set; }
        public JsonNode capturedNexomon { get; set; }
        public List<string> playerBodies { get; set; }
        public List<string> petBodies { get; set; }
        public Party playerParty { get; set; }
        public Storage playerHatchery { get; set; }
        public Inventory inventory { get; set; }
        public int coins { get; set; }
        public int diamonds { get; set; }
        public int goldenBoxes { get; set; }
        public int silverBoxes { get; set; }
        public int freeGoldenBoxes { get; set; }
        public int freeSilverBoxes { get; set; }
        public int goldenTraps { get; set; }

        public int playtimeSeconds { get; set; }
        public SaveData(string savedirectory)
        {
            filename = Path.GetFileName(savedirectory);
            byte[] bytes = File.ReadAllBytes(savedirectory);
            string savedata = Encoding.UTF8.GetString(bytes);
            savedata = Utils.UnprotectString(savedata);
            Jsonnodesave = JsonNode.Parse(savedata)!;
            playerX = Jsonnodesave["PLAYER_X"]!.GetValue<int>();
            playerY = Jsonnodesave["PLAYER_Y"]!.GetValue<int>();
            mapID = Jsonnodesave["MAP_ID"]!.GetValue<int>();
            lastRespawnX = Jsonnodesave["RESPAWN_X"]!.GetValue<int>();
            lastRespawnY = Jsonnodesave["RESPAWN_Y"]!.GetValue<int>();
            lastRespawnMapID = Jsonnodesave["RESPAWN_MAP"]!.GetValue<int>();
            playerName = Jsonnodesave["PLAYER_NAME"]!.GetValue<string>()!;
            playerBodyName = Jsonnodesave["PLAYER_BODY_NAME"]!.GetValue<string>()!;
            petBodyName = Jsonnodesave["PET_BODY_NAME"]!.GetValue<string>()!;
            seenNexomon = Jsonnodesave["SEEN_NEXOMON"]!;
            capturedNexomon = Jsonnodesave["CAPTURED_NEXOMON"]!;
            coins = Jsonnodesave["COINS"]!.GetValue<int>();
            diamonds = Jsonnodesave["DIAMONDS"]!.GetValue<int>();
            goldenBoxes = Jsonnodesave["GOLDEN_BOXES"]!.GetValue<int>();
            silverBoxes = Jsonnodesave["SILVER_BOXES"]!.GetValue<int>();
            goldenTraps = Jsonnodesave["GOLDEN_TRAPS"]!.GetValue<int>();
            playtimeSeconds = Jsonnodesave["PLAYTIME"]!.GetValue<int>();
            playerParty = new Party(Jsonnodesave["PARTY"]!);
            playerHatchery = new Storage(Jsonnodesave["HATCHERY"]!);
            inventory = new Inventory(Jsonnodesave["INVENTORY"]!);
            playerBodies = new List<string>();
            foreach (JsonNode? body in Jsonnodesave["PLAYER_BODIES"]!.AsArray())
            {
                if(body!=null)
                playerBodies.Add(body.GetValue<string>()!);
            }
            petBodies = new List<string>();
            foreach (JsonNode? body in Jsonnodesave["PET_BODIES"]!.AsArray())
            {
                if(body!=null)
                petBodies.Add(body.GetValue<string>()!);
            }
            freeGoldenBoxes = Jsonnodesave["FREE_GOLDEN_BOXES"]!.GetValue<int>();
            freeSilverBoxes = Jsonnodesave["FREE_SILVER_BOXES"]!.GetValue<int>();

        }
        public void SaveToDir(string dir)
        {
            List<string> ownedunits = new List<string>();
            foreach (Slot<Unit> unit in playerParty.Units)
            {
                if (!ownedunits.Contains(unit.Content.name)) ownedunits.Add(unit.Content.name);
            }
            foreach (Slot<Unit> unit in playerHatchery.Monsters)
            {
                if (!ownedunits.Contains(unit.Content.name)) ownedunits.Add(unit.Content.name);
            }
            foreach(string mons in ownedunits)
            {
                capturedNexomon[mons] = true.ToString();
                seenNexomon[mons] = true.ToString();
            }
            Jsonnodesave["PLAYER_X"] = playerX;
            Jsonnodesave["PLAYER_Y"] = playerY;
            Jsonnodesave["MAP_ID"] = mapID;
            Jsonnodesave["RESPAWN_X"] = lastRespawnX;
            Jsonnodesave["RESPAWN_Y"] = lastRespawnY;
            Jsonnodesave["RESPAWN_MAP"] = lastRespawnMapID;
            Jsonnodesave["PLAYER_NAME"] = playerName;
            Jsonnodesave["PLAYER_BODY_NAME"] = playerBodyName;
            Jsonnodesave["PET_BODY_NAME"] = petBodyName;
            Jsonnodesave["SEEN_NEXOMON"] = seenNexomon;//Todo: Convert to list of strings
            Jsonnodesave["CAPTURED_NEXOMON"] = capturedNexomon;// Todo: Convert to list of strings
            Jsonnodesave["COINS"] = coins;
            Jsonnodesave["DIAMONDS"] = diamonds;
            Jsonnodesave["GOLDEN_BOXES"] = goldenBoxes;
            Jsonnodesave["SILVER_BOXES"] = silverBoxes;
            Jsonnodesave["GOLDEN_TRAPS"] = goldenTraps;
            Jsonnodesave["PLAYTIME"] = playtimeSeconds;
            Jsonnodesave["PARTY"] = playerParty.ToNode();
            Jsonnodesave["HATCHERY"] = playerHatchery.ToNode();
            Jsonnodesave["INVENTORY"] = inventory.ToNode();
            Jsonnodesave["PLAYER_BODIES"] = JsonValue.Create(playerBodies);
            Jsonnodesave["PET_BODIES"] = JsonValue.Create(petBodies);
            Jsonnodesave["FREE_GOLDEN_BOXES"] = freeGoldenBoxes;
            Jsonnodesave["FREE_SILVER_BOXES"] = freeSilverBoxes;
            string jsonstring = Jsonnodesave.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
            jsonstring = Utils.ProtectString(jsonstring);
            byte[] bytejsonstring = Encoding.UTF8.GetBytes(jsonstring);
            File.WriteAllBytes(Path.Combine(dir, filename), bytejsonstring);
        }
    }
    public class Unit : ObservableObject
    {
        public string name { get; set; }
        private int lvl;
        public int level 
        {
            get
            {
                return lvl;
            }
            set
            {
                if (lvl != value)
                {
                    lvl = value;
                    this.maxHP = (int)(25f + (float)level * 4f * (Consts.MonsterGrowthData[name]["hpGrowth"] / 100f));
                    this.hp = maxHP;
                    this.attack = (int)(6f + (float)level * 2.5f * (Consts.MonsterGrowthData[name]["atkGrowth"] / 100f));
                    this.defense = (int)(3f + (float)level * 1.55f * (Consts.MonsterGrowthData[name]["defGrowth"] / 100f));
                    this.speed = (int)(1f + (float)level * (Consts.MonsterGrowthData[name]["speedGrowth"] / 100f));
                    this.nextEXP = 25 + level * 18;
                    if (level >= 10)
                    {
                        nextEXP += level * 8;
                    }
                    if (level >= 20)
                    {
                        nextEXP += level * 8;
                    }
                    if (level >= 30)
                    {
                        nextEXP += level * 8;
                    }
                    if (level >= 40)
                    {
                        nextEXP += level * 8;
                    }
                    if (level >= 50)
                    {
                        nextEXP += level * 8;
                    }
                    if (level >= 60)
                    {
                        nextEXP += level * 8;
                    }
                    if (level >= 70)
                    {
                        nextEXP += level * 8;
                    }
                    if (level >= 80)
                    {
                        nextEXP += level * 8;
                    }
                    if (level >= 90)
                    {
                        nextEXP += level * 8;
                    }
                    OnPropertyChanged(nameof(level));
                    OnPropertyChanged(nameof(maxHP));
                    OnPropertyChanged(nameof(attack));
                    OnPropertyChanged(nameof(defense));
                    OnPropertyChanged(nameof(speed));
                }
            } 
        }
        public int hp { get; set; }
        public int maxHP { get; set; }
        public int mp { get; set; }
        public int maxMP { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public int speed { get; set; }
        public int exp { get; set; }
        public int nextEXP { get; set; }
        public int rebirths { get; set; }
        public string IconPath { get; set; }
        public Bitmap Icon { get; set; }
        public List<Slot<Skill>> skills { get; set; }
        public static Unit NexoNull { get; set; } = new Unit();
        public Unit()
        {
            name = "--none--";
            skills = new List<Slot<Skill>>();
            IconPath = "";
            Icon = Consts.DefaultBitmap;
        }
        public Unit(string name, int level)
        {
            this.name = name;
            this.skills = new List<Slot<Skill>>();
            this.level = level;
            this.maxHP = (int)(25f + (float)level * 4f * (Consts.MonsterGrowthData[name]["hpGrowth"] / 100f));
            this.hp = maxHP;
            this.maxMP = 100;
            this.mp = maxMP;
            this.attack = (int)(6f + (float)level * 2.5f * (Consts.MonsterGrowthData[name]["atkGrowth"] / 100f));
            this.defense = (int)(3f + (float)level * 1.55f * (Consts.MonsterGrowthData[name]["defGrowth"] / 100f));
            this.speed = (int)(1f + (float)level * (Consts.MonsterGrowthData[name]["speedGrowth"] / 100f));
            this.exp = 0;
            this.nextEXP = 25 + level * 18;
            this.rebirths = 0;
            if (level >= 10)
            {
                nextEXP += level * 8;
            }
            if (level >= 20)
            {
                nextEXP += level * 8;
            }
            if (level >= 30)
            {
                nextEXP += level * 8;
            }
            if (level >= 40)
            {
                nextEXP += level * 8;
            }
            if (level >= 50)
            {
                nextEXP += level * 8;
            }
            if (level >= 60)
            {
                nextEXP += level * 8;
            }
            if (level >= 70)
            {
                nextEXP += level * 8;
            }
            if (level >= 80)
            {
                nextEXP += level * 8;
            }
            if (level >= 90)
            {
                nextEXP += level * 8;
            }
            this.IconPath = $"Nexomon1-data-refined/monsters-icons/{name}.png";
            this.Icon = Consts.MonstersPic[this.name];
        }
        public Unit(JsonNode jsonelem)
        {
            this.name = jsonelem["NAME"]!.GetValue<string>()!;
            this.level = jsonelem["LEVEL"]!.GetValue<int>();
            this.hp = jsonelem["HP"]!.GetValue<int>();
            this.maxHP = jsonelem["MAX_HP"]!.GetValue<int>();
            this.mp = jsonelem["MP"]!.GetValue<int>();
            this.maxMP = jsonelem["MAX_HP"]!.GetValue<int>();
            this.attack = jsonelem["ATTACK"]!.GetValue<int>();
            this.defense = jsonelem["DEFENSE"]!.GetValue<int>();
            this.speed = jsonelem["SPEED"]!.GetValue<int>();
            this.exp = jsonelem["EXP"]!.GetValue<int>();
            this.nextEXP = jsonelem["NEXT_EXP"]!.GetValue<int>();
            if (jsonelem["REBIRTHS"] == null) this.rebirths = 0;
            else this.rebirths = jsonelem["REBIRTHS"]!.GetValue<int>();
            this.skills = new List<Slot<Skill>>();
            foreach (JsonNode? skill in jsonelem["SKILLS"]!.AsArray())
            {
                if(skill!=null)
                this.skills.Add(new Slot<Skill>(Consts.Skills[skill.GetValue<int>()]));
            }
            for(int i = 0; i < 4 - this.skills.Count; i++)
            {
                this.skills.Add(new Slot<Skill>(Consts.Skills[0]));
            }
            this.IconPath = $"Nexomon1-data-refined/monsters-icons/{this.name}.png";
            this.Icon = Consts.MonstersPic[this.name];
        }
        public Unit(Unit unit)
        {
            this.name = unit.name;
            this.level = unit.level;
            this.hp = unit.hp;
            this.maxHP = unit.maxHP;
            this.mp = unit.mp;
            this.maxMP = unit.maxMP;
            this.attack = unit.attack;
            this.defense = unit.defense;
            this.speed = unit.speed;
            this.exp = unit.exp;
            this.nextEXP = unit.nextEXP;
            this.rebirths = unit.rebirths;
            this.skills = new List<Slot<Skill>>();
            foreach (Slot<Skill> skill in unit.skills)
            {
                this.skills.Add(new Slot<Skill>(Consts.Skills[skill.Content.Id]));
            }
            this.IconPath = $"Nexomon1-data-refined/monsters-icons/{this.name}.png";
            this.Icon = Consts.MonstersPic[this.name];
        }
        public JsonNode ToNode()
        {
            JsonArray skillsasNodearray = new JsonArray();
            foreach (Slot<Skill> skill in skills)
            {
                if(skill.Content.Id!=0)
                skillsasNodearray.Add(skill.Content.Id);
            }
            JsonNode skillsasNode = skillsasNodearray;
            JsonObject preresult = new JsonObject
            {
                ["NAME"] = this.name,
                ["LEVEL"] = this.level,
                ["HP"] = this.hp,
                ["MAX_HP"] = this.maxHP,
                ["MP"] = this.mp,
                ["MAX_HP"] = this.maxMP,
                ["ATTACK"] = this.attack,
                ["DEFENSE"] = this.defense,
                ["SPEED"] = this.speed,
                ["EXP"] = this.exp,
                ["NEXT_EXP"] = this.nextEXP,
                ["REBIRTHS"] = this.rebirths,
                ["SKILLS"] = skillsasNode,
            };
            return preresult;
        }
    }
    public class Slot<T> : ObservableObject
    {
        private T content;
        public T Content
        {
            get => content;
            set
            {
                if (EqualityComparer<T>.Default.Equals(content, value)) return;
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        [JsonConstructor]
        public Slot(T content)
        {
            this.content = content;
        }
    }
    public class Party
    {
        public ObservableCollection<Slot<Unit>> Units { get; set; }
        public void Add(Unit unit)
        {
            foreach (Slot<Unit> slot in Units)
            {
                if (slot.Content == Unit.NexoNull)
                {
                    slot.Content = unit;
                    break;
                }
            }
        }
        public Party(JsonNode rawparty)
        {
            Units = new ObservableCollection<Slot<Unit>>();
            foreach (JsonNode? unit in rawparty.AsArray())
            {
                if (unit != null)
                    Add(new Unit(unit));
            }
        }
        public Party(ObservableCollection<Unit> units)
        {
            Units = new ObservableCollection<Slot<Unit>>();
            for (int i = 1; i < 7; i++)
            {
                Units.Add(new Slot<Unit>(Unit.NexoNull));
            }
            foreach (Unit unit in units)
            {
                Add(unit);
            }
        }
        public JsonNode ToNode()
        {
            JsonArray partyasnodearray = new JsonArray();
            foreach(Slot<Unit> unit in Units) 
            {
                if(!unit.Content.name.Equals("--none--"))
                partyasnodearray.Add(unit.Content.ToNode());
            }
            return partyasnodearray;
        }

    }
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Equipped { get; set; }
        public int Category { get; set; }
        public int MaxValue { get; set; }
        public string IconPath { get; set; }
        public Bitmap Icon { get; set; }
        public Item(string Name, int Quantity)
        {
            this.Name = Name;
            this.Quantity = Quantity;
            Equipped = 0;
            Category = GetCategory();
            MaxValue = GetMaxValue();
            IconPath = $"Nexomon1-data-refined/items-icons/{Name}.png";
            Icon = Consts.ItemsPic[Name];
        }
        public Item(JsonNode nodeitem)
        {
            Name = nodeitem["ID"]!.GetValue<string>();
            Quantity = nodeitem["AMOUNT"]!.GetValue<int>();
            Equipped = nodeitem["EQUIPPED"]!.GetValue<int>();
            Category = GetCategory();
            MaxValue = GetMaxValue();
            IconPath = $"Nexomon1-data-refined/items-icons/{Name}.png";
            Icon = Consts.ItemsPic[Name];
        }
        public JsonNode ToNode()
        {
            JsonObject result = new JsonObject
            {
                ["ID"] = Name,
                ["AMOUNT"] = Quantity,
                ["EQUIPPED"] = Equipped,
            };
            return result;
        }
        private static Dictionary<int, List<string>>? itemsbycategory;
        public static  Dictionary<int, List<string>> ItemsByCategory
        {
            get
            {
                if (itemsbycategory == null)
                {
                    itemsbycategory = new Dictionary<int, List<string>>
                    {
                        { 0, new List<string> { "POTION", "SUPER_POTION", "MAX_POTION", "ETHER", "SUPER_ETHER", "MAX_ETHER", "REVIVE", "SUPER_REVIVE", "MAX_REVIVE", "ELIXIR" } },
                        { 1, new List<string> { "ANTI_BURN", "ANTI_POISON", "ANTI_FREEZE", "ANTI_PARALYZE", "ANTI_BIND", "ANTI_SLEEP", "ANTI_CONFUSE" } },
                        { 2, new List<string> { "NEXOTRAP" } },
                        { 3, new List<string> { "NEXOMITE", "NEXOKEY", "RADAR" } },
                        { 4, new List<string> { "CHARM", "SKATES", "REPEL", "EXP-SHARE", "EXP-SHARE-WEAK" } },
                        { 39, new List<string> { "INTERCOM" } }
                    };
                    return itemsbycategory;
                }
                return itemsbycategory;
            }
        }
        public int GetCategory()
        {
            foreach (int key in ItemsByCategory.Keys)
            {
                if (ItemsByCategory[key].Contains(Name)) return key;
            }
            return 1;
        }
        public int GetMaxValue()
        {
            if(Category==4 || Category == 5 )
            {
                return 1;
            }
            return 995;
        }
    }
    public class Inventory
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<ItemCategory> ItemsByCategories { get; set; }
        public void AddItem(Item item)
        {
            Items.Add(item);
        }
        public ObservableCollection<ItemCategory> ToCategories()
        {
            ObservableCollection<ItemCategory> tocategs = new ObservableCollection<ItemCategory>();
            foreach(int categ in Item.ItemsByCategory.Keys)
            {
                tocategs.Add(new ItemCategory(categ, this));
            }
            return tocategs;
        }
        public Inventory(ObservableCollection<Item> items)
        {
            Items = items;
            ItemsByCategories = ToCategories();
        }
        public Inventory(ObservableCollection<ItemCategory> itemcategs)
        {
            Items = new ObservableCollection<Item>();
            foreach(ItemCategory itemcateg in itemcategs)
            {
                foreach(Item item in itemcateg.Items)
                {
                    if (item.Quantity != 0)
                    {
                        Items.Add(new Item(item.Name, item.Quantity));
                    }
                }
            }
            ItemsByCategories = itemcategs;
        }
        public Inventory(JsonNode inventnode)
        {
            Items = new ObservableCollection<Item>();
            foreach(JsonNode? item in inventnode.AsArray())
            {
                if (item != null)
                {
                    AddItem(new Item(item));
                }
            }
            ItemsByCategories = ToCategories();
        }
        public JsonNode ToNode()
        {
            Reconsolidate();
            JsonArray itemarray = new JsonArray();
            foreach(Item item in Items) itemarray.Add(item.ToNode());
            return itemarray;
        }
        public Inventory()
        {
            Items = new ObservableCollection<Item>();
            ItemsByCategories = ToCategories();
        }
        public void Reconsolidate()
        {
            Items = new ObservableCollection<Item>();
            foreach (ItemCategory itemcateg in ItemsByCategories)
            {
                foreach (Item item in itemcateg.Items)
                {
                    if (item.Quantity != 0)
                    {
                        Items.Add(new Item(item.Name, item.Quantity));
                    }
                }
            }
        }
    }
    public class ItemCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Item> Items { get; set; }
        public void MaxAll()
        {
            foreach (Item item in Items)
            {
                item.Quantity = item.MaxValue;
            }
        }
        public ICommand? maxAllCommand;
        public ICommand MaxAllCommand
        {
            get
            {
                if (maxAllCommand == null)
                {
                    maxAllCommand = new RelayCommand(MaxAll);
                }
                return maxAllCommand;
            }
        }
        public ItemCategory(int id, Inventory inventory)
        {
            Id = id;
            Items = new ObservableCollection<Item>();
            foreach(string name in Item.ItemsByCategory[id])
            {
                Items.Add(new Item(name,0));
            }
            foreach(Item item in inventory.Items)
            {
                if(item.Category == id)
                {
                    foreach(Item item1 in Items)
                    {
                        if (item1.Name.Equals(item.Name))
                        {
                            item1.Quantity = item.Quantity;
                            break;
                        }
                    }
                }
            }
            if (id == 0) Name = "Health";
            else if (id == 1) Name = "Cure";
            else if (id == 2) Name = "Traps";
            else if (id == 3) Name = "Key Items";
            else if (id == 4) Name = "Gadgets";
            else Name = "Others";
        }
    }
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Bitmap Icon { get; set; }
        public Skill(int id)
        {
            Id = id;
            if (id == 0)
            {
                Name = "--none--";
                Icon = Consts.DefaultBitmap;
            }
            else
            {
                Name = Consts.SkillsName[id];
                Icon = Consts.SkillsPic[id];
            }
        }
    }
    public class Storage
    {
        public ObservableCollection<Slot<Unit>> Monsters { get; set; }
        public Storage(JsonNode rawstor)
        {
            Monsters = new ObservableCollection<Slot<Unit>>();
            foreach (JsonNode? mons in rawstor.AsArray())
            {
                if (mons != null)
                {
                    Monsters.Add(new Slot<Unit>(new Unit(mons)));
                }
            }
        }
        public void GetAll(SaveData save) 
        {
            List<string> ownedunits = new List<string>();
            foreach(Slot<Unit> unit in save.playerParty.Units)
            {
                if (!ownedunits.Contains(unit.Content.name)) ownedunits.Add(unit.Content.name);
            }
            foreach(Slot<Unit> unit in save.playerHatchery.Monsters)
            {
                if (!ownedunits.Contains(unit.Content.name)) ownedunits.Add(unit.Content.name);
            }
            foreach(string mons in Consts.MonstersPic.Keys)
            {
                if (!ownedunits.Contains(mons))
                {
                    Monsters.Add(new Slot<Unit>(new Unit(mons, 1)));
                }
            }
        }
        public JsonNode ToNode()
        {
            JsonArray partyasnodearray = new JsonArray();
            foreach (Slot<Unit> unit in Monsters)
            {
                if(!unit.Content.name.Equals("--none--"))
                partyasnodearray.Add(unit.Content.ToNode());
            }
            return partyasnodearray;
        }
    }
}
