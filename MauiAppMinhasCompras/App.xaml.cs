using MauiAppMinhasCompras.Helpers;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        /* 
        AVISANDO QUE VAMOS TRABALHAR COM SQLYTE
        Nele vamos tornar a classe disponivel para o app
        */
        static SQLiteDatabaseHelper _db; // CAMPO

        public static SQLiteDatabaseHelper Db // PROPRIEDADE DE LEITURA
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.bd3");

                    _db = new SQLiteDatabaseHelper(path);
                }

                return _db;

            }
        }

        public App()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // return new Window(new AppShell());

            return new Window(new NavigationPage(new Views.ListaProduto()));

        }
    }
}