using System;
using System.Collections.Generic;

namespace Nuntium.Core
{
    public class Email
    {
        public string Address, Message, SenderName, Subject, Id;
        public DateTime SendDate;
        public List<string> ToAddresses, FromAddresses;
        public List<AttachedFile> Attachments;

        public Email()
        {
            ToAddresses = new List<string>();
            FromAddresses = new List<string>();
            Attachments = new List<AttachedFile>();
        }
    }
}
