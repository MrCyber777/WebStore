namespace WebStore.Utility
{
    public class SD // <-- Специальный класс для хранения абсолютного пути к каталогу сохранения изображений и хранения названия изображения по умолчанию
    {
        public const string DefaultProductImage = "default_product.png";
        public const string ImageFolder = @"Images\ProductImage"; // <-- Абсолютный путь до каталога сохранения картинок продукта
        public const string SessionKey = "sShoppingCart";
        public const string AdminEndUser = "Admin";
        public const string SuperAdminEndUser = "Super Admin";
        public const string User = "User";
    }
}
