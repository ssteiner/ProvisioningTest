using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvisioningTest
{
    internal class Tester
    {


        internal void RunTest()
        {
            WebexUser user1 = new WebexUser { PhoneNumbers = null };
            WebexUser user2 = new WebexUser
            {
                PhoneNumbers = new List<PhoneNumber>
                {
                    new PhoneNumber { Type = PhoneNumberType.Work, Value = "9988" }
                }
            };
            WebexUser user3 = new WebexUser
            {
                PhoneNumbers = new List<PhoneNumber>
                {
                    new PhoneNumber { Type = PhoneNumberType.Work, Value = "9988" },
                    new PhoneNumber { Type = PhoneNumberType.Mobile, Value = "0767772222" }
                }
            };
            WebexUser user4 = new WebexUser
            {
                PhoneNumbers = new List<PhoneNumber>
                {
                    new PhoneNumber { Type = PhoneNumberType.Work, Value = "9988" },
                    new PhoneNumber { Type = PhoneNumberType.Mobile, Value = "1235" }
                }
            };

            ProvisioningOperation op1 = new ProvisioningOperation { PreviousMobileNumber = null, DesiredMobileNumber = "1235" };
            UpdateWebexUser(user1, op1);
            UpdateWebexUser(user2, op1);
            UpdateWebexUser(user3, op1);
            UpdateWebexUser(user4, op1);

            ProvisioningOperation op2 = new ProvisioningOperation { PreviousMobileNumber = "1235", DesiredMobileNumber = "1235" };
            UpdateWebexUser(user1, op2);
            UpdateWebexUser(user2, op2);
            UpdateWebexUser(user3, op2);
            UpdateWebexUser(user4, op2);

            ProvisioningOperation op3 = new ProvisioningOperation { PreviousMobileNumber = "1236", DesiredMobileNumber = "1235" };
            UpdateWebexUser(user1, op3);
            UpdateWebexUser(user2, op3);
            UpdateWebexUser(user3, op3);
            UpdateWebexUser(user4, op3);

            ProvisioningOperation op4 = new ProvisioningOperation { PreviousMobileNumber = "1236", DesiredMobileNumber = null };
            UpdateWebexUser(user1, op4);
            UpdateWebexUser(user2, op4);
            UpdateWebexUser(user3, op4);
            UpdateWebexUser(user4, op4);

            ProvisioningOperation op5 = new ProvisioningOperation { PreviousMobileNumber = null, DesiredMobileNumber = null };
            UpdateWebexUser(user1, op5);
            UpdateWebexUser(user2, op5);
            UpdateWebexUser(user3, op5);
            UpdateWebexUser(user4, op5);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">the user as it is currently configured on the Webex system</param>
        /// <param name="op">the data that is in the AUDM database</param>
        internal void UpdateWebexUser(WebexUser user, ProvisioningOperation op)
        {
            WebexUser updateToBeSentToWebex;
            WebexUser updateToBeSentToWebexOnRollback;

            //this method must fill out updateToBeSentToWebex and updateToBeSentToWebexOnRollback;
        }




    }
}
