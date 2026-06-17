using System.Collections.Generic;
using System.IO;
using static Nexomon2Model.DataReader;

namespace Nexomon2Model
{
    public class DataWriter
    {
        public DataReader read { get; set; }
        public BinaryWriter writer { get; set; }
        public void SetPlayerState(PlayerState player)
        {
            read.GetBeginningText(false);
            WriteString(player.Name);
            WriteString(player.Body);
            SetLocation(player.Location);
            SetWallet(player.Wallet);
        }

        public void SetPetBody(string body)
        {
            read.ms.Position = 0;
            read.GetPlayTime(false);
            WriteString(body);
            read.ms.Position = 0;
        }
        public void SetLocation(Location location)
        {
            read.ms.Position = 0;
            read.GetPetBody(false);
            writer.Write(location.MapId);
            writer.Write(location.XCoord);
            writer.Write(location.YCoord);
            WriteString(location.Direction);
            writer.Write(location.CptMapId);
            writer.Write(location.CptXCoord);
            writer.Write(location.CptYCoord);
            read.ms.Position = 0;
        }
        public void WriteUnit(Unit unit)
        {
            writer.Write((short)unit.Id);
            if (unit.NicknameExists)
            {
                writer.Write(true);
                writer.Write(unit.Nickname);
            }
            else
            {
                writer.Write(false);
            }
            writer.Write((short)(unit.Level));
            writer.Write((short)(unit.hp));
            writer.Write((short)(unit.stamina));
            writer.Write((short)(unit.xp));
            writer.Write((byte)(unit.NumberSkills));
            foreach (Slot<Skill> skillslot in unit.skills)
            {
                if(skillslot.Content!.Id!=-1)
                writer.Write(skillslot.Content.Id);
            }
            writer.Write((byte)(unit.NumberCores));
            foreach (Slot<Item.ItemBase> coreslot in unit.cores)
            {
                if(coreslot.Content!.Id!=-1)
                writer.Write(coreslot.Content!.Id);
            }
            writer.Write(unit.BattleStatus);
            if (unit.BattleStatus)
            {
                writer.Write(unit.BattleStatus1);
                writer.Write(unit.BattleStatus2);
                writer.Write(unit.BattleStatusDuration);
            }
            writer.Write(unit.cosmic);
            writer.Write((byte)unit.harmony);
        }

        public void SetParty(Party team)
        {
            read.ms.Position = 0;
            read.PartyCtor(false);
            byte[] tail1 = read.read.ReadBytes((int)(read.ms.Length - read.ms.Position));
            read.GetLanguageID(false);
            read.ms.SetLength(read.ms.Position);
            writer.Write(team.totalunits);
            foreach (Slot<Unit> slot in team.Units)
            {
                if (slot.Content != Unit.NexoNull)
                WriteUnit(slot.Content);
            }
            writer.Write(tail1);
            read.ms.Position = 0;
        }

        public void SetStorageBox(Storage stor)
        {
            read.StorageBoxCtor(false);
            long position = read.ms.Position;
            byte[] tail = read.read.ReadBytes((int)(read.ms.Length - position));
            read.ms.Position = 0;
            read.PartyCtor(false);
            writer.Write(stor.Boxes.Count);
            foreach (Box box in stor.Boxes)
            {
                writer.Write(box.Name);
                writer.Write(60);
                writer.Write(60);
                foreach (Slot<Unit> slot in box.Units)
                {
                    if (slot.Content !=Unit.NexoNull)
                    {
                        writer.Write(true);
                        WriteUnit(slot.Content);
                    }
                    else
                        writer.Write(false);
                }
            }
            read.ms.SetLength(read.ms.Position);
            read.ms.Position = read.ms.Length;
            writer.Write(tail);
            read.ms.Position = 0;

        }

