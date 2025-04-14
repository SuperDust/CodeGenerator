using CodeGeneratorForm.Entity;

namespace CodeGeneratorForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DbContext.Instance.CodeFirst.InitTables(typeof(DbConfig));
            ApplicationConfiguration.Initialize();
            Application.Run(new FormMain());
        }
    }
}
