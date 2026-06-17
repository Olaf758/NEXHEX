using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using Nexomon2Model;
using System.Linq;
using System.Text.Json;
using Avalonia.Controls.Notifications;
using System;

namespace NEXHEX
{
    
    public partial class MainWindow : Window
    {
        private WindowNotificationManager notificationManager;
        public MainWindow()
        {
            InitializeComponent();
            this.CanResize = false;
            notificationManager = new WindowNotificationManager(this)
            {
                Position = NotificationPosition.BottomCenter,
                MaxItems = 5
            };
            DataContext = new ViewModel(this);
        }
        public void ShowNotification(string title, string message, NotificationType type = NotificationType.Information, int seconds = 5)
        {
            notificationManager.Show(new Notification(
                title:title,
                message:message,
                type:type,
                expiration: TimeSpan.FromSeconds(seconds)
                ));
        }
    }
}