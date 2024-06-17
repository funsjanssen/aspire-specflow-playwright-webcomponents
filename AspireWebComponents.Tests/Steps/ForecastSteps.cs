using AspireWebComponents.Tests.Tests.Hooks;
using AspireWebComponents.Tests.Tests.Pages;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace AspireWebComponents.Tests;

[Binding]
public class ForecastSteps(ForecastPage forecastPage)
{
    private readonly ForecastPage _forecastPage = forecastPage;

    [Given(@"the forecast page is shown")]
    public async Task GivenTheForecastPageIsShown()
    {
        await _forecastPage.GoToPageAsync();
    }
    
    [When(@"the load forecast button is pressed")]
    public async Task WhenTheLoadForecastButtonIsPressed()
    {
        await _forecastPage.ClickLoadButtonAsync();
    }

    [Then(@"the page shows a bar chart")]
    public async Task ThenThePageShowsABarChart()
    {
        await _forecastPage.AssertBarChartAsync();
    }
}