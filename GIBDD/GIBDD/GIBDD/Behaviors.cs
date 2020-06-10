using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace GIBDD
{
    class ImportantEntryBehavior : Behavior<Entry> 
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var curEntry = (Entry)sender;
            var isValid = curEntry.Text != "" && curEntry.Text.All(x => char.IsLetter(x) || x == '-');
            if (!isValid)
                curEntry.BackgroundColor = Color.Salmon;
            else
                curEntry.BackgroundColor = Color.Default;
        }
    }

    class EmailEntryBehavior : Behavior<Entry>
    {
        private EmailValidator validator = new EmailValidator();
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var curEntry = (Entry)sender;
            var isValid = validator.Validate(curEntry.Text).IsValid;
            if (!isValid)
                curEntry.BackgroundColor = Color.Salmon;
            else
                curEntry.BackgroundColor = Color.Default;
        }
    }
    class PhoneEntryBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var curEntry = (Entry)sender;
            var isValid = IsPhoneNumber(curEntry.Text);
            if (!isValid)
                curEntry.BackgroundColor = Color.Salmon;
            else
                curEntry.BackgroundColor = Color.Default;
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9,11})$").Success;
        }
    }
    
}
