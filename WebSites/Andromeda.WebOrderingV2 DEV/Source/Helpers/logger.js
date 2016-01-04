var AndroWeb;
(function (AndroWeb) {
    "use strict";
    var Logger = (function () {
        function Logger() {
            this.UseNotify = true;
            this.UseDebug = true;
            this.UseError = true;
        }
        Logger.Notify = function (o) {
            if (logger.UseNotify) {
            }
        };
        Logger.Debug = function (o) {
            if (logger.UseDebug) {
            }
        };
        Logger.Error = function (o) {
            if (logger.UseError) {
            }
        };
        return Logger;
    })();
    AndroWeb.Logger = Logger;
    var logger = new Logger();
})(AndroWeb || (AndroWeb = {}));
//# sourceMappingURL=logger.js.map