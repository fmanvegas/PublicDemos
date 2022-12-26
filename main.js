import { ICON_MAP } from "./iconMap";
import "./style.css"
import { getWeather } from "./weather"

navigator.geolocation.getCurrentPosition(positionSuccess, positionError)

function positionSuccess({ coords }) {
  getWeather(
    coords.latitude,
    coords.longitude,
    Intl.DateTimeFormat().resolvedOptions().timeZone)
    .then(renderWeather)
    .catch(e => {
      console.error(e);
      alert("Error Caught Getting Weather");
    });
}
function positionError() {
  alert("Could Not Get GeoLocation");
}



function getIconUrl(iconCode) {
  return `icons/${ICON_MAP.get(iconCode)}.svg`;
}

const currentIcon = document.querySelector("[data-current-icon]")


function renderWeather({ current, daily, hourly }) {


  renderCurrentWeather(current)
  renderDaily(daily)
  renderHourly(hourly)

  document.body.classList.remove("blurred")
}

function setValue(selector, value, { parent = document } = {}) {
  parent.querySelector(`[data-${selector}]`).textContent = value;
}


function renderCurrentWeather(current) {

  currentIcon.src = getIconUrl(current.iconCode)

  setValue("current-temp", current.currentTemp);
  setValue("current-high", current.highTemp);
  setValue("current-low", current.lowTemp);
  setValue("current-fl-high", current.highFeelsLike);
  setValue("current-fl-low", current.lowFeelsLike);
  setValue("current-wind", current.windSpeed);
  setValue("current-precip", current.precip);

}


const DAY_FORMATTER = new Intl.DateTimeFormat(undefined, { weekday: "long" });
const dailySection = document.querySelector("[data-day-section]")
const dayCard = document.getElementById("day-card-template");
function renderDaily(daily) {

  dailySection.innerHTML = "";
  daily.forEach(day => {

    const element = dayCard.content.cloneNode(true);
    setValue("highT", day.tempMax, { parent: element });
    setValue("lowT", day.tempMin, { parent: element });
    setValue("date", DAY_FORMATTER.format(day.time), { parent: element });
    element.querySelector("[data-icon]").src = getIconUrl(day.iconCode);
    dailySection.append(element);
  });


}

const HOUR_FORMATTER = new Intl.DateTimeFormat(undefined, { hour: "numeric" });
const hourlySection = document.querySelector("[data-hour-section]")
const hourRow = document.getElementById("hour-row-template");
function renderHourly(hourly) {
  hourlySection.innerHTML = "";
  hourly.forEach(day => {
    const element = hourRow.content.cloneNode(true);

    setValue("temp", day.temp, { parent: element });
    setValue("day", DAY_FORMATTER.format(day.timestamp), { parent: element });
    setValue("time", HOUR_FORMATTER.format(day.timestamp), { parent: element });
    setValue("fl-temp", day.feelsLike, { parent: element });
    setValue("wind", day.windSpeed, { parent: element });
    setValue("precip", day.precip, { parent: element });
    element.querySelector("[data-icon]").src = getIconUrl(day.iconCode);



    hourlySection.append(element);
  });

}

