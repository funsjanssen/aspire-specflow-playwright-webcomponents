import {LitElement, html, css} from 'lit';
import {customElement} from 'lit/decorators.js';

import { max, sort, scaleLinear, create, select, scaleBand, axisTop, axisLeft } from 'd3';
import {provide} from "@lit/context";
import {weatherContext, WeatherService} from "./weather.service";
import {Weather} from './weather.types';
import {Task} from '@lit/task';

@customElement('weather-forecast')
export class WeatherForecast extends LitElement {
    static override styles = css`
        :host {
            display: block;
            padding: 16px;
        }
    `;

    @provide({context: weatherContext})
    weatherService: WeatherService = new WeatherService();

    private _weatherTask = new Task(this, {
        task: async ([], {signal}) => {
            const response = await this.weatherService.getWeatherForecast(signal);
            if (!response.ok) {
                throw new Error(`Status code: ${response.status}`);
            }
            return await response.json() as Weather[];
        },
        args: () => []
    });

    override render() {
        return this._weatherTask.render({
            pending: () => html`<p>Loading weather</p>`,
            complete: (weather) => html`
                ${this.getChart(weather)}
            `,
            error: (e) => html`<p>Error: ${e}</p>`
        });
    }

    private getChart(forecast: Weather[]) {

        // Specify the chart’s dimensions, based on a bar’s height.
        const barHeight = 20;
        const marginTop = 24;
        const marginRight = 0;
        const marginBottom = 0;
        const marginLeft = 80;
        const width = 600;
        const height = Math.ceil((forecast!.length + 0.1) * barHeight) + marginTop + marginBottom;

        // Create the scales.
        const x = scaleLinear()
            .domain([0, max(forecast!, d => d.temperatureC)] as number[])
            .range([marginLeft, width - marginRight]);

        const y = scaleBand()
            .domain(sort(forecast!, d => d.date).map(d => d.date.toString()))
            .rangeRound([marginTop, height - marginBottom])
            .padding(0.1);

        // Create a value format.
        const format = x.tickFormat(20);

        // Create the SVG container.
        const svg = create("svg")
            .attr("data-testid", "forecast-chart")
            .attr("width", width)
            .attr("height", height)
            .attr("viewBox", [0, 0, width, height])
            .attr("style", "max-width: 100%; height: auto; font-size: 11px;");

        function mouseOver(e: any, _: Weather) {
            select(e.target)
                .style("opacity", 0.8);
        }

        function mouseLeave(e: any) {
            select(e.target)
                .style("opacity", 1)
        }

        // Append a rect for each letter.
        svg.append("g")
            .attr("fill", "#090b7d")
            .selectAll()
            .data(forecast!)
            .join("rect")
            .attr("aria-describedby", "tooltip")
            .on("mouseover", mouseOver)
            .on("mouseleave", mouseLeave)
            .attr("x", x(0))
            .attr("y", (d) => y(d.date.toString()) as number)
            .attr("height", y.bandwidth())
            .transition()
            .duration(250)
            .attr("width", (d) => x(d.temperatureC) - x(0));

        // Append a label for each letter.
        svg.append("g")
            .attr("fill", "white")
            .attr("text-anchor", "end")
            .selectAll()
            .data(forecast!)
            .join("text")
            .style("opacity", 0)
            .attr("x", (d) => x(d.temperatureC))
            .attr("y", (d) => y(d.date.toString()) as number + y.bandwidth() / 2)
            .attr("dy", "0.35em")
            .attr("dx", -4)
            .text((d) => format(d.temperatureC))
            .call((text) => text.filter(d => x(d.temperatureC) - x(0) < 20) // short bars
                .attr("dx", +4)
                .attr("fill", "black")
                .attr("text-anchor", "start"))
            .transition()
            .delay(200)
            .style("opacity", 1);

        // Create the axes.
        svg.append("g")
            .attr("transform", `translate(0,${marginTop})`)
            .call(axisTop(x).ticks(width / 80))
            .call(g => g.select(".domain").remove());

        svg.append("g")
            .attr("transform", `translate(${marginLeft},0)`)
            .call(axisLeft(y).tickSizeOuter(0));

        return svg.node();
    }
}