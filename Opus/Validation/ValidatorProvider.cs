using FluentValidation;
using System;
using System.Collections.Generic;

namespace Opus.Validation
{
    public interface IValidatorProvider
    {
        void Add<T>(IValidator validator);
        IValidator For<T>();
        IValidator For(Type type);
        bool Exists<T>();
        bool Exists(Type type);
    }

    public class ValidatorProvider : IValidatorProvider
    {
        private readonly Dictionary<Type, IValidator> _validators;

        public ValidatorProvider()
        {
            _validators = new Dictionary<Type, IValidator>();
        }

        public void Add<T>(IValidator validator)
        {
            if (!_validators.TryAdd(typeof(T), validator))
                throw new Exception("Validator has already been defined");
        }

        public bool Exists<T>()
        {
            return Exists(typeof(T));
        }

        public bool Exists(Type type)
        {
            return _validators.ContainsKey(type);
        }

        public IValidator For<T>()
        {
            return For(typeof(T));
        }

        public IValidator For(Type type)
        {
            if (!_validators.TryGetValue(type, out var validator))
                throw new Exception("Validator for target has not been defined");
            return validator;
        }
    }
}
