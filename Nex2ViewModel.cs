using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Nexomon2Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NEXHEX
{
    public partial class Nex2ViewModel : ObservableObject
    {
        private bool unitbool = true;
        [ObservableProperty]
        private SaveData _actualSave;
        [ObservableProperty]
        private string _actualNexomonName;
        [ObservableProperty]
        private Slot<Unit> _actualNexomon;
        [ObservableProperty]
        private Nex2StorageManager _nex2StorageManagerUnit;
        [ObservableProperty]
        private Nex2SlotManager _nex2SlotManagerUnit;
        public Nex2ViewModel(string savedir)
        {
            ActualSave = new SaveData(savedir);
            Nex2StorageManagerUnit = new Nex2StorageManager(this);
            ActualNexomonName = MonstersConsts.MonstersNameList[0];
            ActualNexomon = new Slot<Unit>(new Unit(1));
            Nex2SlotManagerUnit = new Nex2SlotManager(this);
        }
        public Nex2ViewModel()
        {
            ActualSave = new SaveData();
            Nex2StorageManagerUnit = new Nex2StorageManager(this);
            ActualNexomonName = MonstersConsts.MonstersNameList[0];
            ActualNexomon = new Slot<Unit>(new Unit(1));
            Nex2SlotManagerUnit = new Nex2SlotManager(this);
        }
    }
    public partial class Nex2ViewModel : ObservableObject
    {
        partial void OnActualNexomonChanged(Slot<Unit>? oldValue, Slot<Unit> newValue)
        {
            if (unitbool)
            {
                unitbool = false;
                if (newValue.Content != Unit.NexoNull)
                    ActualNexomonName = MonstersConsts.MonstersNameList[newValue.Content.Id - 1];
                unitbool = true;
            }
        }
        partial void OnActualNexomonNameChanged(string? oldValue, string newValue)
        {
            if (unitbool)
            {
                unitbool = false;
                ActualNexomon = new Slot<Unit>(new Unit(MonstersConsts.GetMonsterId(newValue)));
                unitbool = true;
            }
        }
        public void SaveTo(string dir)
        {
            ActualSave.SaveFileToDirectory(dir);
        }
    }
    public partial class Nex2ViewModel : ObservableObject
    {
        public Bitmap DefaultBitmap => OtherConsts.DefaultBitmap;
        public ObservableCollection<Skill> SkillsListBuilt => SkillsConsts.SkillsListBuilt;
        public ObservableCollection<Item.ItemBase> CoresListBuilt => ItemsConsts.CoresListBuilt;
        public List<string> MonsterNameList { get; } = MonstersConsts.MonstersNameList;
        public List<string> PlayerBodiesList { get; } = OtherConsts.PlayerBodyList;
        public List<string> PetBodiesList { get; } = OtherConsts.PetBodyList;
    }

}
