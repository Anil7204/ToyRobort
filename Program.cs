

namespace ToyRobort
{
    public enum Command
    {
        Place,
        Move,
        Left,
        Right,
        Report
    }
    public enum Direction
    {
        North,
        East,
        South,
        West
    }
 
    public class MainProgram
    {
        public static void Main(string[] args)
        {
            const string description =
@" 

  1: Place the toy on a 6 x 6 grid
     using the following command:

     PLACE X,Y,Direction

     PLACE will put the toy robot on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST

  2: When the toy is placed, use the following commands to move the position of toy.
                
     REPORT – REPORT will announce the X,Y and Direction of the robot. 
     LEFT   – turns the toy 90 degrees left.
     RIGHT  – turns the toy 90 degrees right.
     MOVE   – will move the toy robot one unit forward in the direction it is currently facing.
     END    - End the application
";

            ToyBoard squareBoard = new ToyBoard(6, 6);
            InputParser inputParser = new InputParser();
            ToyRobot robot = new ToyRobot();
            var simulator = new Behaviour(robot, squareBoard, inputParser);

            var stopApplication = false;
            Console.WriteLine(description);
            do
            {
                var command = Console.ReadLine();
                if (command == null) continue;

                if (command.Equals("END"))
                    stopApplication = true;
                else
                {
                    try
                    {
                        var output = simulator.ProcessCommand(command.Split(' '));
                        if (!string.IsNullOrEmpty(output))
                            Console.WriteLine(output);
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
            } while (!stopApplication);
        }
    }

    public class ToyBoard
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public ToyBoard(int rows, int columns)
        {
            this.Rows = rows;
            this.Columns = columns;
        }

        public bool IsValidPosition(Position position)
        {
            return position.X < Columns && position.X >= 0 &&
                   position.Y < Rows && position.Y >= 0;
        }
    }
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    public class InputParser 
    {
        // this method takes a string array and compares the first element to the list of commands
        // if the command doesn't exist then an error is thrown, otherwise the command is returned
        public Command ParseCommand(string[] rawInput)
        {
            Command command;
            if (!Enum.TryParse(rawInput[0], true, out command))
                throw new ArgumentException("Sorry, your command was not recognised. Please try again using the following format: PLACE X,Y,Direction|MOVE|LEFT|RIGHT|REPORT");
            return command;
        }

        // Extracts the parameters from the user input and returns it       
        public PlaceCommandParameter ParseCommandParameter(string[] input)
        {
            var thisPlaceCommandParameter = new PlaceCommandParameterParser();
            return thisPlaceCommandParameter.ParseParameters(input);
        }
    }

    public class PlaceCommandParameter
    {
        public Position Position { get; set; }
        public Direction Direction { get; set; }

        public PlaceCommandParameter(Position position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }
    }

    // This class checks the parameters for the "PLACE" command.
    public class PlaceCommandParameterParser
    {
        // Number of parameters provided for "PLACE" Command. (X,Y,F)
        private const int ParameterCount = 3;

        // Number of raw input items expected from user.
        private const int CommandInputCount = 2;

        // Checks the toy's initial position and the direction it is going to be facing.
        public PlaceCommandParameter ParseParameters(string[] input)
        {
            Direction direction;
            Position position = null;

            // Checks that Place command is followed by valid command parameters (X,Y and F toy's face direction).
            if (input.Length != CommandInputCount)
                throw new ArgumentException("Incomplete command. Please ensure that the PLACE command is using format: PLACE X,Y,Direction");

            // Checks that three command parameters are provided for the PLACE command.   
            var commandParams = input[1].Split(',');
            if (commandParams.Length != ParameterCount)
                throw new ArgumentException("Incomplete command. Please ensure that the PLACE command is using format: PLACE X,Y,Direction");

            // Checks the direction which the toy is going to be facing.
            if (!Enum.TryParse(commandParams[commandParams.Length - 1], true, out direction))
                throw new ArgumentException("Invalid direction. Please select from one of the following directions: NORTH|EAST|SOUTH|WEST");

            var x = Convert.ToInt32(commandParams[0]);
            var y = Convert.ToInt32(commandParams[1]);
            position = new Position(x, y);

            return new PlaceCommandParameter(position, direction);
        }
    }


}
