using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LDN.Emit
{
    [TestClass]
    public class EmitBase
    {
        [TestMethod]
        public void CreatePetsDLL()
        {
            var assemblyName = new AssemblyName("Pets");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("PetsModule", "Pets.dll");
            var typeBuilder = moduleBuilder.DefineType("Kitty", TypeAttributes.Public);

            #region 字段
            var fieldId = typeBuilder.DefineField("_id", typeof(string), FieldAttributes.Private);
            var fieldName = typeBuilder.DefineField("_name", typeof(string), FieldAttributes.Private);
            #endregion

            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);
            Type[] constructorArgs = { typeof(int), typeof(string) };

            #region 构造函数
            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorArgs);
            ILGenerator ilOfCtor = constructorBuilder.GetILGenerator();
            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Call, objCtor);
            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Ldarg_1);
            ilOfCtor.Emit(OpCodes.Stfld, fieldId);
            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Ldarg_1);
            ilOfCtor.Emit(OpCodes.Stfld, fieldName);
            ilOfCtor.Emit(OpCodes.Ret);
            #endregion

            #region id 、ame 属性
            var methodGetId = typeBuilder.DefineMethod("GetId", MethodAttributes.Public, typeof(int), null);
            var methodSetId = typeBuilder.DefineMethod("SetId", MethodAttributes.Public, null, new Type[] { typeof(int) });

            var ilOfGetId = methodGetId.GetILGenerator();
            ilOfGetId.Emit(OpCodes.Ldarg_0);
            ilOfGetId.Emit(OpCodes.Ldfld, fieldId);
            ilOfGetId.Emit(OpCodes.Ret);

            var ilOfSetId = methodSetId.GetILGenerator();
            ilOfSetId.Emit(OpCodes.Ldarg_0);
            ilOfSetId.Emit(OpCodes.Ldarg_1);
            ilOfSetId.Emit(OpCodes.Stfld, fieldId);
            ilOfSetId.Emit(OpCodes.Ret);

            var propertyId = typeBuilder.DefineProperty("Id", PropertyAttributes.None, typeof(int), null);
            propertyId.SetGetMethod(methodGetId);
            propertyId.SetSetMethod(methodSetId);

            var methodGetName = typeBuilder.DefineMethod("GetName", MethodAttributes.Public, typeof(string), null);
            var methodSetName = typeBuilder.DefineMethod("SetName", MethodAttributes.Public, null, new Type[] { typeof(string) });

            var ilOfGetName = methodGetName.GetILGenerator();
            ilOfGetName.Emit(OpCodes.Ldarg_0); // this
            ilOfGetName.Emit(OpCodes.Ldfld, fieldName);
            ilOfGetName.Emit(OpCodes.Ret);

            var ilOfSetName = methodSetName.GetILGenerator();
            ilOfSetName.Emit(OpCodes.Ldarg_0); // this
            ilOfSetName.Emit(OpCodes.Ldarg_1); // the first one in arguments list
            ilOfSetName.Emit(OpCodes.Stfld, fieldName);
            ilOfSetName.Emit(OpCodes.Ret);

            // create Name property
            var propertyName = typeBuilder.DefineProperty("Name", PropertyAttributes.None, typeof(string), null);
            propertyName.SetGetMethod(methodGetName);
            propertyName.SetSetMethod(methodSetName);
            #endregion

            #region ToString
            var methodToString = typeBuilder.DefineMethod("ToString", MethodAttributes.Virtual | MethodAttributes.Public, typeof(string), null);
            var ilOfToString = methodToString.GetILGenerator();
            var local = ilOfToString.DeclareLocal(typeof(string));
            ilOfToString.Emit(OpCodes.Ldstr, "Id:[{0}],Name:[{1}]");
            ilOfToString.Emit(OpCodes.Ldarg_0);
            ilOfToString.Emit(OpCodes.Ldfld, fieldId);
            ilOfToString.Emit(OpCodes.Box, typeof(int));
            ilOfToString.Emit(OpCodes.Ldarg_0);
            ilOfToString.Emit(OpCodes.Ldfld, fieldName);
            ilOfToString.Emit(OpCodes.Call, typeof(string).GetMethod("Format",
                new Type[] { typeof(string), typeof(object), typeof(object) }));
            ilOfToString.Emit(OpCodes.Stloc, local);
            ilOfToString.Emit(OpCodes.Ldloc, local);
            ilOfToString.Emit(OpCodes.Ret);
            #endregion

            var classType = typeBuilder.CreateType();
            assemblyBuilder.Save("Pets.dll");
        }
    }
}
