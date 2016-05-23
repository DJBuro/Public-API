module MyAndromeda.WebOrdering {
    "use strict";
    export class Logger
    {
        public UseNotify: boolean = true;
        public UseDebug: boolean = true;
        public UseError: boolean = true;

        public static Notify(o) {
            if (logger.UseNotify) {
                console.log(o);
            }
        }

        public static Debug(o)
        {
            if (logger.UseDebug) { 
                console.log(o);
            }
        }   
        public static Error(o)
        {
            if (logger.UseError) { 
                console.log(o);
            }
        }

        public static SettingUpController(name, state)
        {
            if (logger.UseNotify) {
                console.log("setting up controller - " + name + " : " + state);
            }
        }
        public static SettingUpService(name, state)
        {
            if (logger.UseNotify) {
                console.log("setting up service - " + name + " : " + state);
            }
        }

        public static AllowDebug(value: boolean) {
            logger.UseDebug = value;
        }
        public static AllowError(value: boolean) {
            logger.UseError = value;
        }
    }

    var logger = new Logger();
} 