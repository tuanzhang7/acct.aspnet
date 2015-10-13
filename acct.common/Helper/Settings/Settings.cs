using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acct.common.Helper.Settings
{
    public interface ISettings
    {
        GeneralSettings General { get; }
        SeoSettings Seo { get; }
        void Save();
    }

    public class Settings : ISettings//, IPerWebRequest
    {
        // 1
        //private readonly Lazy<GeneralSettings> _generalSettings;
        private readonly Lazy<GeneralSettings> _generalSettings;
        // 2
        public GeneralSettings General { get { return _generalSettings.Value; } }

        private readonly Lazy<SeoSettings> _seoSettings;
        public SeoSettings Seo { get { return _seoSettings.Value; } }

        private readonly IUnitOfWork _unitOfWork;
        //private readonly ICache _cache;

        public Settings(IUnitOfWork unitOfWork)
        {
            // ARGUMENT CHECKING SKIPPED FOR BREVITY
            _unitOfWork = unitOfWork;
            // 3
            _generalSettings = new Lazy<GeneralSettings>(CreateSettings<GeneralSettings>);
            _seoSettings = new Lazy<SeoSettings>(CreateSettings<SeoSettings>);
        }
        public static GeneralSettings GetGeneralSettings()
        {
            UnitOfWork uow = new UnitOfWork();
            GeneralSettings settings = new GeneralSettings();
            settings.Load(uow);
            return settings;
        }
        //public Settings(IUnitOfWork unitOfWork, ICache cache)
        //{
        //    _unitOfWork = unitOfWork;
        //    _cache = cache;

        //    _generalSettings = new Lazy<GeneralSettings>(CreateSettingsWithCache<GeneralSettings>);
        //    _seoSettings = new Lazy<SeoSettings>(CreateSettingsWithCache<SeoSettings>);
        //}

        public void Save()
        {
            // only save changes to settings that have been loaded
            if (_generalSettings.IsValueCreated)
                _generalSettings.Value.Save(_unitOfWork);

            if (_seoSettings.IsValueCreated)
                _seoSettings.Value.Save(_unitOfWork);

            _unitOfWork.SaveChanges();
        }
        // 4
        private T CreateSettings<T>() where T : SettingsBase, new()
        {
            var settings = new T();
            settings.Load(_unitOfWork);
            return settings;
        }

        //private T CreateSettingsWithCache<T>() where T : SettingsBase, new()
        //{

        //    string name = typeof(T).Name;
        //    var settings = _cache.Get(name) as T;
        //    if (settings == null)
        //    {
        //        settings = new T();
        //        settings.Load(_unitOfWork);
        //        _cache.Store(name, settings, false, _cache.MaxDuration);
        //    }

        //    return settings;
        //}
    }
}
