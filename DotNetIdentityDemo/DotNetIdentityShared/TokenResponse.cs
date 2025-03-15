using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetIdentityShared
{
    public class TokenResponse : UserManagerResponse
    {
        public DateTime? ExpiresOn { get; set; }
    }
}
