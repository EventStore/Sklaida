Sklaida
=======

This is a sample application around using EventStore. This sample app implements a scatter/gather pattern with the event store as the backend. The javascript client reads directly from atom feeds in order to update the UI for the result set in real time as results are returned from the adapters. 

The sample essentially has three parts that are hosted in two areas. The "back end" has a web server endpoint and a background processor that it hosts. In a production environment these would be separate processes for simplicity they are combined in the sample. Then there is the front end which is a Single Page JS app.

After building the backend can be run by running "ConsoleHost". It expects an event store node to be available on the default ports on localhost. You can change this here: https://github.com/EventStore/Sklaida/blob/master/backend/src/ConsoleHost/ScatterGatherWireUp.cs#L38 and https://github.com/EventStore/Sklaida/blob/master/backend/src/ConsoleHost/ScatterGatherWireUp.cs#L57

The front end is a single page app and self contained. It can be run just by pointing your browser at index.html in the frontend folder.

![screenshot](https://raw.githubusercontent.com/EventStore/Sklaida/master/screenshots/sklaida_screenshot.png)
