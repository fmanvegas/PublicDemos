// https://api.open-meteo.com/v1/forecast?latitude=36.17&longitude=-115.14&hourly=temperature_2m,apparent_temperature,precipitation,weathercode,windspeed_10m&daily=weathercode,temperature_2m_max,temperature_2m_min,apparent_temperature_max,apparent_temperature_min,precipitation_sum,windspeed_10m_max&current_weather=true&temperature_unit=fahrenheit&windspeed_unit=mph&precipitation_unit=inch&timeformat=unixtime&timezone=America%2FLos_Angeles
import axios from "axios";

export function getWeather(lat, lon, tz) {

    return axios.get("https://api.open-meteo.com/v1/forecast?hourly=temperature_2m,apparent_temperature,precipitation,weathercode,windspeed_10m&daily=weathercode,temperature_2m_max,temperature_2m_min,apparent_temperature_max,apparent_temperature_min,precipitation_sum,windspeed_10m_max&current_weather=true&temperature_unit=fahrenheit&windspeed_unit=mph&precipitation_unit=inch&timeformat=unixtime",
        {
            params: {
                latitude: lat,
                longitude: lon,
                timezone: tz
            }
        }
    )
        .then(({ data }) => {

            return {
                current: parseCurrentWeather(data),
                daily: parseDailyWeather(data),
                hourly: parseHourlyWeather(data),
            }
        })
        ;
}

function parseCurrentWeather({ current_weather, daily }) {

    const {
        temperature: currenttemp,
        windspeed: windSpeed,
        weathercode: iconCode,
        winddirection: windDirection,
    } = current_weather

    const {
        precipitation_sum: [precip],
        apparent_temperature_max: [highFeelsLike],
        apparent_temperature_min: [lowFeelsLike],
        temperature_2m_max: [highTemp],
        temperature_2m_min: [lowTemp],
    } = daily

    return {
        currentTemp: Math.round(currenttemp),
        highTemp: Math.round(highTemp),
        lowTemp: Math.round(lowTemp),
        highFeelsLike: Math.round(highFeelsLike),
        lowFeelsLike: Math.round(lowFeelsLike),
        windSpeed: Math.round(windSpeed),
        precip: Math.round((precip * 100) / 100),
        iconCode
    }

}
function parseDailyWeather({ daily }) {

    return daily.time.map((time, index) => {
        return {
            time: time * 1000,
            iconCode: daily.weathercode[index],
            tempMax: daily.temperature_2m_max[index],
            tempMin: daily.temperature_2m_min[index]
        }

    });

}
function parseHourlyWeather({ hourly, current_weather }) {
    return hourly.time.map((time, index) => {
        return {
            timestamp: (time * 1000),
            iconCode: hourly.weathercode[index],
            temp: Math.round(hourly.temperature_2m[index]),
            feelsLike: Math.round(hourly.apparent_temperature[index]),
            windSpeed: Math.round(hourly.windspeed_10m[index]),
            precip: Math.round((hourly.precipitation[index] * 100) / 100),
        }

    }).filter(({ timestamp }) => timestamp >= current_weather.time * 1000)

}