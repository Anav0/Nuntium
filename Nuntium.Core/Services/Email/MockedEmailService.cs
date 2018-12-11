using System.Collections.Generic;
namespace Nuntium.Core
{
    public class MockedEmailService : IEmailService
    {
        private readonly List<Email> mEmailsList;

        public MockedEmailService()
        {
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

                email.ToAddresses.Add("test@test.mail.com");

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
