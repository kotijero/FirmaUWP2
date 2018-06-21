using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Firma.CustomControls
{
    
    public sealed partial class ValidationTextBox : UserControl
    {
        public ValidationTextBox()
        {
            this.InitializeComponent();
            ValueTextBox.InputScope = new InputScope()
                {
                    Names = { new InputScopeName(InputScopeNameValue.Number) }
                };
        }

        public string AttributeName { get; set; }
        public string PlaceHolderText { get; set; }

        
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ValidationTextBox), new PropertyMetadata(null));
        
        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(ValidationTextBox), new PropertyMetadata(null));



        public string InputScope
        {
            get { return (string)GetValue(InputScopeProperty); }
            set
            {
                SetValue(InputScopeProperty, value);
            }
        }

        private void SetInputScope(string inputScopeName)
        {
            switch (inputScopeName)
            {
                case "Number":
                    ValueTextBox.InputScope = new InputScope()
                    {
                        Names = { new InputScopeName(InputScopeNameValue.Number) }
                    };
                    break;
                case "Text":
                    ValueTextBox.InputScope = new InputScope()
                    {
                        Names = { new InputScopeName(InputScopeNameValue.Text) }
                    };
                    break;
            }
        }

        // Using a DependencyProperty as the backing store for InputScope.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputScopeProperty =
            DependencyProperty.Register(nameof(InputScope), typeof(string), typeof(ValidationTextBox), new PropertyMetadata(null));



        public Action<string> TextChangedAction { get; set; }

        private int totalElementWidth;

        public int TotalElementWidth
        {
            get { return totalElementWidth; }
            set
            {
                totalElementWidth = value;
                ValueTextBox.Width = totalElementWidth;
            }
        }

        private void ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                TextChangedAction?.Invoke(AttributeName);
        }

        private void ValueTextBox_FocusDisengaged(Control sender, FocusDisengagedEventArgs args)
        {
            TextChangedAction?.Invoke(AttributeName);
        }
    }
}
