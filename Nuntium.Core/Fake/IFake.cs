using System;
using System.Collections.Generic;
using System.Text;

namespace Nuntium.Core.Fake
{
    public interface IFake
    {
        List<string> FakeNames(int howMany);

        string FakeFirstName(bool isMaleName = true);

        string FakeLastName();

        List<Person> FakePeople(int howMany);
    }
}
