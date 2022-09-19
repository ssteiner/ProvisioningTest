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
            WebexUser user1 = new WebexUser
            {
                PhoneNumbers = null
            };
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
            WebexUser user5 = new WebexUser
            {
                PhoneNumbers = new List<PhoneNumber>
                {
                    new PhoneNumber { Type = PhoneNumberType.Work, Value = "9988" },
                    new PhoneNumber { Type = PhoneNumberType.Mobile, Value = "1236" }
                }
            };
            // If exists previous than update the number to the desired one, otherwise just add the number. If desired number is null delete the previous one if it matches.

            ProvisioningOperation op1 = new ProvisioningOperation { PreviousMobileNumber = null, DesiredMobileNumber = "1235" };
            UpdateWebexUser(user1, op1); // updateToBeSentToWebex contains one number of type mobile with value = "1235"
            UpdateWebexUser(user2, op1); // updateToBeSentToWebex contains two numbers, one of type mobile, with value "1235"
            UpdateWebexUser(user3, op1); // updateToBeSentToWebex contains the existing two numbers, plus a new one of type mobile with value "1235"
            UpdateWebexUser(user4, op1); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user5, op1); // updateToBeSentToWebex contains three numbers, the existing ones, plus a mobile number with value 1236

            ProvisioningOperation op2 = new ProvisioningOperation { PreviousMobileNumber = "1235", DesiredMobileNumber = "1235" };
            UpdateWebexUser(user1, op2); // updateToBeSentToWebex contains one number of type mobile with value = "1235"
            UpdateWebexUser(user2, op2); // updateToBeSentToWebex contains two numbers, one of type mobile, with value "1235"
            UpdateWebexUser(user3, op2); // updateToBeSentToWebex contains the existing two numbers, plus a new one of type mobile with value "1235"
            UpdateWebexUser(user4, op2); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user5, op2); // updateToBeSentToWebex contains three numbers, the existing ones, plus a mobile number with value 1236

            ProvisioningOperation op3 = new ProvisioningOperation { PreviousMobileNumber = "1236", DesiredMobileNumber = "1235" };
            UpdateWebexUser(user1, op3); // updateToBeSentToWebex contains one number of type mobile with value = "1235"
            UpdateWebexUser(user2, op3); // updateToBeSentToWebex contains two numbers, one of type mobile, with value "1235"
            UpdateWebexUser(user3, op3); // updateToBeSentToWebex contains the existing two numbers, plus a new one of type mobile with value "1235"
            UpdateWebexUser(user4, op3); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user5, op3); // updateToBeSentToWebex contains two numbers: 9988 and a mobile number with value 1235

            ProvisioningOperation op4 = new ProvisioningOperation { PreviousMobileNumber = "1236", DesiredMobileNumber = null };
            UpdateWebexUser(user1, op4); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user2, op4); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user3, op4); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user4, op4); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user5, op4); // updateToBeSentToWebex contains one number: 9988

            ProvisioningOperation op5 = new ProvisioningOperation { PreviousMobileNumber = null, DesiredMobileNumber = null };
            UpdateWebexUser(user1, op5); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user2, op5); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user3, op5); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user4, op5); // updateToBeSentToWebex contains a null number collection
            UpdateWebexUser(user5, op5); // updateToBeSentToWebex contains a null number collection

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">the user as it is currently configured on the Webex system</param>
        /// <param name="op">the data that is in the AUDM database</param>
        internal void UpdateWebexUser(WebexUser user, ProvisioningOperation op)
        {
            WebexUser updateToBeSentToWebex = new();
            updateToBeSentToWebex.PhoneNumbers = new();
            WebexUser updateToBeSentToWebexOnRollback = new();
            updateToBeSentToWebexOnRollback.PhoneNumbers = new();

            if (op.PreviousMobileNumber == null && op.DesiredMobileNumber == null)
            {
                updateToBeSentToWebex.PhoneNumbers = null;
            }
            else
            {
                var HasPreviousVal = op.PreviousMobileNumber != null;
                var HasDesiredVal = op.DesiredMobileNumber != null;
                var foundPrevValNumber = user.PhoneNumbers?.FirstOrDefault(mobileNumber => mobileNumber.Type == PhoneNumberType.Mobile && mobileNumber.Value == op.PreviousMobileNumber);
                var foundDesValNumber = user.PhoneNumbers?.FirstOrDefault(mobileNumber => mobileNumber.Type == PhoneNumberType.Mobile && mobileNumber.Value == op.DesiredMobileNumber);
                var HasPreviousValNumberFound = foundPrevValNumber != null;
                var HasDesiredValNumberFound = foundDesValNumber != null;

                if (HasPreviousValNumberFound && HasDesiredValNumberFound) // number already in place
                {
                    updateToBeSentToWebex.PhoneNumbers = null;
                }

                //Add
                if (HasDesiredVal && !HasPreviousValNumberFound && !HasDesiredValNumberFound)
                {
                    if (user.PhoneNumbers != null)
                    {
                        foreach (var item in user.PhoneNumbers)
                        {
                            updateToBeSentToWebex.PhoneNumbers.Add(new() { Type = item.Type, Value = item.Value });
                        }
                    }
                    updateToBeSentToWebex.PhoneNumbers.Add(new PhoneNumber() { Type = PhoneNumberType.Mobile, Value = op.DesiredMobileNumber });
                }

                //Nothing to do
                if (HasDesiredVal && !HasPreviousValNumberFound && HasDesiredValNumberFound)
                {
                    updateToBeSentToWebex.PhoneNumbers = null;
                }

                //Update
                if (HasPreviousVal && HasDesiredVal && HasPreviousValNumberFound && !HasDesiredValNumberFound)
                {
                    foreach (var item in user.PhoneNumbers)
                    {
                        if (!item.Value.Contains(foundPrevValNumber.Value))
                            updateToBeSentToWebex.PhoneNumbers.Add(new() { Type = item.Type, Value = item.Value });
                        else
                            updateToBeSentToWebex.PhoneNumbers.Add(new() { Type = foundPrevValNumber.Type, Value = op.DesiredMobileNumber });
                    }
                }

                //Delete or send null
                if (HasPreviousVal && !HasDesiredVal)
                {
                    if (user.PhoneNumbers != null && HasPreviousValNumberFound)
                    {
                        foreach (var item in user.PhoneNumbers)
                        {
                            if (!item.Value.Contains(foundPrevValNumber.Value))
                                updateToBeSentToWebex.PhoneNumbers.Add(new() { Type = item.Type, Value = item.Value });
                        }
                    }
                    else
                        updateToBeSentToWebex.PhoneNumbers = null;
                }

            }
            if (updateToBeSentToWebex.PhoneNumbers == null) Console.WriteLine("PhoneNumbers = null");
            else Console.WriteLine(string.Join('\n', updateToBeSentToWebex.PhoneNumbers.Select(o => $"{o.Value} - {o.Type}")));
            Console.WriteLine(new String('-', 15));
        }
    }
}
