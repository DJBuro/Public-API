using System;

namespace MyAndromeda.Services.WebHooks.Events
{
    [Flags]
    public enum WebHookType 
    {
        Unset = 0,
        StoreStaus = 1,

        OrderEta = 1 << 1,
        OrderStatus = 1 << 2,

        MenuChange = 1 << 3,
        MenuItemChange = 1 << 4,

        BringgUpdate = 1 << 5,
        BringgEtaUpdate = 1<< 6,

        All = 1 + 1<<1 + 1<<2 + 1<<3 + 1<<4 + 1<<5 + 1<<6
    }
}