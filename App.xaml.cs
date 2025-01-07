using FWAPPA;
using Syncfusion.Licensing;
namespace Lab6_Starter;

public partial class App : Application
{
	public App()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUzOTQ4NEAzMjM3MmUzMDJlMzBERzdTUWMxa3VsTjFiRDN1TzE0YThiMk1wMlJFUzM5Zkc3K2d2ZkthS1l3PQ==");
		InitializeComponent();

		MainPage = new LoginPage();
	}
}

