using Microsoft.AspNet.Identity;

namespace MyAndromeda.Identity
{
    public class MyAndromedaIdentityRole : IRole<int>
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }

}
