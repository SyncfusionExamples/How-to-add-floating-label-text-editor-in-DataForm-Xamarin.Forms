using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using static Xamarin.Forms.Entry;
using Foundation;
using FloatingLabelTextEditor;
using FloatingLabelTextEditor.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]

namespace FloatingLabelTextEditor.iOS
{
    public class CustomEntryRenderer : ViewRenderer<CustomEntry, FloatLabeledTextField>
    {
        private readonly CGColor _editingUnderlineColor = UIColor.Blue.CGColor;
        private UIColor _defaultPlaceholderColor = UIColor.Red;
        private UIColor _defaultTextColor;
        private IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomEntry> e)
        {
            base.OnElementChanged(e);

            var base_entry = Element as CustomEntry;

            if (e.OldElement != null)
            {
                Control.EditingChanged -= ViewOnEditingChanged;
            }

            if (e.NewElement != null)
            {
                var ctrl = CreateNativeControl();
                SetNativeControl(ctrl);

                if (!string.IsNullOrWhiteSpace(Element.AutomationId))
                    SetAutomationId(Element.AutomationId);

                _defaultTextColor = Control.FloatingLabelTextColor;
                _defaultPlaceholderColor = Control.FloatingLabelTextColor;

                SetIsPassword();
                SetText();
                SetHintText();
                SetTextColor();
                SetBackgroundColor();
                SetPlaceholderColor();
                SetKeyboard();
                SetHorizontalTextAlignment();
                SetFont();

                Control.ErrorTextIsVisible = true;
                Control.EditingChanged += ViewOnEditingChanged;
            }

            if (Control != null && base_entry != null)
            {
                SetReturnType(base_entry);

                Control.ShouldReturn += (UITextField textField) =>
                {
                    base_entry?.InvokeCompleted();
                    return true;
                };
            }
        }

        void SetReturnType(CustomEntry entry)
        {
            var type = entry.ReturnType;

            switch (type)
            {
                case ReturnType.Go:
                    Control.ReturnKeyType = UIReturnKeyType.Go;
                    break;
                case ReturnType.Next:
                    Control.ReturnKeyType = UIReturnKeyType.Next;
                    break;
                case ReturnType.Send:
                    Control.ReturnKeyType = UIReturnKeyType.Send;
                    break;
                case ReturnType.Search:
                    Control.ReturnKeyType = UIReturnKeyType.Search;
                    break;
                case ReturnType.Done:
                    Control.ReturnKeyType = UIReturnKeyType.Done;
                    break;
                default:
                    Control.ReturnKeyType = UIReturnKeyType.Default;
                    break;
            }
        }

        protected virtual FloatLabeledTextField CreateNativeControl()
        {
            return new FloatLabeledTextField();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == PlaceholderProperty.PropertyName)
                SetHintText();
            else if (e.PropertyName == TextColorProperty.PropertyName)
                SetTextColor();
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                SetBackgroundColor();
            else if (e.PropertyName == IsPasswordProperty.PropertyName)
                SetIsPassword();
            else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            {
                SetIsPassword();
                SetTextColor();
            }
            else if (e.PropertyName == TextProperty.PropertyName)
                SetText();
            else if (e.PropertyName == PlaceholderColorProperty.PropertyName)
                SetPlaceholderColor();
            else if (e.PropertyName == Xamarin.Forms.InputView.KeyboardProperty.PropertyName)
                SetKeyboard();
            else if (e.PropertyName == HorizontalTextAlignmentProperty.PropertyName)
                SetHorizontalTextAlignment();
            else if ((e.PropertyName == FontAttributesProperty.PropertyName) ||
                     (e.PropertyName == FontFamilyProperty.PropertyName) ||
                     (e.PropertyName == FontSizeProperty.PropertyName))
                SetFont();
        }

        private void ViewOnEditingChanged(object sender, EventArgs eventArgs)
        {
            ElementController?.SetValueFromRenderer(TextProperty, Control.Text);
        }


        private void SetBackgroundColor()
        {
            NativeView.BackgroundColor = Element.BackgroundColor.ToUIColor();
        }

        private void SetText()
        {
            if (Control.Text != Element.Text)
                Control.Text = Element.Text;
        }

        private void SetIsPassword()
        {
            if (Element.IsPassword && Control.IsFirstResponder)
            {
                Control.Enabled = false;
                Control.SecureTextEntry = true;
                Control.Enabled = Element.IsEnabled;
                Control.BecomeFirstResponder();
            }
            else
            {
                Control.SecureTextEntry = Element.IsPassword;
            }
        }

        private void SetHintText()
        {
            Control.Placeholder = Element.Placeholder;
        }

        private void SetPlaceholderColor()
        {
            if (Element.PlaceholderColor == Color.Default)
            {
                Control.FloatingLabelTextColor = _defaultPlaceholderColor;
            }
            else
            {
                Control.FloatingLabelTextColor = Element.PlaceholderColor.ToUIColor();
                Control.AttributedPlaceholder = new NSAttributedString(Control.Placeholder, new UIStringAttributes { ForegroundColor = Element.PlaceholderColor.ToUIColor() });
            }
        }

        private void SetTextColor()
        {
            if ((Element.TextColor == Color.Default) || !Element.IsEnabled)
                Control.TextColor = _defaultTextColor;
            else
                Control.TextColor = Element.TextColor.ToUIColor();
        }

        private void SetFont()
        {
            //Control.Font = Element.ToUIFont();
        }

        private void SetHorizontalTextAlignment()
        {
            switch (Element.HorizontalTextAlignment)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                default:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

        private void SetKeyboard()
        {
            //var kbd = Element.Keyboard;
            //Control.KeyboardType = kbd;
            //Control.InputAccessoryView = kbd == UIKeyboardType.NumberPad ? NumberpadAccessoryView() : null;
            //Control.ShouldReturn = InvokeCompleted;
        }

        private UIToolbar NumberpadAccessoryView()
        {
            return new UIToolbar(new System.Drawing.RectangleF(0.0f, 0.0f, (float)Control.Frame.Size.Width, 44.0f))
            {
                Items = new[]
                {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { InvokeCompleted(null); })
                }
            };
        }

        private bool InvokeCompleted(UITextField textField)
        {
            Control.ResignFirstResponder();
            ((IEntryController)Element).SendCompleted();
            return true;
        }
    }
}
