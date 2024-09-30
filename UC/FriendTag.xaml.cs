﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MessApp.DB.Model;

namespace MessApp.UC
{
    /// <summary>
    /// Interaction logic for FriendTag.xaml
    /// </summary>
    public partial class FriendTag : UserControl
    {
        public ConversationModel Conversation {  get; private set; }
        public event EventHandler<ConversationModel> OnFriendTagClicked;
        public FriendTag(ConversationModel conversation)
        {
            InitializeComponent();
            Conversation = conversation;
            FriendName.Text = conversation.name;
        }

        private void FriendTag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnFriendTagClicked?.Invoke(this, Conversation);
        }
    }
}
