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

        // See if there is a page query
        queryStringHelper.extractPageData();

        // If the path contains the page, extract the path - not really query string related but the codes gotta go somewhere
        queryStringHelper.translatePathToPage();
    },
    extractData: function ()
    {
        // Extract data from the query
        queryStringHelper.externalSiteId = queryStringHelper.queryString['s'];
        queryStringHelper.email = queryStringHelper.queryString['e'];
        queryStringHelper.firstName = queryStringHelper.queryString['f'];
        queryStringHelper.lastName = queryStringHelper.queryString['l'];
        queryStringHelper.telephoneNumber = queryStringHelper.queryString['t'];
        queryStringHelper.marketing = false;
        queryStringHelper.town = queryStringHelper.queryString['c'];
        queryStringHelper.postcode = queryStringHelper.queryString['p'];
        queryStringHelper.houseNumber = queryStringHelper.queryString['n'];
        queryStringHelper.roadName = queryStringHelper.queryString['r'];
        queryStringHelper.orderType = queryStringHelper.queryString['ot'];
        
        var marketing = queryStringHelper.queryString['m'];
        if (marketing != undefined && typeof (marketing) == 'string' && marketing.toUpperCase() == 'Y')
        {
            queryStringHelper.marketing = true
        }

        queryStringHelper.prem2 = queryStringHelper.queryString['p2'];
        queryStringHelper.prem3 = queryStringHelper.queryString['p3'];
        queryStringHelper.org1 = queryStringHelper.queryString['or1'];
        queryStringHelper.org2 = queryStringHelper.queryString['or2'];
        queryStringHelper.userLocality1 = queryStringHelper.queryString['u1'];
        queryStringHelper.userLocality2 = queryStringHelper.queryString['u2'];
        queryStringHelper.userLocality3 = queryStringHelper.queryString['u3'];
        queryStringHelper.directions = queryStringHelper.queryString['d'];

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

        queryStringHelper.passwordReset = queryStringHelper.queryString['passwordreset'];
        queryStringHelper.forUser = queryStringHelper.queryString['for'];
    },
    extractPageData: function()
    {
        if (queryStringHelper.queryString['page'] === undefined) return;

        // Extract the query string elements
        var pageQueryString = queryStringHelper.queryString['page'].toUpperCase();

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
    }
}