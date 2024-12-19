resource "azurerm_mysql_flexible_server" "main" {
  name                   = "simple-library-mysql"
  resource_group_name    = azurerm_resource_group.main.name
  location               = azurerm_resource_group.main.location
  administrator_login    = var.sql_user
  administrator_password = var.sql_password
  sku_name               = "B_Standard_B1ms"
  version                = "8.0.21"
  zone                   = "2"
}

resource "azurerm_mysql_flexible_database" "main" {
  name                = var.database_name
  resource_group_name = azurerm_resource_group.main.name
  server_name         = azurerm_mysql_flexible_server.main.name
  depends_on          = [azurerm_mysql_flexible_server.main]
  charset             = "utf8"
  collation           = "utf8_unicode_ci"
}

locals {
  mysql_connection_string = "Server=${azurerm_mysql_flexible_server.main.fqdn};User=${var.sql_user};Password=${var.sql_password};Database=${var.database_name};"
}

output "mysql_connection_string" {
  value = local.mysql_connection_string
}
