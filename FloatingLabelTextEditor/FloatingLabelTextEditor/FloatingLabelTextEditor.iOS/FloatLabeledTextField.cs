using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FloatingLabelTextEditor.iOS
{
    public class FloatLabeledTextField : UITextField
    {
        private readonly UILabel _errorLabel;
        private readonly UILabel _floatingLabel;

        public FloatLabeledTextField()
        {
            _floatingLabel = new UILabel { Alpha = 0.0f };
            _errorLabel = new UILabel { Font = UIFont.SystemFontOfSize(11) };

            AddSubview(_errorLabel);
            AddSubview(_floatingLabel);

            BorderStyle = UITextBorderStyle.None;
            ErrorTextColor = UIColor.Red;
            ErrorTextIsVisible = false;
            FloatingLabelTextColor = Color.FromHex("#141414").ToUIColor();
            TextColor = Color.FromHex("#141414").ToUIColor();

            FloatingLabelActiveTextColor = Color.FromHex("#141414").ToUIColor();
            FloatingLabelFont = UIFont.BoldSystemFontOfSize(14);

        }

        public UIColor FloatingLabelTextColor { get; set; }
        public UIColor FloatingLabelActiveTextColor { get; set; }
        public bool FloatingLabelEnabled { get; set; } = true;
        public UIColor ErrorTextColor
        {
            get { return _errorLabel.TextColor; }
            set { _errorLabel.TextColor = value; }
        }

        public bool ErrorTextIsVisible
        {
            get { return !_errorLabel.Hidden; }
            set
            {
                _errorLabel.Hidden = !value;
            }
        }

        public UIFont FloatingLabelFont
        {
            get { return _floatingLabel.Font; }
            set { _floatingLabel.Font = value; }
        }

        public string ErrorText
        {
            get { return _errorLabel.Text; }
            set
            {
                _errorLabel.Text = value;
                _errorLabel.SizeToFit();
                _errorLabel.Frame =
                    new CGRect(
                        0,
                        _errorLabel.Font.LineHeight + 30,
                        _errorLabel.Frame.Size.Width,
                        _errorLabel.Frame.Size.Height);
            }
        }

        public override string Placeholder
        {
            get { return base.Placeholder; }
            set
            {
                base.Placeholder = value;

                _floatingLabel.Text = value;
                _floatingLabel.SizeToFit();
                _floatingLabel.Frame =
                    new CGRect(
                        0,
                        _floatingLabel.Font.LineHeight,
                        _floatingLabel.Frame.Size.Width,
                        _floatingLabel.Frame.Size.Height);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Action updateLabel = () =>
            {
                if (!string.IsNullOrEmpty(Text) && FloatingLabelEnabled)
                {
                    _floatingLabel.Alpha = 1.0f;
                    _floatingLabel.Frame =
                        new CGRect(
                            _floatingLabel.Frame.Location.X,
                            2.0f,
                            _floatingLabel.Frame.Size.Width,
                            _floatingLabel.Frame.Size.Height);
                }
                else
                {
                    _floatingLabel.Alpha = 0.0f;
                    _floatingLabel.Frame =
                        new CGRect(
                            _floatingLabel.Frame.Location.X,
                            _floatingLabel.Font.LineHeight,
                            _floatingLabel.Frame.Size.Width,
                            _floatingLabel.Frame.Size.Height);
                }
            };

            if (IsFirstResponder)
            {
                _floatingLabel.TextColor = FloatingLabelActiveTextColor;

                var shouldFloat = !string.IsNullOrEmpty(Text) && FloatingLabelEnabled;
                var isFloating = _floatingLabel.Alpha == 1f;

                if (shouldFloat == isFloating)
                {
                    updateLabel();
                }
                else
                {
                    Animate(
                        0.3f,
                        0.0f,
                        UIViewAnimationOptions.BeginFromCurrentState
                        | UIViewAnimationOptions.CurveEaseOut,
                        () => updateLabel(),
                        () => { });
                }
            }
            else
            {
                _floatingLabel.TextColor = FloatingLabelTextColor;

                updateLabel();
            }
        }

    }
}