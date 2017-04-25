---
services: azure-billing
platforms: dotnet-core
author: jlian
---
# Get Azure invoice with .NET Core

This is a simple .NET Core sample that uses the Azure .NET SDK to programmatically get your invoices.

## Run this sample

1. Get the [.NET Core SDK](https://www.microsoft.com/net/core).

1. Get the Account Admin of the subscription to [opt in and turn on API access to invoices](https://docs.microsoft.com/azure/billing/billing-manage-access).

1. Create an Azure service principal either through
    [Azure CLI](https://docs.microsoft.com/azure/azure-resource-manager/resource-group-authenticate-service-principal-cli/),
    [PowerShell](https://docs.microsoft.com/azure/azure-resource-manager/resource-group-authenticate-service-principal/)
    or [the portal](https://docs.microsoft.com/azure/azure-resource-manager/resource-group-create-service-principal-portal/).

1. Clone the repository and install dependencies

    ```bash
    git clone https://github.com/Azure-Samples/billing-dotnet-core-invoice-download.git
    cd billing-dotnet-core-invoice-download
    dotnet restore
    ```

1. Edit `appsettings.json` using your subscription ID, tenant domain, client ID, and client secret from the service principle that you created. Example:

    ```json
    {
        "TenantDomain": "yourtenant.onmicrosoft.com",
        "SubscriptionID": "your subscription ID",
        "ClientID": "your client ID",
        "ClientSecret": "your client secret"
    }
    ```

1. Run the sample.

    ```bash
    dotnet run
    ```

## Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
