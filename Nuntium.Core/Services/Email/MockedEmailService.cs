using System;
using System.Collections.Generic;
namespace Nuntium.Core
{
    public class MockedEmailService : IEmailService
    {
        private readonly List<Email> mEmailsList;

        private Random mRnd;

        public MockedEmailService()
        {
            mRnd = new Random();
            mEmailsList = new List<Email>();
            var faker = new Fake.Fake();

            var people = faker.FakePeople(4);

            int i = 0;
            foreach (var person in people)
            {

                var email = new Email
                {
                    Id = i.ToString(),
                    SenderName = person.FirstName + " " + person.LastName,
                    Address = person.Email,
                    Subject = faker.FakeWords(10).ToTitleCase(),
                    Message = faker.FakeWords(50).ToTitleCase(),
                    SendDate = DateHelper.RandomDate(),
                };

                if(mRnd.Next(0, 100) > 10) email.ToAddresses.Add("second_address@test.mail.com");
                if (mRnd.Next(0, 100) > 50) email.WasRead = true;

                mEmailsList.Add(email);

                i++;
            }
        }

        public List<Email> GetAllEmails()
        {
            return mEmailsList;
        }

        public Email GetEmailById(string id)
        {
            return mEmailsList.Find(x => x.Id == id);
        }
    }
}
