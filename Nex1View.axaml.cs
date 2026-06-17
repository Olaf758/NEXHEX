using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NEXHEX
{
    public partial class Nex1View : UserControl
    {
        public Nex1View()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}