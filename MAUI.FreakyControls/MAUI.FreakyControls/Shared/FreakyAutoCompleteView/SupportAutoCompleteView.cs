using System;
using Maui.FreakyControls.Shared.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.FreakyControls
{
    public enum SupportEntryReturnType
    {
        Go,
        Next,
        Done,
        Send,
        Search
    }

    public interface IDropItemSelected
    {
        void IF_ItemSelectd(int position);
    }


    public class IntegerEventArgs : EventArgs
    {
        public int IntegerValue { get; set; }

        public IntegerEventArgs(int _value)
        {
            IntegerValue = _value;
        }
    }

    public class SupportAutoComplete : SupportViewDrop
    {
        public SupportAutoComplete()
        {
            HeightRequest = 40;
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create("Placeholder", typeof(string), typeof(SupportAutoComplete), "");
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly BindableProperty PaddingInsideProperty = BindableProperty.Create("PaddingInside", typeof(double), typeof(SupportAutoComplete), 5d);
        public double PaddingInside
        {
            get { return (double)GetValue(PaddingInsideProperty); }
            set { SetValue(PaddingInsideProperty, value); }
        }

        public static readonly BindableProperty FocusCornerColorProperty = BindableProperty.Create("FocusCornerColor", typeof(Color), typeof(SupportAutoComplete), Colors.Black);
        public Color FocusCornerColor
        {
            get => (Color)GetValue(FocusCornerColorProperty);
            set => SetValue(FocusCornerColorProperty, value);
        }

        public static readonly BindableProperty CurrentCornerColorProperty = BindableProperty.Create("CurrentCornerColor", typeof(Color), typeof(SupportAutoComplete), Colors.Black);
        public Color CurrentCornerColor
        {
            get => (Color)GetValue(CurrentCornerColorProperty);
            set => SetValue(CurrentCornerColorProperty, value);
        }

        public static readonly BindableProperty InvalidCornerColorProperty = BindableProperty.Create("InvalidCornerColor", typeof(Color), typeof(SupportAutoComplete), Colors.Black);
        public Color InvalidCornerColor
        {
            get => (Color)GetValue(InvalidCornerColorProperty);
            set => SetValue(InvalidCornerColorProperty, value);
        }

        public static readonly BindableProperty PlaceHolderColorProperty = BindableProperty.Create("PlaceHolderColor", typeof(Color), typeof(SupportAutoComplete), Colors.Black);
        public Color PlaceHolderColor
        {
            get => (Color)GetValue(PlaceHolderColorProperty);
            set => SetValue(PlaceHolderColorProperty, value);
        }

        public static readonly BindableProperty IsValidProperty = BindableProperty.Create("IsValid", typeof(bool), typeof(SupportAutoComplete), true);
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        public static readonly BindableProperty NextViewProperty = BindableProperty.Create("NextView", typeof(View), typeof(SupportAutoComplete), null);
        public View NextView
        {
            get => (View)GetValue(NextViewProperty);
            set => SetValue(NextViewProperty, value);
        }

        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType), typeof(SupportEntryReturnType), typeof(SupportAutoComplete), SupportEntryReturnType.Done, BindingMode.OneWay);
        public SupportEntryReturnType ReturnType
        {
            get => (SupportEntryReturnType)GetValue(ReturnTypeProperty);
            set => SetValue(ReturnTypeProperty, value);
        }
        /*
         * Function
         */

        /*
         * Command
         */
        public static readonly BindableProperty OnItemSelectedCommandProperty = BindableProperty.Create("OnItemSelectedCommand", typeof(ICommand), typeof(SupportAutoComplete), null);
        public ICommand OnItemSelectedCommand
        {
            get { return (ICommand)GetValue(OnItemSelectedCommandProperty); }
            set { SetValue(OnItemSelectedCommandProperty, value); }
        }

        public static readonly BindableProperty OnTextChangedCommandProperty = BindableProperty.Create("OnTextChangedCommand", typeof(ICommand), typeof(SupportAutoComplete), null);
        public ICommand OnTextChangedCommand
        {
            get { return (ICommand)GetValue(OnTextChangedCommandProperty); }
            set { SetValue(OnTextChangedCommandProperty, value); }
        }

        public static readonly BindableProperty OnTextFocusedCommandProperty = BindableProperty.Create("OnTextFocusedCommand", typeof(ICommand), typeof(SupportAutoComplete), null);
        public ICommand OnTextFocusedCommand
        {
            get { return (ICommand)GetValue(OnTextFocusedCommandProperty); }
            set { SetValue(OnTextFocusedCommandProperty, value); }
        }


        /*
         * Event
         */
        public event EventHandler<TextChangedEventArgs> OnTextChanged;
        public event EventHandler OnReturnKeyClicked;
        public event EventHandler<IntegerEventArgs> OnItemSelected;
        public event EventHandler<FocusEventArgs> OnTextFocused;

        public void SendOnTextChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                //nothing
                text = "";
            }
            else
            {
                if (string.IsNullOrEmpty(Text) || !Text.Equals(text))
                {
                    Text = text;
                }
                else
                {
                    text = "";
                }
            }
            var paramete = new TextChangedEventArgs(text, text);
            OnTextChanged?.Invoke(this, paramete);
            OnTextChangedCommand?.Execute(paramete);
        }

        public void SendOnReturnKeyClicked()
        {
            NextView?.Focus();
            OnReturnKeyClicked?.Invoke(this, null);
        }

        public void SendOnItemSelected(int position)
        {
            var paramete = new IntegerEventArgs(position);
            OnItemSelected?.Invoke(this, paramete);
            OnItemSelectedCommand?.Execute(paramete);
        }

        public void SendOnTextFocused(bool hasFocus)
        {
            IsFocus = hasFocus;
            CurrentCornerColor = IsFocus ? FocusCornerColor : CornerColor;

            var paramete = new FocusEventArgs(this, hasFocus);
            OnTextFocused?.Invoke(this, paramete);
            OnTextFocusedCommand?.Execute(paramete);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName.Equals(IsValidProperty.PropertyName))
            {
                if (!IsValid)
                    CurrentCornerColor = InvalidCornerColor;
            }
            else if (propertyName.Equals(TextProperty.PropertyName))
            {
                if (!IsFocus)
                    SendOnTextChanged(Text);
            }
        }
    }
}

