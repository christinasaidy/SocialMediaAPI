<h3>I collaborated with [Chirstina Saidy](https://github.com/christinasaidy) on this API. We followed the clean architecture from [this site](https://www.c-sharpcorner.com/article/how-to-build-a-clean-architecture-web-api-with-net-core-8/?authuser=0) to complete the APIs. The steps we took: </h3>
1) Created a solution for the Domain layer including our entities (Users.cs, Posts.cs, Notifications.cs, Comments.cs, Categories.cs, Votes.cs)
2) Created a solution for the Application layer which includes the interfaces for our entities, as well as the services.
3) Created a solution for the Infrastructure layer, applying the interfaces for our repositories.
4) Created an ASP.NET Core Web API project for our application layer. Implementing the necessary controllers and configuring the program.cs.
5) We tested our CRUD functions on swagger insuring everything works as intended.

-------------------------------------------------------------------------------------

<h3>The CRUD Functions for Users consist of:</h3>
1)Post: Where users can create a new account or sign in to there accounts using there credentials(usernam & Password)
2)Get: where we can return a user by there ID
3)Delete: users have the ability to delete there account if they are signed in

-------------------------------------------------------------------------------------

<h3>The CRUD Functions for Posts consist of:</h3>
1)Post: where Users can create a post by providing a Title, Description, Tags, and a Category Name
2)Get: Returns a post by providing its ID
3)Delete: Users can delete their posts if they are signed in
4)Put: Users can update their already existing post by providing their fields.

-------------------------------------------------------------------------------------

<h3>The CRUD Functions for Notifications consist of:</h3>
1)Post: Notifications can be created by providing Notification Type and a Message
2)Get: Returns all the Notifications that the user got.
3)Delete: deletes a Notification by providing its ID

-------------------------------------------------------------------------------------

<h3>The CRUD Functions for Notifications consist of:</h3>
1)Post: Users can create a Comment by providing Post ID and the content of the comment
...


