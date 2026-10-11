# Arduino Serial Plotter
A Windows Forms application that works similar to the Arduino IDE Serial Plotter, but with some additional functionality.

## Features
- It will (currently) show an unlimited number of data points.
- You can set the number of data points visible within the form, and scroll along the whole dataset.
- You can export the data to a CSV file.

<img width="1668" height="984" alt="screenshot" src="https://github.com/user-attachments/assets/c8e8ca5c-bb05-433d-8009-64934931c972" />

---

The tool works in the same way as the Arduino Serial Plotter in that it reads data from the serial port, and converts it in to a plot.

The data should be in the format that the Arduino Serial Plotter uses, which is a comma-seperated list of ***label***/***value*** pairs (aka **parameters**), where the label and value are seperated by a colon, or an equals sign:

> ***label***:***value***,***label***:***value*** ...


Note that each message recieved, and its parsed parameters, constitute to one data point on the graph. If a message does not contain a ***label***/***value*** pair, then the graph will have a gap for that parameter where the value was missing.

---

MIT License. See the LICENSE file for details.
