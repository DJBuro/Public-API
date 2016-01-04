
module MyAndromeda.Validation {
    //http://docs.telerik.com/kendo-ui/getting-started/framework/validator/overview#pattern--constrains-the-value-to-match-a-specific-regular-expression


	export class MyAndromedaValidation 
	{
		public static Options = {
            rules : {}
        }; 
		public static ActiveValidators = new Array<IActiveValidators>(); 
		
		constructor(){
				
		}
		
		public static CreateValidator(element: any, options : any) : kendo.ui.Validator {
			//var optionRules = $.extend({}, MyAndromedaValidation.Rules, options.rules)
            
            if(options.rules){
                options.rules = $.extend({}, MyAndromedaValidation.Options.rules, options.rules);
            }

            var $e = $(element);
             
            return $e.kendoValidator(options).data("kendovalidator");
		}
	}
	/* automatically hook up all forms with a validator */
	$(function(){
        console.log("setting up validators");

		var forms= document.getElementsByTagName("form");

        for(var i=0; i< forms.length; i++){
            var $f = $(forms[i]);
            if($f.data("ignore")){ return; }
			var validator = MyAndromedaValidation.CreateValidator($f, {});

        }
        
        console.log("setup " + forms.length + " validators");
	});
}

interface IActiveValidators
{
	id : string;
	getValidator() : kendo.ui.Validator
}

/* todo add add custom rules */
/* 
MyAndromeda.Validation.MyAndromedaValidation
{
	
}

*/