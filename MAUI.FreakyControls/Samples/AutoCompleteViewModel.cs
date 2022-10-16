using System;
using Maui.FreakyControls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Maui.FreakyControls.Shared.Controls;

namespace Samples
{
    public class AutoCompleteViewModel : BaseViewModel
    {
        private List<IAutoDropItem> _ItemDemo;
        public List<IAutoDropItem> ItemDemo
        {
            get => _ItemDemo;
            set
            {
                _ItemDemo = value;
                OnPropertyChanged();
            }
        }


        public AutoCompleteViewModel()
        {
            ItemDemo = new List<IAutoDropItem>();
            
        }
    }

    public class YourClass : IAutoDropItem
    {
        public string YouDefineTitle { set; get; }
        public string YouDefineDescription { set; get; }
        public string YouDefineIcon { set; get; }
        public bool YouChecked { set; get; }

        public string IF_GetDescription()
        {
            return YouDefineDescription;
        }

        public string IF_GetIcon()
        {
            return YouDefineIcon;
        }

        public string IF_GetTitle()
        {
            return YouDefineTitle;
        }

        public Action IF_GetAction()
        {
            return null;
        }

        public bool IF_GetChecked()
        {
            return YouChecked;
        }

        public void IF_SetChecked(bool _Checked)
        {
            YouChecked = _Checked;
        }

        public YourClass(string _title)
        {
            YouDefineTitle = _title;
            YouChecked = false;
        }

        public YourClass(string _title, string _des)
        {
            YouDefineTitle = _title;
            YouDefineDescription = _des;
            YouChecked = false;
        }

        public YourClass(string _title, string _des, string _icon)
        {
            YouDefineTitle = _title;
            YouDefineDescription = _des;
            YouDefineIcon = _icon;
            YouChecked = false;
        }
    }
}

