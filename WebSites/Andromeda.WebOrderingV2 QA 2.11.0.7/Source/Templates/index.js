var webPage = require('webpage');
var page = null;
var system = require('system');
var fs = require('fs'); // We need to be able to write the static pages out to disk
    
// Global state :(
var scg_pages = '';          // A list of pages that need to be statically generated
var scg_currrentIndex = -1;  // The current page being statically generated (index into the scg_pages array)
var scg_baseUrl = '';        // The base url of the website we're statically generating
var scg_outputFolder = '';   // The base folder to output the static pages to
var scg_timeoutTimer = null; // A window timer used to time-out the entire operation (otherwise it can hang indefinitely)

function scg_checkIfPageReady(outputFilename)
{
    // The web page being loaded by PhantomJS will set window.isFinished true when it's finished running it's JavaScript
    var finished = page.evaluate
    (
        function() 
        {
            return window.isFinished;            
        }
    );

    // Has the web page finished doing it's JavaScript?
    if (finished == undefined || finished)
    {
        // Page is fully loaded.  Write out the static page to disk
        scg_writeOutStaticPage();
        
        // Generate the next page (if there is one)
        setTimeout(scg_generateNextPage, 100);
    }
    else
    {
        // Page is still loading.  Wait a bit and then check again
        window.setTimeout
        (
            function()
            {
                try
                {
                    // Check again
                    scg_checkIfPageReady();
                }
                catch(exception)
                {
                    scg_log("ERROR", exception.name + " " + exception.message);
                    scg_finished();
                }
            }, 
            100
        );
    }
}

function scg_writeOutStaticPage()
{
    // Where to write out the static page to
    var path = scg_outputFolder + "\\" + scg_pages[scg_currrentIndex].replace("/", "\\") + "\\index.html";
   
    scg_log("DEBUG", "Writing static page file to " + path);
    
    // Write the file
    fs.write(path, page.content, 'w');
}

function scg_finished()
{
    scg_log("DEBUG", "Finished");
    
    // Exit phantomjs
    phantom.exit();
}
        
function scg_onLoadFinished(status) 
{
    // Since this is an event, we're no longer in the main try/catch block
    try
    {
        // PhantomJS has loaded the web page
        scg_log("DEBUG", "Page loaded with status " + status);
        
        if (scg_timeoutTimer != undefined && scg_timeoutTimer != null) 
        {
            scg_log("DEBUG", "Killing timer");
            window.clearTimeout(scg_timeoutTimer);
        }
        
        // As it's a Single Page Application, the web page itself does a bunch of JavaScript magic before it's really loaded
        // The web page sets a flag to tell us when it's finished doing it thing.  Check if the page has finished loading
        scg_checkIfPageReady();
        
        // If for some reason the web page never finishes loading we need a time-out
        scg_timeoutTimer = window.setTimeout
        (
            function()
            {
                scg_log("ERROR", "Time-out");
                scg_finished();
            }
            ,60000 // 60 seconds
        );
    }
    catch(exception)
    {
        scg_log("ERROR", exception.name + " " + exception.message);
        scg_finished();
    }
};

function scg_generateNextPage()
{
    // Reset the loaded flag (this will be set true by the web page being loaded when all the JavaScripty bits are finished)
    // This is probably unnecessary...

    // The next page
    scg_currrentIndex = scg_currrentIndex + 1;

    if (scg_currrentIndex < scg_pages.length)  // Array indexes are zero based but lengths are 1 based
    {
        // Figure out the full page url
        var url = scg_baseUrl + "?page=" + scg_pages[scg_currrentIndex];
        
        // Get PhantomJS to open the page
        // Note that this is asynchronous - "page.onLoadFinished" event gets called at some point after the page is loaded
        scg_log("DEBUG", "Loading page " + (scg_currrentIndex + 1) + " of " + scg_pages.length + " : " + url);

        // Create a new page & load it
        page = webPage.create();
        page.settings.resourceTimeout = 60000;
    
        page.onResourceTimeout = function()
        {
            scg_log("ERROR", "Page load time-out");
            scg_finished();
        }
        
        page.open(url, scg_onLoadFinished);
    }
    else
    {
        // All done
        scg_log("DEBUG", "All pages successfully generated");
        scg_finished();
    }
}

function scg_log(type, message)
{
    // Logs a message to the log file - no exceptions allowed here!	
    try
    {
        // The message
        var message = '{ "t":"' + type + ', "d":"' + scg_getDateTimeText() + '", "m":"' + message + '" }\r\n';

        // Write the message out to the log file
        //    var path = 'c:\\logs\\debug.log';
        var currentdate = new Date();
        var path = 'c:\\logs\\' +
            currentdate.getFullYear() + "-" +
            (currentdate.getMonth() + 1) + "-" +
            currentdate.getDate() + '.log';

        fs.write(path, message, 'a');
    }
    catch(e)
    {
        console.log("Failed to log: " + e.message);
    }
}

function scg_getDateTimeText()
{
    var currentdate = new Date(); 
    var dateTime = currentdate.getDate() + "/" +
                   (currentdate.getMonth() + 1) + "/" +
                   currentdate.getFullYear() + " @ " +  
                   currentdate.getHours() + ":" +
                   currentdate.getMinutes() + ":" +
                   currentdate.getSeconds();
                    
    return dateTime;
}

// arg 0 = This filename
// arg 1 = Base folder to output the static pages to
// arg 2 = Base URL of the website to create static pages
// arg 3 = List of hash bangs to generate pages from, delimited by "|||" - NOT including the hash part e.g. "home", "storedetails"

try
{
    var allGood = true;

    scg_log("DEBUG", "Started");

    for (var index = 0; index < system.args.length; index++)
    {
        scg_log("DEBUG", "Argument " + index + ": " + system.args[index]);
    }

    // Sense check
    if (system.args.length != 4) // Zero based index 
    {
        scg_log("ERROR", "Wrong number of arguments. Expected 4 but got " + system.args.length);
        scg_finished();
        allGood = false;
    }
    
    if (allGood)
    {
        // It's a bit crap but since this thing is event driven we lose context 
        // so we'll have to keep a global copy of the variables we're currently working with 

        scg_outputFolder = system.args[1];
        if (scg_outputFolder == undefined || scg_outputFolder == null || scg_outputFolder.length == 0)
        {
            scg_log("ERROR", "Missing argument 1");
            scg_finished();
            allGood = false;
        }
    }

    if (allGood)
    {
        scg_baseUrl = system.args[2];
        if (scg_baseUrl == undefined || scg_baseUrl == null || scg_baseUrl.length == 0)
        {
            scg_log("ERROR", "Missing argument 2");
            scg_finished();
            allGood = false;
        }
    }

    if (allGood)
    {
        var pagesRaw = system.args[3];
        if (pagesRaw == undefined || pagesRaw == null || pagesRaw.length == 0)
        {
            scg_log("ERROR", "Missing argument 3");
            scg_finished();
            allGood = false;
        }
    }

    if (allGood)
    {
        scg_pages = pagesRaw.split("_+_");
        scg_currrentIndex = -1; // Needs to start at -1 so generateNextPage will increment to it zero for the first page

        // Kick off the first page
        scg_generateNextPage();
    }
}
catch(exception)
{
    scg_log("ERROR", exception.name + " " + exception.message);
    scg_finished();
}