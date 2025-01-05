<<<<<<< HEAD
# I collaborated with [Chirstina Saidy](https://github.com/christinasaidy) on this API. We followed the clean architecture from [this site](https://www.c-sharpcorner.com/article/how-to-build-a-clean-architecture-web-api-with-net-core-8/?authuser=0) to complete the APIs. The steps we took: 
=======
# I collaborated with [Hani Abdel Ghani](https://github.com/Hani0101) on this API. We followed the clean architecture from [this site](https://www.c-sharpcorner.com/article/how-to-build-a-clean-architecture-web-api-with-net-core-8/?authuser=0) to complete the APIs. The steps we took: 
>>>>>>> cdb514234c6841e43ecdea5138c6e0c480455d38
1) Created a solution for the Domain layer including our entities (Users.cs, Posts.cs, Notifications.cs, Comments.cs, Categories.cs, Votes.cs)
2) Created a solution for the Application layer which includes the interfaces for our entities, as well as the services.
3) Created a solution for the Infrastructure layer, applying the interfaces for our repositories.
4) Created an ASP.NET Core Web API project for our application layer. Implementing the necessary controllers and configuring the program.cs.
5) We tested our CRUD functions on swagger insuring everything works as intended.

# The following is a detailed description of what the APIs are capable of doing:
-------------------------------------------------------------------------------------

### The CRUD Functions for Users consist of:
1) Post: Where Users can create a new account or sign in to their accounts using their credentials(username & Password)
2) Get: Where we can return a user by there ID
3) Delete: Users have the ability to delete there account if they are signed in

-------------------------------------------------------------------------------------

### The CRUD Functions for Posts consist of:
1) Post: where Users can create a post by providing a Title, Description, Tags, and a Category Name
2) Get: Returns a post by providing its ID
3) Delete: Users can delete their posts if they are signed in
4) Put: Users can update their already existing post by providing their fields.

-------------------------------------------------------------------------------------

### The CRUD Functions for Notifications consist of:
1) Post: Notifications can be created and sent to the user that is signed in, it takes Notification Type, Message, and isRead as arguments to be called.
2) Get: Returns all the Notifications to the user that is signed in.
3) Delete: deletes a Notification by providing its ID

-------------------------------------------------------------------------------------

### The CRUD Functions for Comments consist of:
1) Post: Users can create a Comment by providing Post ID and the content of the comment
2) Get: Returns Comments that the user signed in did to a specific post by providing the Post ID
3) Update: Changes a comment's content by providing the Comment ID
4) Delete: Deletes a comment by providing its Comment ID

-------------------------------------------------------------------------------------

### The CRUD Functions for Categories consist of:

1) Post: Users can create a category by providing its Name
2) Get: Returns all available Categories or you can get a specific one by its Category ID
3) Delete: Deletes a Category by providing its Category ID

-------------------------------------------------------------------------------------

### The CRUD Functions for Votes consist of:

1) Post: A User can vote on a post by providing PostID and Vote Type
2) Delete: A User can Delete there Vote By providing the ID of the vote
3) Get: All Votes can be returned by Providing the Post ID or the you can return one vote by providing the Vote ID
   
-------------------------------------------------------------------------------------

# Security Measures:
#### JWT  along with Microsoft.AspNetCore.Authorization Libraries were implemented to generate a Token for each signed in user, and from this Token users get the privlige to use the features of some of the APIs(such as creating post, voting, commenting....)
   
