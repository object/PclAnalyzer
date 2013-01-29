namespace PclAnalyzer.UI.Services
{
    public static class DialogService
    {
        public static string SelectAssembly()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Assembly Files|*.dll;*.exe)";

            var result = dlg.ShowDialog();
            return result == true ? dlg.FileName : null;
        }
    }
}