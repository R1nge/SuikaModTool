namespace SuikaModTool;

public partial class MainWindow
{
    public struct GameConfig : IComparable<GameConfig>
    {
        public string ModName;
        public string ModIconPath;
        public string ContainerImagePath;
        public string[] SuikaSkinsImagesPaths;
        public string[] SuikaIconsPaths;
        public string[] SuikaAudioPaths;
        public float[] SuikaDropChances;
        public float TimeBeforeTimerTrigger;
        public float TimerStartTime;
        public string InGameBackgroundPath;

        public int CompareTo(GameConfig other)
        {
            var modNameComparison = string.Compare(ModName, other.ModName, StringComparison.Ordinal);
            if (modNameComparison != 0) return modNameComparison;
            var modIconPathComparison = string.Compare(ModIconPath, other.ModIconPath, StringComparison.Ordinal);
            if (modIconPathComparison != 0) return modIconPathComparison;
            var containerImagePathComparison = string.Compare(ContainerImagePath, other.ContainerImagePath, StringComparison.Ordinal);
            if (containerImagePathComparison != 0) return containerImagePathComparison;
            var timeBeforeTimerTriggerComparison = TimeBeforeTimerTrigger.CompareTo(other.TimeBeforeTimerTrigger);
            if (timeBeforeTimerTriggerComparison != 0) return timeBeforeTimerTriggerComparison;
            var timerStartTimeComparison = TimerStartTime.CompareTo(other.TimerStartTime);
            if (timerStartTimeComparison != 0) return timerStartTimeComparison;
            return string.Compare(InGameBackgroundPath, other.InGameBackgroundPath, StringComparison.Ordinal);
        }
    }
}