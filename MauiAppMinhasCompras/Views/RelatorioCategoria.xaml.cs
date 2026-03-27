using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;


namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    ObservableCollection<Models.RelatorioCategoria> lista = new();

    public RelatorioCategoria()
    {
        InitializeComponent();
        lst_relatorio.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            var dados = await App.Db.GetTotalPorCategoria();

            dados.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}