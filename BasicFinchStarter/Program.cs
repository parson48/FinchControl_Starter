using System;
using FinchAPI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace FinchControl_Starter
{
    class Program
    {
        //
        // List of all the "user programming" commands
        // Make sure to keep none and done as the first and last, respectively
        //
        public enum Command
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARDS,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            GETCURRENTLIGHT,
            GETCURRENTTEMP,
            NOTEON,
            NOTEOFF,
            GOCRAZY,
            GOSTUPID,
            DONE
        }
        
        /// <summary>
        /// basic starter solution for the Finch robot
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // **************************************************
            //
            // Assignment: Finch Control
            // Application Type: Console
            // Description: Using a menu based interface, allows for the moderate control of the finch robot and its abilities.
            // Author: Robert Parons
            // Creation Date: 9/25/2019
            // Last Modified: 11/04/2019
            //
            // **************************************************
            Finch finchRobot = new Finch();

            

            DisplayWelcomeScreen();

            bool continueToProgram = DisplayLoginRegisterOption();

            if (continueToProgram)
            {
                DisplayMenu(finchRobot);
            }


            DisplayClosingScreen();
        }

        #region HELPER METHODS & SINGLE USE METHODS

        /// <summary>
        /// Displays the continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Use at the end of methods if they goto the menu method.
        /// </summary>
        static void DisplayContinueToMenuPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue to the main menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays a header, needs a string used for text
        /// </summary>
        /// <param name="headerText">the text used for a header</param>
        private static void DisplayHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t\t\t\t\t{0}", headerText);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        /// <summary>
        /// Displays the welcome screen w/ a short description (should not be used outside of main)
        /// </summary>
        private static void DisplayWelcomeScreen()
        {
            Console.Clear();
            DisplayHeader("Welcome!");
            Console.WriteLine("Hello, and welcome to the application!");
            Console.WriteLine("This application allows for the control of a plugged-in finch robot.");
            DisplayContinuePrompt();
        }

        /// <summary>
        /// Displays the closing screen (should not be used outside of main)
        /// </summary>
        private static void DisplayClosingScreen()
        {
            DisplayHeader("Goodbye!");
            Console.WriteLine("Thank for you using this application. This application will now close.");
            DisplayContinuePrompt();
        }

        #endregion

        //
        // Contains all the methods and code for the Login and Registration abilities
        //
        #region LOGIN AND REGISTRATION METHODS

        /// <summary>
        /// Shows the menu and does methods upon user choice.
        /// </summary>
        /// <returns></returns>
        private static bool DisplayLoginRegisterOption()
        {
            bool proceedToProgram = false;
            string userResponse;
            bool validResponse;
            bool continueProgram = false;
            string dataPath = @"Data\LoginInfo.txt";
            bool validLogin = false;


                do
                {
                    //
                    // If the user login is a success, it skips the menu and will continue to the robot program
                    //
                    if (validLogin)
                    {
                    proceedToProgram = true;
                    continueProgram = true;
                    }
                    else
                    {
                        //
                        //Displays the menu, having options for numbers 1-3
                        //
                        DisplayHeader("Login and Registration Menu");

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("[1] ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Login");

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("[2] ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Register");

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("[3] ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Quit");
                        Console.WriteLine();
                        Console.Write("Please enter the appropiate number to choose what program to run >> ");

                        do
                        {
                            //
                            //Executes the corresponding method for numbers 1-3, else it loops
                            //
                            ConsoleKeyInfo userCharacter = Console.ReadKey();
                            userResponse = userCharacter.KeyChar.ToString();
                            validResponse = true;

                            switch (userResponse)
                            {
                                case ("1"):

                                    validLogin = DisplayLogin(dataPath);

                                    break;
                                case ("2"):
                                    validLogin = DisplayRegister(dataPath);
                                    break;
                                case ("3"):
                                    continueProgram = true;
                                    DisplayContinuePrompt();
                                    break;
                                default:
                                    Console.WriteLine();
                                    Console.WriteLine("Please enter a number 1-3 in number form to goto the cooresponding menu.");
                                    Console.Write("Enter the appropiate number to choose what menu to goto >> ");
                                    validResponse = false;
                                    break;

                            }

                        } while (!validResponse);
                    }
                } while (!continueProgram);
            
            return proceedToProgram;
        }

        /// <summary>
        /// Displays the login menu 
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        private static bool DisplayLogin(string dataPath)
        {
            bool existingLogin = false;
            bool exitLogin = false;
            int attempts = 0;
            bool loginSuccess = false;
            string[] userInput = new string[2];
            const int MAX_ATTEMPTS = 5;

            DisplayHeader("Login");

            Console.WriteLine("Please enter a username, then a password, which are seperated by a |");
            Console.WriteLine("For example: USERNAME|PASSWORD");

            //
            // Login Success is true if the user's input matches a registered input.
            // It also kicks the user back to the main menu after a few attempts, mostly as flavor
            //
            do
            {
                userInput = Console.ReadLine().Split('|');

                userInput[0] = userInput[0].ToLower();

                loginSuccess = CompareAllLogins(dataPath, userInput);

                if (loginSuccess)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Welcome {userInput[0]}!");
                    Console.WriteLine("You will now continue to the robot control menu.");
                    existingLogin = true;
                    exitLogin = true;
                }
                else if (attempts >= MAX_ATTEMPTS)
                {
                    Console.WriteLine();
                    Console.WriteLine("Max atttempts reached.");
                    Console.WriteLine("The program will now exit to the previous menu.");
                    exitLogin = true;
                }
                else
                {
                    attempts++;
                    Console.WriteLine();
                    Console.WriteLine($"Login Failed. Attempts left: {(MAX_ATTEMPTS - attempts)+1}");
                    Console.WriteLine("Please enter a valid username and password.");
                }


            } while (!exitLogin);

            DisplayContinuePrompt();

            return existingLogin;
        }

        /// <summary>
        /// Lets the user register a new username & password
        /// </summary>
        /// <returns></returns>
        private static bool DisplayRegister(string dataPath)
        {
            //
            // variables
            //
            bool registered = false;
            bool exitRegistration = false;
            string userInput;
            string[] arrayedUserInput;
            string[] currentRegistrations;
            string[] registrationSplit;
            string errorOutput = "";
            bool error;

            DisplayHeader("Register");

            Console.WriteLine("This will allow you to register a new username and attached password.");
            Console.WriteLine("Much like the login function, please enter the username and password seperated via a |, ie USERNAME|PASSWORD.");
            Console.WriteLine("To exit the registration, please type in '~'.");


            do
            {

                error = false;
                Console.WriteLine();
                Console.Write("Please enter your username & password >> ");

                //
                // if user types in a ~, it'll exit the program.
                //
                userInput = Console.ReadLine().Trim();
                if (userInput == "~")
                {
                    Console.WriteLine("Exiting registration!");
                    exitRegistration = true;
                }
                else
                {

                    currentRegistrations = ReadFromFile(dataPath);

                    foreach (string eachLogin in currentRegistrations)
                    {
                        //
                        // first tests to see if any perfect input is the same as any already registered.
                        //
                        if (userInput == eachLogin)
                        {
                            errorOutput = "Username and password combination already exists.";
                            error = true;
                        }
                        else
                        {
                            arrayedUserInput = userInput.Split('|');
                            arrayedUserInput[0] = arrayedUserInput[0].ToLower();
                            registrationSplit = eachLogin.Split('|');
                            registrationSplit[0] = registrationSplit[0].ToLower();

                            //
                            // Checks to see if the the user input is the correct length
                            //
                            if (arrayedUserInput.Length == 2)
                            {
                                //
                                //Checks to see if the username exists with another password
                                //also checks to see if the username/password 
                                //
                                if (arrayedUserInput[0] == registrationSplit[0])
                                {
                                    if (arrayedUserInput[1] == registrationSplit[1])
                                    {
                                        errorOutput = "Username and password combination already exists.";
                                        error = true;
                                    }
                                    else
                                    {
                                        errorOutput = "Username already exists with another password.";
                                        error = true;
                                    }
                                }
                                else
                                {
                                    exitRegistration = true;
                                }
                            }
                            else
                            {
                                errorOutput = "Incorrect format. Please use the format of USERNAME|PASSWORD.";
                                error = true;
                            }
                        }
                    }

                    //
                    // If there is no errors, it'll output nothing
                    // Also clears the error output incase the next input is a good registration
                    //
                    Console.WriteLine(errorOutput);
                    errorOutput = "";
                }
            } while (!exitRegistration || error);

            if (userInput == "~")
            { }
            else
            {

                File.AppendAllText(dataPath, Environment.NewLine);
                File.AppendAllText(dataPath, userInput);

                arrayedUserInput = userInput.Split('|');

                Console.WriteLine($"Welcome to the program, {arrayedUserInput[0]}!");
                registered = true;
            }

            DisplayContinuePrompt();

            return registered;
        }

        /// <summary>
        /// Upon taking a user's input, compares the input to all the file passwords/usernames.
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        private static bool CompareAllLogins(string dataPath, string[] userInput)
        {
            //
            // variables
            //
            string[] loginList;
            bool userMatch = false;
            string[] pass;
            int usernameFound = 0;

            loginList = ReadFromFile(dataPath);

            //
            //Makes sure the user input via XX|XX, otherwise it doesn't bother to check all the logins.
            //
            if (userInput.Length == 2)
            {
                foreach (string eachLogin in loginList)
                {
                    pass = eachLogin.Split('|');
                    pass[0] = pass[0].ToLower();

                    if (userInput[0] == pass[0])
                    {
                        if (userInput[1] == pass[1])
                        {
                            userMatch = true;
                        }

                        usernameFound = 1;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Input. Please use the input USERNAME|PASSWORD");
            }

            if (!userMatch && usernameFound == 1)
            {
                Console.WriteLine("Incorrect Password");
            }
            return userMatch;
        }

        /// <summary>
        /// Gives all the outputs from the given datapath
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        static string[] ReadFromFile(string dataPath)
        {
            string[] loginList;

            loginList = File.ReadAllLines(dataPath);

            return loginList;
        }

        #endregion

        /// <summary>
        /// Connects to the finch robot, returns if it is connected
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <returns>if the finch robot is connect, (should be returned true)</returns>
        private static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            const int MAX_ATTEMPTS = 3;
            int attempts = 1;
            bool finchRobotConnected = false;
            DisplayHeader("Connecting to the robot");
            Console.WriteLine("The application is prepared to connect to the finch robot.");
            Console.WriteLine("Please connect the robot to the computer using a USB cord.");
            DisplayContinuePrompt();
            while (!finchRobotConnected && attempts <= MAX_ATTEMPTS)
            {
                finchRobotConnected = finchRobot.connect();
                if (finchRobotConnected == true)
                {
                    finchRobot.setLED(0, 255, 0);
                    finchRobot.wait(250);
                    finchRobot.setLED(255, 0, 0);
                    finchRobot.wait(250);
                    finchRobot.setLED(0, 0, 255);
                    finchRobot.wait(250);
                    finchRobot.setLED(0, 0, 0);
                    Console.WriteLine();
                    Console.WriteLine("The finch robot is now connected!");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The finch robot is not connected, please try again.");
                    ++attempts;
                    DisplayContinuePrompt();
                }
            }
            if (finchRobotConnected)
            {
                Console.WriteLine();
                Console.WriteLine("Now that the robot is connected, you can now use the other programs!");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("After {0} attempts, the program could not connect to the robot. The program will now exit to the menu.", attempts);
            }
            DisplayContinueToMenuPrompt();
            return finchRobotConnected;
        }

        /// <summary>
        /// Disconnects to the finch robot, returns as not connected
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <returns></returns>
        private static bool DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            DisplayHeader("Disconnecting to the robot");
            Console.WriteLine("The application will now disconnect from the finch robot");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine();
            Console.WriteLine("The robot is now disconnected.");
            DisplayContinueToMenuPrompt();

            bool finchRobotConnected = false;
            return finchRobotConnected;
        }

        /// <summary>
        /// Does the talent show, which is a multiple step process of the robot doing various things
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void TalentShow(Finch finchRobot)
        {


            DisplayHeader("The Talent Show");
            Console.WriteLine("The Finch robot is now ready to show you its talent!");
            DisplayContinuePrompt();

            TalentShowMorseCode(finchRobot);

            TalentShowLightAndSound(finchRobot);

            TalentShowBriefTune(finchRobot);

            TalentShowUserMovement(finchRobot);

            DisplayHeader("The Talent Show - Closer");
            Console.WriteLine("Thank you for watching and inputting on this robot's talent show!");
            Console.WriteLine("It appreciates you watching.");

            DisplayContinueToMenuPrompt();
        }

        #region TALENT SHOW METHODS

        /// <summary>
        /// Part 1 of the Talent Show
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void TalentShowMorseCode(Finch finchRobot)
        {
            //
            // variables
            //
            string userResponse;
            bool responseWorks;
            int i;

            DisplayHeader("The Talent Show - Part 1");

            #region MORSE CODE - RED LIGHT + VALIDATED USER INPUT
            //
            //Using user inputs, flashes a word out in morse code (750 long, 250 short).
            //
            Console.WriteLine("The robot will now display a word in morse code, using red flashes");
            Console.WriteLine("What word should it display? Please enter a number 1-3 cooresponding to the word.");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[1] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("SOS");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[2] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Robot");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[3] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Hello");

            //
            //Validation loop, checks to see if the input is 1,2,3
            //
            do
            {
                ConsoleKeyInfo userCharacter = Console.ReadKey();
                userResponse = userCharacter.KeyChar.ToString();
                responseWorks = true;
                switch (userResponse)
                {

                    case ("1"):
                        #region SOS MORSE
                        //s
                        for (i = 0; i < 3; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //o
                        for (i = 0; i < 3; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(750);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //s
                        for (i = 0; i < 3; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        #endregion 
                        break;
                    case ("2"):
                        #region ROBOT MORSE
                        //r
                        finchRobot.setLED(255, 0, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(255, 0, 0);
                        finchRobot.wait(750);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(255, 0, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //o
                        for (i = 0; i < 3; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(750);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //b
                        finchRobot.setLED(255, 0, 0);
                        finchRobot.wait(750);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(250);
                        for (i = 0; i < 3; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //o
                        for (i = 0; i < 3; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(750);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //t
                        finchRobot.setLED(255, 0, 0);
                        finchRobot.wait(750);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);

                        #endregion 
                        break;
                    case ("3"):
                        #region HELLO MORSE
                        //h
                        for (i = 0; i < 4; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //e
                        finchRobot.setLED(255, 0, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //l l
                        for (i = 0; i < 2; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(750);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(250);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        //o
                        for (i = 0; i < 3; i++)
                        {
                            finchRobot.setLED(255, 0, 0);
                            finchRobot.wait(750);
                            finchRobot.setLED(0, 0, 0);
                            finchRobot.wait(250);
                        }
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(750);
                        #endregion
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("You did not enter a proper input. Please input a a number between 1-3");
                        responseWorks = false;
                        break;
                }
            } while (!responseWorks);

            finchRobot.noteOn(400);
            finchRobot.wait(500);
            finchRobot.noteOff();
            #endregion

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Part 2 of the Talent Show
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void TalentShowLightAndSound(Finch finchRobot)
        {
            DisplayHeader("The Talent Show - Part 2");
            #region BLUE AND GREEN LIGHT + A TONE
            //
            //Plays a note, flashes a blue light, then lowers sound and flashes a green light
            //
            Console.WriteLine("The Robot will now flash a blue light, followed by a green light");
            Console.WriteLine("During this period, the robot will also emit a tone.");
            DisplayContinuePrompt();
            finchRobot.noteOn(600);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(1000);
            finchRobot.noteOn(400);
            finchRobot.setMotors(50, 50);
            finchRobot.setLED(0, 0, 0);
            finchRobot.wait(250);
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(1750);
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();
            finchRobot.setMotors(0, 0);
            #endregion
            DisplayContinuePrompt();
        }

        /// <summary>
        /// Part 3 of the Talent Show
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void TalentShowBriefTune(Finch finchRobot)
        {
            DisplayHeader("The Talent Show - Part 3");

            //
            //Robot does a brief tune
            //
            #region A (BRIEF) TUNE
            Console.WriteLine("The robot will now sing a short tune");
            Console.WriteLine("Just sit back, and enjoy.");
            DisplayContinuePrompt();

            finchRobot.noteOn(500);
            finchRobot.wait(400);
            finchRobot.noteOff();
            finchRobot.wait(40);

            finchRobot.noteOn(490);
            finchRobot.wait(300);
            finchRobot.noteOff();
            finchRobot.wait(40);

            finchRobot.noteOn(480);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(10);

            finchRobot.noteOn(480);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(45);

            finchRobot.noteOn(525);
            finchRobot.wait(400);
            finchRobot.noteOff();
            finchRobot.wait(200);

            finchRobot.noteOn(345);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(10);

            finchRobot.noteOn(293);
            finchRobot.wait(300);
            finchRobot.noteOff();
            finchRobot.wait(500);

            finchRobot.noteOn(220);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(100);

            finchRobot.noteOn(200);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(50);

            finchRobot.noteOn(200);
            finchRobot.wait(250);
            finchRobot.noteOff();
            finchRobot.wait(5);

            finchRobot.noteOn(230);
            finchRobot.wait(359);
            finchRobot.noteOff();
            finchRobot.wait(20);

            finchRobot.noteOn(246);
            finchRobot.wait(400);
            finchRobot.noteOff();
            finchRobot.wait(1);

            #endregion

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Part 4 of the Talent Show
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void TalentShowUserMovement(Finch finchRobot)
        {
            string userResponse;

            DisplayHeader("The Talent Show - Part 4");
            #region USER MOVEMENT
            Console.WriteLine("The robot will now listen to you as it moves!");
            Console.WriteLine("Simply type in the corresponding numbers, and the robot will move on your command!.");
            Console.WriteLine("The robot moves a second at a time, and you can control its speed and direction");

            #region movement menu

            //
            //Simply displays the movement menu
            //
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[1] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Slow Forward");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[2] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Fast Forward");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[3] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Medium Backward");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[4] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Slow Backward");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[5] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Pivot Left on Wheel");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[6] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Turn Left");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[7] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Pivot Right on Wheel");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[8] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Turn Right");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[9] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Exit");
            #endregion movement menu

            //
            //movement loops, quits out if user presses 9
            //
            bool userContinue = true;
            do
            {
                Console.WriteLine("  Please input a movement option (1-9)");
                ConsoleKeyInfo userCharacter = Console.ReadKey();
                userResponse = userCharacter.KeyChar.ToString();
                switch (userResponse)
                {
                    case ("1"):
                        finchRobot.setMotors(50, 50);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case ("2"):
                        finchRobot.setMotors(200, 200);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case ("3"):
                        finchRobot.setMotors(-100, -100);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case ("4"):
                        finchRobot.setMotors(-50, -50);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case ("5"):
                        finchRobot.setMotors(0, 100);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case ("6"):
                        finchRobot.setMotors(50, 200);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case ("7"):
                        finchRobot.setMotors(100, 0);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case ("8"):
                        finchRobot.setMotors(200, 50);
                        finchRobot.wait(1000);
                        finchRobot.setMotors(0, 0);
                        break;
                    case ("9"):
                        userContinue = false;
                        break;
                    default:
                        Console.Write("Invalid input. ");
                        DisplayContinuePrompt();
                        break;


                }
            } while (userContinue);

            #endregion

            DisplayContinuePrompt();

        }

        #endregion

        /// <summary>
        /// Starts the process of data recording on the finch.
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void DataRecording(Finch finchRobot)
        {
            DisplayHeader("The Data Recorder");
            Console.WriteLine("This program allows the robot to read either temperature or the light brightness in front of the robot.");
            DisplayContinuePrompt();

            double dataFrequency = DisplayGetDataRecorderFrequencies();
            int numberOfReadings = DisplayGetDataRecorderNumber();

            GetDataReadings(dataFrequency, numberOfReadings, finchRobot);

            DisplayHeader("Data Recording - Closer");

            Console.WriteLine("The data recording process has completed!");

            DisplayContinueToMenuPrompt();
        }

        #region DATA RECORDING METHODS

        /// <summary>
        /// Prompts user for frequency of the readings
        /// </summary>
        /// <returns>frequency of readings in seconds, positive double</returns>
        private static double DisplayGetDataRecorderFrequencies()
        {
            double dataFrequency = 0;
            bool validated = false;
            DisplayHeader("Data Recorder Frequencies");
            Console.WriteLine();
            Console.WriteLine("Please enter how many seconds between the recording of each data point.");
            Console.Write("I'd recommend a small number, such as 1 or 0.5: ");

            //
            //Makes sure the input is a positive number
            //
            do
            {
                validated = double.TryParse(Console.ReadLine(), out dataFrequency);
                if (!validated || dataFrequency < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("The input must be a positive number, such as 2 or 0.4.");
                    Console.Write("Please enter another postive number: ");
                }
                else { };

            } while (!validated || dataFrequency < 0);

            Console.Write($"A data point will be made every ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{dataFrequency} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("seconds.");

            DisplayContinuePrompt();
            return dataFrequency;
        }

        /// <summary>
        /// Prompts user for how many readings they would like the robot to make.
        /// </summary>
        /// <returns>How many readings to make</returns>
        private static int DisplayGetDataRecorderNumber()
        {
            int secondsOfReading;
            bool validated;
            DisplayHeader("Data Recorder Readings");
            Console.WriteLine();
            Console.Write("Please enter how many readings will be made: ");

            //
            //makes sure the input is a positive, whole number
            //
            do
            {
                validated = int.TryParse(Console.ReadLine(), out secondsOfReading);
                if (!validated || secondsOfReading < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("The input must be a positive, whole number, such as 1 or 3.");
                    Console.Write("Please enter another postive, whole number: ");
                }
                else { };

            } while (!validated || secondsOfReading < 0);

            Console.Write("There will be ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{secondsOfReading} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("points of data.");

            DisplayContinuePrompt();
            return secondsOfReading;
        }

        /// <summary>
        /// Does all the data readings,  
        /// </summary>
        /// <param name="data">Spits back all the readings from the robot</param>
        private static void GetDataReadings(double dataFrequency, int numberOfReadings, Finch finchRobot)
        {
            //
            // variable declaration
            // data2 and averageData is used only in light measurements
            //
            double totalTime = dataFrequency * numberOfReadings;
            bool aOrBValidated = false;
            char lightOrTemp = 't';
            double[] data = new double[numberOfReadings];
            double[] data2 = new double[numberOfReadings];
            double[] averageData = new double[numberOfReadings];
            int frequencyToMilliseconds = Convert.ToInt32(dataFrequency * 1000);

            //
            // dumps all the information to the user
            //
            DisplayHeader("Obtain Data Readings");
            Console.WriteLine("The robot will now obtain the needed data readings");
            Console.Write("This will take ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{numberOfReadings} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("readings, with a reading at every ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{dataFrequency} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("second(s)");

            Console.WriteLine($"Overall, this will take {totalTime} seconds.");
            Console.WriteLine();
            Console.Write("The robot is ready to being recording data. Would you like it to ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("(A) ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Record light data or ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("(B) ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Record temperature data?");

            //
            // Validates and checks to see if user wants light or temperature data
            //
            do
            {
                ConsoleKeyInfo userCharacter = Console.ReadKey();
                string userResponse = userCharacter.KeyChar.ToString().ToLower();

                switch (userResponse)
                {
                    case ("a"):
                        Console.WriteLine();
                        Console.WriteLine("The robot will record light data!");
                        aOrBValidated = true;
                        lightOrTemp = 'l';
                        break;
                    case ("b"):
                        Console.WriteLine();
                        Console.WriteLine("The robot will record temperature data in fahrenheit!");
                        aOrBValidated = true;
                        lightOrTemp = 't';
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter either ''A'' for light data or ''B'' for temperature data.");
                        aOrBValidated = false;
                        break;
                }
            } while (!aOrBValidated);
                       
            DisplayContinuePrompt();

            //
            //the first for loop records data if the user picked temperature
            //the second for loop records data from both the left and right light sensors
            //
            Console.WriteLine();
            if (lightOrTemp == 't')
            {
                for (int i = 0; i < numberOfReadings; i++)
                {
                    data[i] = finchRobot.getTemperature();
                    data[i] = ConvertCelsiusToFahrenheit(data[i]);

                    Console.WriteLine("Temperature {0}: {1}", i+1, data[i]);
                    finchRobot.wait(frequencyToMilliseconds);
                }
            }
            else
            {
                for (int i = 0; i < numberOfReadings; i++)
                {
                    data[i] = finchRobot.getLeftLightSensor();
                    data2[i] = finchRobot.getRightLightSensor();

                    averageData[i] = (data[i] + data2[i]) / 2;
                    Console.WriteLine("Brightness {0}: {1}", i+1, averageData[i]);
                    finchRobot.wait(frequencyToMilliseconds);
                }
            }

            Console.WriteLine();
            Console.WriteLine("The data is done recording! We will now process the data and display it via a bar graph.");
            Console.WriteLine("The bars are approximate, and are much more helpful to describe larger changes in numbers than the smaller ones.");
            DisplayContinuePrompt();

            DisplayDataRecorderData(data, averageData, lightOrTemp);
                

        }

        /// <summary>
        /// Diplays the data in an organized fashion
        /// </summary>
        /// <param name="data"></param>
        private static void DisplayDataRecorderData(double[] temperatureData, double[] lightData, char lightOrTemp)
        {

            //
            // If user wanted temperature, does the first option
            // else, which should always be the user wanting light, does the second option
            //
            if (lightOrTemp == 't')
            {
                DisplayHeader("Temperatures");

                //
                // Makes an upper part of the graph
                //
                Console.Write("    ");
                for (int i = 0; i < 100; i++)
                {
                    i += 5;
                    Console.Write($"{i:00}  ");
                    i -= 1;
                }
                Console.WriteLine();
                Console.WriteLine("_____________________________________________________________________________");

                //
                // In addition to displaying the numbers, also makes them having varying places sticking out based on the temperature
                //
                for (int i = 0; i < temperatureData.Length; i++)
                {
                    for (int index = 0; index < (temperatureData[i]/5); index++)
                    {
                        Console.Write("@@@@");
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("  {0}*F  (Temperature {1})", temperatureData[i], i+1);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            else
            {
                DisplayHeader("Brightness of Light");

                //
                // Makes an upper part of the graph
                //
                Console.Write("    ");
                for (int i = 0; i < 130; i++)
                {
                    i += 10;
                    Console.Write($"     {i:000}");
                    i -= 1;
                }
                Console.WriteLine();
                Console.WriteLine("_____________________________________________________________________________________________");

                //
                // In addition to displaying the numbers, also makes them having varying places sticking out based on the brightness
                //
                for (int i = 0; i < lightData.Length; i++)
                {
                    for (int index = 0; index < (lightData[i] / 10); index++)
                    {
                        Console.Write("@@@@@@@@");
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("  {0}  (Brightness {1})", lightData[i], i+1);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            DisplayContinuePrompt();
        }

        /// <summary>
        /// Converts inputted temp in C* to F*
        /// </summary>
        /// <param name="celsiusTemp">a temperature in C*, most likely from the robot readings</param>
        /// <returns>temp in F*</returns>
        private static double ConvertCelsiusToFahrenheit(double celsiusTemp)
        {
            double fahrenheitTemp;

            fahrenheitTemp = (celsiusTemp * 9 / 5) + 32;          
            return fahrenheitTemp;
        }

        #endregion

        /// <summary>
        /// Starts the alarm system process
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void AlarmSystem(Finch finchRobot)
        {
            DisplayHeader("The Alarm System");
            Console.WriteLine("This module allows the robot to measure either temperature or light brightness, and gives feedback if either crosses a user-given input.");
            DisplayContinuePrompt();

            //
            // variables, as I shouldn't intialize them within the confirmation loop
            //
            string alarmType;
            int alarmSeconds;
            bool alarmContinue = false;

            //
            // alarm triggered
            // 0 is whether or not the alarm has been triggered
            // 1 is upper light, 2 is lower light, 3 is upper temperature, 4 is lower temperature
            //
            bool[] alarmTriggered = new bool[5];

            ///[summary]
            /// 0 & 1 are light, 2 & 3 are temperature thresholds
            /// 0 & 2 are upper, 1 & 3 are lower thresholds
            ///[/summary]
            double[] alarmThreshold = new double[4];


            //
            // If alarm confirmation sends back true, exits the loop, other asks user for new values to replace old ones 
            //
            do
            {
                alarmType = DisplayGetAlarmType();
                alarmThreshold = DisplayGetAlarmThreshold(alarmType, finchRobot);
                alarmSeconds = DisplayGetMaxSeconds();
                alarmContinue = DisplayAlarmConfirmation(alarmType, alarmThreshold, alarmSeconds, finchRobot);

            } while (!alarmContinue);

            alarmTriggered = DisplayMonitorCurrentLevels(alarmType, alarmThreshold, alarmSeconds, finchRobot);

            DisplayAlarmSystemEnding(alarmTriggered, finchRobot, alarmType);
        }

        #region ALARM SYSTEM METHODS

        /// <summary>
        /// Asks the user for what data they want the alarm to be triggered by
        /// </summary>
        /// <returns>Either temperature or light</returns>
        private static string DisplayGetAlarmType()
        {
            string alarmType = "INVALID";
            string userResponse;
            bool validResponse = false;

            DisplayHeader("Enter Alarm Type");

            //
            // color coded text
            //
            Console.Write("What would you like the robot to measure for its alarm? ");
            Console.Write("Please enter either ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[a] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("for light or ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[b] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("for temperature or ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[c] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("for both light and temperature. ");

            //
            // Validation loop for the user input
            //
            do
            {

                //
                //converts user answer into a single key, either a (for light) or b (for temp) or c (for both)
                //
                ConsoleKeyInfo userCharacter = Console.ReadKey();
                userResponse = userCharacter.KeyChar.ToString().ToLower();
               
                switch (userResponse)
                {
                    case ("a"):
                        alarmType = "Light";
                        Console.WriteLine(" The robot will measure Light for the alarm system!");
                        validResponse = true;
                        break;
                    case ("b"):
                        alarmType = "Temperature";
                        Console.WriteLine(" The robot will measure Temperature for the alarm system!");
                        validResponse = true;
                        break;
                    case ("c"):
                        alarmType = "Both";
                        Console.WriteLine(" The robot will measure both Light and Temperature for the alarm system!");
                        validResponse = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine(" Your response is invalid.");
                        Console.Write("Please enter either 'a' for light, 'b' for temperature, or 'c' for both: ");
                        validResponse = false;
                        break;
                }
            } while (!validResponse);

            DisplayContinuePrompt();
                
            return alarmType;
        }

        /// <summary>
        /// Asks the user for what they want the threshold to be.
        /// </summary>
        /// <returns></returns>
        private static double[] DisplayGetAlarmThreshold(string alarmType, Finch finchRobot)
        {
            //
            // variables
            //
            double[] alarmThreshold = new double[4];
            bool validAnswer = false;
            double ambientLevelTemperature;
            double ambientLevelLight;

            DisplayHeader("The Alarm Threshold");

            //
            // depending on the alarm type, has 3 seperate ways to get the thresholds.
            //
            switch (alarmType)
            {
                case ("Light"):
                    ambientLevelLight = GetCurrentAmbientXLevels(alarmType, finchRobot);
                    Console.WriteLine($"The robot is going to be recording light, and the current light level is {ambientLevelLight}");
                    Console.WriteLine();

                    //
                    // Loops twice, the with first loop giving an upper threshold and the second giving a lower threshold
                    //
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            Console.Write("Please enter an upper threshold >> ");

                        }
                        else
                        {
                            Console.Write("Please enter an lower threshold >> ");
                        }
                        do
                        {
                            validAnswer = Double.TryParse((Console.ReadLine().ToString()), out alarmThreshold[i]);
                            if (!validAnswer || alarmThreshold[i] < 0 || alarmThreshold[i] >= 255)
                            {
                                Console.WriteLine("That was not a valid response. Please enter a number greater than 0 but less than 255.");
                                validAnswer = false;
                            }
                            else if (alarmThreshold[1] >= alarmThreshold[0])
                            {
                                if (i == 0)
                                {
                                    Console.WriteLine("Please write a non-zero upper threshold.");
                                }
                                else
                                {
                                    Console.WriteLine("Please enter a lower threshold that is lower than the upper threshold.");
                                }
                                validAnswer = false;
                            }

                        } while (!validAnswer);
                    }

                    break;

                case ("Temperature"):
                    ambientLevelTemperature = GetCurrentAmbientXLevels(alarmType, finchRobot);
                    Console.WriteLine($"The robot is going to be recording temperature, and the current temperature is {ambientLevelTemperature}");

                    //
                    // Loops twice, the with first loop giving an upper threshold and the second giving a lower threshold
                    //
                    for (int i = 2; i < 4; i++)
                    {
                        if (i == 2)
                        {
                            Console.Write("Please enter an upper threshold >> ");

                        }
                        else
                        {
                            Console.Write("Please enter an lower threshold >> ");
                        }
                        do
                        {
                            validAnswer = Double.TryParse((Console.ReadLine().ToString()), out alarmThreshold[i]);
                            if (!validAnswer || alarmThreshold[i] < 0 || alarmThreshold[i] > 100)
                            {
                                Console.WriteLine("That was not a valid response. Please enter a number greater than 0 but less than 100.");
                                validAnswer = false;
                            }
                            else if (alarmThreshold[3] >= alarmThreshold[2])
                            {
                                if (i == 2)
                                {
                                    Console.WriteLine("Please write a non-zero upper threshold.");
                                }
                                else
                                {
                                    Console.WriteLine("Please enter a lower threshold that is lower than the upper threshold.");
                                }
                                validAnswer = false;
                            }

                        } while (!validAnswer);
                    }

                    break;

                case ("Both"):
                    ambientLevelTemperature = GetCurrentAmbientXLevels("Temperature", finchRobot);
                    ambientLevelLight = GetCurrentAmbientXLevels("Light", finchRobot);
                    Console.WriteLine("The robot is going to be recording both temperature and light.");
                    Console.WriteLine($"The current light level is {ambientLevelLight} and the current temperature is {ambientLevelTemperature}");

                    //
                    //alarm threshold 2 has to be set to 1, otherwise a later check freaks out and the program softlocks.
                    // the else if (alarmthreshold[3] >= alarmThreshold[2] || ...
                    //
                    alarmThreshold[2] = 1;

                    //
                    // loops 4 times, with it filling up light followed by temperature.
                    //
                    for (int i = 0; i < alarmThreshold.Length; i++)
                    {
                        switch (i)
                        {
                            case (0):
                                Console.Write("Please enter an upper threshold for light >> ");
                                break;
                            case (1):
                                Console.Write("Please enter an lower threshold for light >> ");
                                break;
                            case (2):
                                Console.Write("Please enter an upper threshold for temperature >> ");
                                break;
                            case (3):
                                Console.Write("Please enter an lower threshold for temperature >> ");
                                break;
                            default:
                                break;
                        }

                        //
                        // user input validation
                        //
                        do
                        {
                            validAnswer = Double.TryParse((Console.ReadLine().ToString()), out alarmThreshold[i]);
                            if (!validAnswer || alarmThreshold[i] < 0 || alarmThreshold[i] > 255)
                            {
                                Console.WriteLine("That was not a valid response. Please enter a number greater than 0 but less than 255.");
                                validAnswer = false;
                            }
                            else if (alarmThreshold[3] >= alarmThreshold[2] || alarmThreshold[1] >= alarmThreshold[0])
                            {
                                if (i == 0 || i == 2)
                                {
                                    Console.WriteLine("Please write a non-zero upper threshold.");
                                }
                                else
                                {
                                    Console.WriteLine("Please enter a lower threshold that is lower than the upper threshold.");
                                }
                                validAnswer = false;
                            }
                            
                        } while (!validAnswer);
                    }
                    break;

                default:
                    Console.WriteLine("A significant error has occured.");
                    Console.WriteLine($"The reason, alarm type was {alarmType}.");
                    break;
            }

            Console.WriteLine("All thresholds have been obtained.");
            DisplayContinuePrompt();

           return alarmThreshold;
        }

        /// <summary>
        /// Asks the user for how long they want to wait until the robot stops recording for the alarm.
        /// </summary>
        /// <returns></returns>
        private static int DisplayGetMaxSeconds()
        {
            int seconds = 0;
            bool validAnswer = false;

            DisplayHeader("The Alarm Duration");

            Console.Write("Please enter the maximum duration, in seconds, of the alarm test: ");

            //
            // Validation loop, also makes sure the user puts in a whole number greater than 0
            //
            do
            {
                validAnswer = Int32.TryParse((Console.ReadLine().ToString()), out seconds);
                if (!validAnswer || seconds <= 0)
                {
                    Console.WriteLine("That was not a valid response. Please enter a whole number greater than 0.");
                    validAnswer = false;
                }

            } while (!validAnswer);
            Console.WriteLine("The maximum duration of the alarm test will be {0} seconds.", seconds);
            Console.WriteLine($"This will take a maximum of {(seconds * 2) + 1} readings.");

            DisplayContinuePrompt();

            return seconds;
        }

        /// <summary>
        /// Echos back alarm type, threshhold, seconds, ambience, and asks the user if they like the numbers.
        /// </summary>
        /// <param name="alarmType"></param>
        /// <param name="alarmThreshhold"></param>
        /// <param name="alarmSeconds"></param>
        /// <returns></returns>
        private static bool DisplayAlarmConfirmation(string alarmType, double[] alarmThreshold, int alarmSeconds, Finch finchRobot)
        {
            bool alarmContinue = false;
            double ambientLight = GetCurrentAmbientXLevels("Light", finchRobot);
            double ambientTemperature = GetCurrentAmbientXLevels("Temperature", finchRobot);

            DisplayHeader("Alarm Confirmation");

            Console.WriteLine("We will now confirm your inputs.");
            Console.Write($"The robot will be recording ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{alarmType} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("and will wait no more than ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{alarmSeconds} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("seconds for the threshold to be crossed.");

            //
            // Simply a text display, showing all previous user inputs.
            //
            switch (alarmType)
            {
                //
                //Long, but only due to constant color changes
                //
                case ("Temperature"):
                    Console.Write($"The upper threshold is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[2]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", the lower threshold is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[3]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", and the current ambient temperature is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{ambientTemperature}. ");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ("Light"):
                    Console.Write($"The upper threshold is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[0]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", the lower threshold is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[1]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", and the current ambient light level is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{ambientLight}. ");
                    Console.ForegroundColor = ConsoleColor.White;
                   
                    break;
                case ("Both"):
                    Console.Write($"The upper temperature threshold is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[2]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", the lower temperature threshold is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[3]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", and the current ambient temperature is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{ambientTemperature}. ");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write($"The upper light threshold is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[0]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", the lower light threshold is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[1]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", and the current ambient light level is ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{ambientLight}. ");
                    Console.ForegroundColor = ConsoleColor.White;

                    break;
                default:
                    break;
            }

            Console.Write("Are these numbers okay? Press ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[1] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("to continue, or press ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[2] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("to re-enter all previous input.");

            //
            // validation loop for 1 and 2 - seeing if they want to re input answers.
            //
            
            bool validAnswer; 
            do
            {
                ConsoleKeyInfo userCharacter = Console.ReadKey();
                string userResponse = userCharacter.KeyChar.ToString().ToLower();
                if (userResponse == "1")
                {
                    Console.WriteLine(" These numbers are okay. The robot will continue with the alarm system.");
                    validAnswer = true;
                    alarmContinue = true;
                }
                else if (userResponse == "2")
                {
                    Console.WriteLine(" You have chosen to restart with new inputs.");
                    validAnswer = true;
                    alarmContinue = false;
                }
                else
                {
                    Console.WriteLine(" Please enter either '1' or '2'.");
                    validAnswer = false;
                }

            } while (!validAnswer);

            DisplayContinuePrompt();

            return alarmContinue;
        }

        /// <summary>
        /// Allows for the running of either monitoring light or monitoring temperature.
        /// </summary>
        /// <param name="alarmType"></param>
        /// <param name="alarmThreshold"></param>
        /// <param name="alarmSeconds"></param>
        /// <param name="finchRobot"></param>
        private static bool[] DisplayMonitorCurrentLevels(string alarmType, double[] alarmThreshold, int alarmSeconds, Finch finchRobot)
        {
            //
            // variables
            //
            bool[] alarmTriggered = new bool[5];
            alarmTriggered[0] = false;
            double timeAccrued = 0;
            double currentTemperature;
            double currentLight;

            DisplayHeader("Alarm System");
            
            finchRobot.setLED(0, 255, 0);
            Console.WriteLine($"Welcome to the alarm system, we will be measuring {alarmType} to determine the alarm.");
            Console.WriteLine();

            //
            // colored text dump
            //
            switch (alarmType)
            {
                case ("Temperature"):
                    Console.Write($"The upper threshold will be ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[2]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", while the lower threshold will be ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{alarmThreshold[3]}.");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("If the measured temperature is above the upper threshold or below the lower threshold, the alarm will trigger.");

                    break;
                case ("Light"):
                    Console.Write($"The upper threshold will be ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[0]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", while the lower threshold will be ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{alarmThreshold[1]}.");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("If the measured temperature is above the upper threshold or below the lower threshold, the alarm will trigger.");

                    break;
                case ("Both"):
                    Console.Write($"For temperature, the upper threshold will be ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[2]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", while the lower threshold will be ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{alarmThreshold[3]}.");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write($"For light, the upper threshold will be ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{alarmThreshold[0]}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(", while the lower threshold will be ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{alarmThreshold[1]}.");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("If either measurement is above its respective upper threshold or below its respective lower threshold, the alarm will trigger.");

                    break;
                default:
                    Console.WriteLine("There is an error in the application, and the application will now exit to the main menu.");
                    break;
            }

            Console.WriteLine("The program will now begin the alarm system process.");
            DisplayContinuePrompt();

            //
            // every ~.5 seconds, checks to see if the thresholds are crossed or if enough time has passed
            // remember, aTr[1] is upper light, aTr[2] is lower light
            // aTr[3] is upper temperature, aTr[4] is lower temperature
            //
            while (!alarmTriggered[0] && timeAccrued <= alarmSeconds)
            {
                switch (alarmType)
                {

                    //
                    // the current reading time accrued is to account for it starting at 0, and it increasing by .5 instead of 1
                    //
                    case ("Temperature"):

                        DisplayHeader("Temperature Alarm");
                        Console.WriteLine();
                        Console.WriteLine($"Reading #{(timeAccrued + .5) * 2}");
                        Console.WriteLine();
                        Console.WriteLine($"Upper Threshold - {alarmThreshold[2]}");
                        Console.WriteLine();

                        currentTemperature = GetCurrentAmbientXLevels("Temperature", finchRobot);

                        Console.WriteLine($"Current Temperature - {currentTemperature}");
                        Console.WriteLine();
                        Console.WriteLine($"Lower Threshold - {alarmThreshold[3]}");

                        if (currentTemperature > alarmThreshold[2] || currentTemperature < alarmThreshold[3])
                        {
                            alarmTriggered[0] = true;
                            if (currentTemperature > alarmThreshold[2])
                            {
                                alarmTriggered[3] = true;
                            }
                            else
                            {
                                alarmTriggered[4] = true;
                            }
                        }

                        finchRobot.wait(500);
                        timeAccrued += .5;

                        break;

                    case ("Light"):

                        DisplayHeader("Light Alarm");
                        Console.WriteLine();
                        Console.WriteLine($"Reading #{(timeAccrued + .5) * 2}");
                        Console.WriteLine();
                        Console.WriteLine($"Upper Threshold - {alarmThreshold[0]}");
                        Console.WriteLine();

                        currentLight = GetCurrentAmbientXLevels("Light", finchRobot);

                        Console.WriteLine($"Current Light - {currentLight}");
                        Console.WriteLine();
                        Console.WriteLine($"Lower Threshold - {alarmThreshold[1]}");

                        if (currentLight > alarmThreshold[0] || currentLight < alarmThreshold[1])
                        {
                            alarmTriggered[0] = true;
                            if (currentLight > alarmThreshold[0])
                            {
                                alarmTriggered[1] = true;
                            }
                            else
                            {
                                alarmTriggered[2] = true;
                            }
                        }

                        finchRobot.wait(500);
                        timeAccrued += .5;

                        break;

                    case ("Both"):

                        DisplayHeader("Temperature & Light Alarm");
                        Console.WriteLine();
                        Console.WriteLine($"Reading #{(timeAccrued + .5) * 2}");
                        Console.WriteLine();
                        Console.WriteLine($"Upper Temperature Threshold - {alarmThreshold[2]}");
                        Console.WriteLine();

                        currentTemperature = GetCurrentAmbientXLevels("Temperature", finchRobot);

                        Console.WriteLine($"Current Temperature - {currentTemperature}");
                        Console.WriteLine();
                        Console.WriteLine($"Lower Temperature Threshold - {alarmThreshold[3]}");

                        Console.WriteLine();
                        Console.WriteLine("-----------------------------------------");
                        Console.WriteLine();
                        Console.WriteLine($"Upper Light Threshold - {alarmThreshold[0]}");
                        Console.WriteLine();

                        currentLight = GetCurrentAmbientXLevels("Light", finchRobot);

                        Console.WriteLine($"Current Light - {currentLight}");
                        Console.WriteLine();
                        Console.WriteLine($"Lower Light Threshold - {alarmThreshold[1]}");

                        //
                        // for the display alarm ending, it helps by telling alarmTriggered[x] which threshold was crossed
                        //
                        if (currentLight > alarmThreshold[0] || currentLight < alarmThreshold[1])
                        {
                            alarmTriggered[0] = true;
                            if (currentLight > alarmThreshold[0])
                            {
                                alarmTriggered[1] = true;
                            }
                            else
                            {
                                alarmTriggered[2] = true;
                            }
                        }
                        else if (currentTemperature > alarmThreshold[2] || currentTemperature < alarmThreshold[3])
                        {
                            alarmTriggered[0] = true;
                            if (currentTemperature > alarmThreshold[2])
                            {
                                alarmTriggered[3] = true;
                            }
                            else
                            {
                                alarmTriggered[4] = true;
                            }
                        }

                        finchRobot.wait(500);
                        timeAccrued += .5;

                        break;
                    default:
                        timeAccrued = alarmSeconds + 1;
                        break;
                }            
            }

            return alarmTriggered;
        }

        /// <summary>
        /// Depending on whether or not the alarm was triggered, completes the closing screen either way
        /// </summary>
        /// <param name="alarmTriggered"></param>
        private static void DisplayAlarmSystemEnding(bool[] alarmTriggered, Finch finchRobot, string alarmType)
        {

            DisplayHeader("Alarm System Closer");

            if (alarmTriggered[0])
            {
                finchRobot.setLED(255, 0, 0);
                finchRobot.noteOn(4000);

                Console.WriteLine();
                Console.WriteLine("A Threshold was crossed!!");
                Console.WriteLine();

                finchRobot.wait(800);
                finchRobot.noteOff();

                //
                // Using the rest of alarm triggered, tells the user which specific threshold was crossed
                // It prefers stating light and upper thresholds first.
                //
                if (alarmTriggered[1] == true)
                {
                    Console.WriteLine("The upper light threshold was crossed!");
                }
                else if (alarmTriggered[2] == true)
                {
                    Console.WriteLine("The lower light threshold was crossed!");
                }
                else if (alarmTriggered[3] == true)
                {
                    Console.WriteLine("The upper temperature threshold was crossed!");
                }
                else if (alarmTriggered[4] == true)
                {
                    Console.WriteLine("The lower temperature threshold was crossed!");
                }

            }
            else
            {
                Console.WriteLine("The thresholds were not crossed within the time period.");
            }

            Console.WriteLine();
            Console.WriteLine("Thank you for using the alarm system.");
            Console.WriteLine("We will now move back to the main menu.");

            DisplayContinueToMenuPrompt();
            finchRobot.setLED(0, 0, 0);
        }

        /// <summary>
        /// Gets the current ambient temperature/light, REQUIRES ROBOT
        /// any temperature is in fahrenheit
        /// </summary>
        /// <param name="alarmType"></param>
        /// <param name="finchRobot"></param>
        /// <returns>Current ambient Light/Temperature - double</returns>
        private static double GetCurrentAmbientXLevels(string alarmType, Finch finchRobot)
        {
            double ambientNumber;

            //
            // the large else ambient number helps to see if something really messed up.
            //
            if (alarmType == "Temperature")
            {
                 double unpreparedAmbientNumber = finchRobot.getTemperature();
                 ambientNumber = ConvertCelsiusToFahrenheit(unpreparedAmbientNumber);
            }
            else if (alarmType == "Light")
            {
                double leftLight = finchRobot.getLeftLightSensor();
                double rightLight = finchRobot.getRightLightSensor();

                ambientNumber = (rightLight + leftLight) / 2;
            }
            else
            {
                ambientNumber = 99999;
            }
            return ambientNumber;
        }

        #endregion

        /// <summary>
        /// starts the user programming process
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void UserProgramming(Finch finchRobot)
        {
            //
            //variable declaration
            //
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;
            string userResponse;
            bool validResponse;
            bool toMainMenu = false;

            (List<Command> commands, List<int> duration) commandAndDuration;
            commandAndDuration.commands = new List<Command>();
            commandAndDuration.duration = new List<int>();

            DisplayHeader("User Programming");
            Console.WriteLine("Welcome to User Programming! This module allows you to semi-control the robot using a custom made language!");
            DisplayContinuePrompt();

            do
            {
                Console.Clear();
                //
                //Displays the menu, having options for numbers 1-6
                //
                DisplayHeader("User Programming Menu");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[1] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Set Command Parameters");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[2] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Add Commands");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[3] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("View Commands");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[4] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Execute Commands");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[5] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Remove Commands");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[6] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Proceed back to Main Menu");
                Console.WriteLine();
                Console.Write("Please enter the appropiate number to choose what section to goto >> ");

                do
                {
                    //
                    //Executes the corresponding method for numbers 1-6, else it loops
                    //
                    ConsoleKeyInfo userCharacter = Console.ReadKey();
                    userResponse = userCharacter.KeyChar.ToString();
                    validResponse = true;
                    switch (userResponse)
                    {
                        case ("1"):
                            commandParameters = DisplayGetCommandParameters();
                            break;
                        case ("2"):
                            DisplayGetFinchCommands(commandAndDuration);
                            break;
                        case ("3"):
                            DisplayFinchCommands(commandAndDuration);
                            break;
                        case ("4"):
                            if (commandParameters.motorSpeed == 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Your motor speed is set at 0. Please set a non-zero motor speed in 'Set Command Parameters'");
                            }
                            else
                            {
                                DisplayExecuteFinchCommands(finchRobot, commandAndDuration, commandParameters);
                            }
                            break;
                        case ("5"):
                            DisplayRemoveFinchCommands(commandAndDuration);
                            break;
                        case ("6"):
                            toMainMenu = true;
                            Console.WriteLine("");
                            break;
                        default:
                            Console.WriteLine();
                            Console.WriteLine("Please enter a number 1-6 in number form to goto the cooresponding program.");
                            Console.Write("Enter the appropiate number to choose what program to run >> ");
                            validResponse = false;
                            break;

                    }
                } while (!validResponse);
            } while (!toMainMenu);


            DisplayContinueToMenuPrompt();

        }

        #region USER PROGRAMMING METHODS

        /// <summary>
        /// Allows the user to obtain their preferred motor speed, led Brightness, and wait seconds.
        /// </summary>
        /// <returns></returns>
        static (int motorSpeed, int ledBrightness, double waitSeconds) DisplayGetCommandParameters()
        {
            //
            //variables
            //
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            bool validAnswer = false;

            DisplayHeader("Getting Command Parameters");

            Console.WriteLine("Hello and welcome to the command parameters section.");
            Console.WriteLine("In this section, you are able to set the speed of the motors, the brightness of the LED, and how long the ''wait'' command is.");

            DisplayContinuePrompt();

            Console.Write("Please enter the speed of the motors >> ");
            
            //
            // validation loop for the motor speed. Makes sure it is a whole number between 0 and 255
            //
            do
            {
                validAnswer = Int32.TryParse(Console.ReadLine(), out commandParameters.motorSpeed);

                if (commandParameters.motorSpeed > 255 || commandParameters.motorSpeed < 0 || !validAnswer)
                {
                    Console.WriteLine("Invalid Response.");
                    Console.Write("Please enter a whole number between 0 and 255 >> ");
                    validAnswer = false;
                }
                else
                {
                    Console.WriteLine($"The motor speed will be {commandParameters.motorSpeed}.");
                }

            } while (!validAnswer);


            Console.WriteLine();
            Console.Write("Please enter the brightness of the LED >> ");

            //
            // validation loop for LED brightness. Makes sure it is a whole number between 0 and 255
            //
            validAnswer = false;
            do
            {
                validAnswer = Int32.TryParse(Console.ReadLine(), out commandParameters.ledBrightness);

                if (commandParameters.ledBrightness > 255 || commandParameters.ledBrightness < 0 || !validAnswer)
                {
                    Console.WriteLine("Invalid Response.");
                    Console.Write("Please enter a whole number between 0 and 255 >> ");
                    validAnswer = false;
                }
                else
                {
                    Console.WriteLine($"The LED brightness will be {commandParameters.ledBrightness}.");
                }

            } while (!validAnswer);

            Console.WriteLine();
            Console.Write("Please enter how long the wait command waits >> ");

            //
            // validation loop for the wait command. Makes sure it is between 0 and 5.
            //

            do
            {
                validAnswer = Double.TryParse(Console.ReadLine(), out commandParameters.waitSeconds);

                if (commandParameters.waitSeconds < 0 || commandParameters.waitSeconds > 10 || !validAnswer)
                {
                    Console.WriteLine("Invalid Response");
                    Console.Write("Please enter a number between 0 and 10 >> ");
                    validAnswer = false;
                }
                else
                {
                    Console.WriteLine($"The wait period for the wait command will be {commandParameters.waitSeconds} seconds.");
                }

            } while (!validAnswer);

            Console.WriteLine("The parameters have all been set!");
            Console.WriteLine($"The motor speed will be {commandParameters.motorSpeed}, the LED brightness will be {commandParameters.ledBrightness}, and the wait period will be {commandParameters.waitSeconds} seconds.");

            DisplayContinuePrompt();

            return commandParameters;
        }

        /// <summary>
        /// Obtains all the users commands
        /// </summary>
        /// <param name="commands"></param>
        static void DisplayGetFinchCommands((List<Command> commands, List<int> duration) commandAndDuration)
        {
            //
            // variable list
            // throwaway and enteredNumber are to ensure that numbers are not entered, as those do not toss errors in enumerations.
            //
            Command command = Command.NONE;
            bool validAnswer = false;
            double throwaway;
            bool enteredNumber;
            string userResponse;
            string[] splitUserResponse;
            int durationResponse;
            bool validDurationResponse;

            DisplayHeader("Programming Commands");

            Console.WriteLine("We will now begin to add commands to the robot's command list.");
            Console.WriteLine("Simply type the command, with no spaces.");
            Console.WriteLine("If you want to the command to last or wait a certain duration, please type its duration, in milliseconds, in after the command with a space between the two.");
            Console.WriteLine("In doing so, please use the format COMMAND DURATION.");
            Console.WriteLine();
            Console.WriteLine("Here is the list of commands:");

            //
            //Shows the user all the possible commands. Is infinitely expandable, and expands automatically
            //
            for (int i = 1; i < (Enum.GetNames(typeof (Command)).Length); i++)
            {
                Console.Write((Command) i);
                Console.Write(", ");
            }

            Console.WriteLine();
            Console.WriteLine("'DONE' will end the command collection.");

            while (command != Command.DONE)
            {
                //
                // validation loop, makes sure that the user response is the same as an enumeration. 
                // also checks to see if a duration is set, and that the duration, if input, is an integer.
                //
                do
                {
                    Console.Write("Enter Command: ");

                    userResponse = Console.ReadLine().ToUpper();
                    splitUserResponse = userResponse.Split(' ');

                    //
                    // This set double checks that the users first input is a command.
                    //
                    validAnswer = Enum.TryParse<Command>(splitUserResponse[0], out command);
                    enteredNumber = double.TryParse(splitUserResponse[0], out throwaway);

                    //
                    // This first if,if/else tree is to check the user response, making sure that it is input correctly
                    // In addition, checks the length of the user response, acting correctly depending on the users input.
                    //
                    if (splitUserResponse.Length > 2)
                    {
                        Console.WriteLine("Invalid command Length, please enter no more than 1 space.");
                    }
                    else if (splitUserResponse.Length == 1)
                    {
                        if (!validAnswer)
                        {
                            Console.WriteLine("Please enter a valid command as seen above.");
                        }
                        else
                        {
                            if (!enteredNumber)
                            {
                                commandAndDuration.commands.Add(command);
                                commandAndDuration.duration.Add(0);
                            }

                            else
                            {
                                Console.WriteLine("Please do not enter a number for the command.");
                            }

                        }
                    }
                    else
                    {
                        validDurationResponse = Int32.TryParse(splitUserResponse[1], out durationResponse);

                        if (!validAnswer)
                        {
                            Console.WriteLine("Please enter a valid command as seen above.");
                        }
                        else
                        {
                            if (!enteredNumber)
                            {
                                if (validDurationResponse)
                                {
                                    commandAndDuration.commands.Add(command);
                                    commandAndDuration.duration.Add(durationResponse);
                                }
                                else
                                {
                                    Console.WriteLine("Please either write a integer or nothing for the duration.");
                                }
                            }

                            else
                            {
                                Console.WriteLine("Please do not enter a number for the command.");
                            }

                        }
                    }
                    

                } while (!validAnswer);
            }

            DisplayContinuePrompt();

            Console.WriteLine("Here is the list of all your current commands for the robot in order.");

            foreach (Command eachCommand in commandAndDuration.commands)
            {
                Console.Write($"{eachCommand}, ");
            }

            DisplayContinuePrompt();

        }

        /// <summary>
        /// Shows the user all of their commands they currently have.
        /// </summary>
        /// <param name="commands"></param>
        static void DisplayFinchCommands((List<Command> commands, List<int> duration) commandAndDuration)
        {

            //
            // duration length increases by 1, and shows the duration of a command whenever it exists.
            //
            int durationLength = 0;
            DisplayHeader("User Commands");

            Console.WriteLine("Here is the list of all your current commands for the robot in order:");
            Console.WriteLine();

            foreach (Command eachCommand in commandAndDuration.commands)
            {
                Console.Write($"{eachCommand}, ");
                if (commandAndDuration.duration[durationLength] != 0)
                {
                    Console.WriteLine($"{commandAndDuration.duration[durationLength]} milliseconds.");
                }
                else
                {
                    Console.WriteLine("No duration.");
                }
                durationLength++;
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Executes all the entered commands.
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="commands"></param>
        /// <param name="commandParameters"></param>
        static void DisplayExecuteFinchCommands(Finch finchRobot, (List<Command> commands, List<int> duration) commandAndDuration, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)

        {
            //
            //temp variable for display commands
            //
            double currentLevels;

            //
            // duration length increases by 1, and allows the robot to wait the correct time.
            //
            int durationLength = 0;

            DisplayHeader("Finch Display of Commands");

            Console.WriteLine("The robot will now enact all the commands that you have previously selected!");
            Console.WriteLine("Make sure your robot is in an open, safe space.");

            DisplayContinuePrompt();

            //
            // Using the commands user typed in, the robot does actions.
            //
            foreach (Command command in commandAndDuration.commands)
            {
                //
                // does the respestive command when it pops up
                //
                switch (command)
                {
                    case Command.NONE:
                        break;
                    case Command.DONE:
                        break;

                    case Command.MOVEFORWARD:
                        Console.WriteLine(command);
                        finchRobot.setMotors(commandParameters.motorSpeed, commandParameters.motorSpeed);
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.MOVEBACKWARDS:
                        Console.WriteLine(command);
                        finchRobot.setMotors(-commandParameters.motorSpeed, -commandParameters.motorSpeed);
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.STOPMOTORS:
                        Console.WriteLine(command);
                        finchRobot.setMotors(0, 0);
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.WAIT:
                        Console.WriteLine(command);
                        finchRobot.wait((int)(commandParameters.waitSeconds * 1000));
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.TURNRIGHT:
                        Console.WriteLine(command);
                        finchRobot.setMotors(commandParameters.motorSpeed, 0);
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.TURNLEFT:
                        Console.WriteLine(command);
                        finchRobot.setMotors(0, commandParameters.motorSpeed);
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.LEDON:
                        Console.WriteLine(command);
                        finchRobot.setLED(commandParameters.ledBrightness, commandParameters.ledBrightness, commandParameters.ledBrightness);
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.LEDOFF:
                        Console.WriteLine(command);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.GETCURRENTLIGHT:
                        Console.Write(command);
                        currentLevels = GetCurrentAmbientXLevels("Light", finchRobot);
                        Console.WriteLine($"  Current Light Level - {currentLevels}");
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.GETCURRENTTEMP:
                        Console.Write(command);
                        currentLevels = GetCurrentAmbientXLevels("Temperature", finchRobot);
                        Console.WriteLine($"  Current Temperature Level - {currentLevels}");
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.NOTEON:
                        Console.WriteLine(command);
                        finchRobot.noteOn((commandParameters.motorSpeed * commandParameters.ledBrightness));
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                    case Command.NOTEOFF:
                        Console.WriteLine(command);
                        finchRobot.noteOff();
                        finchRobot.wait(commandAndDuration.duration[durationLength]);
                        break;

                        //
                        // GO CRAZY is a mixed bag, it does multiple things
                        //
                    case Command.GOCRAZY:
                        Console.WriteLine(command);
                        finchRobot.noteOn(8000);
                        finchRobot.setMotors(commandParameters.motorSpeed / 2, 0);
                        for (int i = 0; i < 255; i++)
                        {
                            finchRobot.setLED(i, 255 - i, i ^ 2);
                        }
                        finchRobot.setMotors(0, commandParameters.motorSpeed ^ 2);
                        for (int i = 0; i < 255; i++)
                        {
                            finchRobot.setLED(0, i, 255 - i);
                        }
                        finchRobot.noteOff();
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.setMotors(0, 0);
                        break;

                        //
                        // GO STUPID is also a mixed bag, also doing multiple things
                        //
                    case Command.GOSTUPID:
                        Console.WriteLine(command);
                        currentLevels = GetCurrentAmbientXLevels("Light", finchRobot);
                        for (int i = 0; i < currentLevels; i++)
                        {
                            finchRobot.setLED(i, i*2, i*2);
                            finchRobot.noteOn(i * 85);
                            finchRobot.setMotors((int)currentLevels, (int)currentLevels * 2);
                        }
                        finchRobot.noteOff();
                        finchRobot.setLED(255, 0 , 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(0, 255, 0);
                        finchRobot.wait(250);
                        finchRobot.setLED(0, 0, 255);
                        finchRobot.wait(250);
                        finchRobot.setLED(150, 100, 55);
                        finchRobot.wait(250);

                        finchRobot.setLED(0, 0, 0);
                        finchRobot.setMotors(0, 0);

                        break;
                    default:
                        break;

                }

                durationLength++;
            }

            Console.WriteLine("The robot has completed your commands!");
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 0);
            finchRobot.setMotors(0, 0);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Clears the command list
        /// </summary>
        /// <param name="commands"></param>
        static void DisplayRemoveFinchCommands((List<Command> commands, List<int> duration) commandAndDuration)
        {
            
            string userResponse;

            DisplayHeader("Clearing Command List");

            Console.WriteLine("We will now clear the command list. This includes the duration list");
            Console.WriteLine("If you would like to keep your commands, please press 1. Otherwise, press any key to continue with the clearing.");

            //
            // Reads the users input.
            // if 1, it goes back to programming menu, else it wipes the lists clean.
            //
            ConsoleKeyInfo userCharacter = Console.ReadKey();
            userResponse = userCharacter.KeyChar.ToString();

            if (userResponse == "1")
            {
                Console.WriteLine();
                Console.WriteLine("We will now exit to the user programming menu.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("We are now clearing the command list.");
                commandAndDuration.commands.Clear();
                commandAndDuration.duration.Clear();
            }

            DisplayContinuePrompt();
        }

        #endregion

        /// <summary>
        /// Displays the menu of other menus that user can go to.
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void DisplayMenu(Finch finchRobot)
        {
            string userResponse;
            bool validResponse;
            bool exitProgram = false;
            bool finchRobotConnected = false;

            do
            {
                Console.Clear();
                //
                //Displays the menu, having options for numbers 1-7
                //
                DisplayHeader("The Main Menu");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[1] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Connect Finch Robot");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[2] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Talent Show");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[3] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Data Recorder");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[4] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Alarm System");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[5] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("User Programming");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[6] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Disconnect Finch Robot");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[7] ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Exit Program");
                Console.WriteLine();
                Console.Write("Please enter the appropiate number to choose what program to run >> ");

                do
                {
                    //
                    //Executes the corresponding method for numbers 1-7, else it loops
                    //In addition, cases 2-6 checks to see if the finch robot is connected, as they require the finch robot to continue.
                    //
                    ConsoleKeyInfo userCharacter = Console.ReadKey(); 
                    userResponse = userCharacter.KeyChar.ToString();
                    validResponse = true;
                    switch (userResponse)
                    {
                        case ("1"):
                            finchRobotConnected = DisplayConnectFinchRobot(finchRobot);
                            break;
                        case ("2"):
                            if (finchRobotConnected)
                            {
                                TalentShow(finchRobot);
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("The Finch robot is not connected, and thus the talent show cannot be started.");
                                DisplayContinuePrompt();
                            }
                            break;
                        case ("3"):
                            if (finchRobotConnected)
                            {
                                DataRecording(finchRobot);
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("The Finch robot is not connected, and thus the data recording process cannot be started.");
                                DisplayContinuePrompt();
                            }
                            break;
                        case ("4"):
                            if (finchRobotConnected)
                            {
                                AlarmSystem(finchRobot);
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("The Finch robot is not connected, and thus the alarm system process cannot be started.");
                                DisplayContinuePrompt();
                            }
                            break;
                        case ("5"):
                            if (finchRobotConnected)
                            {
                                UserProgramming(finchRobot);
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("The Finch robot is not connected, and thus the user programming process cannot be started.");
                                DisplayContinuePrompt();
                            }
                            break;
                        case ("6"):
                            if (finchRobotConnected)
                            {
                                finchRobotConnected = DisplayDisconnectFinchRobot(finchRobot);
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("The Finch robot is not connected, and thus the disconnection process cannot be started.");
                                DisplayContinuePrompt();
                            }
                            break;
                        case ("7"):
                            if (!finchRobotConnected)
                            {
                                exitProgram = true;
                            }
                            else
                            {
                                finchRobot.disConnect();
                                exitProgram = true;
                            }
                            break;
                        default:
                            Console.WriteLine();
                            Console.WriteLine("Please enter a number 1-7 in number form to goto the cooresponding program.");
                            Console.Write("Enter the appropiate number to choose what program to run >> ");
                            validResponse = false;
                            break;

                    }
                } while (!validResponse);
            } while (!exitProgram);


        }

    }
}
