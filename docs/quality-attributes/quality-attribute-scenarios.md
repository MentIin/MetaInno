## Reliability
### Availability - degree to which a system, product or component is operational and accessible when required for use.

**Why important:** the server should be able to hold large online since we are making a MMO.

- **Source:** Players play(enter) the game
- **Stimulus:** Number of CCU reaches the limit
- **Artifact:** The server multiplayer framework
- **Environment:** Production environment during peak usage period
- **Response:** The ping of the players is getting too high
- **Response measure:** Average ping in ms below 500ms

**How to test:** add fake players to the server and debug the ping.

## Performance Efficiency
### Time Behaviour - degree to which the response time and throughput rates of a product or system, when performing its functions, meet requirements.

**Why important:** the movement should feel responsive.

- **Source:** Player play as any of the characters
- **Stimulus:** Ping gets high (for any reason)
- **Artifact:** The server multiplayer framework
- **Environment:** Game during the high CCU or unstable connection to the server
- **Response:** In-game character movement gets unresponsive
- **Response measure:** The time between keypress and character movement under 50ms

**How to test:** in-game debugging in edit mode.

## Security
### Resistance - degree to which the product or system sustains operations while under attack from a malicious actor.

**Why important:** DDoS attacks are very common nowadays.

- **Source:** The server is running
- **Stimulus:** The hacker starts a DDoS attack
- **Artifact:** The server
- **Environment:** Server during the attack
- **Response:** Ping getting higher, complete shutdown
- **Response measure:** The system can hold on for 5 minutes

**How to test:** simulate a DDoS attack.
