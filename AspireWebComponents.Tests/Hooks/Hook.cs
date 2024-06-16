using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace AspireWebComponents.Tests.Tests.Hooks;

[Binding]
public sealed class Hook
{
    private DistributedApplication _app;
    public IPage User { get; private set; } = null!;

    public string WebFrontendUri => _app.GetEndpoint("webfrontend").ToString();
    
    [BeforeScenario]
    public async Task BeforeScenario()
    {
        // Initialize AppHost
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireWebComponents_AppHost>();
        _app = await appHost.BuildAsync();
        await _app.StartAsync();
        
        //Initialise Playwright
        var playwright = await Playwright.CreateAsync();
        //Initialise a browser - 'Chromium' can be changed to 'Firefox' or 'Webkit'
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Timeout = 0,
            Headless = false // -> Use this option to be able to see your test running
        });
        
        //Setup a browser context
        var context1 = await browser.NewContextAsync();

        //Initialise a page on the browser context.
        User = await context1.NewPageAsync(); 
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _app.Dispose();
    }
}