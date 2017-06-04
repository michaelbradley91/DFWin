namespace DFWin.Core.Inputs.DwarfFortress
{
    public interface IStartInput : IDwarfFortressInput
    {
        string Title { get; }
        string[] MenuOptions { get; }
        int SelectedOption { get; }
        string Version { get; }
        string VisitText { get; }
        string VisitUrl { get; }
        string ProgrammedBy { get; }
        string DesignedBy { get; }
    }

    public class StartInput : DwarfFortressInput, IStartInput
    {
        public string Title { get; set; }
        public string[] MenuOptions { get; set; }
        public int SelectedOption { get; set; }
        public string Version { get; set; }
        public string VisitText { get; set; }
        public string VisitUrl { get; set; }
        public string ProgrammedBy { get; set; }
        public string DesignedBy { get; set; }
    }
}
