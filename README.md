# URL-Shortener
Design and implement a high-scale URL shortener system. This assignment evaluates your ability to: 1. Architect and implement a reliable and highly available system. 2. Choose appropriate technologies and justify their usage. 3. Demonstrate good coding practices, maintainability, and performance optimization.

#Step-by-Step Guide for Setting up the URL Shortener Application
1. Clone the Repository
Open your terminal or command prompt.
Navigate to the directory where you want to clone the repository.
Run the following command:
bash
Copy code
git clone <repository-url>
Replace <repository-url> with the actual URL of the repository.
2. Set Up the Database and Table
Navigate to the script file for database and table creation.
Open the SQL script file in your preferred SQL editor.
Execute the script to create the required database and tables.
3. Configure the Connection String in appsettings.json
Open the UrlShortenerApi project in Visual Studio 2022.
In the appsettings.json file, locate the connection string section.
Modify the connection string to point to your database server, or add your own connection string if necessary.
Example:
json
Copy code
"ConnectionStrings": {
    "DefaultConnection": "Server=your-server-name;Database=your-database-name;User Id=your-username;Password=your-password;"
}
Save the changes.
4. Build and Run the .NET Project
Build the UrlShortenerApi project in Visual Studio 2022 by selecting Build > Build Solution or pressing Ctrl+Shift+B.
Make sure the project builds without errors.
Run the API project by pressing F5 or by selecting Debug > Start Debugging. Ensure the API is running successfully.
5. Set Up the Frontend (UI) Project
Open the UI-Short-URL project in Visual Studio Code.
Open a terminal within Visual Studio Code (you can open it via Terminal > New Terminal).
6. Check API URL in the environment.ts File
In the UI-Short-URL project, navigate to the src/environments/environment.ts file.
Ensure that the API URL defined here matches the URL where your API is running.
Example:
typescript
Copy code
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7239'  // Replace with your API URL
};
If the API URL is different, update it to match your API's running URL.
7. Install Dependencies
Run the following command in the terminal to install the necessary dependencies:
bash
Copy code
npm install
8. Start the Frontend
Once the dependencies are installed, run the following command to start the frontend application:
bash
Copy code
ng serve --o
This will open the frontend page in your default browser.
