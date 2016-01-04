var queryStringHelper =
{
    queryString: {},
    email: undefined,
    firstName: undefined,
    lastName: undefined,
    telephoneNumber: undefined,
    marketing: undefined,
    city: undefined,
    postcode: undefined,
    houseNumber: undefined,
    roadName: undefined,
    orderType: undefined,
    initialise: function ()
    {
        // Extract the query string and parse it
        queryStringHelper.queryString = {};
        location.search.substr(1).split("&").forEach
        (
            function (pair)
            {
                if (pair === "") return;
                var parts = pair.split("=");
                queryStringHelper.queryString[parts[0]] = parts[1] && decodeURIComponent(parts[1].replace(/\+/g, " "));
            }
        );

        // Check to see if any other information was passed in through the query string
        queryStringHelper.extractData();
    },
    extractData: function ()
    {
        // Extract data from the query
        queryStringHelper.externalSiteId = queryStringHelper.queryString['s'];
        queryStringHelper.email = queryStringHelper.queryString['e'];
        queryStringHelper.firstName = queryStringHelper.queryString['f'];
        queryStringHelper.lastName = queryStringHelper.queryString['l'];
        queryStringHelper.telephoneNumber = queryStringHelper.queryString['t'];
        queryStringHelper.marketing = queryStringHelper.queryString['m'] == 'y' ? true : false;
        queryStringHelper.town = queryStringHelper.queryString['c'];
        queryStringHelper.postcode = queryStringHelper.queryString['p'];
        queryStringHelper.houseNumber = queryStringHelper.queryString['n'];
        queryStringHelper.roadName = queryStringHelper.queryString['r'];
        queryStringHelper.orderType = queryStringHelper.queryString['ot'];

        // Sanitise the url shown in the browser
        var url = window.location.href;
        var queryIndex = url.indexOf('/?');
        if (queryStringHelper.externalSiteId != undefined && queryIndex != -1)
        {
            url = url.substring(0, queryIndex);

            // Do we need to hide the passthrough query string?
            if (settings.hidePassthroughQueryString)
            {
                History.pushState(null, document.title, url);
            }
        }
    }
}