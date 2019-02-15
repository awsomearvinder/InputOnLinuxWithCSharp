namespace InputLibrary
{
    public class Keycode
    {
        private IDictionary<short, string> dict = new Dictionary<short, string>()
        {
            {1,"ESC"},
            {2,"1"},
            {3,"2"}
        };
        public char KEY_ESC{get{ return 1; }}
        public char KEY_1{get;}
    }
}