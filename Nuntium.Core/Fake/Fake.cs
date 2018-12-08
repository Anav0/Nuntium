using System;
using System.Collections.Generic;
using System.Text;

namespace Nuntium.Core.Fake
{
    class Fake : IFake
    {
        string[] mMaleNames, mFemaleNames, mLastNames, mLorem;
        Random mRand;

        public Fake()
        {
            mRand = new Random();

            mMaleNames = new string[]
            {
                "Igor", "Andrzej", "Jacek", "Roman","Krzysztof","Aduś","Hieronim","Tadeusz","Czesław"
            };

            mFemaleNames = new string[]
            {
                "Alicja", "Basia", "Kasia", "Adrianna","Oliwia","Weronika","Zuzanna","Leokadia","Zdzisława"
            };

            mLastNames = new string[]
            {
                "Grzędowicz", "Waleszczak", "Nowak", "Zduń","Motyka","Prewiak","Wittun","Czarnecki","Kwiat", "Satoń"
            };

            mLorem = new string[]
            {
                "Lorem", "ipsum","dolor","sit","amet","consectetur", "adipiscing", "elit","Fusce","eget", "mauris", "porta", "sollicitudin", "ante", "vitae", "hendrerit", "tortor",
            };
        }

        public string FakeFirstName(bool isMaleName = true)
        {
            if (isMaleName)
            {
                return mMaleNames[mRand.Next(0, mMaleNames.Length - 1)];
            }
            else
            {
                return mFemaleNames[mRand.Next(0, mFemaleNames.Length - 1)];
            }
        }

        public string FakeLastName()
        {
            return mLastNames[mRand.Next(0, mLastNames.Length - 1)];
        }

        public List<string> FakeNames(int howMany)
        {
            var output = new List<string>();

            for (int i = 0; i < howMany; i++)
            {
                var isMale = i % 2 == 0 ? true : false;

                var firstName = FakeFirstName(isMale);
                var lastName = FakeLastName();
                output.Add(firstName + " " + lastName);
            }

            return output;
        }

        public List<Person> FakePeople(int howMany)
        {
            var output = new List<Person>();

            for (int i = 0; i < howMany; i++)
            {
                var isMale = i % 2 == 0 ? true : false;

                var person = new Person
                {
                    BirthDate = DateHelper.RandomDate(new DateTime(1980, 12, 30)),
                    FirstName = FakeFirstName(isMale),
                    LastName = FakeLastName(),
                    Sex = isMale ? Gender.Male : Gender.Female,
                };

                person.Email = person.FirstName + "_" + person.LastName + "@mail.com";

                output.Add(person);
            }

            return output;
        }

        public string FakeWords(int howManyWords)
        {
            var output = "";

            for(int i = 0; i < howManyWords; i++)
            {
                output += " "+mLorem[mRand.Next(0, mLorem.Length - 1)];
            }

            return output;
        }
    }
}
