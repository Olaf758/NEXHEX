using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;

namespace Nexomon2Model
{
    public class DataReader
    {
        public MemoryStream ms;
        public BinaryReader read;
        public int GetVersion(bool origin)
        {
            ms.Position = 0;
            int version = read.ReadInt32();
            if (version != 61) throw new ArgumentException("Unsupported version", nameof(version));
            if (origin)
            {
                ms.Position = 0;
            }
            return version;
        }
        public string GetBeginningText(bool origin)
        {
            GetVersion(false);
            string b = $"{read.ReadString()} {read.ReadString()} {read.ReadString()} {read.ReadString()}";
            if (origin)
            {
                ms.Position = 0;
            }
            return b;
        }
        public string GetPlayerName(bool origin)
        {
            GetBeginningText(false);
            string b = read.ReadString();
            if (origin)
            {
                ms.Position = 0;
            }
            return b;
        }
        public string GetPlayerBody(bool origin)
        {
            GetPlayerName(false);
            string b = read.ReadString();
            if (!OtherConsts.PlayerBodyList.Contains(b)) throw new ArgumentException("Invalid Entry", nameof(b));
            if (origin)
            {
                ms.Position = 0;
            }
            return b;
        }
        public int GetPlayTime(bool origin)
        {
            GetPlayerBody(false);
            int a = read.ReadInt32();
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public string GetPetBody(bool origin)
        {
            GetPlayTime(false);
            string b = read.ReadString();
            if (!string.IsNullOrEmpty(b) & !OtherConsts.PetBodyList.Contains(b))
            {
                //Debug.WriteLine($"Petbody en question : {b}");
                throw new ArgumentException("Invalid Entry", nameof(b));
            }
            if (origin)
            {
                ms.Position = 0;
            }
            return b;
        }
        public int GetMapID(bool origin)
        {
            GetPetBody(false);
            int a = read.ReadInt32();
            if (!OtherConsts.MapList.Contains(a)) throw new ArgumentException("Invalid Entry", nameof(a));
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public int GetXCoordinate(bool origin)
        {
            GetMapID(false);
            int a = read.ReadInt32();
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public int GetYCoordinate(bool origin)
        {
            GetXCoordinate(false);
            int a = read.ReadInt32();
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public string GetDirection(bool origin)
        {
            GetYCoordinate(false);
            string b = read.ReadString();
            if (origin)
            {
                ms.Position = 0;
            }
            return b;
        }
        public int GetCheckpointMapID(bool origin)
        {
            GetDirection(false);
            int a = read.ReadInt32();
            if (!OtherConsts.MapList.Contains(a)) throw new ArgumentException("Invalid Entry", nameof(a));
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public int GetCheckpointXCoordinate(bool origin)
        {
            GetCheckpointMapID(false);
            int a = read.ReadInt32();
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public int GetCheckpointYCoordinate(bool origin)
        {
            GetCheckpointXCoordinate(false);
            int a = read.ReadInt32();
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public int GetVolumeBgm(bool origin)
        {
            GetCheckpointYCoordinate(false);
            int a = read.ReadInt32();
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public int GetVolumeSfx(bool origin)
        {
            GetVolumeBgm(false);
            int a = read.ReadInt32();
            if (origin)
            {
                ms.Position = 0;
            }
            return a;
        }
        public bool SaveEnabled(bool origin)
        {
            GetVolumeSfx(false);
            bool c = read.ReadBoolean();
            if (origin)
            {
                ms.Position = 0;
            }
            return c;
        }
        public string GetLanguageID(bool origin)
        {
            SaveEnabled(false);
            string b = read.ReadString();
            if (origin)
            {
                ms.Position = 0;
            }
            return b;
        }
        public Unit UnitPass(bool origin)
        {
            // l'ID du nexomon
            Unit unit = new Unit(read.ReadInt16()) ;
            if (!MonstersConsts.MonsterIdList.Contains(unit.Id))
            {
                throw new ArgumentException("Invalid Entry", nameof(unit.Id));
            }
            bool c = read.ReadBoolean();// c'est pour voir s'il y a un nickname.
            if (c)
            {
                string b = read.ReadString();//nickname
                unit.NicknameExists = c;
                unit.Nickname = b;
            }
            else
            {
                unit.NicknameExists = c;
                unit.Nickname = String.Empty;
            }
            unit.Level = read.ReadInt16(); //level
            unit.hp = read.ReadInt16(); // hp
            unit.stamina = read.ReadInt16(); // stamina
            unit.xp = read.ReadInt16(); // xp
            byte d = read.ReadByte(); // sensé être le nombre de skills:
            for (int i = 0; i < (int)d; i++)
            {
                int a = read.ReadInt32();
                if (!SkillsConsts.SkillsIdList.Contains(a))
                {
                    throw new ArgumentException("Invalid Entry", nameof(a));
                }
                unit.AddSkill(a);//skill id je suppose
            }
            d = read.ReadByte(); //nombre de cores
            for (int i = 0; i < (int)d; i++)
            {
                int a = read.ReadInt32();
                if (!ItemsConsts.CoresIdList.Contains(a))
                {
                    throw new ArgumentException("Invalid Entry", nameof(a));
                }
                unit.AddCore(a); //core serialization code
            }
            c = read.ReadBoolean(); //BattleStatus
            if (c)
            {
                read.ReadString();
                read.ReadInt32();
                read.ReadInt32();
                c = read.ReadBoolean(); // "duration"
            }
            unit.cosmic = read.ReadBoolean();
            unit.harmony = (int)(read.ReadByte());
            if (origin)
            {
                ms.Position = 0;
            }
            return unit;
        }
        public Party PartyCtor(bool origin)
        {
            GetLanguageID(false);
            long posit = ms.Position;
            Party party1 = new Party();
            int number = read.ReadInt32();
            for (int i = 0; i < number; i++)
            {
                party1.Add(UnitPass(false));
            }
            if (origin)
            {
                ms.Position = 0;
            }
            return party1;
        }
        public Storage StorageBoxCtor(bool origin)
        {
            PartyCtor(false);
            int boxnumber = read.ReadInt32(); //nombre de boxes
            Storage storage1 = new Storage();
            for (int i = 0; i < boxnumber; i++)
            {
                Box box = storage1.CreateBox();
                box.Name = read.ReadString();
                int capacity = read.ReadInt32();
                if (!(0 <= capacity || capacity <= 60)) throw new ArgumentException("Invalid Entry", nameof(capacity));
                read.ReadInt32();
                for (int j = 0; j < capacity; j++)
                {
                    bool c = read.ReadBoolean();
                    if (c)
                    {
                        int a1 = j;
                        box.Add(a1, UnitPass(false));
                    }
                }
            }
            if (origin)
            {
                ms.Position = 0;
            }
            return (storage1);
        }
        public Inventory InventoryPass(bool origin)
        {
            StorageBoxCtor(false);
            Inventory inventory = new Inventory();
            int totalobjects = read.ReadInt32();
            for (int i = 0; i < totalobjects; i++)
            {
                int a = read.ReadInt32();
                if (!ItemsConsts.ItemsIdList.Contains(a))
                {
                    throw new ArgumentException("Invalid Entry", nameof(a));
                }
                inventory.Add(new Item(a, read.ReadInt32()));
            }
            if (origin)
            {
                ms.Position = 0;
            }
            return (inventory);
        }
        public Wallet WalletPass(bool origin)
        {
            InventoryPass(false);
            Wallet result = new Wallet(read.ReadInt32(), read.ReadInt32(), read.ReadInt32());
            if (origin)
            {
                ms.Position = 0;
            }
            return result;
        }
        public Tamers TamersPass(bool origin)
        {
            WalletPass(false);
            int total = read.ReadInt32();
            Tamers tamers = new Tamers();
            for (int i = 0; i < total; i++)
            {
                Tamer tamer = new Tamer(read.ReadString(), read.ReadInt32(), read.ReadInt32());
                tamers.Add(tamer);
                Debug.WriteLine($" Tamers : {tamer.Name}, {tamer.Int1}, {tamer.Int2} ");
            }
            if (origin)
            {
                ms.Position = 0;
            }
            return tamers;
        }
        public void MiningPass(bool origin)
        {
            TamersPass(false);
            int total1 = read.ReadInt32();
            for (int i = 0; i < total1; i++)
            {
                read.ReadInt16();
                int total2 = read.ReadInt16();
                for (int j = 0; j < total2; j++)
                {
                    Debug.WriteLine($"Mining {j}");
                    Debug.WriteLine(read.ReadInt16());
                    Debug.WriteLine(read.ReadInt32());
                }
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void RematherPass(bool origin)
        {
            MiningPass(false);
            int total = read.ReadInt32();
            Debug.WriteLine($"Total rematchers : {total}");
            for (int i = 0; i < total; i++)
            {
                Debug.WriteLine($"Rematcher {i}: {read.ReadString()}");
                read.ReadInt32();
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void AchievementsPass(bool origin)
        {
            RematherPass(false);
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                Debug.WriteLine($"Achievement {i} :");
                Debug.WriteLine($"{read.ReadString()}");
                Debug.WriteLine($"{read.ReadInt32()}");
                
            }
            total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadInt32();
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void EquipmentsPass(bool origin)
        {
            AchievementsPass(false);
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadInt32();
                read.ReadBoolean();
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void BeatenMonstersPass(bool origin)
        {
            EquipmentsPass(false);
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadInt16();
                read.ReadUInt16();
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void CustomPass(bool origin)
        {
            BeatenMonstersPass(false);
            read.ReadBoolean();
            read.ReadInt32();
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadByte();
                read.ReadBoolean();
            }
            total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadByte();
                read.ReadInt32();
            }
            total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadInt16();
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void LuringPass(bool origin)
        {
            CustomPass(false);
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadInt32();
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void SwitchPass(bool origin)
        {
            LuringPass(false);
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadInt16();
                read.ReadBoolean();
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void VariablesPass(bool origin)
        {
            SwitchPass(false);
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadString();
                read.ReadInt32();
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void DestroyedEntitiesPass(bool origin)
        {
            VariablesPass(false);
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadInt32();
                int total1 = read.ReadInt16();
                for (int j = 0; j < total1; j++)
                {
                    read.ReadInt16();
                }
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public void KilledFlagsPass(bool origin)
        {
            DestroyedEntitiesPass(false);
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                read.ReadInt32();
                int total1 = read.ReadInt32();
                for (int j = 0; j < total1; j++)
                {
                    read.ReadString();
                }
            }
            if (origin)
            {
                ms.Position = 0;
            }
        }
        public List<int> SeenMonstersPass(bool origin)
        {
            KilledFlagsPass(false);
            List<int> seenmonsters = new List<int>();
            int total = read.ReadInt32();
            for (int i = 0; i < total; i++)
            {
                int a = read.ReadInt16();
                seenmonsters.Add(a);
            }
            if (origin)
            {
                ms.Position = 0;
            }
            return seenmonsters;

        }
        public List<int> OwnedMonstersPass(bool origin)
        {
            SeenMonstersPass(false);
            int total = read.ReadInt32();
            List<int> ownedmonsters = new List<int>();
            for (int i = 0; i < total; i++)
            {
                int a = read.ReadInt16();
                ownedmonsters.Add(a);
            }
            if (origin)
            {
                ms.Position = 0;
            }
            return ownedmonsters;
        }
        public DataReader(BinaryReader read, MemoryStream ms)
        {
            this.read = read;
            this.ms = ms;
        }

    }
}
