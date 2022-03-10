


namespace ToyRobort
{

    public class ToyRobot 
    {
        public Direction Direction { get; set; }
        public Position Position { get; set; }

        // Sets the toy's position and direction.
        public void Place(Position position, Direction direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        // Determines the next position of the toy based on the direction it's currently facing.
        public Position GetNextPosition()
        {
            var newPosition = new Position(Position.X, Position.Y);
            switch (Direction)
            {
                case Direction.North:
                    newPosition.Y = newPosition.Y + 1;
                    break;
                case Direction.East:
                    newPosition.X = newPosition.X + 1;
                    break;
                case Direction.South:
                    newPosition.Y = newPosition.Y - 1;
                    break;
                case Direction.West:
                    newPosition.X = newPosition.X - 1;
                    break;
            }
            return newPosition;
        }

        // Rotates the direction of the toy 90 degrees to the left.
        public void RotateLeft()
        {
            Rotate(-1);
        }

        // Rotates the direction of the toy 90 degrees to the right.
        public void RotateRight()
        {
            Rotate(1);
        }

        // Determines the new direction of the toy. The new direction is based
        // on current direction and the rotation command (LEFT - Right)
        
        public void Rotate(int rotationNumber)
        {
            var directions = (Direction[])Enum.GetValues(typeof(Direction));
            Direction newDirection;
            if ((Direction + rotationNumber) < 0)
                newDirection = directions[directions.Length - 1];
            else
            {
                var index = ((int)(Direction + rotationNumber)) % directions.Length;
                newDirection = directions[index];
            }
            Direction = newDirection;
            Behaviour.PlaceDirection = newDirection.ToString();
        }       
    }
}
