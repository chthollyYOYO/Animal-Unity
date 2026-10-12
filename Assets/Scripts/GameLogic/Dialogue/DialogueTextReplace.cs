using Framework;

namespace GameLogic
{
    public static class DialogueTextReplace
    {
        public static string Replace(string str)
        {
            if (!string.IsNullOrEmpty(Record.current.name))
                str = str.Replace("{playerName}", Record.current.name);
            return str;
        }
    }
}
