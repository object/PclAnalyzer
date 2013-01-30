using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using PclAnalyzer.Core;

namespace PclAnalyzer.Reflection
{
    public class AssemblyParser
    {
        private string _assemblyPath;

        public AssemblyParser(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        public IList<MethodCall> GetAssemblyCalls()
        {
            var module = ModuleDefinition.ReadModule(_assemblyPath);

            return (from t in module.Types
                    from m in t.Methods
                    where m.Body != null
                    from i in m.Body.Instructions
                    where IsExternalCall(module, i)
                    select new MethodCall(GetMethodInfo(m), GetMethodInfo(i.Operand as MethodReference)))
                    .Distinct()
                    .Where(x => !IsExcluded(x.ReferencedMethod)).ToList();
        }

        public IList<MethodCall> GetTypeCalls(string typeName)
        {
            var module = ModuleDefinition.ReadModule(_assemblyPath);

            return (from t in module.Types
                    where GetTypeFullName(t) == typeName
                    from m in t.Methods
                    where m.Body != null
                    from i in m.Body.Instructions
                    where IsExternalCall(module, i)
                    select new MethodCall(GetMethodInfo(m), GetMethodInfo(i.Operand as MethodReference)))
                    .Distinct()
                    .Where(x => !IsExcluded(x.ReferencedMethod)).ToList();
        }

        public IList<MethodCall> GetMethodCalls(string typeName, string methodName)
        {
            var module = ModuleDefinition.ReadModule(_assemblyPath);

            var a = from t in module.Types
                    where GetTypeFullName(t) == typeName
                    from m in t.Methods
                    where m.Body != null && GetMethodFullName(m) == string.Join(".", typeName, methodName)
                    from i in m.Body.Instructions
                    select i;

            return (from t in module.Types
                    where GetTypeFullName(t) == typeName
                    from m in t.Methods
                    where m.Body != null && GetMethodFullName(m) == string.Join(".", typeName, methodName)
                    from i in m.Body.Instructions
                    where IsExternalCall(module, i)
                    select new MethodCall(GetMethodInfo(m), GetMethodInfo(i.Operand as MethodReference)))
                    .Distinct()
                    .Where(x => !IsExcluded(x.ReferencedMethod)).ToList();
        }

        private string GetTypeFullName(TypeDefinition type)
        {
            return string.Join(".", type.Namespace, type.Name);
        }

        private string GetMethodFullName(MethodDefinition method)
        {
            return string.Join(".", method.DeclaringType.Namespace, method.DeclaringType.Name, method.Name);
        }

        private bool IsExternalCall(ModuleDefinition module, Instruction i)
        {
            return i.Operand is MethodReference && (i.Operand as MethodReference).DeclaringType.Scope != module;
        }

        private bool IsExcluded(Member methodInfo)
        {
            return methodInfo.TypeName.Contains("[") || methodInfo.TypeName.Contains("]");
        }

        private Member GetMethodInfo(MethodDefinition method)
        {
            return new Member(method.DeclaringType.Scope.Name, method.DeclaringType.Namespace, method.DeclaringType.Name, method.Name);
        }

        private Member GetMethodInfo(MethodReference method)
        {
            return new Member(method.DeclaringType.Scope.Name, method.DeclaringType.Namespace, method.DeclaringType.Name, method.Name);
        }
    }
}
