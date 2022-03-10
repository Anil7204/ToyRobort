
namespace ToyRobort
{
    /// <summary>
    /// This class is used to simulate the behaviour of the toy.
    /// </summary>
    public class Behaviour
    {
        public ToyRobot ToyRobot { get; private set; }
        public ToyBoard SquareBoard { get; private set; }
        public InputParser InputParser { get; private set; }
        public static string PlaceDirection { get; set; }
        public Behaviour(ToyRobot toyRobot, ToyBoard squareBoard, InputParser inputParser)
        {
            ToyRobot = toyRobot;
            SquareBoard = squareBoard;
            InputParser = inputParser;
        }

        public string ProcessCommand(string[] input)
        {
            var command = InputParser.ParseCommand(input);
            if (command != Command.Place && ToyRobot.Position == null) return string.Empty;

            if (command == Command.Place && input.Length < 3)
            {
                string[] placeParams = input[1].Split(',');
                if (placeParams.Length < 3 && PlaceDirection != string.Empty)
                {
                    input[1] = input[1] + ',' + PlaceDirection;
                }
                else
                {
                    PlaceDirection = placeParams[2];
                }
            }

            switch (command)
            {
                case Command.Place:
                    var placeCommandParameter = InputParser.ParseCommandParameter(input);
                    if (SquareBoard.IsValidPosition(placeCommandParameter.Position))
                        ToyRobot.Place(placeCommandParameter.Position, placeCommandParameter.Direction);
                    break;
                case Command.Move:
                    var newPosition = ToyRobot.GetNextPosition();
                    if (SquareBoard.IsValidPosition(newPosition))
                        ToyRobot.Position = newPosition;
                    break;
                case Command.Left:
                    ToyRobot.RotateLeft();
                    break;
                case Command.Right:
                    ToyRobot.RotateRight();
                    break;
                case Command.Report:
                    return GetReport();
            }
            return string.Empty;
        }

        public string GetReport()
        {
            return string.Format("Output: {0},{1},{2}", ToyRobot.Position.X,
                ToyRobot.Position.Y, ToyRobot.Direction.ToString().ToUpper());
        }
    }
}
