# Auduino Serial Plotter
A Windows Forms application that works similar to the Arduino IDE Serial Plotter, but with some additional functionality, and it wll show more then 50 data points.

## Features
- It will (currently) show an unlimited number of data points.
- You can set the number of data points visible within the form, and scroll along the while dataset.
- You can export the data to a CSV file.

<img style="width: 100%; height: auto;" alt="Screenshot" src="https://github.com/user-attachments/assets/96c14574-ffdb-4b39-b2bd-eeb380e4b47d" />

---

The tool works in the same way as the Arduino Serial Plotter in that it reads data from the serial port, and converts it in to a plot.

The data should be in the format that the Arduino Serial Plotter uses, which is a comma seperated list of ***label***/***value*** pairs, where the label and value are seperated by a column, or an equals sign:-

> ***label***:***value***,***label***:***value*** ...

---

MIT License. See the LICENSE file for details.
