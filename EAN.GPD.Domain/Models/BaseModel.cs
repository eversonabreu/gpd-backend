using EAN.GPD.Domain.Entities;
using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EAN.GPD.Domain.Models
{
    public abstract class BaseModel
    {
        public abstract long? GetId();

        public TEntity ToEntity<TEntity>() where TEntity : BaseEntity
        {
            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity), (long?)null);
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var property = entity.GetType().GetProperty(prop.Name);
                if (prop.GetCustomAttribute<NotMappedAttribute>() != null)
                {
                    var value = prop.GetValue(this);
                    if (value != null)
                    {
                        property?.SetValue(entity, Convert.ToInt64(value));
                    }
                }
                else
                {
                    property?.SetValue(entity, prop.GetValue(this));
                }
            }

            return entity;
        }

        public virtual bool AdditionalValidations(out string messages)
        {
            messages = string.Empty;
            return true;
        }
    }

    public static class BaseModelExtension
    {
        public static bool IsValid(this BaseModel baseModel, out string messages)
        {
            messages = string.Empty;
            if (baseModel is null)
            {
                messages = "Model em estado nulo ou não definido.";
                return false;
            }

            var result = new List<ValidationResult>();
            var context = new ValidationContext(baseModel, null, null);
            if (Validator.TryValidateObject(baseModel, context, result, true))
            {
                if (baseModel.AdditionalValidations(out messages))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            var resultMessages = new StringBuilder();
            foreach (var item in result)
            {
                resultMessages.AppendLine(item.ErrorMessage);
            }

            messages = resultMessages.ToString();
            return false;
        }
    }
}
