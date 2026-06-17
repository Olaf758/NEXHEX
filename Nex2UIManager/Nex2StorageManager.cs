using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nexomon2Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;
using Avalonia.Media.Imaging;
using System.Diagnostics;
namespace NEXHEX
{
    public partial class Nex2StorageManager : ObservableObject 
    {

        private Nex2ViewModel viewmodel;
        [ObservableProperty]
        private Box _selectedBox;
        public bool canmovenext
        {
            get
            {
                if (SelectedBox != null) return SelectedBox.Id < viewmodel.ActualSave.Storage.Boxes.Count;
                return false;
            }
        }
        public bool canmoveback
        {
            get
            {
                if(SelectedBox != null) return SelectedBox.Id > 1;
                return false;
            }
        }
        [RelayCommand]
        public void GoNext()
        {
            if (canmovenext)
            {
                SelectedBox = viewmodel.ActualSave.Storage.Boxes[viewmodel.ActualSave.Storage.Boxes.IndexOf(SelectedBox) + 1];
                RefreshState();
            }
        }
        [RelayCommand]
        public void GoBack()
        {
            if (canmoveback)
            {
                SelectedBox = viewmodel.ActualSave.Storage.Boxes[viewmodel.ActualSave.Storage.Boxes.IndexOf(SelectedBox) - 1];
                RefreshState();
            }
        }
        partial void OnSelectedBoxChanged(Box? oldValue, Box newValue)
        {
            RefreshState();
        }
        public void RefreshState()
        {
            OnPropertyChanged(nameof(canmoveback));
            OnPropertyChanged(nameof(canmovenext));
        }
        [RelayCommand]
        public void CompleteNexodex()
        {
            viewmodel.ActualSave.GetThemAll();
            viewmodel.Nex2StorageManagerUnit.RefreshState();
        }
        public Nex2StorageManager(Nex2ViewModel viewmodel)
        {
            this.viewmodel = viewmodel;
            SelectedBox = viewmodel.ActualSave.Storage.Boxes[0];
        }
    }
}
