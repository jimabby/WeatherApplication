# Weather Application
A simple application to fetch and display weather data based on user input.

## Description
This Weather Application allows users to input a city name and get the weather forecast for the next 5 days. It fetches the data from the OpenWeather API and displays it in a graph using ScottPlot.

## Features
- User inputs city name
- Fetches weather data for the next 5 days from OpenWeather API
- Displays temperature data in a plot
- Allows easy customization of city and API key using configuration

## Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/weather-application.git
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

## Usage

1. Run the application:

    ```bash
    dotnet run
    ```

2. Enter the city name when prompted.

3. The weather forecast will be displayed, and a plot will be saved as `weather_forecast.png`.

## Contributing

1. Fork the repository.
2. Create your feature branch (`git checkout -b feature-name`).
3. Commit your changes (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature-name`).
5. Create a new pull request.

## License

This project is licensed under the MIT License
