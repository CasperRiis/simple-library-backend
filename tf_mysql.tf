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

#Allow all IPs
resource "azurerm_mysql_flexible_server_firewall_rule" "allow_all_ips" {
  name                = "allow_all_ips"
  resource_group_name = azurerm_resource_group.main.name
  server_name         = azurerm_mysql_flexible_server.main.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "255.255.255.255"
}

locals {
  mysql_connection_string = "server=${azurerm_mysql_flexible_server.main.fqdn};port=3306;database=${var.database_name};uid=${var.sql_user};pwd=${var.sql_password}"
}

output "mysql_connection_string" {
  value = local.mysql_connection_string
}
