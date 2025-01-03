resource "azurerm_windows_web_app" "main" {
  name                = "simple-library-backend"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  service_plan_id     = azurerm_service_plan.main.id
  depends_on          = [azurerm_mysql_flexible_database.main]

  site_config {
    always_on = false #needed for service plans with F1 (free) SKU    
  }

  identity {
    type = "SystemAssigned"
  }

  zip_deploy_file = var.zip_deploy_file_path
  #ENVIRONMENT VARIABLE
  app_settings = {
    "WEBSITE_RUN_FROM_PACKAGE"       = "1",
    "ASPNETCORE_ENVIRONMENT"         = "Production"
    "MYSQL_CONNECTION_STRING"        = local.mysql_connection_string
    "JWT_TOKEN_SECRET"               = var.jwt_token_secret
    "AzureStorage__ConnectionString" = azurerm_storage_account.main.primary_connection_string
    "AzureStorage__ContainerName"    = azurerm_storage_container.main.name
  }

  logs {
    application_logs {
      file_system_level = "Information"
    }

    http_logs {
      file_system {
        retention_in_mb   = 100
        retention_in_days = 7
      }
    }

    detailed_error_messages = true

    failed_request_tracing = true
  }
}

output "web_app_name" {
  value = azurerm_windows_web_app.main.name
}

output "api_uri" {
  value = "${azurerm_windows_web_app.main.name}.azurewebsites.net"
}
