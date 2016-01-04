(function($) {
jQuery.zoomLook = function() {
    var cookie = $.cookie('Dashboard.Zoom'); // get cookie

    if (cookie != null) {
        var values = cookie.split('&');
        for (var i = 0; i < values.length; i++) {
            var zoomSize = values[i].split('=');
            $(document.body).css("zoom", zoomSize[0]);
        }
    }
}

jQuery.zoom = function(zoom) {
   $.cookie('Dashboard.Zoom', zoom, {path: '/'}); 
    $(document.body).css("zoom", zoom);
}

})(jQuery);