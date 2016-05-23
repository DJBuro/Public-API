var MyAndromeda;
(function (MyAndromeda) {
    var Validation;
    (function (Validation) {
        //http://docs.telerik.com/kendo-ui/getting-started/framework/validator/overview#pattern--constrains-the-value-to-match-a-specific-regular-expression
        var MyAndromedaValidation = (function () {
            function MyAndromedaValidation() {
            }
            MyAndromedaValidation.CreateValidator = function (element, options) {
                //var optionRules = $.extend({}, MyAndromedaValidation.Rules, options.rules)
                if (options.rules) {
                    options.rules = $.extend({}, MyAndromedaValidation.Options.rules, options.rules);
                }
                var $e = $(element);
                return $e.kendoValidator(options).data("kendovalidator");
            };
            MyAndromedaValidation.Options = {
                rules: {}
            };
            MyAndromedaValidation.ActiveValidators = new Array();
            return MyAndromedaValidation;
        }());
        Validation.MyAndromedaValidation = MyAndromedaValidation;
        /* automatically hook up all forms with a validator */
        $(function () {
            console.log("setting up validators");
            var forms = document.getElementsByTagName("form");
            for (var i = 0; i < forms.length; i++) {
                var $f = $(forms[i]);
                if ($f.data("ignore")) {
                    return;
                }
                var validator = MyAndromedaValidation.CreateValidator($f, {});
            }
            console.log("setup " + forms.length + " validators");
        });
    })(Validation = MyAndromeda.Validation || (MyAndromeda.Validation = {}));
})(MyAndromeda || (MyAndromeda = {}));
