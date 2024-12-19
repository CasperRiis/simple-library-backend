# HOW TO USE:
# GIVE PERMISSIONS
# chmod +x ./tf_deploy.sh
# RUN
# ./tf_deploy.sh

terraform init -upgrade
terraform apply -auto-approve

WEBAPP_NAME="$(terraform output -raw web_app_name)"
RESOURCE_GROUP="$(terraform output -raw resource_group_name)"

dotnet clean
dotnet restore
dotnet publish -c Release

az webapp up --runtime "dotnet:8" --name $WEBAPP_NAME --resource-group $RESOURCE_GROUP
#az webapp up --runtime "dotnet:8" --name simple-library-backend --resource-group simple-library