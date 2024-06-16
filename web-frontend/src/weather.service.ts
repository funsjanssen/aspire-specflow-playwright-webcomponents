import { createContext } from "@lit/context";

export const weatherContext = createContext<WeatherService>('weather');

export class WeatherService {
    async getWeatherForecast(signal: AbortSignal): Promise<Response> {
        console.log('getting weather forecast');
        return await fetch('/api/weatherforecast', { signal });
    }
}