using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvisioningTest
{

    //this class contains the desired state that should be in webex after running the job
    internal class ProvisioningOperation
    {
        /// <summary>
        /// enw value in the AUDM database
        /// => this value should be written into the webex user (PhoneNumber.Value = this value, Type = Mobile)
        /// </summary>
        internal string DesiredMobileNumber { get; set; }

        /// <summary>
        /// current value in the AUDM database
        /// </summary>
        internal string PreviousMobileNumber { get; set; }
    }
}
