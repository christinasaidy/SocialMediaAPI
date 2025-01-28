# I collaborated with [Hani Abdel Ghani](https://github.com/Hani0101) on this API. We followed the clean architecture from [this site](https://www.c-sharpcorner.com/article/how-to-build-a-clean-architecture-web-api-with-net-core-8/?authuser=0) to complete the APIs. The steps we took: 

1) Created a solution for the Domain layer including our entities (Users.cs, Posts.cs, Notifications.cs, Comments.cs, Categories.cs, Votes.cs)
2) Created a solution for the Application layer which includes the interfaces for our entities, as well as the services.
3) Created a solution for the Infrastructure layer, applying the interfaces for our repositories.
4) Created an ASP.NET Core Web API project for our application layer. Implementing the necessary controllers and configuring the program.cs.
5) We tested our CRUD functions on swagger insuring everything works as intended.

# The following is a detailed description of what the APIs are capable of doing:
-------------------------------------------------------------------------------------

### The CRUD Functions for Users consist of:
1) Post Sign in: lets Users Sign in
2) Post Sign up: lets Users Sign up to a new account(Username should be unique)
3) Get Users: lets you return a user by there ID
4) Delete User: Signed in Users with their token activated can delete their account by providing their password
5) Get Username: Returns the Username of the user signed in
6) Get Email: Returns the Email of the user signed in
7) Get Posts: Returns the posts of the user signed in
8) Get Bio: Returns the bio of the user signed in
9) Post Bio: lets Users create or update their Bio
10) Get Profile Picture: Returns signed in user's profile picture
11) Post Profile Picture: Lets Users upload or update their Profile picture
12) Get profile picture {id}: Returns a profile picture by its id
13) Get activity: Returns user's activity by providing the user Id
14) Patch username: Updates the username of a signed in user (User should provide their new Username)
15) patch email: Update the email of a signed in user (User should provide their new email)
16) patch password: Updates the passowrd of a signed in user (User should provide their current password, new password, and confirm password fields)

-------------------------------------------------------------------------------------

### The CRUD Functions for Posts consist of:
1) Post Posts: where Users can create a post by providing a Title, Description, Tags, and a Category Name
2) Get Posts: Returns a post by providing its ID
3) Delete Posts: Users can delete their posts if they are signed in
4) Put Posts: Users can update their already existing post by providing their fields.
5) Get Post images: gets the post images by providing the post ID
6) Post Posts Images: lets Users to upload images to their posts
7) Get Top Posts: Returns Posts according to how many upvotes they have
8) Get Latest Posts: Returns Posts according to how recent they are (Takes input number of posts you want to return and offset(used for pagination))
9) Get Posts Count: Returns the number of posts present in the Database
10) Get Posts by Category: Returns posts that are present in a specific Category (Takes input Category ID)
11) Get Posts Search: Users can Query a post by providing the author name, the post title or the desciption
 
-------------------------------------------------------------------------------------

### The CRUD Functions for Notifications consist of:
1) Post Notification: Notifications can be created and sent to the user that is signed in, it takes Notification Type, Message, and isRead, PostId, and receiverId.
2) Get User Notification: Returns all the Notifications to the user that is signed in.
3) Patch Notification: Updates the notification to be read or unread by providing its ID
4) Delete: deletes a Notification by providing its ID

-------------------------------------------------------------------------------------

### The CRUD Functions for Comments consist of:
1) Post: Users can create a Comment by providing Post ID and the content of the comment
2) Get: Returns Comments that the user signed in did to a specific post by providing the Post ID
3) Put: Changes a comment's content by providing the Comment ID
4) Delete: Deletes a comment by providing its Comment ID
5) Get Comments by Post ID: Returns comments specific to a post by providing the postID

-------------------------------------------------------------------------------------

### The CRUD Functions for Categories consist of:

1) Post: Users can create a category by providing its Name
2) Get: Returns all available Categories or you can get a specific one by its Category ID
3) Delete: Deletes a Category by providing its Category ID

-------------------------------------------------------------------------------------

### The CRUD Functions for Votes consist of:

1) Post: A User can vote on a post by providing PostID and Vote Type (Note: A vote gets deleted if a user Upvotes/Downvotes and then proceed to do the same action again on the same posts)
2) Delete: Deletes the Vote By providing its ID 
3) Get: All Votes can be returned by Providing the Post ID or you can return one vote by providing the Vote ID
   
-------------------------------------------------------------------------------------

# Security Measures:
#### JWT  along with Microsoft.AspNetCore.Authorization Libraries were implemented to generate a Token for each signed in user, and from this Token users get the privlige to use the features of some of the APIs(such as creating post, voting, commenting....)

### As well BCrypt was implemented to Hash passwords being sent to the database.
   
