1. I have used EntityFramework core and asp.net core identity for handling identities and database related functionalities.
2. We would not require database backup file, and creation of database is handled by code and any pending migration.
3. When you open the site first register the user, and choose the role of user as Admin/User.
4. Admin role can create, update and delete the product.
5. User role can't create, update or delete the product.
6. After registering the user register confirmation page will get open, which will have below text:
   
Register confirmation
This app does not currently have a real email sender registered, see these docs for how to configure a real email sender. Normally this would be emailed: Click here to confirm your account

Here click on Click here to confirm your account link to register the user,
then click on Home button to go to login page, and then enter the credentials and login.

8.After you have login when you click on Home page it will not go to login page, you have to first signout then it will redirect to login page,
After Addition,edition and deletion when you click on Home buttion it will go to homepage.
9.There is signout button which displays only when the user is authenticated.
10. For role purpose authorization I had used custom authorization.
11. For logging to file I have use serilog logger.
    
