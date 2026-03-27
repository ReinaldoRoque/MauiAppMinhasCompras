using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using MauiAppMinhasCompras.Views;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    string categoriaSelecionada = "Todas";

    private async Task CarregarLista()
    {
        lista.Clear();

        List<Produto> tmp = await App.Db.GetAll();

        tmp.ForEach(i => lista.Add(i));
    }



    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;

        picker_categoria.ItemsSource = new List<string>
        {
            "Todas",
            "Alimentos",
            "Higiene",
            "Limpeza"
        };

        picker_categoria.SelectedIndex = 0;
    }

    

    protected async override void OnAppearing()
    {
        try
        {
            await CarregarLista();

            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            if (categoriaSelecionada != "Todas")
            {
                tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
            }

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;
            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            if (categoriaSelecionada != "Todas")
            {
                tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
            }

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);

            string msg = $"O Total é {soma:C}";

            DisplayAlert("Total dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
           DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private  async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            await CarregarLista();

            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            if (categoriaSelecionada != "Todas")
            {
                tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
            }

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void OnCategoriaSelecionada(object sender, EventArgs e)
    {
        if (picker_categoria.SelectedItem == null)
            return;

        categoriaSelecionada = picker_categoria.SelectedItem.ToString();

        lista.Clear();

        List<Produto> tmp = await App.Db.GetAll();

        if (categoriaSelecionada != "Todas")
        {
            tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
        }

        tmp.ForEach(i => lista.Add(i));
    }

    // 🔽 NOVO MÉTODO - abrir tela de relatório
    private async void ToolbarItem_Relatorio(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new RelatorioCategoria());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}