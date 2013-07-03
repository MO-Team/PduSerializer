using System;
using System.Reflection;

namespace PduSerializer
{
    public interface ITypeConfiguration
    {
        /// <summary>
        /// register a specified type
        /// </summary>
        /// <param name="type">type to register</param>
        /// <returns></returns>
        ISerializationConfiguration AddType(Type type);

        /// <summary>
        /// register a specified type
        /// </summary>
        /// <typeparam name="T">type to register</typeparam>
        /// <returns></returns>
        ISerializationConfiguration AddType<T>();

        /// <summary>
        /// register all types from a specified assembly
        /// </summary>
        /// <typeparam name="T">type to register</typeparam>
        /// <returns></returns>
        ISerializationConfiguration AddTypesFromAssemblyOf<T>();

        /// <summary>
        /// register  a specified type
        /// </summary>
        /// <param name="assembly">assembly to register types from</param>
        /// <returns></returns>
        ISerializationConfiguration AddTypesFromAssembly(Assembly assembly);
    }
}