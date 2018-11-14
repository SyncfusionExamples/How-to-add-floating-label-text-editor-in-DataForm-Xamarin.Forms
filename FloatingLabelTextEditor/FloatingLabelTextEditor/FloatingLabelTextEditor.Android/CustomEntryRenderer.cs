using System;
using System.ComponentModel;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views.InputMethods;
using Android.Text;
using Android.Widget;
using FloatingLabelTextEditor;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;
using Android.Content.Res;
using NativeColor = Android.Graphics.Color;
using Syncfusion.Android.DataForm;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(FloatingLabelTextEditor.Droid.CustomEntryRenderer))]
namespace FloatingLabelTextEditor.Droid
{

    #region Renderer

    public class CustomEntryRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Entry, TextInputLayout>
    {
        private EditText _defaultEditTextForValues;
        private bool _preventTextLoop;

        private CustomEntry CustomEntry { get; set; }
        private EditText EditText => Control.EditText;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                EditText.FocusChange -= ControlOnFocusChange;
                Control.EditText.KeyPress -= EditTextOnKeyPress;
                Control.EditText.TextChanged -= EditTextOnTextChanged;
            }

            if (e.NewElement != null)
            {
                var nativeControl = CreateNativeControl();
                CustomEntry = e.NewElement as CustomEntry;
                SetNativeControl(nativeControl);
                _defaultEditTextForValues = new EditText(Context);

                Focusable = true;
                Control.HintEnabled = true;
                Control.HintAnimationEnabled = true;
                EditText.ShowSoftInputOnFocus = true;

                EditText.FocusChange += ControlOnFocusChange;
                EditText.ImeOptions = ImeAction.Done;

                SetText();
                SetHintText();
                SetTextColor();
                SetBackgroundColor();
                SetHintColor();
                SetIsPassword();
                SetKeyboard();

                SetBorder((e.NewElement as CustomEntry).BottomBorderColor.ToAndroid());

                var nativeEditText = Control.EditText;

                Control.EditText.TextChanged += EditTextOnTextChanged;
                Control.EditText.KeyPress += EditTextOnKeyPress;
            }
        }

        private void ControlOnFocusChange(object sender, FocusChangeEventArgs args)
        {
            if (args.HasFocus)
            {
                var manager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);

                EditText.PostDelayed(() =>
                {
                    EditText.RequestFocus();
                    manager.ShowSoftInput(EditText, 0);
                },
                    100);
            }
            else
            {
                CustomEntry.SfDataForm.Validate("Name");
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
            {
                SetHintText();
            }

            if (e.PropertyName == Entry.TextColorProperty.PropertyName)
            {
                SetTextColor();
            }

            if (e.PropertyName == Entry.BackgroundColorProperty.PropertyName)
            {
                SetBackgroundColor();
            }

            if (e.PropertyName == Entry.IsPasswordProperty.PropertyName)
            {
                SetIsPassword();
            }

            if (e.PropertyName == Entry.TextProperty.PropertyName)
            {
                SetText();
            }

            if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
            {
                SetHintColor();
            }

            if (e.PropertyName == InputView.KeyboardProperty.PropertyName)
            {
                SetKeyboard();
            }
        }
        private void ElementOnHideKeyboard(object sender, EventArgs eventArgs)
        {
            var manager = (InputMethodManager)Android.App.Application.Context.GetSystemService(Context.InputMethodService);
            manager.HideSoftInputFromWindow(Control.EditText.WindowToken, 0);
        }

        private void SetIsPassword()
        {
            Control.EditText.InputType = Element.IsPassword
                ? InputTypes.TextVariationPassword | InputTypes.ClassText
                : Control.EditText.InputType;
        }

        private void SetBackgroundColor()
        {
            Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
        }

        private void SetHintText()
        {
            Control.Hint = Element.Placeholder;
        }

        private void SetHintColor()
        {
            if (Element.PlaceholderColor == Color.Default)
            {
                Control.EditText.SetHintTextColor(_defaultEditTextForValues.HintTextColors);
            }
            else
            {
                Control.EditText.SetHintTextColor(Element.PlaceholderColor.ToAndroid());
            }
        }

        private void SetTextColor()
        {
            if (Element.TextColor == Color.Default)
            {
                Control.EditText.SetTextColor(_defaultEditTextForValues.TextColors);
            }
            else
            {
                Control.EditText.SetTextColor(Element.TextColor.ToAndroid());
            }
        }

        private void SetKeyboard()
        {
            Control.EditText.InputType = Element.Keyboard.ToNative();
        }


        private void SetBorder(NativeColor borderColor)
        {
            var nativeEntry = (Android.Support.V4.View.ITintableBackgroundView)EditText;
            if (nativeEntry != null)
                nativeEntry.SupportBackgroundTintList = ColorStateList.ValueOf(borderColor);
        }
        protected override TextInputLayout CreateNativeControl()
        {
            var layout = (TextInputLayout)LayoutInflater.From(Context).Inflate(Resource.Layout.TextInputLayout, null);
            var inner = layout.FindViewById(Resource.Id.textInputLayout);
            if (!string.IsNullOrWhiteSpace(Element.AutomationId))
            {
                inner.ContentDescription = Element.AutomationId;
            }

            return layout;
        }
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            this.SetMeasuredDimension(widthMeasureSpec, heightMeasureSpec);
        }

        protected override void MeasureChildWithMargins(Android.Views.View child, int parentWidthMeasureSpec, int widthUsed, int parentHeightMeasureSpec, int heightUsed)
        {
            base.MeasureChildWithMargins(child, parentWidthMeasureSpec, widthUsed, parentHeightMeasureSpec, heightUsed);
        }

        protected override void MeasureChild(Android.Views.View child, int parentWidthMeasureSpec, int parentHeightMeasureSpec)
        {
            base.MeasureChild(child, parentWidthMeasureSpec, parentHeightMeasureSpec);
        }

        protected override void MeasureChildren(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.MeasureChildren(widthMeasureSpec, heightMeasureSpec);
        }

        public override void MeasureAndLayout(int p0, int p1, int p2, int p3, int p4, int p5)
        {
            base.MeasureAndLayout(p0, p1, p2, p3, p4, p5);
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
        }
        private void EditTextOnKeyPress(object sender, KeyEventArgs args)
        {
            args.Handled = args.KeyCode == Keycode.Enter;
            if (args.KeyCode == Keycode.Enter && args.Event.Action == KeyEventActions.Up)
            {
                Control.ClearFocus();
                HideKeyboard();
                ((IEntryController)Element).SendCompleted();
            }
        }

        private void EditTextOnTextChanged(object sender, Android.Text.TextChangedEventArgs args)
        {
            var selection = Control.EditText.SelectionStart;
            if (!_preventTextLoop)
            {
                Element.Text = args.Text.ToString();
            }
            if (Element == null || Element.Text == null) return;

            var index = selection > Element.Text.Length ? Element.Text.Length : selection;
            Control.EditText.SetSelection(index);
        }

        private void SetText()
        {
            _preventTextLoop = true;
            if (Control.EditText.Text != Element.Text)
            {
                Control.EditText.Text = Element.Text;
                if (EditText.IsFocused)
                    EditText.SetSelection(EditText.Text.Length);
            }
            _preventTextLoop = false;
        }

        protected override void Dispose(bool disposing)
        {
            Control.EditText.KeyPress -= EditTextOnKeyPress;
            Control.EditText.TextChanged -= EditTextOnTextChanged;
            base.Dispose(disposing);
        }

        protected void HideKeyboard()
        {
            var manager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
            manager.HideSoftInputFromWindow(EditText.WindowToken, 0);
        }
    }
    #endregion

    public static class KeyboardExtensions
    {
        public static InputTypes ToNative(this Keyboard input)
        {
            if (input == Keyboard.Url)
            {
                return InputTypes.ClassText | InputTypes.TextVariationUri;
            }
            if (input == Keyboard.Email)
            {
                return InputTypes.ClassText | InputTypes.TextVariationEmailAddress;
            }
            if (input == Keyboard.Numeric)
            {
                return InputTypes.ClassNumber;
            }
            if (input == Keyboard.Chat)
            {
                return InputTypes.ClassText | InputTypes.TextVariationShortMessage;
            }
            if (input == Keyboard.Telephone)
            {
                return InputTypes.ClassPhone;
            }
            if (input == Keyboard.Text)
            {
                return InputTypes.ClassText | InputTypes.TextFlagNoSuggestions;
            }
            return InputTypes.ClassText;
        }
    }
}
