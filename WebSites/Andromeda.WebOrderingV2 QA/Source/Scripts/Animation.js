ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.unwrap(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        // Whenever the value subsequently changes, slowly fade the element in or out
        var value = valueAccessor();
        
        var fadeIn = ko.unwrap(value);
        
        if(fadeIn){
            $(element).animate({ opacity: 1, height: "auto" });  
        }else{
            $(element).animate({ opacity: 0, height: 0 });//fadeOut()
        }
        //? .fadeIn() : ;
    }
};