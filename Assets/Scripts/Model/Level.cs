namespace Model
{
    public class Level
    {
        public static readonly Level[] Levels =
        {
            new(1, 1500),
            new(2, 3566),
        };

        public int Id;
        public int Target;

        private Level(int id, int target)
        {
            Id = id;
            Target = target;
        }
    }
}