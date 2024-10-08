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

using MessApp.DB.Model;

namespace MessApp.UC
{
    /// <summary>
    /// Interaction logic for FriendTag.xaml
    /// </summary>
    public partial class ConversationTag : UserControl
    {
        public ConversationModel Conversation {  get; private set; }
        public event EventHandler<ConversationModel> OnConversationTagClick;
        public ConversationTag(ConversationModel conversation, ParticipantModel participant)
        {
            InitializeComponent();
            Conversation = conversation;
            ConversationName.Text = participant.chatname;
        }

        private void ConversationTag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnConversationTagClick?.Invoke(this, Conversation);
        }
    }
}
