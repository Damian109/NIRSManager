using NIRSCore;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;

namespace NIRSManagerClient.ViewModels.SettingsViewModels
{
    /// <summary>
    /// Модель представления настроек пользовательского интерфейса
    /// </summary>
    public sealed class UserInterfaceSettingsViewModel : ViewModel
    {
        #region Private
        /// <summary>
        /// Смена цветовой схемы приложения
        /// </summary>
        /// <param name="isDark">Темная ли тема</param>
        private static void ApplyTheme(bool isDark)
        {
            new PaletteHelper().SetLightDark(isDark);
            NirsSystem.User.IsDarkTheme = isDark;
        }  

        /// <summary>
        /// Смена основной цветовой схемы
        /// </summary>
        /// <param name="swatch">Цветовая схема</param>
        private static void ApplyPrimary(Swatch swatch)
        {
            new PaletteHelper().ReplacePrimaryColor(swatch);
            NirsSystem.User.MainColors = swatch.Name;
        }     

        /// <summary>
        /// Смена дополнительной цветовой схемы
        /// </summary>
        /// <param name="swatch">Цветовая схема</param>
        private static void ApplyAccent(Swatch swatch)
        {
            new PaletteHelper().ReplaceAccentColor(swatch);
            NirsSystem.User.AdditionalColors = swatch.Name;
        }
            
        #endregion

        /// <summary>
        /// Загрузка цветовых схем
        /// </summary>
        public UserInterfaceSettingsViewModel() : base("Главная форма") =>
            Swatches = new SwatchesProvider().Swatches;

        /// <summary>
        /// Цветовые схемы
        /// </summary>
        public IEnumerable<Swatch> Swatches { get; }

        /// <summary>
        /// Команда смены цвета
        /// </summary>
        public RelayCommand CommandApplyTheme
        {
            get => new RelayCommand(obj => ApplyTheme((bool)obj));
        }

        /// <summary>
        /// Команда смены основных цветов
        /// </summary>
        public RelayCommand CommandApplyPrimary
        {
            get => new RelayCommand(obj => ApplyPrimary((Swatch)obj));
        }

        /// <summary>
        /// Команда смены дополнительных цветов
        /// </summary>
        public RelayCommand CommandApplyAccent
        {
            get => new RelayCommand(obj => ApplyAccent((Swatch)obj));
        }
    }
}