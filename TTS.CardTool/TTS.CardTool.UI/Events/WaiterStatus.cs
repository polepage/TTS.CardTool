namespace TTS.CardTool.UI.Events
{
    struct WaiterStatus
    {
        private WaiterStatus(string text, int maximum)
        {
            Text = text;
            Maximum = maximum;
        }

        public bool IsIndeterminate => Maximum < 0;
        public string Text { get; }
        public int Maximum { get; }

        public static WaiterStatus Indeterminate()
        {
            return Indeterminate(null);
        }

        public static WaiterStatus Indeterminate(string text)
        {
            return new WaiterStatus(text, -1);
        }

        public static WaiterStatus Range(string text, int maximum)
        {
            return new WaiterStatus(text, maximum);
        }
    }
}
