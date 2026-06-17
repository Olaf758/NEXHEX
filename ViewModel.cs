using Avalonia.Controls;
using Avalonia.Platform.Storage;
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
    public partial class ViewModel : ObservableObject
    {
        public readonly TopLevel _topLevel;
        public readonly MainWindow _window;
        [ObservableProperty]
        private SaveFileManager _saveFileManagerUnit;
        [ObservableProperty]
        private Object currentViewModel;
        public ViewModel(MainWindow window)
        {
            _window = window;
            _topLevel = _window;
            CurrentViewModel = new Nex2ViewModel();
            SaveFileManagerUnit = new SaveFileManager(this);
        }
    }

}
