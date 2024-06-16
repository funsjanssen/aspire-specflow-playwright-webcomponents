using AspireWebComponents.Tests.Tests.Hooks;
using AspireWebComponents.Tests.Tests.Pages;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace AspireWebComponents.Tests;

[Binding]
public class ForecastSteps(Hook hooks, ForecastPage forecastPage)
{
    private readonly IPage _user = hooks.User;
    
    private readonly string _webFrontendUri = hooks.WebFrontendUri;
    
    private readonly ForecastPage _forecastPage = forecastPage;

    [Then(@"the page shows a bar chart")]
    public async Task ThenThePageShowsABarChart()
    {
        await _forecastPage.AssertBarChart();
    }

    [When(@"the forecast is requested")]
    public async Task WhenTheForecastIsRequested()
    {
        await _user.GotoAsync(_webFrontendUri, new PageGotoOptions{ Timeout = 0 });
    }
}