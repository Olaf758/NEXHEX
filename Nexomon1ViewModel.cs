using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Nexomon1Model;

namespace NEXHEX
{
    public partial class Nex1ViewModel : ObservableObject
    {
        private bool unitbool = true;
        [ObservableProperty]
        private string _actualNexomonName;
        [ObservableProperty]
        private Slot<Unit> _actualNexomon;
        [ObservableProperty]
        private SaveData _actualSave;
        [ObservableProperty]
        private Nex1SlotManager _nex1SlotManagerUnit;
        partial void OnActualNexomonChanged(Slot<Unit>? oldValue, Slot<Unit> newValue)
        {
            if (unitbool)
            {
                unitbool = false;
                if (newValue.Content != null)
                    ActualNexomonName = NexomonNames[NexomonNames.IndexOf(newValue.Content.name)];
                unitbool = true;
            }
        }
        partial void OnActualNexomonNameChanged(string? oldValue, string newValue)
        {
            if (unitbool)
            {
                unitbool = false;
                ActualNexomon = new Slot<Unit>(new Unit(newValue));
                unitbool = true;
            }
        }
        public Nex1ViewModel(string path)
        {
            ActualSave = new SaveData(path);
            ActualNexomonName = NexomonNames[0];
            ActualNexomon = new Slot<Unit>(new Unit(ActualNexomonName, 1));
            Nex1SlotManagerUnit = new Nex1SlotManager(this);
        }
        [RelayCommand]
        public void GetThemAll()
        {
            ActualSave.playerHatchery.GetAll(ActualSave);
        }
    }
    public partial class Nex1ViewModel : ObservableObject
    {
        ObservableCollection<string> NexomonNames => Consts.MonstersNames;
        ObservableCollection<Skill> Skills => Consts.Skills;
    }
}
