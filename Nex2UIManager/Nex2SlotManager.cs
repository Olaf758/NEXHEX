using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nexomon2Model;
using System.Diagnostics;

namespace NEXHEX
{
    public partial class Nex2SlotManager : ObservableObject
    {
        private Nex2ViewModel viewmodel;
        [RelayCommand]
        public void Edit(Slot<Unit> slot)
        {
            if (slot.Content != Unit.NexoNull)
            {
                Debug.WriteLine($"Editing {slot.Content.Name}");
                viewmodel.ActualNexomon = new Slot<Unit>(new Unit(slot.Content));
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
            slot.Content = new Unit(viewmodel.ActualNexomon.Content);
        }
        public Nex2SlotManager(Nex2ViewModel viewModel)
        {
            this.viewmodel = viewModel;
        }
    }
}
