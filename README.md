# SensorPoC

## Requirements
The solution consists of various projects that provide a working implementation for given requirements which are:

- A company providing some sensor data (ImaginaryCompany).
- Another company uses this data to analyze time series generated data (SensorPoC) given by aforementioned company (ImaginaryCompany).
- CRUD operations for sensors metadata (meaning name, location, warning limits for the defined sensor) for the consumer company (SensorPoC).

## Implementation details
For that an API project along with a UI project established. 
The proxy-call to the imaginary company is been wrapped behind an interface and implemented in an in-proc manner (meaning no call to a web api, simulating the out-proc call).
Chart.js is used to visualize the data being fetched via proxy-call in front-end.
CRUD operations implemented in the backend, using in memory EF Core.
.Net 8 is used.
Onion architecture applied for the client part where sensor metadata needed to be stored, updated, deleted and fetch.
Business logic kept in the domain (even there is almost no logic apart from few validations), storage concerns are addressed in a seperate assembly. Finally a WebAPI project is defined that glues mentioned assemblies together.

## How to run
As a prerequisite, make sure Docker Desktop installed and docker-compose calls can be made in a CLI.
From the solution directory execute `docker-compose build` which will build the images. Later `docker-compose up` will run the projects, namely WebAPI and UI projects. UI project will be accessible from port 8080 where WebAPI project
can be accessed from port 8081, both on localhost.

Also simply opening the solution from Visual Studio and run the projects as different instances will work. API should run before the UI since the Overview page makes an immediate call to fetch all sensors.







