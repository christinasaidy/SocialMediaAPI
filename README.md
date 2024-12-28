I collaborated with Hani Abdel Ghani on this API. We followed the clean architecture from {[link](https://www.c-sharpcorner.com/article/how-to-build-a-clean-architecture-web-api-with-net-core-8/?authuser=0)} to complete this process. The steps we took:
1) Created a solution for the Domain layer including our entities (Users.cs, Posts.cs, Notifications.cs, Comments.cs, Categories.cs, Votes.cs)
2) Created a solution for the Application layer which includes the interfaces for our entities, as well as the services.
3) Created a solution for the Infrastructure layer, applying the interfaces for our repositories.
4) Created an ASP.NET Core Web API project for our application layer. Implementing the necessary controllers and configuring the program.cs.
5) We tested our CRUD functions on swagger insuring everything works as intended.
