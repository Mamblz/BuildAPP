
# 🏗️ BuildAPP – WPF-приложение для строительных расчетов  

**BuildAPP** – это десктопное приложение на C#/WPF для автоматизации строительных расчетов, управления проектами и составления смет.  

[![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/ru-ru/dotnet/csharp/)
[![WPF](https://img.shields.io/badge/WPF-5C2D91?style=flat&logo=.net&logoColor=white)](https://learn.microsoft.com/ru-ru/dotnet/desktop/wpf/)
[![License](https://img.shields.io/github/license/Mamblz/BuildAPP)](LICENSE)

## 🔍 Основные возможности (из кода ветки Kobzar)
- **Расчет строительных материалов** (бетон, кирпич, перекрытия)
- **Управление проектами** (CRUD-операции)
- **Визуализация данных** через WPF-графики
- **Работа с базой данных** (Entity Framework)

## 🛠 Технологический стек
```mermaid
pie
    title Технологии в проекте
    "C#" : 45
    "WPF (XAML)" : 30
    "Entity Framework" : 15
    "MS SQL Server" : 10
```

## ⚙️ Установка и запуск
1. **Требования**:  
   - .NET 6.0+  
   - Visual Studio 2022  
   - SQL Server (или LocalDB)

2. **Сборка**:
```bash
git clone https://github.com/Mamblz/BuildAPP.git
cd BuildAPP
git checkout Kobzar
dotnet restore
dotnet build
```


## 📂 Структура проекта
```
BuildAPP/
├── BuildAPP.Core/      # Бизнес-логика
├── BuildAPP.Data/      # Репозитории + EF
├── BuildAPP.UI/        # WPF-интерфейс
└── BuildAPP.Tests/     # Юнит-тесты
```
