# Weather Application

A simple application to fetch and display weather data based on user input, with the option to choose the graph type. The data is saved both in a database and as a local image file, and a chart is also saved to the database.

## Description

This Weather Application allows users to input a city name and select the type of graph they want to display the weather data for the next 5 days. The application fetches weather data from the OpenWeather API, stores it in a database, and displays it on a chart using ScottPlot. The chart is saved both in the database and as a PNG file on the local system.

## Features

- User inputs the city name.
- User selects the type of graph (e.g., Line, Bar).
- Fetches weather data for the next 5 days from OpenWeather API.
- Displays temperature and other weather metrics (humidity, wind speed, etc.) in the chosen graph type.
- Stores weather data and chart in a database.
- Saves the generated chart as a PNG file in the local system.
- Allows easy customization of city and API key using configuration.

## Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/jimabby/WeatherApplication.git
    cd weather-application
    ```

2. Install the required dependencies:

    If you're using .NET, run the following command:

    ```bash
    dotnet restore
    ```

3. Set up your API key using environment variables:

    ```bash
    $env:ApiKey="your_api_key_here" 
    ```

4. Set up your database (SQLite is used in this example). Ensure that your connection string is properly configured in the application.

## Usage

1. Run the application:

    ```bash
    dotnet run
    ```

2. Enter the city name when prompted.

3. Select the desired graph type (e.g., "Line" or "Bar").

4. The weather forecast will be displayed in the selected graph type and saved in the local directory as `weather_forecast.png`.

5. The weather data, along with the chart, will also be saved in the database.

6. You will also see the weather data printed out in the console.

## Example Flow

1. **Enter city**: Enter the city name (You can enter 'exit' to exit.):
   
2. **Select graph type**: After entering the city, you will be prompted to select the graph type, e.g., "Scatter" or "Line" or "Bar".
   
3. **Weather data**: The application will display the weather forecast for the next 5 days, including temperature, humidity, wind speed, and pressure.
   
4. **Chart**: A chart of the weather data will be generated based on the selected graph type, saved locally as `weather_forecast.png`, and stored in the database.

## Contributing

1. Fork the repository.
2. Create your feature branch (`git checkout -b feature-name`).
3. Commit your changes (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature-name`).
5. Create a new pull request.

## License

This project is licensed under the MIT License.
