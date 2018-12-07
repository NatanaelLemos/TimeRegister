# Time Register Application

It's a very simple app that I developed to register my entrance and exit hours from work.

![Picture of the app](./doc/picture.png)

The only three functions are:
* Start: Register the date and time when it was clicked locally as a start event
* End: Register the date and time when it was clicked locally as an end event
* Sync: Send all local data to the remote API

---

After sync, there are two GET endpoints in the API where you can summarize the information:
* /v1/Time/[FilterDateStart]/[FilterDateEnd]: Returns a JSON file with all Start and End registers for the dates in between Filter Start and Filter End (see the test project for more details)
* /v1/Time/[FilterDateStart]/[FilterDateEnd]/csv: Returns a CSV file with all Start and End registers for the dates in between Filter Start and Filter End (see the test project for more details)

---

Softwares used:
* Docker 18.09
* Dotnet Core 2.1
* Ionic 4.1.2
* Linux Mint 19 (as host for Docker)
* MongoDB 3.6.4
* NodeJS 10.11
* Visual Studio 2017 (15.9.3)
* VS Code
* Robo 3T and Postman to test

---

To run the Api, on a Linux machine with Docker and MongoDB installed, you just need to run docker-setup.sh. This should start the container for you

For the app, usually you just need to perform a npm install and after that run the app against an android or ios device. To make things easier, there are two scripts in the App folder. 1st to run as debug in a Visual Studio default emulator and the 2nd to run in a real device
