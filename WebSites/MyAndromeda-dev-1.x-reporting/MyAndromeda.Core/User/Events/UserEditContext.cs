using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Core.User.Events
{

    public class UserEditContext
    {
        public UserEditContext(MyAndromedaUser user, bool isCancel)
        {
            this.User = user;
            this.Cancel = isCancel;
        }
        public MyAndromedaUser User { get; set; }
        public bool Cancel { get; set; }

    }

}