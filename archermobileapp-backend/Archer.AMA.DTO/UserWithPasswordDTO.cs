using System;
using System.Collections.Generic;
using System.Text;

using Archer.AMA.DTO.Base;

namespace Archer.AMA.DTO
{
    public class UserWithPasswordDTO : UserDTO
    {
        public string Password { get; set; }

        public string OldPassword { get; set; }

        public string InputRole { get; set; }
    }
}
