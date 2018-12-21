using FloatingLabelTextEditor;
using FloatingLabelTextEditor.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace FloatingLabelTextEditor.UWP
{
    public class CustomEntryRenderer : EntryRenderer
    {
        internal TextBox NativeTextBox { get; set; }
        internal string PlaceholderText { get; set; }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element is CustomEntry)
            {
                PlaceholderText = Control.PlaceholderText;
                Control.Style = (Style)Application.Current.Resources["FloatingLabelStyle"];
                NativeTextBox = Control as TextBox;

                NativeTextBox.GotFocus += NativeTextBox_GotFocus;
                NativeTextBox.LostFocus += NativeTextBox_LostFocus;

                NativeTextBox.BorderBrush = new SolidColorBrush(Colors.Green);

                if (!string.IsNullOrEmpty(NativeTextBox.Text))
                    NativeTextBox.Header = NativeTextBox.PlaceholderText;
            }
        }

        private void NativeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NativeTextBox.PlaceholderText = PlaceholderText;
            if (string.IsNullOrEmpty(NativeTextBox.Text))
                NativeTextBox.Header = string.Empty;
        }

        private void NativeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var color = Element.PlaceholderColor;
            var windowsColor = Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
            NativeTextBox.Header = new TextBlock() { Text = Element.Placeholder, Foreground = new SolidColorBrush(windowsColor) };

            NativeTextBox.PlaceholderText = string.Empty;

            if (string.IsNullOrEmpty(NativeTextBox.Text))
                NativeTextBox.Text = string.Empty;
        }
    }
}
