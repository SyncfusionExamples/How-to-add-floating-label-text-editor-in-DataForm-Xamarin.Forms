using Syncfusion.XForms.DataForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FloatingLabelTextEditor
{
    public class ContactInfo : NotificationObject
    {
        private string id;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Id should not be empty")]
        [Display(Prompt = "Id")]

        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                RaisePropertyChanged("ID");
            }
        }

        private string name;

        [DisplayOptions(RowSpan = 2)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name should not be empty")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
    }
    public class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
