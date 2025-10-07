cd Backend

models=("Project" "User" "Command" "Status" "Task")

for model in "${models[@]}"; do
    
    controllerName="${model}sController"
    
    dotnet tool run dotnet-aspnet-codegenerator controller -name $controllerName -api -m $model -dc TodoListDbContext -outDir Controllers
done