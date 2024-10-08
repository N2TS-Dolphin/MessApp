﻿using MessApp.DB.Dao;
using MessApp.DB.Model;
using System;
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

namespace MessApp.UC
{
    /// <summary>
    /// Interaction logic for UserMessageTag.xaml
    /// </summary>
    public partial class UserMessageTag : UserControl
    {
        public MessageModel Message { get; private set; }

        public UserMessageTag(MessageModel message)
        {
            InitializeComponent();
            Message = message;

            LoadMessage();
        }

        private void LoadMessage()
        {
            MessageContent.Text = Message.message;
        }
    }
}
