# The Project Game

> For the beginning is thought to be more than half the whole.
>
> \- Aristotele, _Nicomachean Ethics_

**Group Instructor** - Janusz Rafałko

**Team 17-PL-12** - Maciej Grzeszczak, Henryk Kaczmarski, Sebastian Konowrocki _(leader)_, Kajetan Kotas, Stanisław Wasilkowski.

## Software Technology
The Project Game is implemented using **.NET 4.5.2** framework and written in **C#** programming language. The main reason for that choice is that every member of the team is familiar with these to at least satisfactory extent. Besides that there is a wide range of avaliable programming libraries and mature community around that platform.

## Methodology
Methodology used to manage product development is **Scrum**. It is one of the most popular software development framework nowadays and learning that may be beneficial for team members as they acquire necessary experience. We are working in one week sprints and holding stand-ups every two days (since meeting every day is really difficult to impossible).

Roles according to Scrum methodology:

- _Scrum Master_ - **Sebastian Konowrocki**
- _Product Owner_ - **Stanisław Wasilkowski**
- _Developers Team_ - **Maciej Grzeszczak**, **Henryk Kaczmarski**, **Sebastian Konowrocki**, **Kajetan Kotas**, **Stanisław Wasilkowski**

In order to help and improve our cooperation we use **[Slack](https://io2team.slack.com)** as our communication platform, where we consult our problems and solve them efficiently and quickly. 
Tasks are organized with the help of **[Trello](https://trello.com)** service, where everyone can get instant information about what is left to do and who is working on which task currently. This is also the place where user stories, product backlog and summaries of the Scrum sprints are being kept, so everyone can access it at any time.
Changes in code are tracked with Git version control system. Elected branching model and commit naming convention is inspired by **[Gitflow Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows#gitflow-workflow)** which is enforced **strictly**.
Development branch is synchronized with TeamCity **continuous integration** server to prevent integration problems and to ensure that project passes test after every significant change is made. TeamCity server is hosted on Azure cloud computing service.

## Submitted issues regarding initial version of the project documentation
- [Direction of keep alive message](https://se2.mini.pw.edu.pl/17-results/17-results/issues/23)
- [Subsequent messages before a reply from game master arrives](https://se2.mini.pw.edu.pl/17-results/17-results/issues/22)
- [What happens when a node disconnects](https://se2.mini.pw.edu.pl/17-results/17-results/issues/1)

## Preliminary work schedule

- **laboratory 2** (10.03) - delivering intial project documentation. 
- **laboratory 3** (17.03) - delivering final project documentation. Planning the first sprint (Communication). Main goal of this sprint is to create initial projet setup, create coninuous integration environment and prepare working base of communication server where most of the funcionalites are still mocked.
- **laboratory 4** (24.03) - setting up continuous integration (TeamCity).
- **laboratory 5** (31.03) - working prototype of communication, implemented messages models, working communication server, Player and GameMaster mocked mostly. Planning the second (Game) sprint, review and retrospect of the first sprint.
- **laboratory 6** (07.04) - planning the third sprint (Game), review and retrospect of the second sprint.
- **laboratory 7** (12.04) - planning the fourth sprint (Game), review and retrospect of the third sprint.
- **laboratory 8** (21.04) - testing phase of game rules implementation, ability to play simple games simultaneously. Planning the fifth sprint (Game), review and retrospect of the fourth sprint. 
- **laboratory 9** (28.04) - working prototype of a whole game, implemented game rules, Communication Server can serve multiple games. Client's, Communication Server's and Game's configurations are kept and read from configuration files. Planning the sixth sprint (Cooperation), review and retrospect of the fifth sprint.
- **laboratory 10** (05.05) - planning the seventh sprint (Cooperation), review and retrospect of the sixth sprint.
- **laboratory 11** (12.05) - testing cooperation phase, modules responsible for loading artifical intelligence. Planning the eight sprint (Cooperation), review and retrospect of the seventh sprint.
- **laboratory 12** (26.05) - module responsible for basic artificial intelligence. Improvement of compatibility with other groups solutions. Planning the final sprint (Cooperation), review and retrospect of the eighth sprint.
- **laboratory 13** (02.06) - championships. Review and retrospect of the final sprint.

## Application links

These are versions of the applications built and published by our Continuous Integration server. In order to download these you need to log in as guest user.

### Development

- [Player](http://mini-dev.westeurope.cloudapp.azure.com:8080/repository/download/TheProjectGame_Development/latest.lastSuccessful/Player.zip)
- [Game Master](http://mini-dev.westeurope.cloudapp.azure.com:8080/repository/download/TheProjectGame_Development/latest.lastSuccessful/GameMaster.zip)
- [Communication Server](http://mini-dev.westeurope.cloudapp.azure.com:8080/repository/download/TheProjectGame_Development/latest.lastSuccessful/CommunicationServer.zip)

### Release

- [Player](http://mini-dev.westeurope.cloudapp.azure.com:8080/repository/download/TheProjectGame_Master/latest.lastSuccessful/Player.zip)
- [Game Master](http://mini-dev.westeurope.cloudapp.azure.com:8080/repository/download/TheProjectGame_Master/latest.lastSuccessful/GameMaster.zip)
- [Communication Server](http://mini-dev.westeurope.cloudapp.azure.com:8080/repository/download/TheProjectGame_Master/latest.lastSuccessful/CommunicationServer.zip)

## Progress

* Tasks completed during the first sprint (as of 31.03):
    * TeamCity configuration
    * Artifacts generation on TeamCity
    * Inversion of Control using Autofac
    * TCP/IP infrastructure
    * Messages templates
    * Configuration loading infrastructure
    * Logging infrastructure
    * Verbose mode
    * Messages routing
    * XSD schema validation
    * Clients handling
    * Unit tests - Options parser
    * Unit tests - Message handler resolver
    * Unit tests - Message handlers
    * Unit tests - Message parser
    * Integration tests

* Tasks completed during the second sprint (as of 07.04):

* Tasks completed during the third sprint (as of 12.04):
    * Create and manage games
    * Player logic interface
    * Game object holding state of the current game
    * Board initialization
    * PlacePiece message handler
    * Move message handler
    * Discover message handler

* Tasks completed during the fourth sprint (as of 21.04):
    * Simple player logic implementation
    * PickupPiece message handler
    * CheckPiece message handler
    * Data message handler
    * Game message handler
    * Ending game and proper gameplay

* Tasks completed during the fifth sprint (as of 28.04):
    * Test piece message handler
    * Handle keepalive
    * BetweenPlayerMessage message handler
    * AcceptExchangeRequest message handler
    * RejectKnowledgeExchange message handler
    * KnowledgeExchangeRequest message handler
    * Improved player logic
    * Rematch after a finished game
    * Unit tests (gameplay)