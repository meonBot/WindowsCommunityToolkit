// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TokenizingTextBoxPage : Page, IXamlRenderListener
    {
        #region Sample Data
        //// TODO: We should use images here.
        private readonly List<SampleEmailDataType> _emailSamples = new List<SampleEmailDataType>()
        {
            new SampleEmailDataType() { FirstName = "Marcus", FamilyName = "Perryman", Icon = Symbol.Account },
            new SampleEmailDataType() { FirstName = "Ian", FamilyName = "Smith", Icon = Symbol.AddFriend },
            new SampleEmailDataType() { FirstName = "Peter", FamilyName = "Strange", Icon = Symbol.Attach },
            new SampleEmailDataType() { FirstName = "Alex", FamilyName = "Wilber", Icon = Symbol.AttachCamera },
            new SampleEmailDataType() { FirstName = "Allan", FamilyName = "Deyoung", Icon = Symbol.Audio },
            new SampleEmailDataType() { FirstName = "Adele", FamilyName = "Vance", Icon = Symbol.BlockContact },
            new SampleEmailDataType() { FirstName = "Grady", FamilyName = "Archie", Icon = Symbol.Calculator },
            new SampleEmailDataType() { FirstName = "Megan", FamilyName = "Bowen", Icon = Symbol.Calendar },
            new SampleEmailDataType() { FirstName = "Ben", FamilyName = "Walters", Icon = Symbol.Camera },
            new SampleEmailDataType() { FirstName = "Debra", FamilyName = "Berger", Icon = Symbol.Contact },
            new SampleEmailDataType() { FirstName = "Emily", FamilyName = "Braun", Icon = Symbol.Favorite },
            new SampleEmailDataType() { FirstName = "Christine", FamilyName = "Cline", Icon = Symbol.Link },
            new SampleEmailDataType() { FirstName = "Enrico", FamilyName = "Catteneo", Icon = Symbol.Mail },
            new SampleEmailDataType() { FirstName = "Davit", FamilyName = "Badalyan", Icon = Symbol.Map },
            new SampleEmailDataType() { FirstName = "Diego", FamilyName = "Siciliani", Icon = Symbol.Phone },
            new SampleEmailDataType() { FirstName = "Raul", FamilyName = "Razo", Icon = Symbol.Pin },
            new SampleEmailDataType() { FirstName = "Miriam", FamilyName = "Graham", Icon = Symbol.Rotate },
            new SampleEmailDataType() { FirstName = "Lynne", FamilyName = "Robbins", Icon = Symbol.RotateCamera },
            new SampleEmailDataType() { FirstName = "Lydia", FamilyName = "Holloway", Icon = Symbol.Send },
            new SampleEmailDataType() { FirstName = "Nestor", FamilyName = "Wilke", Icon = Symbol.Tag },
            new SampleEmailDataType() { FirstName = "Patti", FamilyName = "Fernandez", Icon = Symbol.UnFavorite },
            new SampleEmailDataType() { FirstName = "Pradeep", FamilyName = "Gupta", Icon = Symbol.UnPin },
            new SampleEmailDataType() { FirstName = "Joni", FamilyName = "Sherman", Icon = Symbol.Zoom },
            new SampleEmailDataType() { FirstName = "Isaiah", FamilyName = "Langer", Icon = Symbol.ZoomIn },
            new SampleEmailDataType() { FirstName = "Irvin", FamilyName = "Sayers", Icon = Symbol.ZoomOut },
        };

        private readonly List<SampleDataType> _samples = new List<SampleDataType>()
        {
            new SampleDataType() { Text = "Account", Icon = Symbol.Account },
            new SampleDataType() { Text = "Add Friend", Icon = Symbol.AddFriend },
            new SampleDataType() { Text = "Attach", Icon = Symbol.Attach },
            new SampleDataType() { Text = "Attach Camera", Icon = Symbol.AttachCamera },
            new SampleDataType() { Text = "Audio", Icon = Symbol.Audio },
            new SampleDataType() { Text = "Block Contact", Icon = Symbol.BlockContact },
            new SampleDataType() { Text = "Calculator", Icon = Symbol.Calculator },
            new SampleDataType() { Text = "Calendar", Icon = Symbol.Calendar },
            new SampleDataType() { Text = "Camera", Icon = Symbol.Camera },
            new SampleDataType() { Text = "Contact", Icon = Symbol.Contact },
            new SampleDataType() { Text = "Favorite", Icon = Symbol.Favorite },
            new SampleDataType() { Text = "Link", Icon = Symbol.Link },
            new SampleDataType() { Text = "Mail", Icon = Symbol.Mail },
            new SampleDataType() { Text = "Map", Icon = Symbol.Map },
            new SampleDataType() { Text = "Phone", Icon = Symbol.Phone },
            new SampleDataType() { Text = "Pin", Icon = Symbol.Pin },
            new SampleDataType() { Text = "Rotate", Icon = Symbol.Rotate },
            new SampleDataType() { Text = "Rotate Camera", Icon = Symbol.RotateCamera },
            new SampleDataType() { Text = "Send", Icon = Symbol.Send },
            new SampleDataType() { Text = "Tags", Icon = Symbol.Tag },
            new SampleDataType() { Text = "UnFavorite", Icon = Symbol.UnFavorite },
            new SampleDataType() { Text = "UnPin", Icon = Symbol.UnPin },
            new SampleDataType() { Text = "Zoom", Icon = Symbol.Zoom },
            new SampleDataType() { Text = "ZoomIn", Icon = Symbol.ZoomIn },
            new SampleDataType() { Text = "ZoomOut", Icon = Symbol.ZoomOut },
        };
        #endregion

        private TokenizingTextBox _ttb;
        private TokenizingTextBox _ttbEmail;
        private ListView _ttbEmailSuggestions;
        private Button _ttbEmailClear;

        public TokenizingTextBoxPage()
        {
            InitializeComponent();
            Loaded += (sernder, e) => { this.OnXamlRendered(this); };
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            if (_ttb != null)
            {
                _ttb.TokenItemAdded -= TokenItemAdded;
                _ttb.TokenItemRemoving -= TokenItemRemoved;
                _ttb.TextChanged -= TextChanged;
                _ttb.TokenItemAdding -= TokenItemCreating;
            }

            if (control.FindChildByName("TokenBox") is TokenizingTextBox ttb)
            {
                _ttb = ttb;

                _ttb.TokenItemAdded += TokenItemAdded;
                _ttb.TokenItemRemoving += TokenItemRemoved;
                _ttb.TextChanged += TextChanged;
                _ttb.TokenItemAdding += TokenItemCreating;
            }

            // For the Email Selection control
            if (_ttbEmail != null)
            {
                _ttbEmail.ItemClick -= EmailList_ItemClick;
                _ttbEmail.TokenItemAdded -= EmailTokenItemAdded;
                _ttbEmail.TokenItemRemoved -= EmailTokenItemRemoved;
                _ttbEmail.TextChanged -= EmailTextChanged;
            }

            if (control.FindChildByName("TokenBoxEmail") is TokenizingTextBox ttbEmail)
            {
                _ttbEmail = ttbEmail;

                _ttbEmail.ItemClick += EmailList_ItemClick;
                _ttbEmail.TokenItemAdded += EmailTokenItemAdded;
                _ttbEmail.TokenItemRemoved += EmailTokenItemRemoved;
                _ttbEmail.TextChanged += EmailTextChanged;
            }

            if (_ttbEmailSuggestions != null)
            {
                _ttbEmailSuggestions.ItemClick -= EmailList_ItemClick;
            }

            if (control.FindChildByName("EmailList") is ListView ttbList)
            {
                _ttbEmailSuggestions = ttbList;

                _ttbEmailSuggestions.ItemClick += EmailList_ItemClick;

                UpdateSuggestions();
            }

            if (_ttbEmailClear != null)
            {
                _ttbEmailClear.Click -= ClearButtonClick;
            }

            if (control.FindChildByName("ClearButton") is Button btn)
            {
                _ttbEmailClear = btn;

                _ttbEmailClear.Click += ClearButtonClick;
            }
        }

        private void TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent() && args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(sender.Text))
                {
                    _ttb.SuggestedItemsSource = Array.Empty<object>();
                }
                else
                {
                    _ttb.SuggestedItemsSource = _samples.Where((item) => item.Text.Contains(sender.Text, System.StringComparison.CurrentCultureIgnoreCase))
                        .Except(_ttb.Items.Cast<SampleDataType>())
                        .OrderByDescending(item => item.Text);
                }
            }
        }

        private void TokenItemCreating(object sender, TokenItemAddingEventArgs e)
        {
            // Take the user's text and convert it to our data type (if we have a matching one).
            e.Item = _samples.FirstOrDefault((item) => item.Text.Contains(e.TokenText, System.StringComparison.CurrentCultureIgnoreCase));

            // Otherwise, create a new version of our data type
            if (e.Item == null)
            {
                e.Item = new SampleDataType()
                {
                    Text = e.TokenText,
                    Icon = Symbol.OutlineStar
                };
            }
        }

        private void TokenItemAdded(TokenizingTextBox sender, object data)
        {
            // TODO: Add InApp Notification?
            if (data is SampleDataType sample)
            {
                Debug.WriteLine("Added Token: " + sample.Text);
            }
            else
            {
                Debug.WriteLine("Added Token: " + data);
            }
        }

        private void TokenItemRemoved(TokenizingTextBox sender, TokenItemRemovingEventArgs args)
        {
            if (args.Item is SampleDataType sample)
            {
                Debug.WriteLine("Removed Token: " + sample.Text);
            }
            else
            {
                Debug.WriteLine("Removed Token: " + args.Item);
            }
        }

        private void EmailTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent() && args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                UpdateSuggestions();
            }
        }

        private void UpdateSuggestions()
        {
            if (_ttbEmail == null || _ttbEmailSuggestions == null)
            {
                return;
            }

            // TODO: Test out AdvancedCollectionView Filter here instead?
            if (string.IsNullOrWhiteSpace(_ttbEmail.Text))
            {
                _ttbEmailSuggestions.ItemsSource = _emailSamples
                    .Except(_ttbEmail.Items.Cast<SampleEmailDataType>())
                    .OrderBy(item => item.DisplayName);
            }
            else
            {
                _ttbEmailSuggestions.ItemsSource = _emailSamples.Where((item) => item.DisplayName.Contains(_ttbEmail.Text, System.StringComparison.CurrentCultureIgnoreCase))
                    .Except(_ttbEmail.Items.Cast<SampleEmailDataType>())
                    .OrderBy(item => item.DisplayName);
            }
        }

        private void EmailTokenItemAdded(TokenizingTextBox sender, object args)
        {
            if (args is SampleEmailDataType sample)
            {
                Debug.WriteLine("Added Email: " + sample.DisplayName);
            }
            else
            {
                Debug.WriteLine("Added Token: " + args);
            }

            UpdateSuggestions();
        }

        private void EmailTokenItemRemoved(TokenizingTextBox sender, object args)
        {
            if (args is SampleEmailDataType sample)
            {
                Debug.WriteLine("Removed Email: " + sample.DisplayName);
            }
            else
            {
                Debug.WriteLine("Removed Token: " + args);
            }

            UpdateSuggestions();
        }

        private void EmailList_ItemClick(object sender, ItemClickEventArgs e)
        {
            _ttbEmail.Items.Add(e.ClickedItem);
            _ttbEmail.Text = string.Empty;

            UpdateSuggestions();
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            _ttbEmail.Items.Clear();

            UpdateSuggestions();
        }
    }
}