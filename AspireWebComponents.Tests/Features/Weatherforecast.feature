Feature: Weatherforecast
	
@barchart
Scenario: View forecast
	Given the forecast page is shown
	When the load forecast button is pressed
	Then the page shows a bar chart