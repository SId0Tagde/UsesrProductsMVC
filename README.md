1. I have used EntityFramework core and asp.net core identity for handling identities and database related functionalities.
2. We would not require database backup file, and creation of database is handled by code and any pending migration.
3. When you open the site first register the user, and choose the role of user as Admin/User.
4. Admin role can create, update and delete the product.
5. User role can't create, update or delete the product.
6. There is signout button which displays only when the user is authenticated.
7. For role purpose authorization I had used custom authorization.
8. For logging to file I have use serilog logger.
    
