using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nexomon2Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NEXHEX
{
    public partial class SaveFileManager : ObservableObject
    {
        private ViewModel viewmodel;
        [RelayCommand]
        private async Task SelectSaveFile()
        {
            if (viewmodel._topLevel == null) { return; }
            var files = await viewmodel._topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Choose a save file from Nexomon or Nexomon:Extinction",
                AllowMultiple = false,
                SuggestedStartLocation = await viewmodel._topLevel.StorageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.Desktop),
                FileTypeFilter = new List<FilePickerFileType> { new("Save file") { Patterns = new[] { "*.dat" } } }
            }
                );
            if (files.Count > 0)
            {
                var file = files[0];
                //DatachangeNexomonEX
                string filename = file.TryGetLocalPath()!;
                if (!(Path.GetFileName(filename).Equals("nexomon-save.dat")))
                {
                    CreateNex2Model(filename);
                }
                else
                {
                    CreateNex1Model(filename);
                }
            }
        }
        public void CreateNex2Model(string path)
        {
            try
            {
                Nex2ViewModel nex2vm = new Nex2ViewModel(path);
                nex2vm.Nex2StorageManagerUnit.SelectedBox = nex2vm.ActualSave.Storage.Boxes[0];
                viewmodel.CurrentViewModel = nex2vm;
            }
            catch (Exception ex)
            {
                string ToBeNotified = ex.Message;
                viewmodel._window.ShowNotification("Message", ToBeNotified);
            }
        }
        public void CreateNex1Model(string path)
        {
            try
            {
                Nex1ViewModel nex1vm = new Nex1ViewModel(path);
                viewmodel.CurrentViewModel = nex1vm;
            }
            catch (Exception ex)
            {
                string ToBeNotified = ex.Message;
                viewmodel._window.ShowNotification("Message", ToBeNotified);
            }
        }
        [RelayCommand]
        private async Task SaveToDir()
        {
            if (viewmodel._topLevel == null) { return; }
            var folder = await viewmodel._topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Choose where to save the file",
                AllowMultiple = false,
                SuggestedStartLocation = await viewmodel._topLevel.StorageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.Desktop)
            }
                );
            if (folder.Count != 0)
            {
                string Folder = folder[0].Path?.LocalPath ?? string.Empty;
                if(viewmodel.CurrentViewModel is Nex2ViewModel)
                {

                    Nex2ViewModel vm = (Nex2ViewModel)viewmodel.CurrentViewModel;
                    vm.SaveTo(Folder);
                }
                else if(viewmodel.CurrentViewModel is Nex1ViewModel)
                {
                    Nex1ViewModel vm = (Nex1ViewModel)viewmodel.CurrentViewModel;
                    vm.ActualSave.SaveToDir(Folder);
                }
            }
        }
        public SaveFileManager(ViewModel viewmodel)
        {
            this.viewmodel = viewmodel;
        }
    }
}
