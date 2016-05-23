
module MyAndromeda.Chain.Services {
    export class chainService implements Models.IChainService
    {
        public chainServiceRoutes: Models.IChainServiceRoutes;

        constructor(chainServiceRoutes: Models.IChainServiceRoutes)
        {
            this.chainServiceRoutes = chainServiceRoutes;
        }

        public get(id: number, callback: { (chain: Models.IChain): void })
        {
            var internal = this,
                route = {
                    type: "POST",
                    dataType: "json",
                    data: { id: id },
                    success: function (data) {
                        callback(data);
                    }
                };

            $.ajax($.extend({}, route, {
                url: internal.chainServiceRoutes.getById
            }));
        }
    }
}
