namespace PclAnalyzer.UI.Services
{
    public static class DialogService
    {
        public static string SelectAssembly()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".dll;.exe";
            dlg.Filter = "Assemblies (.dll)|*.exe";

            var result = dlg.ShowDialog();
            return result == true ? dlg.FileName : null;
        }
    }
}