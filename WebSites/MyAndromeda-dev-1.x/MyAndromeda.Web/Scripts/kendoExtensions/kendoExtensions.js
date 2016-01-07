/// <reference path="kendo.all.js" />

kendo.data.binders.twoDecimal = kendo.data.Binder.extend({
    refresh: function () {
        var value = this.bindings["twoDecimal"].get();

        if (value) {
            $(this.element).text(kendo.toString(value, "0.00"));
        }
    }
});

kendo.data.binders.currency = kendo.data.Binder.extend({
    refresh: function () {
        var value = this.bindings["currency"].get();

        if (value) {
            $(this.element).text(kendo.toString(value, "c"));
        }
    }
});

kendo.data.binders.percent = kendo.data.Binder.extend({
    refresh: function (key) {
        var value = this.bindings["percent"].get();

        if (value) {
            $(this.element).text(kendo.toString(value, "p"));
        }
    }
});

kendo.data.binders.widget.maxDate = kendo.data.Binder.extend({
    init: function (widget, bindings, options) {
        //call the base constructor
        kendo.data.Binder.fn.init.call(this, widget.element[0], bindings, options);
    },
    refresh: function () {
        var that = this,
        value = that.bindings["maxDate"].get(); //get the value from the View-Model
        $(that.element).data("kendoDatePicker").max(value); //update the widget
    }
});

kendo.data.binders.widget.minDate = kendo.data.Binder.extend({
    init: function (widget, bindings, options) {
        //call the base constructor
        kendo.data.Binder.fn.init.call(this, widget.element[0], bindings, options);
    },
    refresh: function () {
        var that = this,
        value = that.bindings["minDate"].get(); //get the value from the View-Model
        $(that.element).data("kendoDatePicker").min(value); //update the widget
    }
});

kendo.data.binders.widget.max = kendo.data.Binder.extend({
    refresh: function () {
        var maxValue = this.bindings["max"].get();
        if (maxValue) {
            $(this.element).attr("data-max", maxValue);
        }
        var widgetValue = this.bindings["value"].get();
        if (widgetValue) { 
            if (widgetValue > maxValue)
            {
                this.bindings["value"].set(maxValue);
            }
        }
        if (this.element instanceof kendo.ui.NumericTextBox)
        {
            this.element.max(maxValue);
        }
    }
});
kendo.data.binders.widget.title = kendo.data.Binder.extend({
    refresh: function () {
        var value = this.bindings["title"].get();
        if (value) {
            $(this.element).attr("title", value);
        }
    }
});




kendo.data.binders.max = kendo.data.Binder.extend({
    refresh: function () {
        var value = this.bindings["max"].get();
        if (value) {
            $(this.element).attr("max", value);
        }
    }
});
kendo.data.binders.title = kendo.data.Binder.extend({
    refresh: function () {
        var value = this.bindings["title"].get();
        if (value) {
            $(this.element).attr("title", value);
        }
    }
});
kendo.data.binders.placeholder = kendo.data.Binder.extend({
    refresh: function () {
        var value = this.bindings["placeholder"].get();
        if (value) {
            $(this.element).attr("placeholder", value);
        }
    }
});

kendo.data.binders.visibleSlide = kendo.data.Binder.extend({
    refresh: function () {
        var value = this.bindings["visibleSlide"].get();
        if (value && value !== "") { 
            $(this.element).slideDown();
        } else {
            $(this.element).slideUp();
        }
    }
});
kendo.data.binders.visibleSoFadeIn = kendo.data.Binder.extend({
    refresh: function () {
        var value = this.bindings["visibleSoFadeIn"].get();

        var isArray = value instanceof Array;
        var isObject = jQuery.isPlainObject(value);
        var isFunction = jQuery.isFunction(value);

        if (value && value !== "") {
            //if (isArray)
            //{
            //    if (value.length === 0) {
            //        $(this.element).fadeOut();
            //        return;
            //    }
            //}
            //if (isObject) {
            //    if (jQuery.isEmptyObject(value)) {
            //        $(this.element).fadeOut();
            //        return;
            //    }
            //}
            //if (isFunction(value)) {
            //    var v = value();

            //    if (!v || v.length === 0) {
            //        $(this.element).fadeOut();
            //        return;
            //    }
            //}

            $(this.element).fadeIn();

        } else {
            $(this.element).fadeOut();
        }
    }
});
