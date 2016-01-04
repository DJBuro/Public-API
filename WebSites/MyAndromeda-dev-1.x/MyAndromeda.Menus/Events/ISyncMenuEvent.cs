using MyAndromeda.Core;

namespace MyAndromeda.Menus.Events
{
    public interface ISyncMenuEvent : IDependency 
    {
        void LoadedMenuFromFtp();
    }
}