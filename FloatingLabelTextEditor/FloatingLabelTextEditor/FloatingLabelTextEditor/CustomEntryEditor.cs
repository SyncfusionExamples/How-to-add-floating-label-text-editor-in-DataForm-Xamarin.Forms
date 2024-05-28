using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Syncfusion.XForms.DataForm;
using Syncfusion.XForms.DataForm.Editors;
using Xamarin.Forms;

namespace FloatingLabelTextEditor
{
    public class CustomEntryEditor : DataFormEditor<CustomEntry>
    {
        public CustomEntryEditor(SfDataForm dataForm) : base(dataForm)
        {
        }
        protected override CustomEntry OnCreateEditorView(DataFormItem dataFormItem)
        {
            var customEntry = new CustomEntry();
            customEntry.SfDataForm = DataForm;
            return customEntry;
        }
        protected override void OnInitializeView(DataFormItem dataFormItem, CustomEntry view)
        {
            view.Placeholder = "Name";
            view.PlaceholderColor = Color.Red;
        }

        protected override void OnWireEvents(CustomEntry view)
        {
            view.TextChanged += View_TextChanged;
            view.Unfocused += View_Unfocused;
        }
        private void View_Unfocused(object sender, FocusEventArgs e)
        {
            OnValidateValue(sender as CustomEntry);
        }

        private void View_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnCommitValue(sender as CustomEntry);
        }

        protected override bool OnValidateValue(CustomEntry view)
        {
            return DataForm.Validate("Name");
        }
        protected override void OnCommitValue(CustomEntry view)
        {
            var dataFormItemView = view.Parent as DataFormItemView;
            var textValue = view.Text;
            this.DataForm.ItemManager.SetValue(dataFormItemView.DataFormItem, view.Text);
        }
    }
    public class CustomEntry : Entry
    {
        public CustomEntry()
        {

        }

        public SfDataForm SfDataForm { get; set; }

        public new event EventHandler<EventArgs> Completed;


        public static readonly BindableProperty ReturnTypeProperty =
       BindableProperty.Create(propertyName: nameof(ReturnType), declaringType: typeof(CustomEntry),
           returnType: typeof(ReturnType), defaultValue: (ReturnType.Done));

        public ReturnType ReturnType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public void InvokeCompleted()
        {
            this.Completed?.Invoke(this, null);
        }


        /// <summary>
        /// Gets or sets BottomBorder Color .
        /// </summary>
        /// <value>This property takes the Color value.</value>
        public Color BottomBorderColor
        {
            get { return (Color)GetValue(BottomBorderColorProperty); }
            set { SetValue(BottomBorderColorProperty, value); }
        }

        public static readonly BindableProperty BottomBorderColorProperty = BindableProperty.Create(
            "BottomBorderColor",
            typeof(Color),
            typeof(CustomEntry),
            Color.Black,
            BindingMode.Default,
            null,
            null);


        public static readonly BindableProperty ActivePlaceholderColorProperty = BindableProperty.Create(nameof(ActivePlaceholderColor),
                                                                                                         typeof(Color), typeof(CustomEntry), Color.Accent);

        /// <summary>
        /// ActivePlaceholderColor summary. This is a bindable property.
        /// </summary>
        public Color ActivePlaceholderColor
        {
            get { return (Color)GetValue(ActivePlaceholderColorProperty); }
            set { SetValue(ActivePlaceholderColorProperty, value); }
        }
    }
    public enum ReturnType
    {
        Go,
        Next,
        Done,
        Send,
        Search
    }
}
