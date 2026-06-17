using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Nexomon1Model;

namespace NEXHEX
{
    public partial class Nex1SlotManager : ObservableObject
    {
        private Nex1ViewModel nex1viewmodel;
        [RelayCommand]
        public void Edit(Slot<Unit> slot)
        {
            if (slot.Content != Unit.NexoNull)
            {
                nex1viewmodel.ActualNexomon = new Slot<Unit>(new Unit(slot.Content));
            }
        }
        [RelayCommand]
        public void Delete(Slot<Unit> slot)
        {
            if (slot.Content != Unit.NexoNull)
            {
                slot.Content = Unit.NexoNull;
            }
        }
        [RelayCommand]
        public void Set(Slot<Unit> slot)
        {
            slot.Content = new Unit(nex1viewmodel.ActualNexomon.Content);
        }
        public Nex1SlotManager(Nex1ViewModel nex1viewmodel)
        {
            this.nex1viewmodel = nex1viewmodel;
        }
    }
}
