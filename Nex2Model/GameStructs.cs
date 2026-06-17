using Avalonia.Controls.Shapes;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Input;
using static Nexomon2Model.Skill;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nexomon2Model
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public string Element { get; set; }
        public string IconElementPath { get; set; }
        public int Cost { get; set; }
        public Bitmap IconElement { get; set; }

        public static Skill None = new Skill(-1);
        public Skill(int Id)
        {
            if (Id == -1)
            {
                this.Id = Id;
                Name = "--none--";
                Element = "normal";
                Cost = 0;
                IconElementPath = $"images/icon-types/{Element}.png";
                IconElement = OtherConsts.DefaultBitmap;
            }
            else
            {
                this.Id = Id;
                Name = SkillsConsts.SkillsDicInverted[Id];
                Element = (string)SkillsConsts.SkillsDicFull[Id]["element"];
                Cost = (int)SkillsConsts.SkillsDicFull[Id]["cost"];
                IconElementPath = $"images/icon-types/{Element}.png";
                var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconElementPath}");
                IconElement = new Bitmap(AssetLoader.Open(imageUri));
            }
                
        }
        public Skill(string name)
        {
            Id = SkillsConsts.SkillsDic[name];
            Name = name;
            Element = (string)SkillsConsts.SkillsDicFull[Id]["element"];
            Cost = (int)SkillsConsts.SkillsDicFull[Id]["cost"];
            IconElementPath = $"images/icon-types/{Element}.png";
            var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconElementPath}");
            IconElement = new Bitmap(AssetLoader.Open(imageUri));
        }
    }


    public class Item:ObservableObject
    {
        public int Id { get; set; }
        public int quantity;
        public int Quantity 
        { 
            get { return quantity; }
            set
            {
                if (quantity == value) return;
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            } 
        }
        public string Name { get; set; }
        public string IconPath { get; set; }
        public Bitmap Icon { get; set; }
        public int MaxValue { get; set; }
        public Item(int Id, int quantity)
        {
            this.Id = Id;
            this.quantity = quantity;
            Name = ItemsConsts.ItemsDicInverted[Id];
            IconPath = $"images/icon-items/{Name}.png";
            try
            {
                var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconPath}");
                using var stream = AssetLoader.Open(imageUri);
                Icon = new Bitmap(stream);
            }
            catch (Exception)
            {
                Icon = OtherConsts.DefaultBitmap;
            }
            MaxValue = GetMaxValue();
            MaxValue = GetMaxValue();
            //Debug.WriteLine($"MaxValue de l'objet {Name} : {MaxValue}");
        }
        public Item(ItemBase itembase, int quantity) 
        {
            Name = itembase.Name;
            Id = itembase.Id;
            this.quantity = quantity;
            IconPath = itembase.IconPath;
            try
            {
                var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconPath}");
                using var stream = AssetLoader.Open(imageUri);
                Icon = new Bitmap(stream);
            }
            catch (Exception)
            {
                Icon = OtherConsts.DefaultBitmap;
            }
            MaxValue = GetMaxValue();
        }
        public int GetMaxValue()
        {
            foreach(string key in ItemsConsts.ItemsPerCategory.Keys)
            {
                bool contain=false;
                foreach(Item.ItemBase itmbase in ItemsConsts.ItemsPerCategory[key])
                {
                    if (itmbase.Id == Id)
                    {
                        contain = true;
                        break;
                    }
                }
                if (!contain) continue;
                if (key.Equals("pet"))
                {
                    return (1);
                }
                if (key.Equals("key"))
                {
                    if (Name.Equals("nexobox-silver") || Name.Equals("nexobox-golden") || Name.Equals("food-friendship"))
                    {
                        return (995);
                    }
                    if (Name.Equals("key-vault"))
                    {
                        return (9);
                    }
                    if (Name.Equals("quest-garbage"))
                    {
                        return (12);
                    }
                    if (Name.Equals("food-abyssal"))
                    {
                        return (6);
                    }
                    return (1);
                }
            }
            return (995);
        }
        
        public class ItemBase
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string IconPath { get; set; }
            public Bitmap Icon { get; set; }
            public static ItemBase None = new ItemBase(-1);
            
            public ItemBase(int Id)
            {
                if (Id < 0)
                {
                    this.Id = Id;
                    Name = "--none--";
                    IconPath = "--none--";
                    Icon = OtherConsts.DefaultBitmap;
                }
                else
                {
                    this.Id = Id;
                    Name = ItemsConsts.ItemsDicInverted[this.Id];
                    IconPath = $"images/icon-items/{ItemsConsts.ItemsDicInverted[this.Id]}.png";
                    try
                    {
                        var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconPath}");
                        using var stream = AssetLoader.Open(imageUri);
                        Icon = new Bitmap(stream);
                    }
                    catch (Exception)
                    {
                        Icon = OtherConsts.DefaultBitmap;
                    }
                }
            }
            public ItemBase(string name)
            {
                Name = name;
                Id = ItemsConsts.ItemsDic[Name];
                IconPath = $"images/icon-items/{ItemsConsts.ItemsDicInverted[Id]}.png";
                try
                {
                    var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconPath}");
                    using var stream = AssetLoader.Open(imageUri);
                    Icon = new Bitmap(stream);
                }
                catch (Exception)
                {
                    Icon = OtherConsts.DefaultBitmap;
                }
            }
        }
    }

    public class Inventory
    {
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public List<int> idsthere 
        {
            get 
            {
                List<int> prod = new List<int>();
                foreach (Item item in Items) 
                {
                    prod.Add(item.Id);
                }
                return prod;
            } 
        }
        public void Add(Item item) {  Items.Add(item); }
        public Inventory() { }
        public Inventory(ObservableCollection<ItemCategory> rawinv)
        {
            foreach(ItemCategory bag in rawinv)
            {
                foreach(Item item in bag.Items)
                {
                    if (item.Quantity != 0)
                    {
                        Items.Add(item);
                    }
                }
            }
        }
        public ObservableCollection<ItemCategory> ToRawInventory()
        {
            ObservableCollection<ItemCategory> prod = new ObservableCollection<ItemCategory>();
            foreach(string category in ItemsConsts.ItemsPerCategory.Keys)
            {
                if (category == "currency") continue;
                prod.Add(new ItemCategory(category,this));
            }
            return prod;
        }
    }
    public class ItemCategory
    {
        public string Name { get; set; }
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public void MaxAll() 
        { 
            foreach(Item item in Items)
            {
                item.Quantity = item.MaxValue;
            }
        }
        public ICommand? maxAllCommand;
        public ICommand MaxAllCommand 
        { 
            get 
            { if(maxAllCommand == null)
                {
                    maxAllCommand = new RelayCommand(MaxAll);
                }
                return maxAllCommand;
            } 
        }
        public ItemCategory(string name, Inventory inventory)
        {
            Name = name;
            List<Item.ItemBase> actualitems = ItemsConsts.ItemsPerCategory[name];
            foreach (Item.ItemBase item in actualitems)
            {
                if (item.Id != -1)
                {
                    if (!inventory.idsthere.Contains(item.Id))
                    {
                        Items.Add(new Item(item, 0));
                    }
                    else
                    {
                        foreach (Item item1 in inventory.Items)
                        {
                            if (item1.Id == item.Id)
                            {
                                Items.Add(item1);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public class Unit:ObservableObject
    {
        public static Unit NexoNull = new Unit(-1);
        public int Id { get; set; } 
        public bool NicknameExists { get; set; } 
        public string Nickname { get; set; } 
        public int level { get; set; } 
        public int Level
        {
            get { return level; }
            set
            {
                if (level == value) return;
                level = value;
                OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(MaxHp));
                OnPropertyChanged(nameof(MaxStamina));
                OnPropertyChanged(nameof(Atk));
                OnPropertyChanged(nameof(Def));
                OnPropertyChanged(nameof(Spd));
            }
        }
        public int hp { get; set; }
        public int MaxHp => Math.Max(10, (int)((int)MonstersConsts.MonstersDicFull[Id]["hp"]*0.04*Level + Level*1.75)+10);
        public int MaxStamina => Math.Max(100, (int)((int)MonstersConsts.MonstersDicFull[Id]["sta"] * 0.025 * Level) + 100 + Level);
        public int Atk => Math.Max(7, (int)(Level*((int)MonstersConsts.MonstersDicFull[Id]["atk"] * 1.75 +30)*2.2)/100 + 7);
        public int Def => Math.Max(3, (int)((Level * ((int)MonstersConsts.MonstersDicFull[Id]["def"] * 1.75 + 30.0) * 1.7) / 100.0) + 3);
        public int Spd => Math.Max(10, (int)((int)MonstersConsts.MonstersDicFull[Id]["spd"] * 0.045 * Level + Level * 1.1) + 10);
        public int stamina { get; set; } 
        public int xp { get; set; } 
        public ObservableCollection<Slot<Skill>> skills { get; set; } 
        public ObservableCollection<Slot<Item.ItemBase>> cores { get; set; }
        public bool cosmic { get; set; } 
        public int harmony { get; set; }
        public string Element { get; set; }
        public int Rarity { get; set; } 
        public string Name {  get; set; }
        public bool BattleStatus { get; set; }
        public string BattleStatusName { get; set; }
        public int BattleStatus1 { get; set; }
        public int BattleStatus2 { get; set; }
        public bool BattleStatusDuration { get; set; }
        public string IconPath { get; set; }
        public Bitmap Icon { get; set; }
        public void AddSkill(int id)
        {
            for(int i=0;i<skills.Count;i++)
            {
                if (skills[i].Content!.Id == -1)
                {
                    skills[i].Content = SkillsConsts.GetSkill(id);
                    return;
                }
            }
        }
        public void AddCore(int id)
        {
            for (int i = 0; i < cores.Count; i++)
            {
                if (cores[i].Content!.Id == -1)
                {
                    cores[i].Content = ItemsConsts.GetCore(id);
                    return;
                }
            }
        }
        public int NumberSkills
        {
            get
            {
                int count = 0;
                foreach(Slot<Skill> skillslot in skills)
                {
                    if(skillslot.Content!.Id>=0) count++;
                }
                return count;
            }
        }
        public int NumberCores
        {
            get
            {
                int count = 0;
                foreach(Slot<Item.ItemBase> coreslot in cores)
                {
                    if(coreslot.Content!.Id>=0) count++;
                }
                return count;
            }
        }
        public Unit() 
        {
            Id = 1;
            NicknameExists= true;
            Level = 1;
            hp = 1;
            stamina = 1;
            xp = 1;
            skills = new ObservableCollection<Slot<Skill>>();
            for(int i = 0; i < 4; i++)
            {
                skills.Add(new Slot<Skill>(Skill.None));
            }
            cores = new ObservableCollection<Slot<Item.ItemBase>>();
            for (int i = 0; i < 4; i++)
            {
                cores.Add(new Slot<Item.ItemBase>(Item.ItemBase.None));
            }
            cosmic = false;
            harmony = 1;
            Element = (string)MonstersConsts.MonstersDicFull[Id]["element"];
            Name = MonstersConsts.MonstersDicInverted[Id];
            Nickname = Name;
            BattleStatus = false;
            BattleStatusName = string.Empty;
            BattleStatus1 = 0;
            BattleStatus2 = 0;
            BattleStatusDuration = false;
            Rarity = (int)MonstersConsts.MonstersDicFull[Id]["rarity"];
            IconPath = $"images/monster-icons/{Name}.png";
            var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconPath}");
            using var stream = AssetLoader.Open(imageUri);
            Icon = new Bitmap(stream);
        }
        public Unit(int id) 
        {
            if (id >= 0)
            {
                Id = id;
                NicknameExists = true;
                level = 1;
                hp = 1;
                stamina = 1;
                xp = 1;
                skills = new ObservableCollection<Slot<Skill>>();
                for (int i = 0; i < 4; i++)
                {
                    skills.Add(new Slot<Skill>(Skill.None));
                }
                cores = new ObservableCollection<Slot<Item.ItemBase>>();
                for (int i = 0; i < 4; i++)
                {
                    cores.Add(new Slot<Item.ItemBase>(Item.ItemBase.None));
                }
                cosmic = false;
                harmony = 1;
                Element = (string)MonstersConsts.MonstersDicFull[Id]["element"];
                Name = MonstersConsts.MonstersDicInverted[Id];
                Nickname = Name;
                BattleStatus = false;
                BattleStatusName = string.Empty;
                BattleStatus1 = 0;
                BattleStatus2 = 0;
                BattleStatusDuration = false;
                Rarity = (int)MonstersConsts.MonstersDicFull[Id]["rarity"];

                try
                {
                    string name = new string(Name);
                    IconPath = $"images/monster-icons/{name}.png";
                    var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconPath}");
                    using var stream = AssetLoader.Open(imageUri);
                    Icon = new Bitmap(stream);
                }
                catch (Exception)
                {
                    StringBuilder name = new StringBuilder(Name);
                    name[0] = char.ToUpper(name[0]);
                    IconPath = $"images/monster-icons/{name}.png";
                    var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconPath}");
                    using var stream = AssetLoader.Open(imageUri);
                    Icon = new Bitmap(stream);
                }
            }
            else
            {
                Id = id;
                NicknameExists = true;
                level = 1;
                hp = 1;
                stamina = 1;
                xp = 1;
                skills = new ObservableCollection<Slot<Skill>>();
                for (int i = 0; i < 4; i++)
                {
                    skills.Add(new Slot<Skill>(Skill.None));
                }
                cores = new ObservableCollection<Slot<Item.ItemBase>>();
                for (int i = 0; i < 4; i++)
                {
                    cores.Add(new Slot<Item.ItemBase>(Item.ItemBase.None));
                }
                cosmic = false;
                harmony = 1;
                Element = (string)MonstersConsts.MonstersDicFull[1]["element"];
                Name = "NexoNull";
                Nickname = Name;
                BattleStatus = false;
                BattleStatusName = string.Empty;
                BattleStatus1 = 0;
                BattleStatus2 = 0;
                BattleStatusDuration = false;
                Rarity = (int)MonstersConsts.MonstersDicFull[1]["rarity"];
                string name = new string(Name);
                IconPath = $"NexoNull";
                Icon = OtherConsts.DefaultBitmap;
            }
        }
        public Unit(Unit unit)
        {
            Id = unit.Id;
            NicknameExists = unit.NicknameExists;
            Nickname = unit.Nickname;
            level = unit.level;
            hp = unit.hp;
            stamina = unit.stamina;
            xp = unit.xp;
            skills = new ObservableCollection<Slot<Skill>>();
            foreach (Slot<Skill> skillslot in unit.skills)
            {
                skills.Add(new Slot<Skill>(skillslot.Content!));
            }
            cores = new ObservableCollection<Slot<Item.ItemBase>>();
            foreach (Slot<Item.ItemBase> coreslot in unit.cores)
            {
                cores.Add(new Slot<Item.ItemBase>(coreslot.Content!));
            }
            cosmic = unit.cosmic;
            harmony = unit.harmony;
            BattleStatus = unit.BattleStatus;
            BattleStatusName = unit.BattleStatusName;
            BattleStatus1 = unit.BattleStatus1;
            BattleStatus2 = unit.BattleStatus2;
            BattleStatusDuration = unit.BattleStatusDuration;
            Element = (string)MonstersConsts.MonstersDicFull[this.Id]["element"];
            Name = MonstersConsts.MonstersDicInverted[this.Id];
            Rarity = (int)MonstersConsts.MonstersDicFull[this.Id]["rarity"];
            IconPath = $"images/monster-icons/{Name}.png";
            var imageUri = new Uri($"avares://NEXHEX/Nex2Assets/{IconPath}");
            using var stream = AssetLoader.Open(imageUri);
            Icon = new Bitmap(stream);
        }
    }

    public class Party
    {
        public ObservableCollection<Slot<Unit>> Units { get; set; } = new ObservableCollection<Slot<Unit>>();
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
        public int totalunits
        {
            get
            {
                int total = 0;
                foreach(Slot<Unit> slot in Units)
                {
                    if (slot.Content != Unit.NexoNull) total++;
                }
                return total;
            }
        }
        public Party() 
        {
            for(int i = 1; i < 7; i++)
            {
                Units.Add(new Slot<Unit>(Unit.NexoNull));
            }
        }
        public Party(ObservableCollection<Unit> units)
        {
            for (int i = 1; i < 7; i++)
            {
                Units.Add(new Slot<Unit>(Unit.NexoNull));
            }
            foreach(Unit unit in units)
            {
                Add(unit);
            }
        }
    }

    public class Storage
    {
        public ObservableCollection<Box> Boxes { get; set; } = new ObservableCollection<Box>();
        public Box CreateBox() 
        {
            Box box = new Box(Boxes.Count+1);
            Boxes.Add(box);
            return box;
        }
        public void Add(Unit unit) 
        {
            foreach(Box box in Boxes)
            {
                foreach(Slot<Unit> slot in box.Units)
                {
                    if (slot.Content == Unit.NexoNull)
                    {
                        slot.Content = unit;
                        return;
                    }
                }
            }
            Box newbox = CreateBox();
            newbox.Units[0].Content=unit;
        }

        public Storage() { }
    }

    public class Box
    {
        public int Id;
        public string Name { get; set; } = "Default";
        public ObservableCollection<Slot<Unit>> Units { get; set; } = new ObservableCollection<Slot<Unit>>();
        public void Add(int index, Unit unit) 
        {
            Units[index].Content=unit; 
        }
        public Box(int id) 
        { 
            Id = id;
            Name =$"Default-{id}";
            for(int i=0; i < 60; i++)
            {
                Units.Add(new Slot<Unit>(Unit.NexoNull));
            }
        }
        
    }
    public class Slot<T>:ObservableObject
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
    public class Wallet
    {
        public int Money { get; set; }
        public int Diamonds { get; set; }
        public int Tokens { get; set; }
        public Wallet(int money, int diamonds, int tokens)
        {
            Money = money;
            Diamonds = diamonds;
            Tokens = tokens;
        }
        public override string ToString() 
        {
            return($"{Money }, {Diamonds }, {Tokens }");
        }
    }

    public class Location
    {
        public int MapId { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public string Direction { get; set; }
        public int CptMapId { get; set; }
        public int CptXCoord { get; set; }
        public int CptYCoord { get; set; }
        public Location(int mapId, int xCoord, int yCoord, string direction, int cptmapid, int cptxCoord, int cptyCoord)
        {
            MapId = mapId;
            XCoord = xCoord;
            YCoord = yCoord;
            Direction = direction;
            CptMapId = cptmapid;
            CptXCoord = cptxCoord;
            CptYCoord = cptyCoord;
        }
    }

    public class PlayerState
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public Location Location { get; set; }
        public Wallet Wallet { get; set; }
        public PlayerState(string name, string body, Wallet wallet, Location location)
        {
            Name = name;
            Body = body;
            Wallet = wallet;
            Location = location;
        }
    }

    public class Tamer
    {
        public string Name { get; set; }
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public Tamer(string name, int int1, int int2)
        {
            Name = name;
            Int1 = int1;
            Int2 = int2;
        }
        public Tamer(string name)
        {
            Name = name;
            Int1 = 0;
            Int2 = 0;
        }
    }
    public class Tamers
    {
        public ObservableCollection<Tamer> TamerList { get; set; }
        public void Add(Tamer tamer) 
        {
            TamerList.Add(tamer);
        }
        public Tamers() 
        {
            TamerList = new ObservableCollection<Tamer>();
        }
    }
    public class SaveData : ObservableObject
    {
        public string fileName { get; set; }
        public DataReader reader { get; set; }
        public DataWriter writer { get; set; }
        public PlayerState Player { get; set; }
        public string PetBody { get; set; }
        public Party Team { get; set; }
        public Storage Storage { get; set; }
        public Inventory Inventory { get; set; }
        public ObservableCollection<ItemCategory> RawInventory { get; set; }
        public SaveData(string savedir)
        {
            fileName = System.IO.Path.GetFileName(savedir);
            byte[] data = File.ReadAllBytes(savedir);
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            BinaryReader binreader = new BinaryReader(ms);
            BinaryWriter binWriter = new BinaryWriter(ms);
            reader = new DataReader(binreader, ms);
            writer = new DataWriter(reader,binWriter);
            int x = reader.GetXCoordinate(true);
            int y = reader.GetYCoordinate(true);
            int cptmapId = reader.GetCheckpointMapID(true);
            int cptx = reader.GetCheckpointXCoordinate(true);
            int cpty = reader.GetCheckpointYCoordinate(true);
            int mapId = reader.GetMapID(true);
            string playername = reader.GetPlayerName(true);
            string body = reader.GetPlayerBody(true);
            string pet = reader.GetPetBody(true);
            string direction = reader.GetDirection(true);
            Location location = new Location(mapId, x, y, direction, cptmapId, cptx, cpty);
            Wallet wallet = reader.WalletPass(true);
            PetBody = pet;
            Player = new PlayerState(playername, body, wallet, location);
            Team = reader.PartyCtor(true);
            Storage = reader.StorageBoxCtor(true);
            this.Inventory = reader.InventoryPass(true);
            RawInventory = Inventory.ToRawInventory();
        }
        public SaveData()
        {
            fileName = "data-1.dat";
            var uri = new Uri("avares://NEXHEX/Nex2Assets/JSONTXT/data-1.dat");
            using var stream = AssetLoader.Open(uri);
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            byte[] data = ms.ToArray();
            BinaryReader binreader = new BinaryReader(ms);
            BinaryWriter binWriter = new BinaryWriter(ms);
            reader = new DataReader(binreader, ms);
            writer = new DataWriter(reader, binWriter);
            int x = reader.GetXCoordinate(true);
            int y = reader.GetYCoordinate(true);
            int cptmapId = reader.GetCheckpointMapID(true);
            int cptx = reader.GetCheckpointXCoordinate(true);
            int cpty = reader.GetCheckpointYCoordinate(true);
            int mapId = reader.GetMapID(true);
            string playername = reader.GetPlayerName(true);
            string body = reader.GetPlayerBody(true);
            string pet = reader.GetPetBody(true);
            string direction = reader.GetDirection(true);
            Location location = new Location(mapId, x, y, direction, cptmapId, cptx, cpty);
            Wallet wallet = reader.WalletPass(true);
            PetBody = pet;
            Player = new PlayerState(playername, body, wallet, location);
            Team = reader.PartyCtor(true);
            Storage = reader.StorageBoxCtor(true);
            this.Inventory = reader.InventoryPass(true);
            RawInventory = Inventory.ToRawInventory();
        }
        public void SaveFileToDirectory(string dir)
        {
            WriteAll();
            File.WriteAllBytes(System.IO.Path.Combine(dir, fileName), reader.ms.ToArray());
        }
        public void WriteAll()
        {
            Reconsolidate();
            writer.SetPlayerState(Player);
            writer.SetPetBody(PetBody);
            writer.SetParty(Team);
            writer.SetStorageBox(Storage);
            writer.SetInventory(Inventory);
        }
        public void GetThemAll()
        {
            List<int> nexomonowned = new List<int>();
            foreach (Slot<Unit> slot in Team.Units)
            {
                if (slot.Content != null)
                {
                    nexomonowned.Add(slot.Content.Id);
                }
            }
            foreach (Box box in Storage.Boxes)
            {
                foreach (Slot<Unit> slot in box.Units)
                {
                    if (slot.Content != null)
                    {
                        nexomonowned.Add(slot.Content.Id);
                    }
                }
            }
            List<int> localMonsterIdList = new List<int>(MonstersConsts.MonsterIdList);
            foreach (int id in nexomonowned)
            {
                if (localMonsterIdList.Contains(id)) localMonsterIdList.Remove(id);
            }
            foreach (int id in localMonsterIdList)
            {
                Storage.Add(new Unit(id));
            }
            writer.SeeMonsters(localMonsterIdList);
            writer.OwnMonsters(localMonsterIdList);
        }
        public void Reconsolidate()
        {
            Inventory = new Inventory(RawInventory);
        }
    }
}
