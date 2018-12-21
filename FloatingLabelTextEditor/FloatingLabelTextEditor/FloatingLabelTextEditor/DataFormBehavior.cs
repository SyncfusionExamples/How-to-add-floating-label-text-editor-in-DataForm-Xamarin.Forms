using Syncfusion.XForms.DataForm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FloatingLabelTextEditor
{
    public class DataFormBehavior : Behavior<ContentPage>
    {
        SfDataForm dataForm;
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);

            dataForm = bindable.FindByName<SfDataForm>("dataForm");

            dataForm.RegisterEditor("Entry", new CustomEntryEditor(dataForm));
            dataForm.RegisterEditor("Name", "Entry");
            dataForm.LabelPosition = LabelPosition.Top;
            dataForm.DataObject = new ContactInfo();
            dataForm.LayoutManager = new DataFormLayoutManagerExt(dataForm);
        }
    }
    public class DataFormLayoutManagerExt : DataFormLayoutManager
    {
        public DataFormLayoutManagerExt(SfDataForm dataForm) : base(dataForm)
        {
        }
        protected override View GenerateViewForLabel(DataFormItem dataFormItem)
        {
            var view = base.GenerateViewForLabel(dataFormItem);
            var textView = (view as Label);
            textView.TextColor = Color.Blue;
            if (dataFormItem.Name == "Name")
            {
                textView.IsVisible = false;
            }
            return textView;
        }
        protected override void OnEditorCreated(DataFormItem dataFormItem, View editor)
        {
            if (dataFormItem.Name == "ID")
            {
                (editor as Entry).TextColor = Color.YellowGreen;
                (editor as Entry).Placeholder = "Enter your id";
                base.OnEditorCreated(dataFormItem, editor);
            }
            else
            {
                base.OnEditorCreated(dataFormItem, editor);
            }
        }
    }
}
