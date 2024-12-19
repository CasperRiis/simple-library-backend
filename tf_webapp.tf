
resource "random_password" "jwt_token_secret" {
  # Generate a random password for JWT token secret
  # This is not used to maintain the same secret between runs and development environments
  length           = 128
  special          = true
  override_special = "!#$%&*()-_=+[]{}<>:?"
}

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

  #ENVIRONMENT VARIABLE
  app_settings = {
    "ASPNETCORE_ENVIRONMENT"  = "Production"
    "MYSQL_CONNECTION_STRING" = local.mysql_connection_string
    "JWT_TOKEN_SECRET"        = var.jwt_token_secret
  }
}

output "web_app_name" {
  value = azurerm_windows_web_app.main.name
}

output "api_uri" {
  value = "${azurerm_windows_web_app.main.name}.azurewebsites.net"
}
