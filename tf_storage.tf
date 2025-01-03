resource "azurerm_storage_account" "main" {
  name                     = "simplelibrarystorageacc"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "main" {
  name                  = "simple-library-container"
  storage_account_id    = azurerm_storage_account.main.id
  container_access_type = "blob"
}
