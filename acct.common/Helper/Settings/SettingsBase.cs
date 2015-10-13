using acct.common.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace acct.common.Helper.Settings
{
    public abstract class SettingsBase
    {
        private readonly string _name;
        private readonly PropertyInfo[] _properties;

        public SettingsBase()
        {
            var type = this.GetType();

            _name = type.Name;
            _properties = type.GetProperties();
        }

        public virtual void Load(IUnitOfWork unitOfWork)
        {
            var settings = unitOfWork.Settings.Where(w => w.Type == _name).ToList();

            foreach (var propertyInfo in _properties)
            {
                // get the setting from the settings list
                var setting = settings.SingleOrDefault(s => s.Name == propertyInfo.Name);
                if (setting != null)
                {
                    propertyInfo.SetValue(this, Convert.ChangeType(setting.Value, propertyInfo.PropertyType));
                }
            }
        }

        public virtual void Save(IUnitOfWork unitOfWork)
        {
            var settings = unitOfWork.Settings.Where(w => w.Type == _name).ToList();

            foreach (var propertyInfo in _properties)
            {
                object propertyValue = propertyInfo.GetValue(this, null);
                string value = (propertyValue == null) ? null : propertyValue.ToString();

                var setting = settings.SingleOrDefault(s => s.Name == propertyInfo.Name);
                if (setting != null)
                {
                    setting.Value = value;
                }
                else
                {
                    var newSetting = new Options()
                    {
                        Name = propertyInfo.Name,
                        Type = _name,
                        Value = value,
                    };

                    unitOfWork.Settings.Add(newSetting);
                }
            }
        }
    }
}
