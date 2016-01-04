module AndroWeb {
    "use strict";
    export class Logger {
        public UseNotify: boolean = true;
        public UseDebug: boolean = true;
        public UseError: boolean = true;

        public static Notify(o) {
            if (logger.UseNotify) {
     //           console.log(o);
            }
        }

        public static Debug(o) {
            if (logger.UseDebug) {
    //            console.log(o);
            }
        }
        public static Error(o) {
            if (logger.UseError) {
     //           console.error(o);
            }
        }
    }

    var logger = new Logger();
} 