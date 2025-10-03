SOLUTION_NAME="TitanHelp"

echo "Creating TitanHelp project structure..."

mkdir -p src docs .GitHub

cd src

dotnet new sln -n $SOLUTION_NAME

dotnet new classlib -n ${SOLUTION_NAME}.DataAccess -f net8.0
dotnet sln add ${SOLUTION_NAME}.DataAccess/${SOLUTION_NAME}.DataAccess.csproj
cd ${SOLUTION_NAME}.DataAccess
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
mkdir -p Data Entities Repositories Interfaces
rm Class1.cs
cd ..

dotnet new classlib -n ${SOLUTION_NAME}.Application -f net8.0
dotnet sln add ${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj
cd ${SOLUTION_NAME}.Application
dotnet add reference ../${SOLUTION_NAME}.DataAccess/${SOLUTION_NAME}.DataAccess.csproj
mkdir -p Services Interfaces DTOs Validators
rm Class1.cs
cd ..

dotnet new mvc -n ${SOLUTION_NAME}.Web -f net8.0
dotnet sln add ${SOLUTION_NAME}.Web/${SOLUTION_NAME}.Web.csproj
cd ${SOLUTION_NAME}.Web
dotnet add reference ../${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj
dotnet add reference ../${SOLUTION_NAME}.DataAccess/${SOLUTION_NAME}.DataAccess.csproj
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.0
cd ..

dotnet new mstest -n ${SOLUTION_NAME}.DataAccess.Tests -f net8.0
dotnet sln add ${SOLUTION_NAME}.DataAccess.Tests/${SOLUTION_NAME}.DataAccess.Tests.csproj
cd ${SOLUTION_NAME}.DataAccess.Tests
dotnet add reference ../${SOLUTION_NAME}.DataAccess/${SOLUTION_NAME}.DataAccess.csproj
dotnet add package Moq --version 4.20.70
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.0
rm Test1.cs
cd ..

dotnet new mstest -n ${SOLUTION_NAME}.Application.Tests -f net8.0
dotnet sln add ${SOLUTION_NAME}.Application.Tests/${SOLUTION_NAME}.Application.Tests.csproj
cd ${SOLUTION_NAME}.Application.Tests
dotnet add reference ../${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj
dotnet add package Moq --version 4.20.70
rm Test1.cs
cd ..

dotnet new mstest -n ${SOLUTION_NAME}.Web.Tests -f net8.0
dotnet sln add ${SOLUTION_NAME}.Web.Tests/${SOLUTION_NAME}.Web.Tests.csproj
cd ${SOLUTION_NAME}.Web.Tests
dotnet add reference ../${SOLUTION_NAME}.Web/${SOLUTION_NAME}.Web.csproj
dotnet add package Moq --version 4.20.70
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.0
rm Test1.cs
cd ..

dotnet build

echo "Project structure created successfully!"
cd ..
