using Nuntium.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nuntium
{
    public class MessageMiniatureListDesign : MessageMiniatureListViewModel
    {
        public static MessageMiniatureListDesign Instance => new MessageMiniatureListDesign();


        public MessageMiniatureListDesign()
        {
            Items = new ObservableCollection<MessageMiniatureViewModel>();

            for (int i = 0; i < 150; i++)
            {
                var msg = new MessageMiniatureViewModel
                {
                    AvatarBackground = ColorHelpers.GenerateRandomColor(),
                    HasAttachments = false,
                    SendDate = DateHelper.RandomDate(),
                    SenderName = Faker.Name.FullName(Faker.NameFormats.Standard),
                    Title = Faker.Lorem.Sentence(),
                    MessageSnipit = string.Join(" ", Faker.Lorem.Sentences(10))

                };

                msg.Initials = msg.SenderName.GetInitials();
                Items.Add(msg);
            }

            Items = new ObservableCollection<MessageMiniatureViewModel>();
            SelectedItems = new ObservableCollection<MessageMiniatureViewModel>();
        }
    }
}
