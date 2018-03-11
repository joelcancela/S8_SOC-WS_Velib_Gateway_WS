# SOAP Middleware Service and Client for JCDecaux API

## Features

* Server
  * All requests between the service and the API are asynchronous **[Extension Development #2]**.
  * The service uses caching to reduce communications with the API **[Extension Development #3]**.

* Client
  * Displays the number of bikes available of the currently selected station **[MVP]**.
  * WPF client instead of a console one **[Extension Development #1]**.
  * Search bars for cities and stations.

## To Improve

* Caching could have been made using C# classes (example [here](https://codeshare.co.uk/blog/simple-reusable-net-caching-example-code-in-c/)).
* Caching uses arbitrary values that might not be optimal ones, currently the stations data are invalidated after 5 minutes and the cities data after 1 hour.

## Client Screenshot

![ClientUI](doc/client.png)