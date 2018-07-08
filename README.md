# Drone Port
### An application that can move drones with packages from a port to a location.


__P0 Start Time: 07 July 14:06__  
__P1 End Time: 08 July 18:40__


## Features

- Grid Generation (Recommended max 20x20)
- Port Creation
- Drone Creation
- Order Creation
- Automatic Drone Delivery
- Query an order
- ~~Relational Database~~
- ~~Proper Git Commits~~


## Description
The user starts off on an empty map, where they can set their port name, location on the grid, and the grid size. When finished, the user can click the `Create` button in order to generate the visual layout and setup the application.

- ## Create a new Drone
    Clicking on `Add Drone` creates a new drone, and stores it into your current port. The drone will wait there until an order becomes available to deliver.

- ## Create a new Order
    Clicking on `Add Order` creates a new order, and stores it in your current port. When a drone beclomes available, it will automatically take the order to the required client.

A drone moves at a speed of 1 block per second. The drone will automatically calculate the best path between itself and the desired destination, and will move along the route until the delivery is completed. Once the delivery is completed, the drone will move back to the port to collect a new parcel.