        public void SetInventory(Inventory invent)
        {
            read.ms.Position = 0;
            read.InventoryPass(false);
            long position = read.ms.Position;
            byte[] tail = read.read.ReadBytes((int)(read.ms.Length - position));
            read.ms.Position = 0;
            read.StorageBoxCtor(false);
            writer.Write(invent.Items.Count);
            foreach (Item item in invent.Items)
            {
                writer.Write(item.Id);
                writer.Write(item.Quantity);
            }
            read.ms.SetLength(read.ms.Position);
            read.ms.Position = read.ms.Length;
            writer.Write(tail);
            read.ms.Position = 0;
        }
        public void SetWallet(Wallet wallet)
        {
            read.ms.Position = 0;
            read.InventoryPass(false);
            writer.Write(wallet.Money);
            writer.Write(wallet.Diamonds);
            writer.Write(wallet.Tokens);
            read.ms.Position = 0;
        }
        public void SeeMonster(int id)
        {
            List<int> SeenMonsters = read.SeenMonstersPass(true);
            if (SeenMonsters.Contains(id)) return;
            read.KilledFlagsPass(false);
            writer.Write(SeenMonsters.Count + 1);
            foreach(int monster in SeenMonsters)
            {
                read.read.ReadInt16();
            }
            writer.Write((short)id);
            read.ms.Position = 0;
        }
        public void SeeMonsters(List<int> ids)
        {
            List<int> SeenMonsters = read.SeenMonstersPass(true);
            List<int> Ids = new List<int>(ids);
            foreach(int id in ids)
            {
                if (SeenMonsters.Contains(id)) Ids.Remove(id);
            }
            read.KilledFlagsPass(false);
            writer.Write(SeenMonsters.Count + Ids.Count);
            foreach (int monster in SeenMonsters)
            {
                read.read.ReadInt16();
            }
            long pos = read.ms.Position;
            byte[] tail = read.read.ReadBytes((int)(read.ms.Length - read.ms.Position));
            read.ms.Position = pos;
            foreach(int monster in Ids)
            writer.Write((short)monster);
            writer.Write(tail);
            read.ms.Position = 0;
        }
        public void OwnMonster(int id)
        {
            List<int> OwnedMonsters = read.OwnedMonstersPass(true);
            if (OwnedMonsters.Contains(id)) return;
            read.SeenMonstersPass(false);
            writer.Write(OwnedMonsters.Count + 1);
            foreach (int monster in OwnedMonsters)
            {
                read.read.ReadInt16();
            }
            writer.Write((short)id);
            read.ms.Position = 0;
        }
        public void OwnMonsters(List<int> ids)
        {
            List<int> OwnedMonsters = read.OwnedMonstersPass(true);
            List<int> Ids = new List<int>(ids);
            foreach (int id in ids)
            {
                if (OwnedMonsters.Contains(id)) Ids.Remove(id);
            }
            read.SeenMonstersPass(false);
            writer.Write(OwnedMonsters.Count + Ids.Count);
            foreach (int monster in OwnedMonsters)
            {
                read.read.ReadInt16();
            }
            long pos = read.ms.Position;
            byte[] tail = read.read.ReadBytes((int)(read.ms.Length - read.ms.Position));
            read.ms.Position = pos;
            foreach (int monster in Ids)
                writer.Write((short)monster);
            writer.Write(tail);
            read.ms.Position = 0;
        }
        public void WriteString(string text)
        {
            long pos = read.ms.Position;
            string b = read.read.ReadString();
            byte[] tail1 = read.read.ReadBytes((int)(read.ms.Length - read.ms.Position));
            read.ms.Position = pos;
            writer.Write(text);
            pos = (int)read.ms.Position;
            read.ms.SetLength(read.ms.Position);
            read.ms.Position = read.ms.Length;
            writer.Write(tail1);
            read.ms.Position = pos;
        }
        public DataWriter(DataReader read, BinaryWriter writer)
        {
            this.read = read;
            this.writer = writer;
        }
    }
}
