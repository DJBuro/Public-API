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
    prem2: undefined,
    prem3: undefined,
    org1: undefined,
    org2: undefined,
    userLocality1: undefined,
    userLocality2: undefined,
    userLocality3: undefined,
    orderType: undefined,
    directions: undefined,
    passwordReset: undefined,
    forUser: undefined,
    siteId: undefined,
    siteName: undefined,
    pageName: undefined,
    menuSection: undefined,
    voucherCode: undefined,
    tableNumber: undefined,

    initialise: function ()
    {
        // Extract the query string and parse it
        queryStringHelper.queryString = {};
        var chunks = location.search.substr(1).split("&")

        // Split the query string down into individual parts
        for (var index = 0; index < chunks.length; index++)
        {
            var keyValuePair = chunks[index];

            // Ignore empty key value pairs
            if (keyValuePair === "") continue;

            // Each part of the query string consists of <key>=<value>
            var parts = keyValuePair.split("=");
            if (parts.length > 1)
            {
                var key = parts[0];
                var value = parts[1];
                
                if (value !== undefined && typeof (value) == "string")
                {
                    value = decodeURIComponent(value.replace(/\+/g, " "));
                }

                queryStringHelper.queryString[key] = value;
            }
            else if (parts.length === 1)
            {
                // No value part = just the key
                // Is this the table number?
                var key = parts[0];

                if (key.length > 5)
                {
                    var keyUpper = key.substr(0, 5).toUpperCase();
                    if (keyUpper === "TABLE")
                    {
                        // Extract the table number
                        var tableNumberText = key.substr(5);
                        if ($.isNumeric(tableNumberText))
                        {
                            queryStringHelper.tableNumber = Number(tableNumberText);

                            // Engage dine in mode
                            viewModel.setDineInMode(true);
                        }
                    }
                }
            }
        }

        // Check to see if any other information was passed in through the query string
        queryStringHelper.extractData();

        // See if there is a page query
        queryStringHelper.extractPageData();

        // If the path contains the page, extract the path - not really query string related but the codes gotta go somewhere
        queryStringHelper.translatePathToPage();
    },
    decodeQueryPart: function (queryPart)
    {
        return queryStringHelper.queryString[queryPart];
     //   var queryValue = queryStringHelper.queryString[queryPart];
     //   return (queryValue === undefined ? undefined : decodeURI(queryPart));
    },
    extractData: function ()
    {
        // Extract data from the query
        queryStringHelper.externalSiteId = this.decodeQueryPart('s');
        queryStringHelper.email = this.decodeQueryPart('e');
        queryStringHelper.firstName = this.decodeQueryPart('f');
        queryStringHelper.lastName = this.decodeQueryPart('l');
        queryStringHelper.telephoneNumber = this.decodeQueryPart('t');
        queryStringHelper.marketing = false;
        queryStringHelper.town = this.decodeQueryPart('c');
        queryStringHelper.postcode = this.decodeQueryPart('p');
        queryStringHelper.houseNumber = this.decodeQueryPart('n');
        queryStringHelper.roadName = this.decodeQueryPart('r');
        queryStringHelper.orderType = this.decodeQueryPart('ot');
        
        var marketing = this.decodeQueryPart('m');
        if (marketing != undefined && typeof (marketing) == 'string' && marketing.toUpperCase() == 'Y')
        {
            queryStringHelper.marketing = true
        }

        queryStringHelper.prem2 = this.decodeQueryPart('p2');
        queryStringHelper.prem3 = this.decodeQueryPart('p3');
        queryStringHelper.org1 = this.decodeQueryPart('or1');
        queryStringHelper.org2 = this.decodeQueryPart('or2');
        queryStringHelper.userLocality1 = this.decodeQueryPart('u1');
        queryStringHelper.userLocality2 = this.decodeQueryPart('u2');
        queryStringHelper.userLocality3 = this.decodeQueryPart('u3');
        queryStringHelper.directions = this.decodeQueryPart('d');
        queryStringHelper.voucherCode = this.decodeQueryPart('v');

        queryStringHelper.passwordReset = this.decodeQueryPart('passwordreset');
        queryStringHelper.forUser = this.decodeQueryPart('for');

        // Sanitise the url shown in the browser
        this.sanitiseUrl();
    },
    extractPageData: function()
    {
        if (queryStringHelper.queryString['page'] === undefined) return;

        // Extract the query string elements
        var pageQueryString = this.decodeQueryPart('page').toUpperCase();

        // Do we need to display a specific page?
        if (pageQueryString === undefined || pageQueryString.length === 0)
        {
            // Just show the choose store page
            viewModel.chooseStore();
            return;
        }

        var pageQueryStringParts = pageQueryString.split('/');

        if (pageQueryStringParts.length >= 2)
        {
            // Extract the page details from the query string
            queryStringHelper.storeName = pageQueryStringParts[0];
            queryStringHelper.pageName = pageQueryStringParts[1];
            queryStringHelper.menuSection = pageQueryStringParts.length === 3 ? pageQueryStringParts[2] : undefined;
        }

        // Sanitise the url shown in the browser
        this.sanitiseUrl();
    },
    translatePathToPage: function()
    {
        var pathUpper = window.location.pathname.toUpperCase();
        if (pathUpper[pathUpper.length - 1] == '/') pathUpper = pathUpper.substring(0, pathUpper.length - 1);

        var pathParts = pathUpper.split("/");

        if (pathParts.length >= 2 && pathParts[pathParts.length - 1] === 'HOME')
        {
            queryStringHelper.storeName = pathParts[pathParts.length - 2];
            queryStringHelper.pageName = 'HOME';
            queryStringHelper.menuSection = '';
        }
        else if (pathParts.length >= 2 && pathParts[pathParts.length - 1] === 'STOREDETAILS')
        {
            queryStringHelper.storeName = pathParts[pathParts.length - 2];
            queryStringHelper.pageName = 'STOREDETAILS';
            queryStringHelper.menuSection = '';
        }
        else if (pathParts.length >= 3 && pathParts[pathParts.length - 2] === 'MENU')
        {
            queryStringHelper.storeName = pathParts[pathParts.length - 3];
            queryStringHelper.pageName = 'MENU';
            queryStringHelper.menuSection = pathParts[pathParts.length - 1];
        }
    },
    sanitiseUrl: function()
    {
        // Sanitise the url shown in the browser
        var url = window.location.href;
        var queryIndex = url.indexOf('?');
        //if (queryStringHelper.externalSiteId != undefined && queryIndex != -1)
        if (queryIndex != -1)
        {
            url = url.substring(0, queryIndex);

            // Do we need to hide the passthrough query string?
         //   if (settings.hidePassthroughQueryString)
            //   {
            window.history.pushState(null, document.title, url);
            //    History.pushState(null, document.title, url);
         //   }
        }
    }
}