using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvisioningTest
{
    internal class WebexUser
    {

        internal string Id { get; set; }

        internal List<PhoneNumber>? PhoneNumbers { get; set; }

    }


    internal class PhoneNumber
    {
        internal string Value { get; set; }

        internal PhoneNumberType Type { get; set; }
    }

    internal enum PhoneNumberType
    {
        Mobile, Work
    }
}
