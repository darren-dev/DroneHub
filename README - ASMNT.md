<h1>Assesment</h1>

<h2>Scenario</h2>
We need a drone delivery system that sends packages between two clients. (Client A = the sender, Client B = the receiver)
You can generate 10 clients upon program startup but note that their locations will change on every order generation.

<h3>Front End Requirements:</h3>
* Basic UI map (1000 by 1000 units) square (Think of this UI as a graph with x and y coordinates and not in terms of google maps)
* View order information
* View Current Drone and state
    * What order is he busy with
    * Which step of the process is he busy with


<h2>Orders</h2>

<h3>The properties of an order are the following:</h3>
Please note that these are just a guideline. Feel free to modify this as applicable.

* id - Auto Increment Id managed within your application
* client's name
* client's location (latitude, longitude)
* client's contact number

<h3>The properties of a drone are the following:</h3>
Please note that these are just a guideline. Feel free to modify this as applicable.

* id - Auto increment ID
* drone name
* drone location (latitude, longitude)

<h3>Drones</h3>
There are 10 drones starting in the centre of the map.

<h3>Allocation Rules</h3>

Orders should be randomly placed every 30 seconds with randomized client locations.

So the rules for allocation are the following:
* Order is allocated to a drone if a drone is not currently busy with another order.
* Order is allocated to the closest available drone first and then the 2nd and so forth.
* If no drones are available a retry policy should be followed.

<h3>Process of an order</h3>

* Allocate drone to Order.
* Deliver from client A to client B.
* Delivery time wil be calculated as follows.
    * Assume a drone will always take 10 seconds to reach client A from wherever it currently is.
    * Assume a drone moves with a velocity of 1 unit per second between clients.
    

<h3>Basic Tech Requirements</h3>

* Any language is fine
* An RDBMS Database
* Code should be easy to understand
* RESTful framework
* Bonus if view can update components without refreshing page
* Git practices will also be reviewed so make sure to make regular commits etc.
